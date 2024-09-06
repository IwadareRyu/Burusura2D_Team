using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class DashAttack : AttackInterface
{
    float _currentTime = 0f;
    [SerializeField] Transform[] _trans;
    int _currentTrans = 0;
    bool _isMove = false;
    [SerializeField] float _attackTime = 1f;
    [SerializeField] float _attackDisTime = 0.2f;
    [SerializeField] BulletSpawnEnemy _bulletSpawnEnemy;
    bool _isAttackTime = false;
    IEnumerator _moveCoroutine;
    Tween _moveTween;

    public void StayUpdate(EnemyBase enemy)
    {
        _currentTime += Time.deltaTime;
        if (_currentTime > enemy.StayTime)
        {
            _currentTime = 0f;
            if (enemy._useGravity) 
            {
                enemy._enemyRb.gravityScale = 0;
                enemy._enemyRb.velocity = Vector2.zero;
            }
            if(_moveCoroutine != null)_moveCoroutine = null;
            enemy._bossState = EnemyBase.BossState.MoveState;
        }
    }


    public void MoveUpdate(EnemyBase enemy)
    {
        if (_moveCoroutine == null)
        {
            _moveCoroutine = Move(enemy);
        }
        else
        {
            _moveCoroutine.MoveNext();
        }
    }

    public IEnumerator Move(EnemyBase enemy)
    {
        yield return _moveTween = enemy.transform.DOMove(_trans[_currentTrans].position, enemy.MoveTime).SetLink(enemy.gameObject);
        _currentTrans++;
        enemy._bossState = EnemyBase.BossState.AttackState;
    }

    public IEnumerator Attack(EnemyBase enemy)
    {
        var currentDisTime = 0f;
        for (; _currentTrans < _trans.Length; _currentTrans++)
        {
            _bulletSpawnEnemy.DangerousSign();
            yield return new WaitForSeconds(enemy.StayTime);
            _moveTween = enemy.transform.DOMove(_trans[_currentTrans].position, _attackTime).SetLink(enemy.gameObject);
            for (float currentAttackTime = 0; currentAttackTime < _attackTime; currentAttackTime += Time.deltaTime)
            {
                currentDisTime += Time.deltaTime;
                if (currentDisTime >= _attackDisTime)
                {
                    enemy.SpawnBulletRef(_bulletSpawnEnemy);
                    currentDisTime = 0f;
                }
                yield return new WaitForFixedUpdate();
            }
            currentDisTime = 0f;
        }
        _currentTrans = 0;
        if (enemy._useGravity) enemy._enemyRb.gravityScale = 1;
        enemy.ResetState();
        enemy._bossState = EnemyBase.BossState.ChangeActionState;
        yield return null;
    }

    public void ActionReset(EnemyBase enemy)
    {
        if (_moveTween.IsPlaying())
        {
            _moveTween.Kill(false);
        }
        enemy.ResetState();
        _currentTrans = 0;
        if (enemy._useGravity) enemy._enemyRb.gravityScale = 1;
    }
}
