using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(WaveSpawnEnemy))]
public class BulletSpawnEnemy : MonoBehaviour
{
    [Header("弾の設定")]
    [Tooltip("弾の動きの設定"),Header("弾の動きの設定")]
    [SerializeField] BulletMoveType _bulletMoveType;
    [Tooltip("弾のスポーン方法の設定"),Header("弾のスポーン方法の設定")]
    [SerializeField] BulletSpawnType _bulletSpawnType;
    [Tooltip("弾を攻撃したときの起こることの設定"), Header("弾を攻撃したときの起こることの設定")]
    [SerializeField] BulletBreakType _bulletBreakType;
    [Tooltip("弾の速さ"), Header("弾の速さ")]
    [SerializeField] float _bulletSpeed = 3f;
    [Tooltip("毎秒弾の回る角度"), Header("毎秒弾の回る角度")]
    [SerializeField] float _bulletRotation = 1f;
    [Tooltip("弾と弾の間隔"), Header("弾と弾の間隔")]
    [SerializeField] float _bulletRange = 30f;
    public float BulletRange => _bulletRange;
    [Tooltip("弾のスポーンし始めの場所の角度(0は真上から)"), Header("弾のスポーンし始めの場所の角度(0は真上から)")]
    public float _bulletDistance = 0f;
    public float BulletDistance => _bulletDistance;

    [SerializeField] float _spawnCoolTime = 2f;
    float _currentCoolTime = 0f;
    [Header("弾のプール")]
    [SerializeField] BulletPoolActive _bulletPool;
    [SerializeField] bool _isAttack;
    bool _isBulletSpawn = true;

    ForwardSpawn _forwardSpawn = new();
    [SerializeField] CircleSpawn _circleSpawn = new();
    WaveSpawnEnemy _waveSpawnEnemy;

    private void Start()
    {
        _waveSpawnEnemy = GetComponent<WaveSpawnEnemy>();
    }

    private void Update()
    {
        if (_isAttack && _isBulletSpawn)
        {
            _currentCoolTime += Time.deltaTime;
            if (_currentCoolTime > _spawnCoolTime)
            {
                _isBulletSpawn = false;
                StartCoroutine(BulletSpawn());
            }
        }
    }

    IEnumerator BulletSpawn()
    {
        switch (_bulletSpawnType)
        {
            case BulletSpawnType.ForwardOnceSpawn:
                _forwardSpawn.Spawn(this);
                break;
            case BulletSpawnType.CircleSpawn:
                _circleSpawn.Spawn(this);
                break;
            case BulletSpawnType.DelayCircleSpawn:
                StartCoroutine(_circleSpawn.DelaySpawn(this));
                break;
            case BulletSpawnType.WaveSpawn:
                yield return StartCoroutine(_waveSpawnEnemy.WaveSpawn(this));
                break;
        }
        _currentCoolTime = 0f;
        _isBulletSpawn = true;
        yield return null;
    }



    public void InitBullet(float rota)
    {
        Debug.Log("スポーンするぞ！");
        var bullet = _bulletPool.GetBullet();
        bullet.transform.position = new Vector2(transform.position.x, transform.position.y);
        bullet.transform.Rotate(0, 0, rota);
        /// Bulletの属性をBulletMoveScriptsに入れる。
        var bulletScripts = bullet.GetComponent<MoveBulletEnemy>();
        bulletScripts.BulletMoveStart(_bulletMoveType, _bulletBreakType, _bulletSpeed, _bulletRotation);
    }


}
