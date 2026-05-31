using UnityEngine;
using UnityEngine.Events;

public class BtnGameStart : BtnEventSetter
{
    private void Start()
    {
        UnityAction gameStart = GameManager.Instance.GameStart;
        SetButtonClickEvent(gameStart);
    }
}
