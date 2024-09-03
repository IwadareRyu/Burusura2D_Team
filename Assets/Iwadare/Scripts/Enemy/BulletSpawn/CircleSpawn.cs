using System.Collections;
using UnityEngine;

public class CircleSpawn : BulletSpawnClass
{
    [SerializeField] float _delaySpawnCoolTime = 0.1f;
    public void Spawn(BulletSpawnEnemy bulletSpawn)
    {
        for (float i = bulletSpawn.BulletDistance; i < 360 + bulletSpawn.BulletDistance; i += bulletSpawn.BulletRange)
        {
            bulletSpawn.InitBullet(i, bulletSpawn.DefaultBulletSpeed, bulletSpawn.BulletActiveTime);
        }
    }

    public IEnumerator DelaySpawn(BulletSpawnEnemy bulletSpawn)
    {
        for (float i = bulletSpawn.BulletDistance; i < 360 + bulletSpawn.BulletDistance; i += bulletSpawn.BulletRange)
        {
            bulletSpawn.InitBullet(i, bulletSpawn.DefaultBulletSpeed, bulletSpawn.BulletActiveTime);
            yield return new WaitForSeconds(_delaySpawnCoolTime);
        }
    }
}
