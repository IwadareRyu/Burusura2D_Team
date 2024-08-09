using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(WaveSpawnEnemy))]
public class BulletSpawnEnemy : MonoBehaviour
{
    [Header("弾の設定")]
    [Tooltip("弾のスポーン回数の大まかな設定"), Header("弾のスポーン回数の大まかな設定")]
    [SerializeField] SpawnCountType _spawnCountType = SpawnCountType.Loop;
    [Tooltip("弾の動きの設定"),Header("弾の動きの設定")]
    [SerializeField] BulletMoveType _bulletMoveType;
    [Tooltip("弾のスポーン方法の設定"),Header("弾のスポーン方法の設定")]
    [SerializeField] BulletSpawnType _bulletSpawnType;
    [Tooltip("弾を攻撃したときの起こることの設定"), Header("弾を攻撃したときの起こることの設定")]
    [SerializeField] BulletBreakType _bulletBreakType;
    [Tooltip("弾の速さ"), Header("弾の速さ")]
    [SerializeField] float _defaultBulletSpeed = 3f;
    public float DefaultBulletSpeed => _defaultBulletSpeed;

    [Tooltip("弾のActiveTime"), Header("弾のActiveTime")]
    [SerializeField] float _bulletActiveTime = 5f;
    public float BulletActiveTime => _bulletActiveTime;

    [Tooltip("毎秒弾の回る角度"), Header("毎秒弾の回る角度")]
    [SerializeField] float _bulletRotation = 1f;
    [Tooltip("弾と弾の間隔"), Header("弾と弾の間隔")]
    [SerializeField] float _bulletRange = 30f;
    public float BulletRange => _bulletRange;
    [Tooltip("弾のスポーンし始めの場所の角度(0は真上から)"), Header("弾のスポーンし始めの場所の角度(0は真上から)")]
    public float _bulletDistance = 90f;
    public float BulletDistance => _bulletDistance;

    [SerializeField] float _spawnCoolTime = 2f;
    float _currentCoolTime = 0f;

    [Header("弾のプール")]
    [SerializeField] BulletPoolActive _bulletPool;
    [SerializeField] bool _isAttack;
    [SerializeField] DangerousDisplayEnemy _dengerousDisplayEnemy;
    bool _isBulletSpawn = true;

    [Header("危険信号設定")]
    [SerializeField] bool _isDangerousDisplay;
    [Tooltip("弾が生成される何秒前に危険信号を出すか"),Header("弾が生成される何秒前に危険信号を出すか")]
    [SerializeField] float _dangerousSpawnBeforeTime = 1f;
    float _dangerousTime;
    bool _isDangerous;

    ForwardSpawn _forwardSpawn = new();
    [SerializeField] CircleSpawn _circleSpawn = new();
    [SerializeField] ForwardAfterSlowSpawn _forwardAfterSlowSpawn = new();
    WaveSpawnEnemy _waveSpawnEnemy;


    private void Start()
    {
        _waveSpawnEnemy = GetComponent<WaveSpawnEnemy>();
        _dangerousTime = _spawnCoolTime - _dangerousSpawnBeforeTime;
        _waveSpawnEnemy.Init(this);
    }

    private void Update()
    {
        if (_isAttack && _isBulletSpawn)
        {
            _currentCoolTime += Time.deltaTime;

            // DangerousSign
            if(_isDangerousDisplay && !_isDangerous && _currentCoolTime > _dangerousTime)
            {
                _isDangerous = true;
                _dengerousDisplayEnemy.DengerousStart(_dangerousSpawnBeforeTime);
            }

            // BulletSpawn
            if (_currentCoolTime > _spawnCoolTime)
            {
                _isBulletSpawn = false;
                StartCoroutine(BulletSpawn());
            }
        }
    }

    /// <summary>弾スポーンの要素を決めるメソッド</summary>
    IEnumerator BulletSpawn()
    {
        yield return StartCoroutine(SetSpawnType());
        _currentCoolTime = 0f;
        _isBulletSpawn = true;
        if (_spawnCountType == SpawnCountType.OneShot) _isAttack = false;
        if (_isDangerous) _isDangerous = false;
        yield return null;
    }

    /// <summary>Stateによってスポーン方法を変えるメソッド</summary>
    /// <returns></returns>
    IEnumerator SetSpawnType()
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
                StartCoroutine(_waveSpawnEnemy.WaveSpawn(this, _circleSpawn));
                break;
            case BulletSpawnType.WaitWaveSpawn:
                yield return StartCoroutine(_waveSpawnEnemy.WaveSpawn(this, _circleSpawn));
                break;
            case BulletSpawnType.ForwardAfterSlowSpawn:
                _forwardAfterSlowSpawn.Spawn(this);
                break;
        }
        yield return null;
    }


    /// <summary>弾をスポーンさせるメソッド</summary>
    /// <param name="rota">回転</param>
    public void InitBullet(float rota,float bulletSpeed,float activeTime = 5f)
    {
        Debug.Log("スポーンするぞ！");
        var bullet = _bulletPool.GetBullet();
        bullet.transform.position = new Vector2(transform.position.x, transform.position.y);
        bullet.transform.Rotate(0, 0, rota);
        /// Bulletの属性をBulletMoveScriptsに入れる。
        var bulletScripts = bullet.GetComponent<MoveBulletEnemy>();
        bulletScripts.BulletMoveStart(_bulletMoveType, _bulletBreakType, bulletSpeed, _bulletRotation,activeTime);
    }


}
