using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBulletEnemy : MonoBehaviour
{
    [SerializeField] float _rotaTime = 0.1f;
    [SerializeField] float _maxRotaAngle = 90f;
    float _currentRotaAngle = 0f;
    [SerializeField] float _activeTime = 5f;
    public float ActiveTime => _activeTime;
    float _currentTime = 0f;
    [SerializeField] float _bulletScale = 3f;
    public float BulletScale => _bulletScale;
    BulletBreakType _breakType;

    ForwardMove _forwardMove = new();
    RotationMove _rotationMove = new();
    [SerializeField] DelayTargetPlayerMove _targetPlayerMove = new();

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
    public void BulletMoveStart(float bulletSpeed, float bulletRota, BulletMoveType moveState,BulletBreakType breakType)
    {
        _breakType = breakType;
        switch (moveState)
        {
            case BulletMoveType.Forward:
                StartCoroutine(_forwardMove.BulletMove(this,bulletSpeed));
                break;
            case BulletMoveType.Rotate:
                StartCoroutine(_rotationMove.BulletMove(this,bulletSpeed,bulletRota));
                break;
            case BulletMoveType.TargetPlayer:
                PlayerTargetMethod();
                StartCoroutine(_forwardMove.BulletMove(this, bulletSpeed));
                break;
            case BulletMoveType.DelayTargetPlayer:
                StartCoroutine(_targetPlayerMove.BulletMove(this, bulletSpeed));
                break;
        }
    }
    #endregion

    #region 弾の動きの処理
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
    }

    /// <summary>当たり判定処理</summary>
    public bool ChackPlayerHit()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, _bulletScale);
        foreach (var col in cols)
        {
            if (col.gameObject.tag == "Player")
            {
                return true;
            }
        }
        return false;
    }

    public bool ChackAttackHit()
    {
        if(_breakType == BulletBreakType.NotBreak) { return false; }
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, _bulletScale);
        foreach (var col in cols)
        {
            if (col.gameObject.tag == "PAttack")
            {
                return true;
            }
        }
        return false;
    }

    public void BulletBreakMehod()
    {
        switch(_breakType)
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
        gameObject.SetActive(false);
    }
}
