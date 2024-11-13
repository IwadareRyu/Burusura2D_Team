using System.Collections;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    bool _isPause = false;

    [SerializeField] Text _pauseText;
    [SerializeField] float _pauseTextTime = 2f;
    Coroutine _pauseCoroutine;
    TimeScaleManager _timeScaleManager;

    private void Start()
    {
        _pauseText.gameObject.SetActive(false);
        _timeScaleManager = TimeScaleManager.Instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isPause)
            {
                _isPause = false;
                GameStateManager.Instance.ChangeState(GameState.InBattleState);
                StopCoroutine(_pauseCoroutine);
                _pauseText.gameObject.SetActive(false);
                _timeScaleManager.EndPauseManager();
            }
            else
            {
                _isPause = true;
                GameStateManager.Instance.ChangeState(GameState.BattleStopState);
                _pauseCoroutine = StartCoroutine(PauseLoop());
                _timeScaleManager.StartPauseManager();
            }
        }
    }

    private IEnumerator PauseLoop()
    {
        while (true)
        {
            _pauseText.gameObject.SetActive(!_pauseText.gameObject.activeSelf);
            yield return new WaitForSecondsRealtime(_pauseTextTime);
        }
    }
}
