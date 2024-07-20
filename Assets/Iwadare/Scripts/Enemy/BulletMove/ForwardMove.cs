using UnityEngine;

public class ForwardMove : BulletMoveClass
{
    float _bulletSpeed;
    float _currentTime;

    public override void BulletMove(float bulletSpeed)
    {
        _currentTime = 0f;
        _bulletSpeed = bulletSpeed;
    }

    public override bool BulletMoveUpdate(MoveBulletEnemy bulletMove)
    {
        _currentTime += Time.deltaTime;
        bulletMove.Move(_bulletSpeed);

        // Playerに当たっているかの判定
        if (bulletMove.ChackPlayerHit()) { return false; }

        //Playerの攻撃に当たっているかの判定
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
