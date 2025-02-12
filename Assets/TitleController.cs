using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TitleController : MonoBehaviour
{
    [SerializeField] private string _titleBGM;
    [SerializeField] private Button[] _buttons;
    [SerializeField] private EventSystem _eventSystem;
    private Button _returnButton;

    private void Start()
    {
        AudioManager.Instance.PlayBGM(_titleBGM);
        if (_eventSystem == null) _eventSystem = FindAnyObjectByType<EventSystem>();
    }
    public static void StartGame(string nextSceneName)
    {
        FadeManager.Instance.SceneChangeStart(nextSceneName);
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
            button.gameObject.SetActive(true);
        }
        _eventSystem.SetSelectedGameObject(_returnButton.gameObject);
    }
    public void SetTarget(Button TargetButton)
    {
        _eventSystem.SetSelectedGameObject(TargetButton.gameObject);
    }
    public void StopInput()
    {
        _eventSystem.SetSelectedGameObject(null);
    }
}
