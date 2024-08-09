using UnityEngine;

public class ForwardMove : BulletMoveClass
{
    float _currentTime;

    public override void BulletMove()
    {
        _currentTime = 0f;
    }

    public override bool BulletMoveUpdate(MoveBulletEnemy bulletMove,float bulletSpeed,float bulletRota)
    {
        _currentTime += Time.deltaTime * bulletMove._timeScale;
        bulletMove.Move(bulletSpeed);

        // Playerに当たっているかの判定
        if (bulletMove.ChackPlayerHit()) { return false; }

        //Playerの攻撃に当たっているかの判定
        if (bulletMove.ChackAttackHit())
        {
            bulletMove.BulletBreakMehod();
            return false;
        }

        if(_currentTime > bulletMove.ActiveTime - bulletMove.FadeTime)
        {
            bulletMove.Fade(bulletMove.ActiveTime - _currentTime);
        }

        // 時間判定
        if (_currentTime > bulletMove.ActiveTime)
        {
            return false;
        }

        return true;
    }
}
