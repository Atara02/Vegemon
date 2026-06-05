
using System.Collections;
using UnityEngine;

public class StateMachine<T> : MonoBehaviour
{
    [Header("<제어>")]
    [SerializeField] protected bool m_isRestric = false;

    protected StateBase<T> m_current = null;

    protected void RestrictState()
    {
        if (!m_isRestric)
        {
            m_isRestric = true;
        }
    }
    private void Update()
    {
        if (!m_isRestric) { m_current?.UpdateState(); }
    }
    protected void UpdateState(StateBase<T> state)
    {
        if (state == null || m_current == state) { return; }

        m_current?.ExitState();
        m_current = null;

        //새로운 상태로 교체
        if (!m_isRestric)
        {
            m_current = state;
            m_current?.EnterState();
        }
    }
}
