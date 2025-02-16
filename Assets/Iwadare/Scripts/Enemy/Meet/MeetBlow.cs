using Cysharp.Threading.Tasks.Triggers;
using DG.Tweening;
using GameLoopTest;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeetBlow : MonoBehaviour
{
    bool _isHit = false;
    [SerializeField] SpriteRenderer _meetRenderer;
    [SerializeField] float _blowPowerX = 3f;
    [SerializeField] float _blowPowerY = 10f;
    [SerializeField] bool _isExplosion = false;
    [SerializeField]float _rotateRadius = 20f;
    [SerializeField] ExplosionMeet _explosion;
    [SerializeField] ParticleSystem _fireParticle;
    int _damage = 5;
    public void Init(float rotate, int damage)
    {
        _damage = damage;
        _rotateRadius = rotate;
    }

    void Start()
    {
        _isHit = false;
        if (_isExplosion) _fireParticle.Play();
    }

    private void FixedUpdate()
    {
        transform.Rotate(0, 0, _rotateRadius);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            if (_isExplosion)
            {
                Explosion();
            }
            else
            {
                FadeMeet();
            }
        }
        if (_isHit) return;
        if(collision.TryGetComponent<PlayerController>(out var player))
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

    private void Explosion()
    {
        var explosion = Instantiate(_explosion, transform.position, Quaternion.identity).GetComponent<ExplosionMeet>();
        explosion.Init(_damage * 2);
        _fireParticle.Stop();
        Destroy(gameObject);
    }

    private void FadeMeet()
    {
        _meetRenderer.DOFade(0, 1f).OnComplete(() => Destroy(gameObject)).SetLink(gameObject);
    }
}
