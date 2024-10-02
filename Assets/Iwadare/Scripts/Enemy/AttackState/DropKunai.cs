using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class DropKunai : MonoBehaviour,AttackInterface
{
    float _currentTime = 0f;
    [SerializeField] float _stayTime = 1f;
    [SerializeField] float _moveTime = 3f;
    [SerializeField] float _disAttackTime = 0.1f;
    [SerializeField] float _attackCoolTime = 2f;
    [SerializeField] GameObject _kunaiGimmick;
    [SerializeField] BulletSpawnEnemy[] _bulletSpawnEnemyOne;
    [SerializeField] BulletSpawnEnemy[] _bulletSpawnEnemyTwo;
    [SerializeField] BulletSpawnEnemy[] _bulletSpawnEnemyThree;
    [SerializeField] Animator[] _bossDammys;
    [SerializeField] Transform _dummyMovePoint;
    [SerializeField] Transform _dummyDafaultPoint;
    Tween[] _moveTween;

    public void Init()
    {
        if (_kunaiGimmick) _kunaiGimmick.SetActive(false);
    }

    public void StayUpdate(EnemyBase enemy)
    {
        _currentTime += Time.deltaTime;
        if (_currentTime > _stayTime)
        {
            _currentTime = 0f;
            if (_kunaiGimmick) _kunaiGimmick.SetActive(true);
            if (enemy._useGravity)
            {
                enemy._enemyRb.gravityScale = 0;
                enemy._enemyRb.velocity = Vector2.zero;
            }
            _moveTween = new Tween[_bossDammys.Length];
            enemy._bossState = EnemyBase.BossState.MoveState;
        }
    }
    public IEnumerator Move(EnemyBase enemy)
    {
        for (var i = 0; i < _bossDammys.Length; i++)
        {
            _moveTween[i] = _bossDammys[i].transform.DOMoveX(_dummyMovePoint.position.x, _moveTime);
        }
        var k = 0;
        for (var j = 0; j < _bulletSpawnEnemyThree.Length; j++)
        {
            if (j % 2 == 0)
            {
                enemy.SpawnBulletRef(_bulletSpawnEnemyTwo[k]);
            }
            else
            {
                enemy.SpawnBulletRef(_bulletSpawnEnemyOne[k]);
                k++;
            }
            enemy.SpawnBulletRef(_bulletSpawnEnemyThree[j]);
            yield return new WaitForSeconds(_moveTime / _bulletSpawnEnemyThree.Length / 2);
        }
        yield return null;
        enemy._bossState = EnemyBase.BossState.AttackState;
    }

    public IEnumerator Attack(EnemyBase enemy)
    {
        yield return StartCoroutine(MoveBullets(_bulletSpawnEnemyOne));
        yield return new WaitForSeconds(_attackCoolTime);
        yield return StartCoroutine(MoveBullets(_bulletSpawnEnemyTwo));
        yield return new WaitForSeconds(_attackCoolTime);
        yield return StartCoroutine(MoveBullets(_bulletSpawnEnemyThree));
        yield return new WaitForSeconds(_attackCoolTime);
        Reset();
        enemy._bossState = EnemyBase.BossState.ChangeActionState;

    }

    public IEnumerator MoveBullets(BulletSpawnEnemy[] bulletSpawns)
    {
        foreach (var spawnPoint in bulletSpawns)
        {
            spawnPoint.MoveBullet();
            yield return new WaitForSeconds(_disAttackTime);
        }
    }

    public void Reset()
    {
        foreach (var dummyEnemy in _bossDammys)
        {
            var pos = dummyEnemy.transform.position;
            pos.x = _dummyDafaultPoint.position.x;
            dummyEnemy.transform.position = pos;
        }
        _currentTime = 0;
        _kunaiGimmick.SetActive(false);
    }

    public void ActionReset(EnemyBase enemy)
    {
        foreach (var dummyEnemy in _bossDammys)
        {
            var pos = dummyEnemy.transform.position;
            pos.x = _dummyDafaultPoint.position.x;
            dummyEnemy.transform.position = pos;
        }
        ResetBulletSpawn();
        _currentTime = 0;
        _kunaiGimmick.SetActive(false);

    }

    void ResetBulletSpawn()
    {
        var k = 0;
        for (var j = 0; j < _bulletSpawnEnemyThree.Length; j++)
        {
            if (j % 2 == 0)
            {
                _bulletSpawnEnemyTwo[k].ResetBullet();
            }
            else
            {
                _bulletSpawnEnemyOne[k].ResetBullet();
                k++;
            }
            _bulletSpawnEnemyThree[j].ResetBullet();
        }
    }
}
