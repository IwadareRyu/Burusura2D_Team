using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CanvasController : MonoBehaviour
{
    [SerializeField] private List<Canvas> _canvasList;
    [SerializeField] bool _loop = true;
    private int _targetIndex = 0;
    private PlayerInput _input;
    private float _horizontalinput;
    private void OnEnable()
    {
        _input = new();
        var moveinput = _input.Player;
        moveinput.Move.performed += ct => _horizontalinput = ct.ReadValue<Vector2>().x;

        _input.Enable();
    }
    private void OnDisable()
    {
        _input.Disable();
    }
    private void Update()
    {
        if (_horizontalinput > 0f && _input.Player.Move.WasPerformedThisFrame() && _loop)
        {
            MoveRight();
        }
        else if (_horizontalinput > 0f && _input.Player.Move.WasPerformedThisFrame() && !_loop)
        {
            MoveRightEx();
        }
        else if (_horizontalinput < 0f && _input.Player.Move.WasPerformedThisFrame() && _loop)
        {
            MoveLeft();
        }
        else if (_horizontalinput < 0f && _input.Player.Move.WasPerformedThisFrame() && !_loop)
        {
            MoveLeftEx();
        }
        else if (_input.Player.Jump.WasPerformedThisFrame())
        {
            gameObject.SetActive(false);
        }
    }
    public void MoveLeft()
    {
        AudioManager.Instance.PlaySE("ButtonClick");

        _targetIndex--;

        if (_targetIndex < 0) _targetIndex = _canvasList.Count - 1;

        UpdateUI(_targetIndex);

    }
    public void MoveLeftEx()
    {
        if (_targetIndex > 0)
        {
            AudioManager.Instance.PlaySE("ButtonClick");
            _targetIndex--;
            UpdateUI(_targetIndex);
        }
    }
    public void MoveRight()
    {
        AudioManager.Instance.PlaySE("ButtonClick");

        _targetIndex++;

        _targetIndex = _targetIndex % _canvasList.Count;

        UpdateUI(_targetIndex);
    }
    public void MoveRightEx()
    {
        if (_targetIndex < _canvasList.Count - 1)
        {
            AudioManager.Instance.PlaySE("ButtonClick");
            _targetIndex++;
            UpdateUI(_targetIndex);
        }
    }
    private void UpdateUI(int DisplayCharactorID)
    {
        for (int i = 1; i < _canvasList.Count; i++)
        {
            _canvasList[i].gameObject.SetActive(false);
        }

        for (int i = 0; i <= DisplayCharactorID; i++)
        {
            _canvasList[i].gameObject.SetActive(true);
        }
    }
}
