using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BattleUISlider : MonoBehaviour
{
    [SerializeField] Slider _playerHpSlider;
    [SerializeField] Slider _enemyHpSlider;
    [SerializeField] Slider _specialGuageSlider;
    [SerializeField] Image _specialGuage;
    [SerializeField] Color _upGuageColor = Color.white;
    [SerializeField] Color _downGuageColor = Color.black;
    [SerializeField] Color _defaultGuageColor = Color.yellow;
    [SerializeField] float _hpMoveTime = 1f;
    [SerializeField] float _shakeTime = 2f;
    [SerializeField] float _shakePower = 5f;
    float _currentShakeTime;
    Tweener _playerHPMoveTween;
    Tweener _enemyHPMoveTween;
    Tweener _specialGuageMoveTween;
    Tweener _playerHPShake;
    Tweener _enemyHPShake;
    Tweener _specialGuageColorChangeTween;

    public static BattleUISlider Instance;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayerHPSlider(float currentHP, float maxHP)
    {
        if (_playerHPMoveTween != null && _playerHPMoveTween.IsActive()) _playerHpSlider.DOComplete();
        if (_playerHPShake != null && _playerHPShake.IsActive()) _playerHpSlider.transform.DOComplete();
        var afterHP = currentHP / maxHP;
        /*if (_playerHpSlider.value > afterHP) */_playerHPMoveTween = _playerHpSlider.DOValue(afterHP, _hpMoveTime).SetLink(gameObject);
        _playerHPShake = _playerHpSlider.transform.DOShakePosition(_shakeTime, _shakePower).SetLink(gameObject);
    }

    public void EnemyHPSlider(float currentHP, float maxHP)
    {
        if (_enemyHPMoveTween != null && _enemyHPMoveTween.IsActive()) _enemyHpSlider.DOComplete();
        if (_enemyHPShake != null && _enemyHPShake.IsActive()) _enemyHpSlider.transform.DOComplete();
        var afterHP = currentHP / maxHP;
        /*if(_enemyHpSlider.value > afterHP)*/_enemyHPShake = _enemyHpSlider.transform.DOShakePosition(_shakeTime, _shakePower).SetLink(gameObject);
        _enemyHPMoveTween = _enemyHpSlider.DOValue(afterHP, _hpMoveTime).SetLink(gameObject);
    }

    public void SpecialGuageSlider(float value)
    {
        if (_specialGuageMoveTween != null && _specialGuageMoveTween.IsActive()) _specialGuageSlider.DOComplete();
        if (_specialGuageColorChangeTween != null && _specialGuageColorChangeTween.IsActive()) _specialGuage.DOComplete();
        _specialGuage.color = value < _specialGuageSlider.value ? _downGuageColor : _upGuageColor;
        _specialGuageColorChangeTween = _specialGuage.DOColor(_defaultGuageColor, _hpMoveTime).SetEase(Ease.InQuad).SetLink(gameObject);
        _specialGuageMoveTween = _specialGuageSlider.DOValue(value, _hpMoveTime).SetLink(gameObject);
    }
}
