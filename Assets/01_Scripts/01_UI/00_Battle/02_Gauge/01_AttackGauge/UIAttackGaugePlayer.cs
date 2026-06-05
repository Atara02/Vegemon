using UnityEngine;

public class UIAttackGaugePlayer : UIBattleGauge
{
    protected override void Start()
    {
        base.Start();

        m_battle.OnUpdatePlayerGauge += SetGauge;
        m_battle.OnPlayerDead += () => SetGauge(false);
    }
}
