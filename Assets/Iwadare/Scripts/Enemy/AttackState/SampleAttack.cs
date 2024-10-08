﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleAttack : AttackInterface
{
    public void StayUpdate(EnemyBase enemy)
    {
        enemy._bossState = EnemyBase.BossState.MoveState;
    }
    public IEnumerator Move(EnemyBase enemy)
    {
        yield return null;
    }

    public IEnumerator Attack(EnemyBase enemy)
    {
        yield return null;
    }

    public void ActionReset(EnemyBase enemy)
    {
        enemy.ResetState();
        if (enemy._useGravity) enemy._enemyRb.gravityScale = 1;
    }

    public void Init()
    {
        return;
    }
}
