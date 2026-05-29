using UnityEngine;

public class BattlePlayerAction : StateBase<BattleSystem>
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
        m_sys.EnterPlayerSequence();
    }
    public override void UpdateState()
    {
        return;
    }
    public override void ExitState()
    {
        
    }


}

