using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class DelayTargetPlayerMove : BulletMoveClass
{
    [SerializeField] float _delayLookPlayerTime = 0.5f;
    bool _isLookPlayer = false;
    float _bulletSpeed;
    float _currentTime = 0;

    public override void BulletMove(float bulletSpeed)
    {
        _currentTime = 0;
        _bulletSpeed = bulletSpeed;
    }

    public override bool BulletMoveUpdate(MoveBulletEnemy bulletMove)
    {
        _currentTime += Time.deltaTime;
        if (!_isLookPlayer && _currentTime > _delayLookPlayerTime)
        {
            _isLookPlayer = true;
            bulletMove.PlayerTargetMethod();
        }

        bulletMove.Move(_bulletSpeed);

        /// 弾破壊判定

        // Playerに当たっているかの判定
        if (bulletMove.ChackPlayerHit()) 
        {
            LookReset();
            return false;
        }

        //Playerの攻撃に当たっているかの判定
        if (bulletMove.ChackAttackHit())
        {
            LookReset();
            bulletMove.BulletBreakMehod();
            return false;
        }

        // 時間判定
        if (_currentTime > bulletMove.ActiveTime)
        {
            LookReset();
            return false;
        }

        return true;
    }

    void LookReset()
    {
        _isLookPlayer = false;
    }
}
