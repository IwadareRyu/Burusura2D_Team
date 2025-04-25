using UnityEngine;
using UnityEngine.InputSystem;

public class CursorSwitcher : MonoBehaviour
{
    public PlayerInput playerInput;
    private string _lastControlScheme = "";
    [SerializeField] private UnityEngine.EventSystems.EventSystem _eventSystem;
    [SerializeField] private TitleController _titleController;

    private void Start()
    {
        playerInput = new();
        if (_titleController == null) _titleController = FindAnyObjectByType<TitleController>();
        //_lastControlScheme = playerInput.controlSchemes[0].name;
    }
    void Update()
    {
        if (Gamepad.current != null && Gamepad.current.wasUpdatedThisFrame)
        {
            SetCursorVisible(false);
            //_lastControlScheme = "Gamepad";
        }
        else if (Mouse.current != null && Mouse.current.wasUpdatedThisFrame)
        {
            SetCursorVisible(true);
            //_lastControlScheme = "Mouse";
        }
    }

    void SetCursorVisible(bool visible)
    {
        _titleController?.SetTarget();
        Cursor.visible = visible;
        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
