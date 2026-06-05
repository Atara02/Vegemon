using UnityEngine;

public class UIBattleGauge : UIGauge
{
    protected BattleSystem m_battle;
    protected virtual void Start()
    {
        m_battle = BattleManager.Instance.GetComponent<BattleSystem>();
        m_battle.OnBattle += () => SetGauge(true);
    }
}
