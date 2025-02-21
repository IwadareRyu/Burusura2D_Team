using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DangerousDisplayEnemy : MonoBehaviour, PauseTimeInterface
{

    [SerializeField] Sprite _dangerousSprite;
    [SerializeField] BulletPoolActive _dangerousFramePool;
    SpriteRenderer _dangerousSpriteRenderer;
    SpriteRenderer _dangerousFrameSprite;
    [SerializeField] Image _dangerousImage;
    [SerializeField] Image _dangerousFrameImage;
    Vector3 _tmpDangerousScale;
    float _displayTime = 1f;
    //float _currentDisplayTime;
    Color _currentColor;

    [Header("危険信号の点滅の色")]
    [SerializeField] Color _lightUpColor = Color.white;
    [SerializeField] Color _lightDownColor = Color.gray;
    [SerializeField] float _defaultMatualTime = 0.2f;
    float _currentMatualTime;
    bool _isDengerous = false;
    bool _isDangerousMove = false;
    [SerializeField] float _rotationDangerousRange = 1800f;
    Sequence _tmpSequence;
    float _timeScale = 1f;
    [SerializeField] bool _isUseImage;
    string _powerUp = "PowerUp";

    // Start is called before the first frame update
    void Start()
    {
        _currentMatualTime = _defaultMatualTime;
        _dangerousSpriteRenderer = GetComponent<SpriteRenderer>();
        if (_isUseImage)
        {
            _dangerousImage.enabled = false;
            _dangerousFrameImage.enabled = false;
            _tmpDangerousScale = _dangerousImage.transform.localScale;
        }
        else
        {
            _dangerousSpriteRenderer.sprite = null;
            _tmpDangerousScale = _dangerousSpriteRenderer.transform.localScale;
        }
    }

    private void OnEnable()
    {
        TimeScaleManager.ChangeTimeScaleAction += TimeScaleChange;
        TimeScaleManager.StartPauseAction += StartPause;
        TimeScaleManager.EndPauseAction += EndPause;
    }

    private void OnDisable()
    {
        TimeScaleManager.ChangeTimeScaleAction -= TimeScaleChange;
        TimeScaleManager.StartPauseAction -= StartPause;
        TimeScaleManager.EndPauseAction -= EndPause;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isDengerous)
        {
            //// 点滅
            //_currentMatualTime += Time.deltaTime * _timeScale;
            //if (_currentMatualTime > _defaultMatualTime)
            //{
            //    RepeatLight();
            //    _currentMatualTime = 0f;
            //}
            //// 警告出てる時間
            //_currentDisplayTime += Time.deltaTime * _timeScale;
            //if (_currentDisplayTime > _displayTime)
            //{
            //    ActionReset();
            //}

            if (!_isDangerousMove)
            {
                _isDangerousMove = true;
                StartCoroutine(DangerousCoroutine());
            }
        }
    }

    public void DangerousStart(float displayTime)
    {
        if (_isDengerous) return;
        AudioManager.Instance.PlaySE(_powerUp);
        _displayTime = displayTime;
        _tmpSequence = DOTween.Sequence()
            .Pause()
            .SetAutoKill(false)
            .SetLink(gameObject);
        if (_isUseImage)
        {
            _dangerousImage.enabled = true;
            _dangerousFrameImage.enabled = true;
            _dangerousImage.sprite = _dangerousSprite;
            _dangerousImage.color = _lightUpColor;
        }
        else
        {
            _dangerousSpriteRenderer.sprite = _dangerousSprite;
            _dangerousSpriteRenderer.color = _lightUpColor;
            _dangerousFrameSprite = _dangerousFramePool.GetPool().GetComponent<SpriteRenderer>();
            _dangerousFrameSprite.transform.position = _dangerousSpriteRenderer.transform.position;
        }
        _currentColor = _lightDownColor;
        _isDengerous = true;
    }

    public void RepeatLight()
    {
        if (_isUseImage)
        {
            _dangerousImage.color = _currentColor;
        }
        else
        {
            _dangerousSpriteRenderer.color = _currentColor;
        }
        if (_currentColor == _lightUpColor) _currentColor = _lightDownColor;
        else _currentColor = _lightUpColor;
    }

    IEnumerator DangerousCoroutine()
    {
        if (_isUseImage)
        {
            _dangerousImage.transform.rotation = Quaternion.identity;
            _tmpSequence
                .Append(_dangerousImage.transform.DORotate(new Vector3(0, 0, 1) * _rotationDangerousRange, _displayTime, RotateMode.LocalAxisAdd)
                    .SetEase(Ease.OutQuart))
                .Insert(0, _dangerousImage.transform.DOScale(_tmpDangerousScale, _displayTime / 2))
                .Insert(0, _dangerousFrameImage.transform.DOScale(_tmpDangerousScale, _displayTime / 4));
            yield return _tmpSequence.Play().SetLink(gameObject).WaitForCompletion();
        }
        else
        {
            _dangerousSpriteRenderer.transform.rotation = Quaternion.identity;
            _dangerousSpriteRenderer.transform.localScale = Vector3.zero; _dangerousFrameSprite.transform.localScale = Vector3.zero;
            _tmpSequence
                .Append(
                    _dangerousSpriteRenderer.transform.DORotate(new Vector3(0, 0, 1) * _rotationDangerousRange, _displayTime,RotateMode.LocalAxisAdd)
                    .SetEase(Ease.OutQuart))
                .Insert(0, _dangerousSpriteRenderer.transform.DOScale(_tmpDangerousScale, _displayTime / 2))
                .Insert(0, _dangerousFrameSprite.transform.DOScale(_tmpDangerousScale, _displayTime / 4));

            yield return _tmpSequence.Play().WaitForCompletion();

        }
        yield return null;
        ActionReset();
    }


    private void ActionReset()
    {
        _isDengerous = false;
        _isDangerousMove = false;
        if (_tmpSequence != null && _tmpSequence.IsActive()) _tmpSequence.Kill();
        if (_isUseImage)
        {
            _dangerousImage.enabled = false;
            _dangerousFrameImage.enabled = false;
        }
        else
        {
            _dangerousSpriteRenderer.sprite = null;
            _dangerousFrameSprite.gameObject.SetActive(false);
        }
    }

    public void TimeScaleChange(float timeScale)
    {
        _timeScale = timeScale;
    }

    public void StartPause()
    {
        _timeScale = 0f;
    }

    public void EndPause()
    {
        _timeScale = 1f;
    }
}
