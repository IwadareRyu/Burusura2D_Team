using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BulletSpawnClass
{
    abstract void Spawn(BulletSpawnEnemy bulletSpawn);

    virtual IEnumerator DelaySpawn(BulletSpawnEnemy bulletSpawn) { yield return null; }
}
