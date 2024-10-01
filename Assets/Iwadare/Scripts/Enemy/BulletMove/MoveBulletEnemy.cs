using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBulletEnemy : MonoBehaviour,HitStopInterface
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
    float _currentBulletAngle = 0f;


    [Tooltip("弾のアクティブ時間")]
    float _activeTime = 5f;
    public float ActiveTime => _activeTime;
    float _currentRotaTime = 0f;

    [Tooltip("弾の大きさ"), Header("弾の大きさ")]
    [SerializeField] float _bulletScale = 3f;
    public float BulletScale => _bulletScale;

    BulletBreakType _breakType;
    public bool _isAttackTime = false;
    public bool _isReset = false;

    [Tooltip("フェードの時間"), Header("フェードの時間")]
    [SerializeField] float _fadeTime = 1f;
    public float FadeTime => _fadeTime;
    float defaultBulleetAlpha = 1f;
    public bool _isFade = false;

    SpriteRenderer _mySpriteRenderer;

    public ParticleSystem _hitParticle;

    [Tooltip("まっすぐ飛ぶ弾のクラス")]
    ForwardMove _forwardMove = new();

    [Tooltip("回転する弾のクラス")]
    RotationMove _rotationMove = new();

    [Tooltip("一定時間たったら追尾する弾のクラス")]
    [SerializeField] DelayTargetPlayerMove _targetPlayerMove = new();

    BulletMoveClass _currentBulletMoveClass;

    List<MoveBulletEnemy> _bulletListRef;

    public float _timeScale = 1f;

    bool _isRota;

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
    /// <param name="moveState">動き方のState</param>
    /// <param name="breakType">壊れ方のState</param>
    /// <param name="bulletSpeed">弾の速さ</param>
    /// <param name="bulletRota">弾の回る角度</param>
    public void BulletMoveStart(BulletSpawnEnemy spawnPoint,
        float bulletSpeed,float direction,float activeTime = 5f)
    {
        _mySpriteRenderer = GetComponent<SpriteRenderer>();
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
            _currentDirection = new Vector3(Mathf.Cos(Mathf.Deg2Rad * _currentBulletAngle), Mathf.Sin(Mathf.Deg2Rad * _currentBulletAngle));
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
            _isReset = _currentBulletMoveClass.BulletMoveUpdate(this,_currentBulletSpeed,_currentBulletRota);
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
        if(_currentRotaAngle > _maxRotaAngle) { return; }
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
                _currentDirection = new Vector3(Mathf.Cos(Mathf.Deg2Rad * _currentBulletAngle), Mathf.Sin(Mathf.Deg2Rad * _currentBulletAngle)); 
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
            float rad = Mathf.Atan2(distance.y,distance.x);
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
        if(_fadeTime != 0) color.a = currentFadeTime / _fadeTime;
        else color.a = _fadeTime;
        _mySpriteRenderer.color = color;
    }

    // <summary>リセット</summary>
    public void Reset()
    {
        _isAttackTime = false;
        if(_bulletListRef != null)
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
