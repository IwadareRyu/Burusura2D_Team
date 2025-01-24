using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameManager : MonoBehaviour
{
    public static InGameManager Instance;
    [NonSerialized] public PlayerSpecialGuage _playerSpecialGuage;
    [SerializeField] public Text _deathCountText;
    [SerializeField] Canvas _gameOverCanvas;
    Animator _gameOverAnimator;
    [SerializeField] Canvas _gameClearCanvas;
    Animator _gameClearAnimator;
    int _deathCount;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        _playerSpecialGuage = GetComponent<PlayerSpecialGuage>();
        _deathCount = 0;
    }

    private void Start()
    {
        _gameOverCanvas.gameObject.SetActive(true);
        _gameClearCanvas.gameObject.SetActive(true);
        _gameOverAnimator = _gameOverCanvas.GetComponent<Animator>();
        _gameClearAnimator = _gameClearCanvas.GetComponent<Animator>();
        _gameOverCanvas.gameObject.SetActive(false);
        _gameClearCanvas.gameObject.SetActive(false);
    }

    public void PlayerRemain(int remain)
    {
        _deathCountText.text = $"残機数: {remain}";
    }

    public void GameOver()
    {
        if (!_gameClearCanvas.gameObject.activeSelf)
        {
            TimeScaleManager.Instance.TimeScaleChange(0);
            _gameOverCanvas.gameObject.SetActive(true);
        }
    }

    public void GameCrear()
    {
        if(!_gameOverCanvas.enabled)
        {
            TimeScaleManager.Instance.TimeScaleChange(0);
            _gameClearCanvas.gameObject.SetActive(true);
        }
    }
}
