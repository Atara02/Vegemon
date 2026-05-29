using UnityEngine;

public class BattleHoldAction : StateBase<BattleSystem>
{
    public override void Init(BattleSystem value)
    {
        if(value != null)
        {
            m_sys = value;
        }
    }
    public override void EnterState()
    {
        return;
    }
    public override void UpdateState()
    {
        m_sys.ChargeGauge();
    }
    public override void ExitState()
    {
        
    }
}
