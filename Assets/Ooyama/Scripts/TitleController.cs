using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TitleController : MonoBehaviour
{
    [SerializeField] private string _titleBGM;
    [SerializeField] private Button[] _buttons;
    [SerializeField] private UnityEngine.EventSystems.EventSystem _eventSystem;
    private Button _returnButton;
    private Button _startMoveButton;

    private void Start()
    {
        AudioManager.Instance.PlayBGM(_titleBGM);
        //if(ResponceManager.Instance._isNetwork)VantanConnect.SystemReset();
        if (_eventSystem == null) _eventSystem = FindAnyObjectByType<UnityEngine.EventSystems.EventSystem>();
    }
    public void StartGame(string nextSceneName)
    {
        if(FadeManager.Instance.SceneChangeStart(nextSceneName))
        {
            StopInput();
        }
    }
    public void OpenTab(Button returnTarget)
    {
        foreach (var button in _buttons)
        {
            button.gameObject.SetActive(false);
        }
        _returnButton = returnTarget;
    }
    public void CloseTab()
    {
        foreach (var button in _buttons)
        {
            button?.gameObject.SetActive(true);
        }
        SetTarget(_returnButton);
        //_eventSystem.SetSelectedGameObject(_returnButton.gameObject);
    }
    public void SetTarget(Button TargetButton)
    {
        _startMoveButton = TargetButton;
        _eventSystem.SetSelectedGameObject(TargetButton.gameObject);
    }
    public void SetTarget()
    {
        _eventSystem.SetSelectedGameObject(_startMoveButton?.gameObject);
    }
    public void StopInput()
    {
        _eventSystem.SetSelectedGameObject(null);
        AudioManager.Instance.PlaySE("ButtonClick");
    }
}
