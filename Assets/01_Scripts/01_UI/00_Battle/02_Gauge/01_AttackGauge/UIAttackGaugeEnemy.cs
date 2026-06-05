using UnityEngine;

public class UIAttackGaugeEnemy : UIBattleGauge
{
    protected override void Start()
    {
        base.Start();
        m_battle.OnUpdateEnemyGauge += SetGauge;
        m_battle.OnEnemyDead += () => SetGauge(false);
    }
}
