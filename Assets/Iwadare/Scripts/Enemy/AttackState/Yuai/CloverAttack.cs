using System.Collections;
using UnityEngine;

public class CloverAttack : MonoBehaviour, IUIAttack
{
    [SerializeField] float _allAttackTime = 10f;
    [SerializeField] int _spawnCount = 3;
    [SerializeField] float _spawnTime = 1f;
    [SerializeField] float _bulletTime = 2f;
    [SerializeField] MixSpawn[] _rightSpawn;
    [SerializeField] MixSpawn[] _leftSpawn;
    [SerializeField] MixSpawn[] _centerSpawn;
    bool _isturn;
    public float GetAllAttackTime()
    {
        return _allAttackTime;
    }

    public void Init()
    {

    }

    public IEnumerator Attack(EnemyBase enemy)
    {
        for (var i = 0; i < _spawnCount; i++)
        {
            _isturn = RamdomMethod.RamdomNumber(99) < 50;
            if (_isturn)
            {
                for (var j = 0; j < _rightSpawn.Length; j++)
                {
                    _rightSpawn[j].EnemyMixSpawnRef(enemy);
                    _leftSpawn[j].EnemyMixSpawnRef(enemy);
                    _centerSpawn[_centerSpawn.Length - (j + 1)].EnemyMixSpawnRef(enemy);
                    yield return WaitforSecondsCashe.Wait(_spawnTime);
                }
            }
            else
            {
                for (var j = _rightSpawn.Length - 1; j >= 0; j--)
                {
                    _rightSpawn[j].EnemyMixSpawnRef(enemy);
                    _leftSpawn[j].EnemyMixSpawnRef(enemy);
                    _centerSpawn[_centerSpawn.Length - (j + 1)].EnemyMixSpawnRef(enemy);
                    yield return WaitforSecondsCashe.Wait(_spawnTime);
                }
            }
            yield return WaitforSecondsCashe.Wait(_bulletTime);
        }
    }
}
