using DG.Tweening;
using System;
using UnityEngine;

public class UIAlarm : MonoBehaviour
{
    CanvasGroup m_group;

    public event Action OnQuitAlarm;
    private void Awake()
    {
        m_group = GetComponent<CanvasGroup>();
        m_group.alpha = 0f;
    }

    public void ShowAlarm(float duration)
    {
        m_group.alpha = 1f;
        m_group.DOKill();
        if(duration > 0f)
        {
            m_group.DOFade(0f, duration).OnComplete(HideAlarm);
        }
    }
    void HideAlarm()
    {
        m_group.alpha = 0f;
        m_group.DOKill();

        OnQuitAlarm?.Invoke();
        OnQuitAlarm = null;
    }
}
