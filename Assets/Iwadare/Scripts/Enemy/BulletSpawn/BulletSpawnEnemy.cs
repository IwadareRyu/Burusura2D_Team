using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[RequireComponent(typeof(WaveSpawnEnemy),typeof(SpawnShotLine))]


public class BulletSpawnEnemy : MonoBehaviour,PauseTimeInterface
{
    [Header("弾の設定")]
    [Tooltip("弾のスポーン設定"), Header("弾のスポーン設定")]
    [SerializeField] SpawnCountType _spawnCountType = SpawnCountType.Loop;
    
    [Tooltip("一度に出す弾のスポーン回数"), Header("一度に出す弾のスポーン回数")]
    [SerializeField]int _oneShotSpawnCount = 1;
    
    [Tooltip("一度に出す弾ごとのスポーン感覚"), Header("一度に出す弾ごとのスポーン感覚")]
    [SerializeField] float _oneShotSpawnCoolTime = 0.0f;

    [Tooltip("BulletMoveに渡す変数一覧"), Header("BulletMoveに渡す変数一覧")]
    [SerializeField] SpawnBulletMoveStruct _spawnBulletMoveStruct;
    public SpawnBulletMoveStruct SpawnBulletMoveStruct => _spawnBulletMoveStruct;

    [Tooltip("弾のスポーン方法の設定"),Header("弾のスポーン方法の設定")]
    [SerializeField] BulletSpawnType _bulletSpawnType;
    
    [Tooltip("弾の速さ"), Header("弾の速さ")]
    [SerializeField] float _defaultBulletSpeed = 3f;
    public float DefaultBulletSpeed => _defaultBulletSpeed;

    [Tooltip("弾のActiveTime"), Header("弾のActiveTime")]
    [SerializeField] float _bulletActiveTime = 5f;
    public float BulletActiveTime => _bulletActiveTime;
    
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
    [SerializeField] DangerousDisplayEnemy _dangerousDisplayEnemy;
    bool _isBulletSpawn = true;

    [Header("危険信号設定")]
    [SerializeField] bool _isDangerousDisplay;
    [Tooltip("弾が生成される何秒前に危険信号を出すか"),Header("弾が生成される何秒前に危険信号を出すか")]
    [SerializeField] float _dangerousSpawnBeforeTime = 1f;
    float _dangerousTime;
    bool _isDangerous;


    [Tooltip("Bulletパターン一覧"), Header("Bulletパターン一覧")]
    [SerializeField] BulletPatterns _bulletPatterns;

    SpawnShotLine _spawnShotLine;
    [SerializeField] bool _isActiveShotLine = false;

    [Tooltip("手動で弾を動かす変数"), Header("手動で弾を動かす変数")]
    [SerializeField] bool _isManualMove = false;
    public bool IsManualMove => _isManualMove;

    float _timeScale = 1f;

    public bool _isAudio = true;
    IEnumerator _spawnCoroutine;
    [NonSerialized]public string _attackName = "Throw";
    
    [NonSerialized]public string _strongAttackName = "Throw";

