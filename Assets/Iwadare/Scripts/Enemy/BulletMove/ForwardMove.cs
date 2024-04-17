using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardMove : BulletMoveClass
{
    public override IEnumerator BulletMove(MoveBulletEnemy bulletMove, float bulletSpeed)
    {
        for(float i = 0f;i < bulletMove.ActiveTime; i += Time.deltaTime)
        {
            bulletMove.Move(bulletSpeed);

            // Playerに当たっているかの判定
            if (bulletMove.ChackPlayerHit()) { break; }

            //Playerの攻撃に当たっているかの判定
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
