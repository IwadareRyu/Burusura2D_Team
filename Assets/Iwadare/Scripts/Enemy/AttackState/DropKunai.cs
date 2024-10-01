using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class DropKunai : AttackInterface
{
    float _currentTime = 0f;
    [SerializeField] float _stayTime = 1f;
    [SerializeField] float _moveTime = 3f;
    [SerializeField] GameObject _kunaiGimmick;
    [SerializeField] BulletSpawnEnemy[] _bulletSpawnEnemyOne;
    [SerializeField] BulletSpawnEnemy[] _bulletSpawnEnemyTwo;
    [SerializeField] BulletSpawnEnemy[] _bulletSpawnEnemyThree;
    [SerializeField] Animator[] _bossDammys;
    [SerializeField] Transform _dummyMovePoint;
    Tween[] _moveTween;

    public void Init()
    {
        if(_kunaiGimmick) _kunaiGimmick.SetActive(false);
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
            _moveTween[i] = _bossDammys[i].transform.DOMoveX(_dummyMovePoint.position.x,_moveTime);
        }
        yield return null;
        //for(var j = 0;j < _bulletSpawnEnemyThree.Length;j++)
        //{

        //}
    }

    public IEnumerator Attack(EnemyBase enemy)
    {
        yield return null;
    }
    public void ActionReset(EnemyBase enemy)
    {

    }
}
