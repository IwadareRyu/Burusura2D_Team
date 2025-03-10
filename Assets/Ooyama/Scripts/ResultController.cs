﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ResultController : MonoBehaviour
{
    [SerializeField] List<string> _winCommentList;
    [SerializeField] List<string> _loseCommentList;
    [SerializeField] Text _targetText;
    [SerializeField] float _printCharSpeed = 0.1f;
    [SerializeField] char _nextLineCommand;
    [SerializeField] Image _nekomataImage;
    [SerializeField] Sprite _nekomataWinSprite;
    [SerializeField] Sprite _nekomataLoseSprite;
    private bool _canMoveScene = false;
    private InputAction _moveSceneAction;
    private void Awake()
    {
        _moveSceneAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/anyKey");
        _moveSceneAction.AddBinding("<Gamepad>/*");
        _moveSceneAction.AddBinding("<Mouse>/leftButton");
        _moveSceneAction.AddBinding("<Mouse>/rightButton");
        _moveSceneAction.performed += OnMoveSceneAction;
    }

    void OnEnable()
    {
        _moveSceneAction.Enable();
    }

    void OnDisable()
    {
        _moveSceneAction.performed -= OnMoveSceneAction;
        _moveSceneAction?.Disable();
    }
    void Start()
    {
        _canMoveScene = true;
        _targetText.text = "";
        ResultPrinter(GameStateManager.Instance._isWin);
    }
    private void OnMoveSceneAction(InputAction.CallbackContext context)
    {
        if (_canMoveScene)
        {
            if(FadeManager.Instance.SceneChangeStart("Title"))
            {
                _canMoveScene = false;
            }
        }
    }
    private void ResultPrinter(bool Win)
    {
        if (Win)
        {
            _nekomataImage.sprite = _nekomataWinSprite;
            StartCoroutine(Printer(_winCommentList[Random.Range(0, _winCommentList.Count)]));
        }
        else
        {
            _nekomataImage.sprite = _nekomataLoseSprite;
            StartCoroutine(Printer(_loseCommentList[Random.Range(0, _loseCommentList.Count)]));
        }
    }
    IEnumerator Printer(string PrintText)
    {
        for (int i = 0; i < PrintText.Length; i++)
        {
            if (PrintText[i] == '$')
            {
                _targetText.text += "\n";
            }
            else
            {
                _targetText.text += PrintText[i];
            }
            yield return WaitforSecondsCashe.Wait(_printCharSpeed);
        }
    }
}
