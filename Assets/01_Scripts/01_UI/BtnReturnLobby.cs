using UnityEngine;
using UnityEngine.Events;

public class BtnReturnLobby : BtnEventSetter
{
    private void Start()
    {
        UnityAction retrunLobby = GameManager.Instance.GetComponent<SceneChanger>().LoadLobbyScene;
        SetButtonClickEvent(retrunLobby);
    }
}
