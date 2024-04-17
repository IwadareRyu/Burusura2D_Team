using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.TerrainTools;
using UnityEngine;

[Serializable]
public class DelayTargetPlayerMove : BulletMoveClass
{
    [SerializeField]float _delayLookPlayerTime = 0.5f;
    bool _isLookPlayer = false;

    public override void Init(MoveBulletEnemy bulletMove)
    {
        return;
    }

    public override IEnumerator BulletMove(MoveBulletEnemy bulletMove, float bulletSpeed)
    {
        for (float i = 0f; i < bulletMove.ActiveTime; i += Time.deltaTime)
        {
            if(!_isLookPlayer && i > _delayLookPlayerTime)
            {
                _isLookPlayer = true;
                bulletMove.PlayerTargetMethod();
            }

            bulletMove.Move(bulletSpeed);
            // Playerに当たっているかの判定
            if (bulletMove.ChackPlayerHit()) { break; }

            //Playerの攻撃に当たっているかの判定
            if (bulletMove.ChackAttackHit())
            {
                bulletMove.BulletBreakMehod();
                break;
            }

            yield return new WaitForFixedUpdate();
        }
        _isLookPlayer = false;
        bulletMove.Reset();
    }
}
