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
    bool _isAttackTime = false;
    IEnumerator _moveCoroutine;

    public void StayUpdate(EnemyBase enemy)
    {
        _currentTime += Time.deltaTime;
        if (_currentTime > enemy.StayTime)
        {
            _currentTime = 0f;
            if (enemy._useGravity) enemy._enemyRb.gravityScale = 0;
            enemy._bossState = EnemyBase.BossState.MoveState;
        }
    }


    public void MoveUpdate(EnemyBase enemy)
    {
        if(_moveCoroutine == null)
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
        yield return enemy.transform.DOMove(_trans[_currentTrans].position, enemy.MoveTime);
        _currentTrans++;
        enemy._bossState = EnemyBase.BossState.AttackState;
        _moveCoroutine = null;
    }

    public IEnumerator Attack(EnemyBase enemy)
    {
        yield return new WaitForSeconds(enemy.StayTime);
        var currentDisTime = 0f;
        for (; _currentTrans >= _trans.Length; _currentTrans++)
        {
            enemy.transform.DOMove(_trans[_currentTrans].position, _attackTime);
            for (float currentAttackTime = 0; currentAttackTime < _attackTime; currentAttackTime += Time.deltaTime)
            {

                yield return new WaitForFixedUpdate();
            }
        }
        yield return null;
    }
}
