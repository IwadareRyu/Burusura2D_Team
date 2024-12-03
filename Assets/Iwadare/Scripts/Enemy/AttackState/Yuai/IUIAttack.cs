using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUIAttack
{
    abstract void Init();
    abstract IEnumerator Attack(EnemyBase enemy);
    abstract float GetAllAttackTime();
}
