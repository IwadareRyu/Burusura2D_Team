﻿using DG.Tweening;
using Spine.Unity;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class DropKunai : MonoBehaviour, AttackInterface, PauseTimeInterface
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
    [SerializeField] Transform[] _bossMovePosition;
    int _bossMoveNumber;
    [SerializeField] float _disMoveTime = 1f;
    float _movetimefirst = 0.5f;
    float _movetimelast = 1.5f;
    [SerializeField] SkeletonAnimation[] _bossDammys;
    [SpineAnimation]
    [SerializeField] string _moveName;
    [SerializeField] Transform _dummyMovePoint;
    [SerializeField] Transform _dummyDafaultPoint;
    [SerializeField] Transform _bossPosition;
    Tween[] _dummyMoveTween;
    Tween _bossMoveTween;

    public void Init()
    {
        if (_kunaiGimmick) _kunaiGimmick.SetActive(false);
    }

    public void StayUpdate(EnemyBase enemy)
    {
        _currentTime += Time.deltaTime * enemy._timeScale;
        if (_currentTime > _stayTime)
        {
            _currentTime = 0f;
            if (_kunaiGimmick) _kunaiGimmick.SetActive(true);
            if (enemy._useGravity)
            {
                enemy._enemyRb.gravityScale = 0;
                enemy._enemyRb.velocity = Vector2.zero;
            }
            _dummyMoveTween = new Tween[_bossDammys.Length];
            _bossMoveNumber = 0;
            enemy._bossState = EnemyBase.BossState.MoveState;
        }
    }
    public IEnumerator Move(EnemyBase enemy)
    {
        UnityActionSet();
        _bossMoveTween = enemy.transform.DOMove(_bossPosition.position,_moveTime).SetLink(enemy.gameObject);
        for (var i = 0; i < _bossDammys.Length; i++)
        {
            var state = _bossDammys[i].AnimationState;
            state.SetAnimation(0,_moveName,true);
            _dummyMoveTween[i] = _bossDammys[i].transform.DOMoveX(_dummyMovePoint.position.x, _moveTime);
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
            yield return WaitforSecondsCashe.Wait((_moveTime / _bulletSpawnEnemyThree.Length / 2));
        }
        yield return null;
        enemy._bossState = EnemyBase.BossState.AttackState;
    }

    public IEnumerator Attack(EnemyBase enemy)
    {
        enemy._enemyAnim.ChangeAnimationSpain(AnimationName.Idle);
        /// 攻撃1回目
        BossMove(enemy, _movetimefirst, false);
        StartCoroutine(MoveBullets(_bulletSpawnEnemyOne, enemy));
        yield return WaitforSecondsCashe.Wait(_disMoveTime);
        enemy._enemyAnim.ChangeAnimationSpain(AnimationName.Run);
        BossMove(enemy, _movetimelast,false);
        yield return WaitforSecondsCashe.Wait(_attackCoolTime);
        enemy._enemyAnim.ChangeAnimationSpain(AnimationName.Idle);
        /// 攻撃2回目
        BossMove(enemy, _movetimefirst,true);
        var bulletSpawnTwoReverse = _bulletSpawnEnemyTwo.Reverse().ToArray();
        StartCoroutine(MoveBullets(bulletSpawnTwoReverse, enemy));
        yield return WaitforSecondsCashe.Wait(_disMoveTime);
        enemy._enemyAnim.ChangeAnimationSpain(AnimationName.Run);
        BossMove(enemy, _movetimelast,true);
        yield return WaitforSecondsCashe.Wait(_attackCoolTime);
        enemy._enemyAnim.ChangeAnimationSpain(AnimationName.Idle);
        /// 攻撃3回目
        BossMove(enemy, _movetimefirst, false);
        StartCoroutine(SetRayBullets(_bulletSpawnEnemyThree, enemy));
        yield return WaitforSecondsCashe.Wait(_disMoveTime - 0.5f);
        enemy._enemyAnim.ChangeAnimationSpain(AnimationName.Run);
        BossMove(enemy, _movetimelast + 1f, false);
        yield return WaitforSecondsCashe.Wait(_attackCoolTime + 1f);
        /// 攻撃終了
        Reset(enemy);
        enemy._bossState = EnemyBase.BossState.ChangeActionState;

    }

    public void BossMove(EnemyBase enemy,float time,bool flip)
    {
        enemy.transform.DOMove(_bossMovePosition[_bossMoveNumber].position, time).SetLink(enemy.gameObject);
        enemy.BossObjFlipX(flip);
        _bossMoveNumber++;
    }

    public IEnumerator MoveBullets(BulletSpawnEnemy[] bulletSpawns,EnemyBase enemy)
    {
        foreach (var spawnPoint in bulletSpawns)
        {
            spawnPoint.MoveBullet();
            yield return WaitforSecondsCashe.Wait(_disAttackTime);
        }
    }

    public IEnumerator SetRayBullets(BulletSpawnEnemy[] bulletSpawns,EnemyBase enemy)
    {
        foreach (var spawnPoint in bulletSpawns)
        {
            spawnPoint.SetRay(spawnPoint.BulletDistance);
            yield return WaitforSecondsCashe.Wait(_disAttackTime);
        }
    }

    public void UnityActionSet()
    {
        TimeScaleManager.ChangeTimeScaleAction += TimeScaleChange;
        TimeScaleManager.StartPauseAction += StartPause;
        TimeScaleManager.EndPauseAction += EndPause;
    }

    private void UnityActionReset()
    {
        TimeScaleManager.ChangeTimeScaleAction -= TimeScaleChange;
        TimeScaleManager.StartPauseAction -= StartPause;
        TimeScaleManager.EndPauseAction -= EndPause;
    }

    private void Reset(EnemyBase enemy)
    {
        foreach (var dummyEnemy in _bossDammys)
        {
            var pos = dummyEnemy.transform.position;
            pos.x = _dummyDafaultPoint.position.x;
            dummyEnemy.transform.position = pos;
        }
        if (enemy._useGravity)
        {
            enemy._enemyRb.gravityScale = 1;
            enemy._enemyRb.velocity = Vector2.zero;
        }
        _currentTime = 0;
        UnityActionReset();
        enemy.ResetState();
        _kunaiGimmick.SetActive(false);
        enemy._enemyAnim.ChangeAnimationSpain(AnimationName.Idle);
    }

    public void ActionReset(EnemyBase enemy)
    {
        foreach (var dummyEnemy in _bossDammys)
        {
            var pos = dummyEnemy.transform.position;
            pos.x = _dummyDafaultPoint.position.x;
            dummyEnemy.transform.position = pos;
        }
        if (enemy._useGravity)
        {
            enemy._enemyRb.gravityScale = 1;
            enemy._enemyRb.velocity = Vector2.zero;
        }
        ResetBulletSpawn();
        UnityActionReset();
        _currentTime = 0;
        enemy.BossObjFlipX(false);
        enemy.ResetState();
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

    public void TimeScaleChange(float timeScale)
    {
        _bossMoveTween.timeScale = timeScale;
        foreach(var dummyTween in _dummyMoveTween)
        {
            dummyTween.timeScale = timeScale;
        }
    }

    public void StartPause()
    {
        _bossMoveTween.timeScale = 0f;
        foreach (var dummyTween in _dummyMoveTween)
        {
            dummyTween.timeScale = 0f;
        }
    }

    public void EndPause()
    {
        _bossMoveTween.timeScale = 1f;
        foreach (var dummyTween in _dummyMoveTween)
        {
            dummyTween.timeScale = 1f;
        }
    }
}
