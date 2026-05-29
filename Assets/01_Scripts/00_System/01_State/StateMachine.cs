
using System.Collections;
using UnityEngine;

public class StateMachine<T> : MonoBehaviour
{
    [Header("<제어>")]
    [SerializeField] protected bool m_isRestric = false;

    protected StateBase<T> m_current = null;
    protected Coroutine m_update = null;

    protected IEnumerator StateUpdate()
    {
        while(!m_isRestric)
        {
            m_current?.UpdateState();
            yield return null;
        }
    }
    protected void UpdateState(StateBase<T> state)
    {
        if (state == null || m_current == state) { return; }

        //상태 종료
        UpdateCoroutine(null);
        m_current?.ExitState();

        //새로운 상태로 교체
        if (!m_isRestric)
        {
            m_current = state;
            m_current?.EnterState();
            UpdateCoroutine(StateUpdate());
        }
    }
    protected void UpdateCoroutine(IEnumerator enumerator = null)
    {
        if (m_update != null)
        {
            StopCoroutine(m_update);
            m_update = null;
        }
        if (enumerator != null)
        {
            m_update = StartCoroutine(enumerator);
        }
    }
}
