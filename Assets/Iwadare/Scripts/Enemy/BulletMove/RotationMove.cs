using System.Collections;
using UnityEngine;

public class RotationMove : BulletMoveClass
{
    public override IEnumerator BulletMove(MoveBulletEnemy bulletMove, float bulletSpeed, float bulletRota)
    {
        for(float i = 0f;i <= bulletMove.ActiveTime;i += Time.deltaTime)
        {
            bulletMove.Rotation(bulletRota);
            bulletMove.Move(bulletSpeed);

            if (bulletMove.ChackPlayerHit()) { break; }

            if(bulletMove.ChackAttackHit())
            {
                bulletMove.BulletBreakMehod();
                break;
            }

            yield return new WaitForFixedUpdate();
        }
        bulletMove.Reset();
    }
}
