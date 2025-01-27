﻿using UnityEngine;

public class GameStateManager : SingletonMonovihair<GameStateManager>
{
    [SerializeField]GameState _gameState = GameState.StayState;
    public GameState GameState => _gameState;

    // Start is called before the first frame update
    protected override void Awake()
    {
        //base.Awake();
        //ChangeState(GameState.InBattleState);
    }

    public void InBattle()
    {
        ChangeState(GameState.InBattleState);
        ResponceManager.Instance.GetPlayerEnemy();
    }

    public void EndBattle(bool win)
    {
        ChangeState(GameState.BattleEndState);
        if (win)
        {
            StartCoroutine(InGameManager.Instance.GameCrear());
        }
        else
        {
            StartCoroutine(InGameManager.Instance.GameOver());
        }
    }

    public void ChangeState(GameState state)
    {
        _gameState = state;
    }
}

public enum GameState
{
    StayState,
    InBattleState,
    BattleStopState,
    BattleEndState,
}
