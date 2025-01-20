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
    [SerializeField] Canvas _gameCrearCanvas;
    int _deathCount;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        _playerSpecialGuage = GetComponent<PlayerSpecialGuage>();
        _deathCount = 0;
        _gameOverCanvas.enabled = false;
        _gameCrearCanvas.enabled = false;
    }

    public void PlayerRemain(int remain)
    {
        _deathCountText.text = $"残機数: {remain}";
    }

    public void GameOver()
    {
        if (!_gameCrearCanvas.enabled)
        {
            TimeScaleManager.Instance.TimeScaleChange(0);
            _gameOverCanvas.enabled = true;
        }
    }

    public void GameCrear()
    {
        if(!_gameOverCanvas.enabled)
        {
            TimeScaleManager.Instance.TimeScaleChange(0);
            _gameCrearCanvas.enabled = true;
        }
    }
}
