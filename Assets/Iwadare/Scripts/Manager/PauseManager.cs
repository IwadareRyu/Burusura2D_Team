using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    bool _isPause = false;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isPause)
            {
                _isPause = false;
                 GameStateManager.instance.ChangeState(GameState.InBattleState);
            }
            else
            {
                _isPause = true;
                GameStateManager.instance.ChangeState(GameState.BattleStopState);
            }
        }
    }
}
