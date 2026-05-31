using DG.Tweening;
using System.Collections;
using UnityEngine;


public class BattleSystem : StateMachine<BattleSystem>
{
    public float m_height = 0f;
    public float m_width = 0f;
    public Vector2 m_center = Vector2.zero;

    public Vector2 m_enemySpawn;
    public Vector2 m_enemyPoint;

    [Header("<유닛>")]
    public BattleUnit m_player;
    public BattleUnit m_enemy;

    [Header("<공격 게이지>")]
    public float m_maxGauge = 100f;
    private float m_ptimer;
    private float m_etimer;

    #region <상태>
    public BattleHoldAction m_hold;
    public BattlePlayerAction m_pAction;
    public BattleEnemyAction m_eAction;

    public BattleDeadPlayer m_pDead;
    public BattleDeadEnemy m_eDead;
    #endregion
    public BattleManager m_bMgr;
    public UIAlarm m_next;

    private void Start()
    {
        GameManager.Instance.onGamePause += RestrictState;
        GameManager.Instance.onGameClear += RestrictState;
        GameManager.Instance.onGameOver += RestrictState;

        m_bMgr = GetComponent<BattleManager>();

        m_center = Camera.main.transform.position;
        m_height = Camera.main.orthographicSize * 2f;
        m_width = m_height * ((float)Screen.width / Screen.height);
        m_enemySpawn = m_center + new Vector2((m_width * 0.5f) + 1f, 0f);
        m_enemyPoint = m_enemy.transform.position;

        m_player.Init();

        SetNextEnemyStatus();
        m_enemy.Init();

        m_ptimer = 0f;
        m_etimer = 0f;

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

    void SetNextEnemyStatus()
    {
        int hp = m_bMgr.CalculateEnemyHp();
        int atk = m_bMgr.CalculateEnemyAtk();
        int def = m_bMgr.CalculateEnemyDef();
        m_enemy.SetStatus(hp, atk, def);
    }

    public void ChargeGauge()
    {
        m_ptimer += m_player.m_speed * Time.deltaTime;
        m_etimer += m_enemy.m_speed * Time.deltaTime;
        if (m_ptimer >= m_maxGauge)
        {
            UpdateState(m_pAction);
        }
        else if (m_etimer >= m_maxGauge)
        {
            UpdateState(m_eAction);
        }
    }

    #region <플레이어 공격>
    public void EnterPlayerSequence()
    {
        m_ptimer = 0f;
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
        m_etimer = 0f;
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
        m_ptimer = 0f;
        m_etimer = 0f;
        m_player.OnUnitDead += GameManager.Instance.GameOver;
    }

    #region <Enemy 사망>
    public void EnterEnemyDead()
    {
        m_ptimer = 0f;
        m_etimer = 0f;
        m_bMgr.KillMob();
        m_enemy.OnUnitDead += ShowNextEnemyUI;
    }
    public void ExitEnemyDead()
    {
        //에너미 정보 갱신
        m_enemy.Init();
        m_enemy.PlayIdleAnimation();
    }
    void ShowNextEnemyUI()
    {
        if (m_isRestric) { return; }

        m_next.ShowAlarm(1f);
        m_next.OnQuitAlarm += NextEnemy;
    }
    public void NextEnemy()
    {
        SetNextEnemyStatus();
        m_enemy.SpawnUnit(() => UpdateState(m_hold));
    }
    #endregion
}
