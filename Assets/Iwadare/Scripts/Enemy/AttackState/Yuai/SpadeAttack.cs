﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpadeAttack : MonoBehaviour,IUIAttack
{
    [SerializeField] BulletSpawnEnemy[] _spadeSpawns;
    [SerializeField] float _allAttackTime = 10f;
    [SerializeField] int _spawnCount = 3;
    [SerializeField] float _spawnTime = 2f;
    [SerializeField] Text _dangerousText;
    int _dangerousCount = 3;
    float _waitDangerousTime = 0.5f;
    float _bulletTime = 10f;

    public void Init()
    {
        _dangerousText.gameObject.SetActive(false);
    }

    public IEnumerator Attack(EnemyBase enemy)
    {
        yield return DangerousText();
        for (var i = 0; i < _spawnCount; i++)
        {
            foreach (var spawn in _spadeSpawns)
            {
                enemy.SpawnBulletRef(spawn);
            }
            yield return WaitforSecondsCashe.Wait(_spawnTime);
        }
        yield return WaitforSecondsCashe.Wait(_bulletTime);
    }

    public IEnumerator DangerousText()
    {
        for(var i = 0;i < _dangerousCount;i++)
        {
            yield return WaitforSecondsCashe.Wait(_waitDangerousTime);
            _dangerousText.gameObject.SetActive(true);
            yield return WaitforSecondsCashe.Wait(_waitDangerousTime);
            _dangerousText.gameObject.SetActive(false);
        }
    }

    public float GetAllAttackTime()
    {
        return _allAttackTime;
    }
}
