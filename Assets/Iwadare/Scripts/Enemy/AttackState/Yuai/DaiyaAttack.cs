using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaiyaAttack : MonoBehaviour,IUIAttack
{
    [SerializeField] BulletSpawnEnemy[] _leftUpLazerSpawn;
    [SerializeField] BulletSpawnEnemy[] _leftDownLazerSpawn;
    [SerializeField] BulletSpawnEnemy[] _rightUpLazerSpawn;
    [SerializeField] BulletSpawnEnemy[] _rightDownLazerSpawn;
    [SerializeField] float _allAttackTime = 10f;
    [SerializeField] int _loopCount = 2;
    [SerializeField] int _spawnCount = 3;
    [SerializeField] float _spawnTime = 2f;
    [SerializeField] float _disSpawnTime = 0.3f;
    //[SerializeField] SpriteRenderer[] _yuaiRenderer;
    //[SerializeField] Transform[] _leftUpYuai;
    //[SerializeField] Transform[] _rightUpYuai;
    //[SerializeField] Transform[] _leftDownYuai;
    //[SerializeField] Transform[] _rightDownYuai;
    bool _isturn;
    public void Init()
    {

    }

    public float GetAllAttackTime()
    {
        return _allAttackTime;
    }

    public IEnumerator Attack(EnemyBase enemy)
    {
        for (var i = 0; i < _loopCount; i++)
        {
            _isturn = RamdomMethod.RamdomNumber0Max(99) < 50;
            if (_isturn)
            {
                for (var j = 0; j < _leftUpLazerSpawn.Length; j++)
                {
                    enemy.SpawnBulletRef(_leftUpLazerSpawn[j]);
                    enemy.SpawnBulletRef(_leftDownLazerSpawn[j]);
                    enemy.SpawnBulletRef(_rightUpLazerSpawn[j]);
                    enemy.SpawnBulletRef(_rightDownLazerSpawn[j]);
                    yield return WaitforSecondsCashe.Wait(_disSpawnTime);

                }
            }
            else
            {
                for(var j = _leftUpLazerSpawn.Length - 1;j > 0;j--)
                {
                    enemy.SpawnBulletRef(_leftUpLazerSpawn[j]);
                    enemy.SpawnBulletRef(_leftDownLazerSpawn[j]);
                    enemy.SpawnBulletRef(_rightUpLazerSpawn[j]);
                    enemy.SpawnBulletRef(_rightDownLazerSpawn[j]);
                    yield return WaitforSecondsCashe.Wait(_disSpawnTime);
                }
            }
            yield return WaitforSecondsCashe.Wait(_spawnTime);
        }
    }
}
