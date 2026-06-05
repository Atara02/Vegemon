using UnityEngine;

public class UIHpBarEnemy : UIBattleGauge
{
    protected override void Start()
    {
        base.Start();
        m_battle.OnEnemyDead += () => SetGauge(false);

        BattleUnit m_owner = m_battle.GetComponentInChildren<EnemyUnit>();
        m_owner.OnUpdateHp += SetGauge;
    }
}
