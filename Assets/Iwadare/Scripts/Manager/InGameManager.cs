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
    [SerializeField] float _gameEndTime = 2f;
    [SerializeField] float _sceneChangeWaitTime = 3f;
    [SerializeField] AudioClip _gameClearClip;
    [SerializeField] AudioClip _gameOverClip;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        _playerSpecialGuage = GetComponent<PlayerSpecialGuage>();
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

    public IEnumerator GameOver()
    {
        AudioManager.Instance.StopBGM();
        yield return new WaitForSecondsRealtime(_gameEndTime);
        if (!_gameClearCanvas.gameObject.activeSelf)
        {
            TimeScaleManager.Instance.TimeScaleChange(0f);
            AudioManager.Instance.PlaySE(_gameOverClip.name);
            _gameOverCanvas.gameObject.SetActive(true);
        }
        yield return new WaitForSecondsRealtime(_sceneChangeWaitTime);
        TimeScaleManager.Instance.TimeScaleChange(1f);
    }

    public IEnumerator GameCrear()
    {
        AudioManager.Instance.StopBGM();
        yield return new WaitForSecondsRealtime(_gameEndTime);
        if (!_gameOverCanvas.gameObject.activeSelf)
        {
            TimeScaleManager.Instance.TimeScaleChange(0f);
            AudioManager.Instance.PlaySE(_gameClearClip.name);
            _gameClearCanvas.gameObject.SetActive(true);
        }
        yield return new WaitForSecondsRealtime(_sceneChangeWaitTime);
        TimeScaleManager.Instance.TimeScaleChange(1f);
    }
}
