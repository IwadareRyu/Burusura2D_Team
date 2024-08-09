using UnityEngine;

public class DangerousDisplayEnemy : MonoBehaviour
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
    bool _isDengerous = true;
    // Start is called before the first frame update
    void Start()
    {
        _currentMatualTime = _defaultMatualTime;
        _dangerousSpriteRenderer = GetComponent<SpriteRenderer>();
        _dangerousSpriteRenderer.sprite = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isDengerous)
        {
            _currentDisplayTime += Time.deltaTime;
            if (_currentDisplayTime > _displayTime)
            {
                Reset();
            }
        }
    }

    public void DengerousStart(float displayTime)
    {
        if (_isDengerous) return;
        _displayTime = displayTime;
        _dangerousSpriteRenderer.sprite = _dangerousSprite;
        _dangerousSpriteRenderer.color = _lightUpColor;
        _currentColor = _lightDownColor;
        InvokeRepeating("RepeatLight", 0f, _currentMatualTime);
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
        _dangerousSpriteRenderer.sprite = null;
    }
}
