using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CircleSpawn : BulletSpawnClass
{
    [SerializeField] float _delaySpawnCoolTime = 0.1f;
    public void Spawn(BulletSpawnEnemy bulletSpawn)
    {
        for(float i = bulletSpawn.BulletDistance;i < 360 + bulletSpawn.BulletDistance;i += bulletSpawn.BulletRange)
        {
            bulletSpawn.InitBullet(i,bulletSpawn.DefaultBulletSpeed);
        }
    }

    public IEnumerator DelaySpawn(BulletSpawnEnemy bulletSpawn)
    {
        for (float i = bulletSpawn.BulletDistance; i < 360 + bulletSpawn.BulletDistance; i += bulletSpawn.BulletRange)
        {
            bulletSpawn.InitBullet(i, bulletSpawn.DefaultBulletSpeed);
            yield return new WaitForSeconds(_delaySpawnCoolTime);
        }
    }
}
