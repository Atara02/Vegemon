using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BtnEventSetter : MonoBehaviour
{
    protected Button m_btn = null;
    private void Awake()
    {
        m_btn = GetComponent<Button>();

    }
    protected void SetButtonClickEvent(UnityAction action)
    {
        if (m_btn)
        {
            m_btn.onClick.AddListener(action);
        }
    }
}
