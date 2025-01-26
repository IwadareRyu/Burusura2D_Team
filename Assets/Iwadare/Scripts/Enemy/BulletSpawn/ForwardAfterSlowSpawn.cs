using System;
using UnityEngine;

[Serializable]
public class ForwardAfterSlowSpawn : BulletSpawnClass
{
    [Tooltip("スポーンされる弾の個数"), Header("スポーンされる弾の個数")]
    [SerializeField] int _spawnCount = 3;
    [Tooltip("後に生成される弾の速さ倍率"), Header("後に生成される弾の速さ倍率")]
    [SerializeField] float _speedPower = 0.8f;
    [Tooltip("反転Spawnの有効化無効化"), Header("反転Spawn有効化無効化")]
    [SerializeField] bool _isInversion;


    public void Spawn(BulletSpawnEnemy bulletSpawn)
    {
        if (!bulletSpawn.IsManualMove && bulletSpawn.SpawnBulletMoveStruct._bulletMoveType != BulletMoveType.DelayFastLazer)
        {
            bulletSpawn.AttackAudio();
        }
        for (var i = 0; i < _spawnCount; i++)
        {
            var speedPower = (float)Math.Pow(_speedPower, i);
            bulletSpawn.InitBullet(bulletSpawn.BulletDistance, bulletSpawn.DefaultBulletSpeed * speedPower, bulletSpawn.BulletActiveTime);
            if (_isInversion) bulletSpawn.InitBullet(bulletSpawn.BulletDistance + 180, bulletSpawn.DefaultBulletSpeed * speedPower, bulletSpawn.BulletActiveTime);
        }
    }
}
