using UnityEngine;

public class RotationMove : BulletMoveClass
{
    float _currentTime;
    public override void BulletMove()
    {
        _currentTime = 0f;
    }

    public override bool BulletMoveUpdate(MoveBulletEnemy bulletMove,float bulletSpeed,float bulletRota)
    {
        _currentTime += Time.deltaTime * bulletMove._timeScale;
        bulletMove.Rotation(bulletRota);
        bulletMove.Move(bulletSpeed);

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
