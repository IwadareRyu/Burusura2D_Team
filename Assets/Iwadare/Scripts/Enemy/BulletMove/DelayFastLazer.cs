using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayFastLazer : BulletMoveClass
{
    Vector3 _hitPosition;
    float _currentTime = 0f;
    float _persent;
    public override void BulletMove() 
    {
        _currentTime = 0f;
    }

    public override bool BulletMoveUpdate(MoveBulletEnemy bulletMove, float bulletSpeed, float bulletRota = 0f) 
    {
        if (!bulletMove.IsRay)
        {
            _hitPosition = bulletMove.RayCatch();
            //Debug.Log(_hitPosition);
            return true;
        }

        _currentTime += Time.deltaTime * bulletMove.TimeScale;


        //Playerの攻撃に当たっているかの判定
        if (bulletMove.ChackAttackHit())
        {
            if (bulletMove.IsAudio) AudioManager.Instance.PlaySE(bulletMove._strongAttackAudio);
            bulletMove.BulletBreakMehod();
            return false;
        }

        // 時間判定
        if (_currentTime > bulletMove.ActiveTime)
        {
            if(bulletMove.IsAudio) AudioManager.Instance.PlaySE(bulletMove._strongAttackAudio);
            bulletMove.AttackRay();
            return false;
        }
        else if(bulletMove.CurrentShotLine)
        {
            _persent = _currentTime / bulletMove.ActiveTime;
            bulletMove.CurrentShotLine.LineUpdate(_persent);
        }
        return true;
    }
}
