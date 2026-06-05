using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIButtonBattleSpeed : BtnEventSetter
{
    Image m_speedIcon;
    public Sprite[] m_icon;
    private void Start()
    {
        m_speedIcon = transform.GetChild(0).GetComponent<Image>();
        m_speedIcon.sprite = m_icon[0];
        UnityAction action = BattleSpeedUp;
        SetButtonClickEvent(action);
    }
    void BattleSpeedUp()
    {
        int index = BattleManager.Instance.UpSpeed();
        m_speedIcon.sprite = m_icon[index];
    }
}
