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
    PlayerInput _input;
    PlayStringAudio _clickAudioPlay;

    private void Awake()
    {
        _input = new PlayerInput();
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    private void Start()
    {
        _pauseText.gameObject.SetActive(false);
        _pauseCanvas.gameObject.SetActive(false);
        _timeScaleManager = TimeScaleManager.Instance;
        _clickAudioPlay = GetComponent<PlayStringAudio>();
    }

    private void Update()
    {
        if (_input.Pause.Pause.WasPerformedThisFrame()
            && (GameStateManager.Instance.GameState == GameState.InBattleState
            || GameStateManager.Instance.GameState == GameState.BattleStopState))
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
        _clickAudioPlay.PlayNameAudio();
    }

    public void PauseEnd()
    {
        _isPause = false;
        GameStateManager.Instance.ChangeState(GameState.InBattleState);
        StopCoroutine(_pauseCoroutine);
        _pauseText.gameObject.SetActive(false);
        _pauseCanvas.gameObject.SetActive(false);
        _timeScaleManager.EndPauseManager();
        _clickAudioPlay.PlayNameAudio();
    }

    public void BattleEnd()
    {
        _isPause = false;
        _clickAudioPlay.PlayNameAudio();
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
