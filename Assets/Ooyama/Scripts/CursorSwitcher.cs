using UnityEngine;
using UnityEngine.InputSystem;

public class CursorSwitcher : MonoBehaviour
{
    public InputAction.CallbackContext _input;
    [SerializeField] private UnityEngine.EventSystems.EventSystem _eventSystem;
    [SerializeField] private TitleController _titleController;
    private void Start()
    {
        if (_titleController == null) _titleController = FindAnyObjectByType<TitleController>();
    }
    private void OnEnable()
    {
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    private void OnDisable()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (device is Gamepad)
        {
            switch (change)
            {
                case InputDeviceChange.Added:
                    Debug.Log($"Gamepad connected: {device.displayName}");
                    OnGamepadConnected(device as Gamepad);                
                    break;

                case InputDeviceChange.Removed:
                    Debug.Log($"Gamepad disconnected: {device.displayName}");
                    OnGamepadDisconnected(device as Gamepad);                  
                    break;

                default:
                    break;
            }
        }
    }

    private void OnGamepadConnected(Gamepad gamepad)
    {
        Debug.Log($"Gamepad {gamepad.displayName} is ready to use.");
        SetCursorVisible(false);
    }

    private void OnGamepadDisconnected(Gamepad gamepad)
    {
        Debug.Log($"Gamepad {gamepad.displayName} has been disconnected.");
        SetCursorVisible(true);
    }
    void SetCursorVisible(bool visible)
    {
        Cursor.visible = visible;
        if (Cursor.visible)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            _titleController.SetTarget();
        }
    }
}
