public class ForwardSpawn : BulletSpawnClass
{
    public void Spawn(BulletSpawnEnemy bulletSpawn)
    {
        if (!bulletSpawn.IsManualMove && bulletSpawn.SpawnBulletMoveStruct._bulletMoveType != BulletMoveType.DelayFastLazer)
        {
            bulletSpawn.AttackAudio();
        }
        bulletSpawn.InitBullet(bulletSpawn.BulletDistance, bulletSpawn.DefaultBulletSpeed, bulletSpawn.BulletActiveTime);
    }
}
