using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;


public class NormalAttackState : MonoBehaviour,AttackInterface
{
    [SerializeField] float _stayTime = 0.1f;
    [SerializeField] float _moveTime = 1.0f;
    [SerializeField] float _distancePlayer = 3;
    [SerializeField] float _damageWaitTime = 0.5f;
    [SerializeField] float _TrueAttackWaitTime = 1f;
    [SerializeField] float _distanceAttack = 0.5f;
    [SerializeField] float _moveMagnitude = 1f;
    [SerializeField] int _attackCount = 0;
    [SerializeField] int _damage = 2;
    [SerializeField] Vector2 _firstAttackPosition;
    [SerializeField] Vector2 _firstAttackSize;
    [SerializeField] ParticleDestroy _firstParticle;
    [SerializeField] Vector2 _secondAttackPosition;
    [SerializeField] Vector2 _secondAttackSize;
    [SerializeField] Transform _secondParticle;
    [SerializeField] MeleeAttackScripts _meleeAttack;
    [SerializeField] AnimationClip _initNormalAttackAnim;
    float _currentTime = 0f;
    float _distance = 0f;

    public void Init()
    {
        return;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        var first = new Vector2(transform.position.x + _firstAttackPosition.x, transform.position.y + _firstAttackPosition.y);
        var second = new Vector2(transform.position.x + _secondAttackPosition.x, transform.position.y + _secondAttackPosition.y);
        Gizmos.DrawWireCube(first,_firstAttackSize);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(second, _secondAttackSize);
    }


    public void StayUpdate(EnemyBase enemy)
    {
        _currentTime += Time.deltaTime * enemy._timeScale;
        if (_currentTime > _stayTime)
        {
            enemy._bossState = EnemyBase.BossState.MoveState;
        }
    }

    public IEnumerator Move(EnemyBase enemy)
    {
         _distance = enemy.transform.position.x - enemy.Player.transform.localPosition.x;
        enemy._enemyAnim.ChangeAnimationAnimator(AnimationName.Run);
        while (_distance > _distancePlayer || _distance < -_distancePlayer)
        {
            ChackDistance(enemy);
            yield return new WaitForFixedUpdate();
        }
        enemy._enemyAnim.ChangeAnimationAnimator(AnimationName.Idle);
        enemy._bossState = EnemyBase.BossState.AttackState;
    }

    public IEnumerator Attack(EnemyBase enemy)
    {
        enemy.GuardMode();
        Debug.Log("攻撃待機");
        enemy._enemyRb.velocity = Vector3.zero;
        _meleeAttack.StartParryTime(enemy);
        enemy._enemyAnim._objAnimator.Play(_initNormalAttackAnim.name);
        enemy._isWaitDamage = true;
        // パリィ受付時間
        for (var i = 0f; i < _damageWaitTime; i += Time.deltaTime)
        {
            _meleeAttack.ParryTimeUpdate();
            if (enemy._isTrueDamage)
            {
                break;
            }
            yield return new WaitForFixedUpdate();
        }
        enemy._isWaitDamage = false;
        _meleeAttack.EndParryTime(enemy);
        //パリィ成功時
        if (enemy._isTrueDamage)
        {
            enemy.BreakGuardMode();
            enemy._parryParticle.Play();
            enemy._bossAudio.ParryAudio();
            TimeScaleManager.Instance.TimeScaleChange(TimeScaleManager.Instance.DefaultTimeScale * 0.8f);
            InGameManager.Instance._playerSpecialGuage.AddGuage(InGameManager.Instance._playerSpecialGuage.ParryAddGuage);
            yield return WaitforSecondsCashe.Wait(_TrueAttackWaitTime);
            TimeScaleManager.Instance.TimeScaleChange(TimeScaleManager.Instance.DefaultTimeScale);
            enemy._isTrueDamage = false;
            enemy._enemyAnim.ChangeAnimationAnimator(AnimationName.Idle);
        }
        else
        {
            enemy._enemyAnim._objAnimator.SetBool("IsAttack",true);
            for (var i = 0; i < _attackCount; i++)
            {
                Vector2 position;
                if (i % 2 == 0)
                {
                    position = _firstAttackPosition;
                    if (enemy._isFlip) position.x = -position.x;
                    position = new Vector2(transform.position.x + position.x, transform.position.y + position.y);
                    _meleeAttack.MeleeAttack(enemy,_firstAttackSize, position, _damage);
                    var AttackParticle = Instantiate(_firstParticle, position, _firstParticle.transform.rotation);
                    if (enemy._isFlip)
                    {
                        var particleSize = AttackParticle.transform.localScale;
                        particleSize.x = -particleSize.x;
                        AttackParticle.transform.localScale = particleSize;
                    }
                    AttackParticle.Init();
                }
                else
                {
                    position = _secondAttackPosition;
                    if (enemy._isFlip) position.x = -position.x;
                    position = new Vector2(transform.position.x + position.x, transform.position.y + position.y);
                    _meleeAttack.MeleeAttack(enemy, _firstAttackSize, position, _damage);
                    var AttackParticle = Instantiate(_secondParticle, position,_secondParticle.transform.rotation).GetComponentInChildren<ParticleDestroy>();
                    if (enemy._isFlip)
                    {
                        var particleSize = AttackParticle.transform.localScale;
                        particleSize.z = -particleSize.z;
                        AttackParticle.transform.localScale = particleSize;
                    }
                    AttackParticle.Init();
                }
                yield return WaitforSecondsCashe.Wait(_distanceAttack);
                ChackDistance(enemy, false);
            }
            enemy.BreakGuardMode();
        }
        yield return null;
        ActionReset(enemy);
        enemy._bossState = EnemyBase.BossState.ChangeActionState;
    }

    public void ActionReset(EnemyBase enemy)
    {
        enemy._enemyAnim._objAnimator.SetBool("IsAttack", false);
    }

    public void ChackDistance(EnemyBase enemy,bool isMove = true)
    {
        _distance = Mathf.Abs(enemy.transform.position.x) - Mathf.Abs(enemy.Player.transform.position.x);
        if (enemy.transform.position.x > enemy.Player.transform.position.x)
        {
            enemy.BossObjFlipX(false);
            if(isMove)enemy.MoveEnemyX(true,_moveMagnitude);
        }
        else
        {
            enemy.BossObjFlipX(true);
            if(isMove)enemy.MoveEnemyX(false,_moveMagnitude);
        }
    }
}