    [NonSerialized]public List<MoveBulletEnemy> _moveBulletList = new List<MoveBulletEnemy>();
    private void Start()
    {
        _bulletPatterns._waveSpawnEnemy = GetComponent<WaveSpawnEnemy>();
        _spawnShotLine = GetComponent<SpawnShotLine>();
        _dangerousTime = _spawnCoolTime - _dangerousSpawnBeforeTime;
        _bulletPatterns._waveSpawnEnemy.Init(this);
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

    private void Update()
    {
        if (_spawnCountType != SpawnCountType.Any && _isBulletSpawn)
        {
            _currentCoolTime += Time.deltaTime * _timeScale;

            // DangerousSignUpdate
            DangerousSignStart(_currentCoolTime, _dangerousTime);

            // BulletSpawn
            if (_currentCoolTime > _spawnCoolTime)
            {
                _isBulletSpawn = false;
                _spawnCoroutine = BulletSpawn();
                StartCoroutine(_spawnCoroutine);
            }
        }

        if(_spawnShotLine && _spawnShotLine.RayUpdate())
        {
            if (_spawnBulletMoveStruct._bulletMoveType != BulletMoveType.NewDelayFastLazer)
            {
                _spawnShotLine.ResetShotLine();
            }
            else
            {
                _spawnShotLine.ShotBullet();
            }
            MoveBullet();
        }
    }

    void DangerousSignStart(float currentCoolTime,float dangerousTime)
    {
        if (_isDangerousDisplay && !_isDangerous && currentCoolTime > dangerousTime)
        {
            _isDangerous = true;
            _dangerousDisplayEnemy.DangerousStart(_dangerousSpawnBeforeTime);
        }
    }

    public IEnumerator BulletSpawnDangerous(float _time)
    {
        DangerousSign(_time);
        yield return WaitforSecondsCashe.Wait(_time);
        StartCoroutine(BulletSpawn());
    }

    public void DangerousSign(float dangerousSpawnBeforeTime = 1f)
    {
        _dangerousDisplayEnemy.DangerousStart(dangerousSpawnBeforeTime);
    }

    /// <summary>弾スポーンの要素を決めるメソッド</summary>
    public IEnumerator BulletSpawn()
    {
        for (var i = 0; i < _oneShotSpawnCount; i++)
        {
            yield return StartCoroutine(SetSpawnType());
            yield return new WaitForSeconds(_oneShotSpawnCoolTime);
        }
        _currentCoolTime = 0f;
        _isBulletSpawn = true;
        if (_spawnCountType == SpawnCountType.OneShot) _isBulletSpawn = false;
        if (_isDangerous) _isDangerous = false;
        _spawnCoroutine = null;
        yield return null;
    }

    /// <summary>Stateによってスポーン方法を変えるメソッド</summary>
    /// <returns></returns>
    IEnumerator SetSpawnType()
    {
        switch (_bulletSpawnType)
        {
            case BulletSpawnType.ForwardOnceSpawn:
                _bulletPatterns._forwardSpawn.Spawn(this);
                break;
            case BulletSpawnType.CircleSpawn:
                _bulletPatterns._circleSpawn.Spawn(this);
                break;
            case BulletSpawnType.DelayCircleSpawn:
                StartCoroutine(_bulletPatterns._circleSpawn.DelaySpawn(this));
                break;
            case BulletSpawnType.WaveSpawn:
                StartCoroutine(_bulletPatterns._waveSpawnEnemy.WaveSpawn(this, _bulletPatterns._circleSpawn));
                break;
            case BulletSpawnType.WaitWaveSpawn:
                yield return StartCoroutine(_bulletPatterns._waveSpawnEnemy.WaveSpawn(this, _bulletPatterns._circleSpawn));
                break;
            case BulletSpawnType.ForwardAfterSlowSpawn:
                _bulletPatterns._forwardAfterSlowSpawn.Spawn(this);
                break;
            case BulletSpawnType.ElipseSpawn:
                _bulletPatterns._ellipseSpawn.Spawn(this);
                break;
        }
        yield return null;
    }


    /// <summary>弾をスポーンさせるメソッド</summary>
    /// <param name="rota">回転</param>
    public void InitBullet(float rota,float bulletSpeed,float activeTime = 5f)
    {
        //Debug.Log("スポーンするぞ！");
        var bullet = _bulletPool.GetPool();
        bullet.transform.position = new Vector2(transform.position.x, transform.position.y);
        if(_spawnBulletMoveStruct._isRota) bullet.transform.Rotate(0, 0, rota);
        /// Bulletの属性をBulletMoveScriptsに入れる。
        var bulletScripts = bullet.GetComponent<MoveBulletEnemy>();
        bulletScripts.BulletMoveStart(this,bulletSpeed,rota,activeTime,_isAudio);
        if (!_isManualMove && _isActiveShotLine) SetRay(rota);
    }

    public void SetRay(float direction)
    {
        if(_spawnShotLine)
        {
            _spawnShotLine.SetRayStart(direction);
        }
    }

    public void MoveBullet()
    {
        if (_spawnBulletMoveStruct._bulletMoveType != BulletMoveType.DelayFastLazer)
        {
            AttackAudio();
        }
        if (_moveBulletList.Count != 0)
        {
            foreach(var bullet in _moveBulletList)
            {
                bullet._isAttackTime = true;
            }
        }
    }

    public void ResetBullet()
    {
        while(_moveBulletList.Count != 0)
        {
            _moveBulletList[0].CancelBullet();
        }
        _spawnShotLine.ResetShotLine();
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

    public void AttackAudio()
    {
        if (_isAudio)
        {
            AudioManager.Instance.PlaySE(_attackName);
        }
    }
}