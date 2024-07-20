using UnityEngine;

public class RotationMove : BulletMoveClass
{
    float _bulletSpeed;
    float _bulletRota;
    float _currentTime;
    public override void BulletMove(float bulletSpeed, float bulletRota)
    {
        _currentTime = 0f;
        _bulletSpeed = bulletSpeed;
        _bulletRota = bulletRota;
    }

    public override bool BulletMoveUpdate(MoveBulletEnemy bulletMove)
    {
        _currentTime += Time.deltaTime;
        bulletMove.Rotation(_bulletRota);
        bulletMove.Move(_bulletSpeed);

        /// 弾破壊判定
        // プレイヤーとの当たり判定
        if (bulletMove.ChackPlayerHit())
        {
            return false;
        }

        // プレイヤーの斬撃との当たり判定
        if (bulletMove.ChackAttackHit())
        {
            bulletMove.BulletBreakMehod();
            return false;
        }

        // 時間判定
        if (_currentTime > bulletMove.ActiveTime)
        {
            return false;
        }
        return true;
    }
}
