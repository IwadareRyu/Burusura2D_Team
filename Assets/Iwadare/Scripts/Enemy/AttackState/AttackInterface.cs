using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface AttackInterface
{

    [Tooltip("Stay時のUpdate")]
    abstract void StayUpdate(EnemyBase enemy);
    [Tooltip("Move時のUpdate")]
    abstract void MoveUpdate(EnemyBase enemy);
    [Tooltip("Attack時のUpdate")]
    abstract IEnumerator Attack(EnemyBase enemy);
    abstract void ActionReset(EnemyBase enemy);
}
