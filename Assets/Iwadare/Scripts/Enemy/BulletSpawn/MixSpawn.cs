using System.Collections;
using System.Linq;
using UnityEngine;

public class MixSpawn : MonoBehaviour
{
    [SerializeField] BulletSpawnEnemy[] _spawns;
    [SerializeField] float _waitDangerousTime = 1f;
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

    public IEnumerator EnemyMixSpawnRefDangerous(EnemyBase enemy)
    {
        foreach(var spawner in _spawns)
        {
            spawner.DangerousSign(_waitDangerousTime);
        }
        yield return WaitforSecondsCashe.Wait(_waitDangerousTime);
        foreach (var spawner in _spawns)
        {
            enemy.SpawnBulletRef(spawner);
        }
    }
}
