using System.Linq;
using UnityEngine;

public class MixSpawn : MonoBehaviour
{
    [SerializeField] BulletSpawnEnemy[] _spawns;
    public void MixSpawnMethod()
    {
        foreach (var spawner in _spawns)
        {
            StartCoroutine(spawner.BulletSpawn());
        }
    }

    public void EnemyMixSpawnRef(EnemyBase enemy)
    {
        foreach (var spawner in _spawns)
        {
            enemy.SpawnBulletRef(spawner);
        }
    }
}
