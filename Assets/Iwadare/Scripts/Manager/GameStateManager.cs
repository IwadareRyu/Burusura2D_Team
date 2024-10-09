using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class GameStateManager : MonoBehaviour
{
    [SerializeField]GameState _gameState = GameState.StayState;
    public GameState GameState => _gameState;

    public static GameStateManager instance;

    bool _isPause = false;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
        ChangeState(GameState.InBattleState);
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
