using TMPro;
using UnityEngine;

public class UIPanelBattleClear : MonoBehaviour
{
    GameManager m_gm = null;
    UIAlarm m_alarm = null;

    [SerializeField] TMP_Text m_txtKill;
    [SerializeField] TMP_Text m_txtSoul;
    [SerializeField] TMP_Text m_txtSeed;
    private void Start()
    {
        m_alarm = GetComponent<UIAlarm>();

        m_gm = GameManager.Instance;
        m_gm.onGameClear += ShowPanel;
    }
    void ShowPanel()
    {
        m_alarm.ShowAlarm(-1);
    }
}
