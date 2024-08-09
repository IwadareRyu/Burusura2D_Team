using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardSpawn : BulletSpawnClass
{
    public void Spawn(BulletSpawnEnemy bulletSpawn)
    {
        bulletSpawn.InitBullet(90, bulletSpawn.DefaultBulletSpeed,bulletSpawn.BulletActiveTime);
    }
}
