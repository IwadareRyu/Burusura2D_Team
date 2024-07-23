using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBulletEnemy : MonoBehaviour,HitStopInterface
{
    /// <summary>弾の速さ</summary>
    float _maxBulletSpeed;
    float _currentBulletSpeed;

    /// <summary>弾の回転</summary>
    float _maxBulletRota;
    float _currentBulletRota;

    [SerializeField] float _rotaTime = 0.1f;
    [SerializeField] float _maxRotaAngle = 90f;
    float _currentRotaAngle = 0f;
    [SerializeField] float _activeTime = 5f;
    public float ActiveTime => _activeTime;
    float _currentTime = 0f;
    [SerializeField] float _bulletScale = 3f;
    public float BulletScale => _bulletScale;
    BulletBreakType _breakType;
    bool _attackTime = false;

    [SerializeField] public ParticleSystem _hitParticle;

    // まっすぐ飛ぶ弾のクラス
    ForwardMove _forwardMove = new();
    // 回転する弾のクラス
    RotationMove _rotationMove = new();
    // 一定時間たったら追尾する弾のクラス
    [SerializeField] DelayTargetPlayerMove _targetPlayerMove = new();

    BulletMoveClass _currentBulletMoveClass;

    public float _timeScale = 1f;


    private void OnEnable()
    {
        HitStopManager.instance._speedHitStopActionStart += HitStopStart;
        HitStopManager.instance._speedHitStopActionEnd += HitStopEnd;
    }

    private void OnDisable()
    {
        HitStopManager.instance._speedHitStopActionStart -= HitStopStart;
        HitStopManager.instance._speedHitStopActionEnd -= HitStopEnd;
    }

    public void Init()
    {
        _targetPlayerMove.Init(this);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // Sphereは3Dの物だが感覚で大きさを決める。
        Gizmos.DrawWireSphere(transform.position, _bulletScale);
    }

    #region 弾の種類
    /// <summary>
    /// バレットが動くときの初期化
    /// </summary>
    /// <param name="moveState">動き方のState</param>
    /// <param name="breakType">壊れ方のState</param>
    /// <param name="bulletSpeed">弾の速さ</param>
    /// <param name="bulletRota">弾の回る角度</param>
    public void BulletMoveStart(BulletMoveType moveState,BulletBreakType breakType, float bulletSpeed, float bulletRota = 0f)
    {
        _breakType = breakType;
        _currentBulletSpeed = _maxBulletSpeed = bulletSpeed;
        if (HitStopManager.instance._isSpeedHitStop) { HitStopStart(HitStopManager.instance._speedHitStopPower); }
        _currentBulletRota = _maxBulletRota = bulletRota;
        switch (moveState)
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
        _attackTime = true;
    }
    #endregion

    #region 弾の動きの処理

    private void Update()
    {
        if (_attackTime)
        {
            _attackTime = _currentBulletMoveClass.BulletMoveUpdate(this,_currentBulletSpeed,_currentBulletRota);
        }
        else if(_currentBulletMoveClass != null)
        {
            Reset();
        }
    }

    /// <summary>弾がTransformで動く処理</summary>
    /// <param name="speed"></param>
    public void Move(float speed)
    {
        transform.position += transform.up * speed;
    }

    /// <summary>回転処理</summary>
    /// <param name="rota"></param>
    public void Rotation(float rota)
    {
        if(_currentRotaAngle > _maxRotaAngle) { return; }
        _currentTime += Time.deltaTime;
        if (_currentTime > _rotaTime)
        {
            Debug.Log("グルグル");
            transform.Rotate(0, 0, rota);
            _currentRotaAngle+= rota;
            _currentTime = 0;
        }
    }

    /// <summary>プレイヤーに弾の向きを合わせる処理</summary>
    public void PlayerTargetMethod()
    {
        var playerTrans = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        var distance = playerTrans.position - transform.position;
        transform.rotation = Quaternion.FromToRotation(Vector3.up, distance);
        transform.eulerAngles = new Vector3(0,0,transform.eulerAngles.z);
    }

    /// <summary>当たり判定処理</summary>
    public bool ChackPlayerHit()
    {
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
        if(_breakType == BulletBreakType.NotBreak)
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
    // <summary>リセット</summary>
    public void Reset()
    {
        _currentTime = 0f;
        _currentRotaAngle = 0f;
        transform.rotation = Quaternion.identity;
        _currentBulletMoveClass = null;
        gameObject.SetActive(false);
    }

    public void HitStopStart(float _hitStopPower)
    {
        _currentBulletSpeed = _maxBulletSpeed * _hitStopPower;
        _timeScale = _hitStopPower;
    }

    public void HitStopEnd()
    {
        _currentBulletSpeed = _maxBulletSpeed;
        _timeScale = 1f;
    }
}
