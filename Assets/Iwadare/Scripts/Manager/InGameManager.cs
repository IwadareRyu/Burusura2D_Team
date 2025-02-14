using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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
    [SerializeField] string _NextsceneName = "DataraBossStage";
    [SerializeField] AudioClip _gameClearClip;
    [SerializeField] AudioClip _gameOverClip;
    [SerializeField] int _playerRemain = 3;
    [NonSerialized] public int _currentPlayerRemain;
    [Header("Bomb系")]
    [SerializeField] ParticleSystem _bombParticle;
    BombScripts _bombScripts;
    [SerializeField] SpriteRenderer _bombCircle;
    [SerializeField] string _bombAudioClipName;
    Color _bombCircleColor;
    Vector3 _bombRadius;
    [SerializeField] float _maxRadius = 10f;
    [SerializeField] float _bombTime = 5f;
    bool _isBomb = false;

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
        _currentPlayerRemain = _playerRemain;
        PlayerRemain(_currentPlayerRemain);
        _bombScripts = _bombParticle.GetComponent<BombScripts>();
        _bombCircleColor = _bombCircle.color;
        _bombRadius = _bombParticle.transform.localScale;
        _bombParticle.gameObject.SetActive(false);
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
        FadeManager.Instance.SceneChangeStart(_NextsceneName);
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
        FadeManager.Instance.SceneChangeStart(_NextsceneName);
    }

    public void AddRemain(int plusRemain)
    {
        _currentPlayerRemain += plusRemain;
        PlayerRemain(_currentPlayerRemain);
    }

    public void BombSystem()
    {
        StartCoroutine(BombSystemCoroutine());
    }

    public IEnumerator BombSystemCoroutine()
    {
        if(_isBomb) yield break;
        _isBomb = true;
        _bombParticle.gameObject.SetActive(true);
        _bombCircle.color = _bombCircleColor;
        var pos = Camera.main.transform.position;
        pos.z = 0f;
        _bombParticle.transform.position = pos;
        _bombParticle.transform.localScale = _bombRadius;
        _bombParticle.Play();
        _bombScripts.StartBomb();
        AudioManager.Instance.PlaySE(_bombAudioClipName);
        yield return _bombParticle.transform.DOScale(_bombRadius * _maxRadius, 5f).SetLink(_bombCircle.gameObject).WaitForCompletion();
        _bombScripts.EndBomb();
        _bombParticle.Stop();
        yield return _bombCircle.DOFade(0, 1f).SetLink(_bombCircle.gameObject).WaitForCompletion();
        _bombParticle.gameObject.SetActive(false);
        _isBomb = false;
        yield return null;
    }
}
