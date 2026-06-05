using UnityEngine;

public class UIHpBarPlayer : UIBattleGauge
{
    protected override void Start()
    {
        base.Start();
        m_battle.OnPlayerDead += () => SetGauge(false);

        BattleUnit m_owner = m_battle.GetComponentInChildren<PlayerUnit>();
        m_owner.OnUpdateHp += SetGauge;
    }
}
