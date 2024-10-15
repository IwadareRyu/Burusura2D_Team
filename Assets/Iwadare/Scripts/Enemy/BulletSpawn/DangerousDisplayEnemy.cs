using UnityEngine;

public class DangerousDisplayEnemy : MonoBehaviour,PauseTimeInterface
{

    [SerializeField] Sprite _dangerousSprite;
    SpriteRenderer _dangerousSpriteRenderer;
    float _displayTime = 1f;
    float _currentDisplayTime;
    Color _currentColor;

    [Header("危険信号の点滅の色")]
    [SerializeField] Color _lightUpColor = Color.white;
    [SerializeField] Color _lightDownColor = Color.gray;
    [SerializeField] float _defaultMatualTime = 0.2f;
    float _currentMatualTime;
    bool _isDengerous = false;
    float _timeScale = 1f;
    // Start is called before the first frame update
    void Start()
    {
        _currentMatualTime = _defaultMatualTime;
        _dangerousSpriteRenderer = GetComponent<SpriteRenderer>();
        _dangerousSpriteRenderer.sprite = null;
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
            // 点滅
            _currentMatualTime += Time.deltaTime * _timeScale;
            if (_currentMatualTime > _defaultMatualTime)
            {
                RepeatLight();
                _currentMatualTime = 0f;
            }

            // 警告出てる時間
            _currentDisplayTime += Time.deltaTime * _timeScale;
            if (_currentDisplayTime > _displayTime)
            {
                Reset();
            }
        }
    }

    public void DangerousStart(float displayTime)
    {
        if (_isDengerous) return;
        _displayTime = displayTime;
        _dangerousSpriteRenderer.sprite = _dangerousSprite;
        _dangerousSpriteRenderer.color = _lightUpColor;
        _currentColor = _lightDownColor;
        _isDengerous = true;
    }

    public void RepeatLight()
    {
        _dangerousSpriteRenderer.color = _currentColor;
        if (_currentColor == _lightUpColor) _currentColor = _lightDownColor;
        else _currentColor = _lightUpColor;
    }


    private void Reset()
    {
        CancelInvoke("RepeatLight");
        _isDengerous = false;
        _currentDisplayTime = 0;
        _currentMatualTime = 0;
        _dangerousSpriteRenderer.sprite = null;
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
