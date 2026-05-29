using UnityEngine;

public class BattleDeadPlayer : StateBase<BattleSystem>
{
    public override void Init(BattleSystem value)
    {
        if (value != null)
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
        
    }
    public override void ExitState()
    {

    }
}
