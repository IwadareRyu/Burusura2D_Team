using DG.Tweening;
using UnityEngine;

public class ExplosionMeet : MonoBehaviour
{
    SpriteRenderer _explosionRenderer; 
    [SerializeField] float _maxRadius = 1f;
    [SerializeField] float _maxRadiusTime = 0.5f;
    [SerializeField] float _blowPowerX = 6f;
    [SerializeField] float _blowPowerY = 20f;
    float _currentTime = 0f;
    int _damage = 5;
    bool _isFade = false;
    bool _isHit = false;
    private void Start()
    {
        _explosionRenderer = GetComponent<SpriteRenderer>();
        _isFade = false;
        _isHit = false;
        gameObject.transform.localScale = Vector3.zero;
        gameObject.transform.DOScale(_maxRadius, _maxRadiusTime).SetLink(gameObject);
    }

    public void Init(int damage)
    {
        _damage = damage;
        AudioManager.Instance.PlaySE("Bakuhatu");
    }

    private void Update()
    {
        _currentTime += Time.deltaTime;
        if (!_isFade && _currentTime > _maxRadiusTime)
        {
            _isFade = true;
            FadeExplosion();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isFade || _isHit) return;
        if (collision.TryGetComponent<PlayerController>(out var player))
        {
            MeetAttack(player);
        }
    }

    private void MeetAttack(PlayerController player)
    {
        _isHit = true;
        player._playerState |= PlayerState.ImpactState;
        Debug.Log(player._playerState);
        var playerVelocity = player._playerRb;

        if (transform.position.x > player.transform.position.x)
        {
            playerVelocity.AddForce(Vector2.right * _blowPowerX + Vector2.up * _blowPowerY, ForceMode2D.Impulse);
        }
        else
        {
            playerVelocity.AddForce(Vector2.left * _blowPowerX + Vector2.up * _blowPowerY, ForceMode2D.Impulse);
        }
        player.AddDamage(_damage);
    }

    private void FadeExplosion()
    {
        _explosionRenderer.DOFade(0, 1f).OnComplete(() => Destroy(gameObject)).SetLink(gameObject);
    }
}
