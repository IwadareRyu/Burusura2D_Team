using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayFastLazer : BulletMoveClass
{
    Vector3 _hitPosition;
    public override void BulletMove() 
    {

        return;
    }

    public override bool BulletMoveUpdate(MoveBulletEnemy bulletMove, float bulletSpeed, float bulletRota = 0f) 
    {
        if (!bulletMove.IsRay)
        {
            _hitPosition = bulletMove.RayCatch();
            Debug.Log(_hitPosition);
            return true;
        }

        return true;
    }
}
