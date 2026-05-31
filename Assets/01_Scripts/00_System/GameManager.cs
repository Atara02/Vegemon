using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public event Action onGameStart;


    public event Action onGamePause;
    public event Action onGameClear;
    public event Action onGameOver;

    public void GameStart()
    {
        onGameStart?.Invoke();
    }
    public void GameOver()
    {
        onGameOver?.Invoke();
    }
    public void GameClear()
    {
        onGameClear?.Invoke();
    }
}
