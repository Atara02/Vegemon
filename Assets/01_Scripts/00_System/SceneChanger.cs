using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChanger : MonoBehaviour
{
    [SerializeField] int m_lobby = 0;
    [SerializeField] int m_battle = 1;

    private void Start()
    {
        GameManager.Instance.onGameStart += LoadBattelScene;
    }
    public void LoadLobbyScene()
    {
        SceneManager.LoadScene(m_lobby);
    }
    public void LoadBattelScene()
    {
        SceneManager.LoadScene(m_battle);
    }

}
