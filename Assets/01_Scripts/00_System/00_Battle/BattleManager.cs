using System.Collections;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [Header("<유닛>")]
    public BattleUnit m_player;
    public BattleUnit m_enemy;

    [Header("<공격 게이지>")]
    public float m_maxGauge = 100f;

    private float m_ptimer;
    private float m_etimer;

    private bool m_battleEnd;

    private void Start()
    {
        m_player.Init();
        m_enemy.Init();

        m_ptimer = 0f;
        m_etimer = 0f;

        StartCoroutine(Battle());
    }

    IEnumerator Battle()
    {
        while (!m_battleEnd)
        {
            ChargeGauge();
            ProcessAttack();
            yield return null;
        }
    }

    void ChargeGauge()
    {
        m_ptimer += m_player.m_speed * Time.deltaTime;
        m_etimer += m_enemy.m_speed * Time.deltaTime;
    }
    void ProcessAttack()
    {
        bool pReady = m_ptimer >= m_maxGauge;
        bool eReady = m_etimer >= m_maxGauge;

        // 둘 다 동시에 준비 완료
        if (pReady && eReady)
        {
            PlayerAttack();
            // 적이 살아있으면 공격
            if (!m_enemy.IsDead)
            {
                EnemyAttack();
            }
            return;
        }

        if (pReady)
        {
            PlayerAttack();
        }
        else if (eReady)
        {
            EnemyAttack();
        }
    }

    void PlayerAttack()
    {
        m_ptimer = 0f;
        m_player.Attack(m_enemy);

        CheckBattleEnd();
    }

    void EnemyAttack()
    {
        m_etimer = 0f;
        m_enemy.Attack(m_player);

        CheckBattleEnd();
    }

    void CheckBattleEnd()
    {
        if (m_player.IsDead)
        {
            m_battleEnd = true;
            Debug.Log("플레이어 패배");
        }
        else if (m_enemy.IsDead)
        {
            m_battleEnd = true;
            Debug.Log("플레이어 승리");
        }
    }
}
