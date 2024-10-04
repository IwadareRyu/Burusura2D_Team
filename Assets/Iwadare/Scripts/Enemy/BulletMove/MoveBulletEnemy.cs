using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveBulletEnemy : MonoBehaviour, HitStopInterface
{

    [Tooltip("弾の速さ")]
    float _maxBulletSpeed;
    float _currentBulletSpeed;

    [Tooltip("弾の回転")]
    float _maxBulletRota;
    float _currentBulletRota;
    [SerializeField] float _rotaTime = 0.1f;
    [SerializeField] float _maxRotaAngle = 90f;
    float _currentRotaAngle = 0f;

    [Tooltip("弾を撃つ方向")]
    Vector3 _currentDirection = Vector3.zero;
    public Vector3 CurrentDirection => _currentDirection;
    float _currentBulletAngle = 0f;


    [Tooltip("弾のアクティブ時間")]
    float _activeTime = 5f;
    public float ActiveTime => _activeTime;
    float _currentRotaTime = 0f;

    [Tooltip("弾の大きさ"), Header("弾の大きさ")]
    [SerializeField] float _bulletScale = 3f;
    public float BulletScale => _bulletScale;

    BulletBreakType _breakType;
    [NonSerialized] public bool _isAttackTime = false;
    [NonSerialized] public bool _isReset = false;

    [Tooltip("フェードの時間"), Header("フェードの時間")]
    [SerializeField] float _fadeTime = 1f;
    public float FadeTime => _fadeTime;
    float defaultBulleetAlpha = 1f;
    [NonSerialized] public bool _isFade = false;

    bool _isRay = false;
    public bool IsRay => _isRay;

    [SerializeField] SpriteRenderer _mySpriteRenderer;

    [SerializeField] ParticleSystem _hitParticle;
    public ParticleSystem HitParticle => _hitParticle;

    [SerializeField] BulletPoolActive _shotPool;
    ShotLine _currentShotLine;
    public ShotLine CurrentShotLine => _currentShotLine;

    [Tooltip("まっすぐ飛ぶ弾のクラス")]
    ForwardMove _forwardMove = new();

    [Tooltip("回転する弾のクラス")]
    RotationMove _rotationMove = new();

    [Tooltip("一定時間たったら追尾する弾のクラス")]
    [SerializeField] DelayTargetPlayerMove _targetPlayerMove = new();

    DelayFastLazer _delayFastLazer = new();

    BulletMoveClass _currentBulletMoveClass;

    List<MoveBulletEnemy> _bulletListRef;

    float _timeScale = 1f;
    public float TimeScale => _timeScale;

    bool _isRota;
    public bool IsRota => _isRota;

    public void Init()
    {
        HitStopManager.instance._speedHitStopActionStart += HitStopStart;
        HitStopManager.instance._speedHitStopActionEnd += HitStopEnd;
    }

    private void OnDisable()
    {
        HitStopManager.instance._speedHitStopActionStart -= HitStopStart;
        HitStopManager.instance._speedHitStopActionEnd -= HitStopEnd;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // Sphereは3Dの物だが感覚で大きさを決める。
        Gizmos.DrawWireSphere(transform.position, _bulletScale);
    }

    #region 弾の種類
    /// <summary>弾が動くときの初期化</summary>
    public void BulletMoveStart(BulletSpawnEnemy spawnPoint,
        float bulletSpeed, float direction, float activeTime = 5f)
    {
        HitStopManager.instance._speedHitStopActionStart += HitStopStart;
        HitStopManager.instance._speedHitStopActionEnd += HitStopEnd;

        _breakType = spawnPoint.SpawnBulletMoveStruct._bulletBreakType;
        _currentBulletSpeed = _maxBulletSpeed = bulletSpeed;
        _activeTime = activeTime + _fadeTime;
        _currentBulletRota = _maxBulletRota = spawnPoint.SpawnBulletMoveStruct._bulletRotation;
        _currentBulletAngle = direction;
        _isRota = spawnPoint.SpawnBulletMoveStruct._isRota;

        if (HitStopManager.instance._isSpeedHitStop) HitStopStart(HitStopManager.instance._speedHitStopPower);

        if (!_isRota)
        {
            _currentDirection = new Vector3(Mathf.Cos(Mathf.Deg2Rad * (_currentBulletAngle + 90f)), Mathf.Sin(Mathf.Deg2Rad * (_currentBulletAngle + 90f)));
        }

        switch (spawnPoint.SpawnBulletMoveStruct._bulletMoveType)
        {
            case BulletMoveType.Forward:
                _currentBulletMoveClass = _forwardMove;
                _currentBulletMoveClass.BulletMove();
                break;
            case BulletMoveType.Rotate:
                _currentBulletMoveClass = _rotationMove;
                _currentBulletMoveClass.BulletMove();
                break;
            case BulletMoveType.TargetPlayer:
                PlayerTargetMethod();
                _currentBulletMoveClass = _forwardMove;
                _currentBulletMoveClass.BulletMove();
                break;
            case BulletMoveType.DelayTargetPlayer:
                _currentBulletMoveClass = _targetPlayerMove;
                _currentBulletMoveClass.BulletMove();
                break;
            case BulletMoveType.DelayFastLazer:
                _currentBulletMoveClass = _delayFastLazer;
                _currentBulletMoveClass.BulletMove();
                break;
        }
        _bulletListRef = spawnPoint._moveBulletList;
        _bulletListRef.Add(this);
        if (!spawnPoint.IsManualMove)
        {
            _isAttackTime = true;
        }
    }
    #endregion

    #region 弾の動きの処理

    private void Update()
    {
        if (_isAttackTime)
        {
            _isReset = _currentBulletMoveClass.BulletMoveUpdate(this, _currentBulletSpeed, _currentBulletRota);
            if (_isReset == false)
            {
                Reset();
            }
        }
    }

    /// <summary>弾がTransformで動く処理</summary>
    /// <param name="speed"></param>
    public void Move(float speed)
    {
        if (_isRota)
        {
            transform.position += transform.up * speed;
        }
        else
        {
            transform.position += _currentDirection * speed;
        }
    }

    /// <summary>回転処理</summary>
    /// <param name="rota"></param>
    public void Rotation(float rota)
    {
        if (_currentRotaAngle > _maxRotaAngle) { return; }
        _currentRotaTime += Time.deltaTime;
        if (_currentRotaTime > _rotaTime)
        {
            Debug.Log("グルグル");

            if (_isRota)
            {
                transform.Rotate(0, 0, rota);
            }
            else
            {
                _currentBulletAngle += rota;
                _currentDirection = new Vector3(Mathf.Cos(Mathf.Deg2Rad * (_currentBulletAngle + 90f)), Mathf.Sin(Mathf.Deg2Rad * (_currentBulletAngle + 90f)));
            }

            _currentRotaAngle += rota;
            _currentRotaTime = 0;
        }
    }

    /// <summary>プレイヤーに弾の向きを合わせる処理</summary>
    public void PlayerTargetMethod()
    {
        var playerTrans = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        var distance = playerTrans.position - transform.position;
        if (_isRota)
        {
            transform.rotation = Quaternion.FromToRotation(Vector3.up, distance);
            transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);
        }
        else
        {
            float rad = Mathf.Atan2(distance.y, distance.x);
            _currentBulletAngle = rad * Mathf.Rad2Deg;
            _currentDirection = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad));
        }
    }

    /// <summary>当たり判定処理</summary>
    public bool ChackPlayerHit()
    {
        if (_isFade) return false;
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, _bulletScale);
        foreach (var col in cols)
        {
            if (col.gameObject.tag == "Player")
            {
                if (_hitParticle) Instantiate(_hitParticle, col.transform.position, Quaternion.identity);
                return true;
            }
        }
        return false;
    }

    public bool ChackAttackHit()
    {
        if (_isFade) return false;
        if (_breakType == BulletBreakType.NotBreak)
        {
            return false;
        }
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, _bulletScale);
        foreach (var col in cols)
        {
            if (col.gameObject.tag == "PAttack")
            {
                if (_hitParticle) Instantiate(_hitParticle, transform.position, Quaternion.identity);
                HitStopManager.instance.SpeedHitStop();
                return true;
            }
        }
        return false;
    }
    #endregion

    #region 弾の壊れ方
    public void BulletBreakMehod()
    {
        switch (_breakType)
        {
            case BulletBreakType.Break:

                break;
            case BulletBreakType.Counter:

                break;
            case BulletBreakType.Homing:

                break;
        }
    }
    #endregion

    /// <summary>弾のフェード処理</summary>
    public void Fade(float currentFadeTime)
    {
        if (!_isFade) _isFade = true;
        var color = _mySpriteRenderer.color;
        if (_fadeTime != 0) color.a = currentFadeTime / _fadeTime;
        else color.a = _fadeTime;
        _mySpriteRenderer.color = color;
    }

    /// <summary>Rayで当たった地面のPositionを取ってきて、予測線の終点に渡すメソッド</summary>
    public Vector3 RayCatch()
    {
        RaycastHit2D[] ray;
        if (_isRota)
        {
            ray = Physics2D.RaycastAll(transform.position, transform.up);
        }
        else
        {
            ray = Physics2D.RaycastAll(transform.position, _currentDirection);
        }
        Vector3 hitPoint = transform.position;
        foreach (var hit in ray)
        {
            if (hit.collider.tag == "Ground" || hit.collider.tag == "Wall")
            {
                hitPoint = hit.point;
                break;
            }
        }
        _currentShotLine = _shotPool.GetBullet().GetComponent<ShotLine>();
        if (_currentShotLine != null)
        {
            _currentShotLine.SetLine(transform.position, hitPoint);
        }
        _isRay = true;
        return hitPoint;
    }

    /// <summary>弾からRayを出して当たった自機を攻撃するメソッド</summary>
    public void AttackRay()
    {
        RaycastHit2D[] hitRays;
        _currentShotLine.ShotBulletRef();
        if (_isRota)
        {
            hitRays = Physics2D.RaycastAll(transform.position, transform.up);
        }
        else
        {
            hitRays = Physics2D.RaycastAll(transform.position, _currentDirection);
        }
        bool isHitRay = false;
        foreach (var hit in hitRays)
        {
            if (hit.transform.TryGetComponent<PlayerController>(out var player))
            {
                if (_hitParticle)
                {
                    Instantiate(_hitParticle, player.transform.position, Quaternion.identity);
                    isHitRay = true;
                }
                break;
            }
        }
        if (!isHitRay)
        {
            _currentShotLine.ShotParticle();
        }
        _isRay = false;
    }

    void ResetRay()
    {
        _isRay = false;
        _currentShotLine.gameObject.SetActive(false);
    }

    public void CancelBullet()
    {
        if (_isRay) ResetRay();
        Reset();
    }

    // <summary>リセット</summary>
    public void Reset()
    {
        _isAttackTime = false;
        if (_bulletListRef != null)
        {
            _bulletListRef.Remove(this);
            _bulletListRef = null;
        }
        //参照MoveClassのリセット
        _currentBulletMoveClass = null;
        //弾の回転処理のリセット
        _currentRotaTime = 0f;
        _currentRotaAngle = 0f;
        //フェード処理のリセット
        _isFade = false;
        var color = _mySpriteRenderer.color;
        color.a = 1f;
        _mySpriteRenderer.color = color;
        //rotationのリセット
        transform.rotation = Quaternion.identity;
        //プールに返す。
        gameObject.SetActive(false);
    }

    public void HitStopStart(float _hitStopPower)
    {
        _currentBulletSpeed = _maxBulletSpeed * _hitStopPower;
        _timeScale = _hitStopPower;
        Debug.Log("ぬっ");
    }

    public void HitStopEnd()
    {
        _currentBulletSpeed = _maxBulletSpeed;
        _timeScale = 1f;
        Debug.Log("ん！");
    }
}
