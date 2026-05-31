using UnityEngine;

public class BattleDeadEnemy : StateBase<BattleSystem>
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
        m_sys.EnterEnemyDead();
    }
    public override void UpdateState()
    {
        
    }
    public override void ExitState()
    {
        m_sys.ExitEnemyDead();
    }
}
