using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class CursorSwitcher : MonoBehaviour
{
    public GameObject gamepadDefaultSelect; // ゲームパッド用の初期選択ボタン
    //[SerializeField] private UnityEngine.EventSystems.EventSystem _eventSystem;
    [SerializeField] private TitleController _titleController;
    private void OnEnable()
    {
        InputSystem.onEvent += OnInputEvent;
    }

    private void OnDisable()
    {
        InputSystem.onEvent -= OnInputEvent;
    }

    private void OnInputEvent(InputEventPtr eventPtr, InputDevice device)
    {
        if (!eventPtr.IsA<StateEvent>() && !eventPtr.IsA<DeltaStateEvent>())
            return;

        if (device is Pointer || device is Mouse)
        {
            if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null)
            {
                EventSystem.current.SetSelectedGameObject(null);
                Debug.Log("Mouse input detected: Deselected UI");
            }
        }
        else if (device is Gamepad)
        {
            // ゲームパッド入力：指定のUIを選択状態に
            if (EventSystem.current != null)
            {
                _titleController.CloseTab();
                Debug.Log("Gamepad input detected: Selected UI");
            }
        }
    }
    private void Start()
    {
        if (_titleController == null) _titleController = FindAnyObjectByType<TitleController>();
    }
    //private void OnEnable()
    //{
    //    InputSystem.onDeviceChange += OnDeviceChange;
    //}

    //private void OnDisable()
    //{
    //    InputSystem.onDeviceChange -= OnDeviceChange;
    //}

    //private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    //{
    //    if (device is Gamepad)
    //    {
    //        switch (change)
    //        {
    //            case InputDeviceChange.Added:
    //                Debug.Log($"Gamepad connected: {device.displayName}");
    //                OnGamepadConnected(device as Gamepad);                
    //                break;

    //            case InputDeviceChange.Removed:
    //                Debug.Log($"Gamepad disconnected: {device.displayName}");
    //                OnGamepadDisconnected(device as Gamepad);                  
    //                break;

    //            default:
    //                break;
    //        }
    //    }
    //}

    //private void OnGamepadConnected(Gamepad gamepad)
    //{
    //    Debug.Log($"Gamepad {gamepad.displayName} is ready to use.");
    //    SetCursorVisible(false);
    //}

    //private void OnGamepadDisconnected(Gamepad gamepad)
    //{
    //    Debug.Log($"Gamepad {gamepad.displayName} has been disconnected.");
    //    SetCursorVisible(true);
    //}
    //void SetCursorVisible(bool visible)
    //{
    //    Cursor.visible = visible;
    //    if (Cursor.visible)
    //    {
    //        Cursor.lockState = CursorLockMode.None;
    //    }
    //    else
    //    {
    //        Cursor.lockState = CursorLockMode.Locked;
    //        _titleController.SetTarget();
    //    }
    //}
}
