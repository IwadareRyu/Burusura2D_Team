using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScripts : MonoBehaviour
{
    bool _isBomb;
    EnemyBase _enemy;
    [SerializeField] float _disDamageTime = 0.5f;
    float _currentDamageTime;
    [SerializeField] int _damage = 2;
    [SerializeField] BulletPoolActive _reflectPool;
    //[SerializeField] string _reflectAudio;
    
    public void StartBomb()
    {
        _isBomb = true;
        _currentDamageTime = 0f;
    }

    public void EndBomb()
    {
        _isBomb = false;
        if(_enemy) _enemy = null;
    }

    public void Update()
    {
        _currentDamageTime += Time.deltaTime;
        if(_currentDamageTime > _disDamageTime)
        {
            if(_enemy) _enemy.AddDamage(_damage,HitEffect.Reflect);
            _currentDamageTime = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(_isBomb)
        {
            if(collision.gameObject.tag == "Enemy")
            {
                var particle = _reflectPool.GetPool().GetComponent<ParticleDestroy>();
                particle.transform.position = collision.transform.position;
                particle.Init();
                //AudioManager.Instance.PlaySE(_reflectAudio);
                Destroy(collision.gameObject);
            }
            else if (collision.TryGetComponent<MoveBulletEnemy>(out var bullet))
            {
                bullet.BombReset();
                InGameManager.Instance._playerSpecialGuage.AddGuage(InGameManager.Instance._playerSpecialGuage.BreakAddGuage);
            }
            else if (collision.TryGetComponent<EnemyBase>(out var enemy))
            {
                _enemy = enemy;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_isBomb)
        {
            if (collision.TryGetComponent<EnemyBase>(out var enemy))
            {
                _enemy = null;
            }
        }
    }
}
