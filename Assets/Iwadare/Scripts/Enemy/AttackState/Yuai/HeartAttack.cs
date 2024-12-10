using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HeartAttack : MonoBehaviour,IUIAttack
{
    [SerializeField] BulletSpawnEnemy[] _leftHeartSpawns;
    [SerializeField] BulletSpawnEnemy[] _rightHeartSpawns;
    [SerializeField] BulletSpawnEnemy[] _centerHeartSpawns;
    [SerializeField] Text _centerDangerousText;
    [SerializeField] int _attackCount = 2;
    [SerializeField] float _allAttackTime = 10f;
    [SerializeField] float _attackMiddleTime = 1f;
    [SerializeField] float _attackEndTime = 2f;
    int _dangerousCount = 3;
    float _waitDangerousTime = 0.5f;

    public void Init()
    {
        _centerDangerousText.gameObject.SetActive(false);
    }

    public IEnumerator Attack(EnemyBase enemy)
    {
        yield return DangerousText();
        for(var i = 0;i < _attackCount;i++)
        {
            foreach(var spawn in _leftHeartSpawns)
            {
                enemy.SpawnBulletRef(spawn);
            }
            foreach(var spawn in _rightHeartSpawns)
            {
                enemy.SpawnBulletRef(spawn);
            }
            yield return WaitforSecondsCashe.Wait(_attackMiddleTime);
            foreach(var spawn in _centerHeartSpawns)
            {
                enemy.SpawnBulletRef(spawn);
            }
            yield return WaitforSecondsCashe.Wait(_attackEndTime);
        }
    }

    public IEnumerator DangerousText()
    {
        for (var i = 0; i < _dangerousCount; i++)
        {
            yield return WaitforSecondsCashe.Wait(_waitDangerousTime);
            _centerDangerousText.gameObject.SetActive(true);
            yield return WaitforSecondsCashe.Wait(_waitDangerousTime);
            _centerDangerousText.gameObject.SetActive(false);
        }
    }

    public float GetAllAttackTime()
    {
        return _allAttackTime;
    }

}
