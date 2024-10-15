using DG.Tweening;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class DashAttack : AttackInterface,PauseTimeInterface
{
    float _currentTime = 0f;
    [SerializeField] Transform[] _trans;
    int _currentTrans = 0;
    bool _isMove = false;
    [SerializeField] float _stayTime = 1f;
    [SerializeField] float _moveTime = 1f;
    [SerializeField] float _attackTime = 1f;
    [SerializeField] float _attackDisTime = 0.2f;
    [SerializeField] BulletSpawnEnemy _bulletSpawnEnemy;
    Tween _moveTween;
    public void Init()
    {
        return;
    }

    public void StayUpdate(EnemyBase enemy)
    {
        _currentTime += Time.deltaTime * enemy._timeScale;
        if (_currentTime > _stayTime)
        {

            _currentTime = 0f;
            if (enemy._useGravity) 
            {
                enemy._enemyRb.gravityScale = 0;
                enemy._enemyRb.velocity = Vector2.zero;
            }
            enemy._bossState = EnemyBase.BossState.MoveState;
        }
    }

    public IEnumerator Move(EnemyBase enemy)
    {
        UnityActionSet();
        _moveTween = enemy.transform.DOMove(_trans[_currentTrans].position, _moveTime).SetLink(enemy.gameObject);
        yield return new WaitForSeconds(_moveTime / enemy._timeScale);
        _currentTrans++;
        Debug.Log("Attackに移行");
        enemy._bossState = EnemyBase.BossState.AttackState;
    }

    public IEnumerator Attack(EnemyBase enemy)
    {
        var currentDisTime = 0f;
        for (; _currentTrans < _trans.Length; _currentTrans++)
        {
            _bulletSpawnEnemy.DangerousSign();
            yield return new WaitForSeconds(_stayTime / enemy._timeScale);
            _moveTween = enemy.transform.DOMove(
                _trans[_currentTrans].position, 
                _attackTime / (Time.timeScale * enemy._timeScale)
                )
                .SetLink(enemy.gameObject);

            for (float currentAttackTime = 0; currentAttackTime < _attackTime; currentAttackTime += Time.deltaTime * enemy._timeScale)
            {
                currentDisTime += Time.deltaTime * enemy._timeScale;
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
        UnityActionReset();
        enemy.ResetState();
        enemy._bossState = EnemyBase.BossState.ChangeActionState;
        yield return null;
    }

    public void ActionReset(EnemyBase enemy)
    {
        if (_moveTween.IsActive())
        {
            _moveTween.Kill(false);
        }
        enemy.ResetState();
        _currentTrans = 0;
        if (enemy._useGravity) enemy._enemyRb.gravityScale = 1;
        _bulletSpawnEnemy.ResetBullet();
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

    public void TimeScaleChange(float timeScale)
    {
        _moveTween.timeScale = timeScale;
    }

    public void StartPause()
    {
        _moveTween.timeScale = 0f;
    }

    public void EndPause()
    {
        _moveTween.timeScale = 1f;
    }
}
