using DG.Tweening;
using System.Collections;
using UnityEngine;


public class BattleSystem : StateMachine<BattleSystem>
{
    private BattleManager m_bMgr;

    [Header("<유닛>")]
    public PlayerUnit m_player;
    public EnemyUnit m_enemy;

    [Header("<공격 게이지>")]
    public float m_maxGauge = 1f;
    public float m_pGauge;
    public float m_eGauge;
    

    #region <상태>
    public BattleHoldAction m_hold;
    public BattlePlayerAction m_pAction;
    public BattleEnemyAction m_eAction;

    public BattleDeadPlayer m_pDead;
    public BattleDeadEnemy m_eDead;
    #endregion

    public event System.Action<float> OnUpdatePlayerGauge;
    public event System.Action<float> OnUpdateEnemyGauge;

    public event System.Action OnBattle;
    public event System.Action OnPlayerDead;
    public event System.Action OnEnemyDead;

    public UIAlarm m_next;

    private void Start()
    {
        GameManager.Instance.onGamePause += RestrictState;
        GameManager.Instance.onGameClear += RestrictState;
        GameManager.Instance.onGameOver += RestrictState;

        m_bMgr = GetComponent<BattleManager>();

        m_player = GetComponentInChildren<PlayerUnit>();
        m_enemy = GetComponentInChildren<EnemyUnit>();

        SetVegemonStat();
        m_player.Init();

        SetEnemyStat();
        m_enemy.Init();

        m_pGauge = 0f;
        m_eGauge = 0f;

        #region <상태>
        m_hold = new BattleHoldAction();
        m_hold.Init(this);

        m_pAction = new BattlePlayerAction();
        m_pAction.Init(this);
        m_eAction = new BattleEnemyAction();
        m_eAction.Init(this);

        m_pDead = new BattleDeadPlayer();
        m_pDead.Init(this);
        m_eDead = new BattleDeadEnemy();
        m_eDead.Init(this);

        UpdateState(m_hold);
        #endregion
    }
    void SetVegemonStat()
    {
        //내 베지몬 정보 불러와서 세팅하기 / 지금은 임시로
        VegemonData vegemon = VegemonDB.Instance.GetVegemonData(0);
        int hp = vegemon.m_hp;
        int atk = vegemon.m_atk;
        int def = vegemon.m_def;
        int spd = vegemon.m_spd;
        int luk = vegemon.m_luk;
        m_player.SetStat(hp, atk, def, spd, luk);
    }

    void SetEnemyStat()
    {
        int hp = m_bMgr.GetEnemyHp();
        int atk = m_bMgr.GetEnemyAtk();
        int def = m_bMgr.GetEnemyDef();
        int spd = m_bMgr.GetEnemySpd();
        int luk = m_bMgr.GetEnemyLuk();
        m_enemy.SetStat(hp, atk, def, spd, luk);
    }

    public void EnterHold()
    {
        OnBattle?.Invoke();
    }
    public void ChargeGauge()
    {
        m_pGauge += Mathf.Clamp01(m_player.Speed * Time.deltaTime);
        m_eGauge += Mathf.Clamp01(m_enemy.Speed * Time.deltaTime);

        OnUpdatePlayerGauge?.Invoke(m_pGauge);
        OnUpdateEnemyGauge?.Invoke(m_eGauge);
        if (m_pGauge >= m_maxGauge)
        {
            UpdateState(m_pAction);
        }
        else if (m_eGauge >= m_maxGauge)
        {
            UpdateState(m_eAction);
        }
    }

    #region <플레이어 공격>
    public void EnterPlayerSequence()
    {
        m_pGauge = 0f;
        m_player.OnUnitAttack += PlayerAttack;
        m_player.OnUnitQuitAttack += CheckQuitBattle;
        m_player.PlayAttackAnimation();
    }
    void PlayerAttack()
    {
        m_player.Attack(m_enemy);
    }
    #endregion

    #region <Enemy 공격>
    public void EnterEnemySequence()
    {
        m_eGauge = 0f;
        m_enemy.OnUnitAttack += EnemyAttack;
        m_enemy.OnUnitQuitAttack += CheckQuitBattle;
        m_enemy.PlayAttackAnimation();
    }
    void EnemyAttack()
    {
        m_enemy.Attack(m_player);
    }
    #endregion

    void CheckQuitBattle()
    {
        if (m_player.IsDead)
        {
            UpdateState(m_pDead);
        }
        else if (m_enemy.IsDead)
        {
            UpdateState(m_eDead);
        }
        else
        {
            UpdateState(m_hold);
        }
    }

    public void EnterPlayerDead()
    {
        m_pGauge = 0f;
        m_eGauge = 0f;
        m_player.OnUnitDead += GameManager.Instance.GameOver;

        OnPlayerDead?.Invoke();
    }

    #region <Enemy 사망>
    public void EnterEnemyDead()
    {
        m_pGauge = 0f;
        m_eGauge = 0f;
        m_bMgr.KillMob();
        m_enemy.OnUnitDead += ShowNextEnemyUI;

        OnEnemyDead?.Invoke();
    }
    public void ExitEnemyDead()
    {
        //에너미 정보 갱신
        m_enemy.Init();
    }
    void ShowNextEnemyUI()
    {
        if (m_isRestric) { return; }

        m_next.ShowAlarm(1f);
        m_next.OnQuitAlarm += NextEnemy;
    }
    public void NextEnemy()
    {
        SetEnemyStat();
        m_enemy.SpawnUnit(() => UpdateState(m_hold));
    }
    #endregion
}
