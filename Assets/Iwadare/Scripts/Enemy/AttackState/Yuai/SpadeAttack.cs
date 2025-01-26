using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpadeAttack : MonoBehaviour,IUIAttack
{
    [SerializeField] BulletSpawnEnemy[] _leftSpadeSpawns;
    [SerializeField] BulletSpawnEnemy[] _rightSpadeSpawns; 
    [SerializeField] float _allAttackTime = 10f;
    [SerializeField] int _spawnCount = 3;
    [SerializeField] float _spawnTime = 2f;
    [SerializeField] Text _leftDangerousText;
    [SerializeField] Text _rightDangerousText;
    int _dangerousCount = 3;
    float _waitDangerousTime = 0.5f;
    float _bulletTime = 10f;
    bool _isRight;

    public void Init()
    {
        _leftDangerousText.gameObject.SetActive(false);
        _rightDangerousText.gameObject.SetActive(false);
    }

    public IEnumerator Attack(EnemyBase enemy)
    {
        yield return DangerousText();
        for (var i = 0; i < _spawnCount; i++)
        {
            _isRight = RamdomMethod.RamdomNumber(99) < 50;
            if (_isRight)
            {
                foreach(var spawn in _rightSpadeSpawns)
                {
                    enemy.SpawnBulletRef(spawn);
                }
            }
            else
            {
                foreach (var spawn in _leftSpadeSpawns)
                {
                    enemy.SpawnBulletRef(spawn);
                }
            }
            enemy._bossAudio.AttackAudioPlay();
            yield return WaitforSecondsCashe.Wait(_spawnTime);
        }
        yield return WaitforSecondsCashe.Wait(_bulletTime);
    }

    public IEnumerator DangerousText()
    {
        for(var i = 0;i < _dangerousCount;i++)
        {
            yield return WaitforSecondsCashe.Wait(_waitDangerousTime);
            _leftDangerousText.gameObject.SetActive(true);
            _rightDangerousText.gameObject.SetActive(true);
            yield return WaitforSecondsCashe.Wait(_waitDangerousTime);
            _leftDangerousText.gameObject.SetActive(false);
            _rightDangerousText.gameObject.SetActive(false);
        }
    }

    public float GetAllAttackTime()
    {
        return _allAttackTime;
    }
}
