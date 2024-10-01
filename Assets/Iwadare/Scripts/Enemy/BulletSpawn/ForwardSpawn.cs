public class ForwardSpawn : BulletSpawnClass
{
    public void Spawn(BulletSpawnEnemy bulletSpawn)
    {
        bulletSpawn.InitBullet(bulletSpawn.BulletDistance, bulletSpawn.DefaultBulletSpeed, bulletSpawn.BulletActiveTime);
    }
}
