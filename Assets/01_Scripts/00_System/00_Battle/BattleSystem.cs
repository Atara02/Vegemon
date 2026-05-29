using System.Collections;
using UnityEngine;


public class BattleSystem : StateMachine<BattleSystem>
{
    [Header("<유닛>")]
    public BattleUnit m_player;
    public BattleUnit m_enemy;

    [Header("<공격 게이지>")]
    public float m_maxGauge = 100f;
    private float m_ptimer;
    private float m_etimer;

    public BattleHoldAction m_hold;
    public BattlePlayerAction m_pAction;
    public BattleEnemyAction m_eAction;

    public BattleDeadPlayer m_pDead;
    public BattleDeadEnemy m_eDead;

    private void Start()
    {
        m_player.Init();
        m_enemy.Init();

        m_ptimer = 0f;
        m_etimer = 0f;

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
    }

    public void ChargeGauge()
    {
        m_ptimer += m_player.m_speed * Time.deltaTime;
        m_etimer += m_enemy.m_speed * Time.deltaTime;
        if(m_ptimer >= m_maxGauge)
        {
            UpdateState(m_pAction);
        }
        else if(m_etimer >= m_maxGauge)
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
}
