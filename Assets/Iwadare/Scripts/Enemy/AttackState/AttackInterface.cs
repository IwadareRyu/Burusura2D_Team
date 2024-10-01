using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface AttackInterface
{

    abstract void Init();

    [Tooltip("Stay時のUpdate")]
    abstract void StayUpdate(EnemyBase enemy);
    [Tooltip("Move時のUpdate")]
    abstract IEnumerator Move(EnemyBase enemy);
    [Tooltip("Attack時のUpdate")]
    abstract IEnumerator Attack(EnemyBase enemy);
    abstract void ActionReset(EnemyBase enemy);
}
