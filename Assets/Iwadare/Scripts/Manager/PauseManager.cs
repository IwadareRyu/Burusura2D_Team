using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    bool _isPause = false;

    [SerializeField] Canvas _pauseCanvas;
    [SerializeField] Text _pauseText;
    [SerializeField] float _pauseTextTime = 2f;
    Coroutine _pauseCoroutine;
    TimeScaleManager _timeScaleManager;

    private void Start()
    {
        _pauseText.gameObject.SetActive(false);
        _pauseCanvas.gameObject.SetActive(false);
        _timeScaleManager = TimeScaleManager.Instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && GameStateManager.Instance.GameState == GameState.InBattleState)
        {
            if (_isPause)
            {
                PauseEnd();
            }
            else
            {
                PauseStart();
            }
        }
    }

    public void PauseStart()
    {
        _isPause = true;
        GameStateManager.Instance.ChangeState(GameState.BattleStopState);
        _pauseCanvas.gameObject.SetActive(true);
        _pauseCoroutine = StartCoroutine(PauseLoop());
        _timeScaleManager.StartPauseManager();
    }

    public void PauseEnd()
    {
        _isPause = false;
        GameStateManager.Instance.ChangeState(GameState.InBattleState);
        StopCoroutine(_pauseCoroutine);
        _pauseText.gameObject.SetActive(false);
        _pauseCanvas.gameObject.SetActive(false);
        _timeScaleManager.EndPauseManager();
    }

    public void BattleEnd()
    {
        _isPause = false;
        GameStateManager.Instance.ChangeState(GameState.BattleEndState);
        StopCoroutine(_pauseCoroutine);
        _pauseText.gameObject.SetActive(false);
        _pauseCanvas.gameObject.SetActive(false);
        _timeScaleManager.EndPauseManager();
        GameStateManager.Instance.EndBattle(false);
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
