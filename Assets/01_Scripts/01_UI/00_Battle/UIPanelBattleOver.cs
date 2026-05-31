using TMPro;
using UnityEngine;

public class UIPanelBattleOver : MonoBehaviour
{
    GameManager m_gm = null;
    UIAlarm m_alarm = null;

    [SerializeField] TMP_Text m_txtTitle;
    [SerializeField] TMP_Text m_txtKill;
    [SerializeField] TMP_Text m_txtSoul;
    [SerializeField] TMP_Text m_txtSeed;

    private void Start()
    {
        m_alarm = GetComponent<UIAlarm>();

        BattleManager.Instance.onUpdateKillCount += SetKillTxt;
        BattleManager.Instance.onUpdateSoulAmount += SetSoulTxt;
        BattleManager.Instance.onUpdateSeedAmount += SetSeedTxt;


        m_gm = GameManager.Instance;
        m_gm.onGamePause += ShowGamePause;
        m_gm.onGameClear += ShowGameClear;
        m_gm.onGameOver += ShowGameOver;
    }
    void ShowPanel()
    {
        m_alarm.ShowAlarm(-1);
    }

    void ShowGamePause()
    {
        m_txtTitle.text = "<Pause>";
        ShowPanel();
    }
    void ShowGameClear()
    {
        m_txtTitle.text = "<GameClear>";
        ShowPanel();
    }
    void ShowGameOver()
    {
        m_txtTitle.text = "<GameOver>";
        ShowPanel();
    }

    void SetKillTxt(int kill)
    {
        m_txtKill.text = kill.ToString();
    }
    void SetSoulTxt(int soul)
    {
        m_txtSoul.text = soul.ToString();
    }
    void SetSeedTxt(int seed)
    {
        m_txtSeed.text = seed.ToString();
    }
}
