using Cinemachine;
using System.Collections;
using UnityEngine;

public class DataraSpecialAttack_Main : MonoBehaviour, AttackInterface
{
    [SerializeField] float _stayTime = 0.1f;
    [SerializeField] float _moveTime = 1.0f;
    [SerializeField] float _distancePoint = 3;
    [SerializeField] float _damageWaitTime = 0.5f;
    [SerializeField] float _TrueAttackWaitTime = 1f;
    [SerializeField] float _distanceAttack = 0.5f;
    [SerializeField] float _moveMagnitude = 1f;
    [SerializeField] float _runMagnitude = 2f;
    [SerializeField] float _runTime = 2f;
    [SerializeField] int _attackCount = 0;
    [SerializeField] int _damage = 2;
    [SerializeField] Transform _initMovePosition;
    [SerializeField] MeleeAttackScripts _meleeAttack;
    [SerializeField] AnimationClip _initNormalAttackAnim;
    [SerializeField] CinemachineVirtualCamera _bossUpCamera;
    [SerializeField] DataraSpecialAttack_UI _ui;
    [SerializeField] ParticleSystem _flareParticle;
    [SerializeField] AudioSource _attackStartAudio;
    [SerializeField] AudioSource _runAudio;
    [SerializeField] float _rayRadius = 3f;
    [SerializeField] LayerMask _wallLayer;
    float _currentTime = 0f;
    float _distance = 0f;
    public void Init()
    {
        _ui.Init();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawLine(transform.position, transform.position + Vector3.left * _rayRadius);
    }

    public void StayUpdate(EnemyBase enemy)
    {
        _currentTime += Time.deltaTime * enemy._timeScale;
        if (_currentTime > _stayTime)
        {
            enemy._bossState = EnemyBase.BossState.MoveState;
            _currentTime = 0f;
        }
    }

    public IEnumerator Move(EnemyBase enemy)
    {
        _distance = enemy.transform.position.x - _initMovePosition.position.x;
        enemy._enemyAnim.ChangeAnimationAnimator(AnimationName.Run);
        if (_bossUpCamera) _bossUpCamera.Priority = 20;
        var time = _ui.BossUpStart();
        _attackStartAudio.Play();
        _flareParticle.Play();
        while (_distance > _distancePoint || _distance < -_distancePoint)
        {
            ChackDistance(enemy);
            if (enemy._bossState == EnemyBase.BossState.DeathState)
            {
                yield break;
            }
            _currentTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        enemy.MoveEnemyX(true, 0);
        enemy._enemyAnim.ChangeAnimationAnimator(AnimationName.Idle);
        ChackDistancePlayer(enemy, false);
        while (_currentTime < time)
        {
            _currentTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        _ui.BossUpEnd();
        if (_bossUpCamera) _bossUpCamera.Priority = 0;
        enemy._bossState = EnemyBase.BossState.AttackState;
    }

    public IEnumerator Attack(EnemyBase enemy)
    {
        enemy.GuardMode();
        yield return WaitforSecondsCashe.Wait(_attackStartAudio.clip.length);
        while (enemy.SpecialHPChack())
        {
            enemy._enemyAnim._objAnimator.speed = 2f;
            enemy._enemyAnim.ChangeAnimationAnimator(AnimationName.Run);
            if (!_runAudio.isPlaying)_runAudio.Play();
            if(!_flareParticle.isPlaying)_flareParticle.Play();
            _attackStartAudio.Play();
            for (var time = 0f; time < _runTime; time += Time.deltaTime)
            {
                AttackMove(enemy);
                yield return new WaitForFixedUpdate();
            }
            enemy._bossAudio.ShieldAudioPlay();
            _meleeAttack.StartParryTime(enemy);
            enemy._isWaitDamage = true;
            //パリィ受付時間
            for (var i = 0f; i < _damageWaitTime; i += Time.deltaTime)
            {
                _meleeAttack.ParryTimeUpdate();
                AttackMove(enemy);
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
                enemy.HPChack();
                enemy._enemyRb.velocity = Vector2.zero;
                enemy._parryParticle.Play();
                _runAudio.Stop();
                enemy._enemyAnim._objAnimator.speed = 1f;
                enemy._enemyAnim.ChangeAnimationAnimator(AnimationName.Parry);
                enemy._bossAudio.ParryAudio();
                TimeScaleManager.Instance.TimeScaleChange(TimeScaleManager.Instance.DefaultTimeScale * 0.8f);
                InGameManager.Instance._playerSpecialGuage.AddGuage(InGameManager.Instance._playerSpecialGuage.ParryAddGuage);
                yield return WaitforSecondsCashe.Wait(_TrueAttackWaitTime);
                TimeScaleManager.Instance.TimeScaleChange(TimeScaleManager.Instance.DefaultTimeScale);
                enemy._isTrueDamage = false;
            }
        }
        enemy.BreakGuardMode();
        ActionReset(enemy);
        enemy._bossState = EnemyBase.BossState.ChangeActionState;
    }

    public void AttackMove(EnemyBase enemy)
    {
        var flip = enemy._isFlip;
        enemy.MoveEnemyX(!flip);
        var hit = Physics2D.RaycastAll(enemy.transform.position, flip ? Vector2.right : Vector2.left, _rayRadius, _wallLayer);
        if (hit.Length != 0)
        {
            enemy.BossObjFlipX(!flip);
        }
    }

    public void ActionReset(EnemyBase enemy)
    {
        enemy._enemyAnim._objAnimator.speed = 1f;
        enemy._enemyAnim.ChangeAnimationAnimator(AnimationName.Idle);
        if (_runAudio.isPlaying) _runAudio.Stop();
        if (_flareParticle.IsAlive()) _flareParticle.Stop();
    }

    public void ChackDistance(EnemyBase enemy, bool isMove = true)
    {
        _distance = enemy.transform.position.x - _initMovePosition.position.x;
        if (enemy.transform.position.x > _initMovePosition.position.x)
        {
            enemy.BossObjFlipX(false);
            if (isMove) enemy.MoveEnemyX(true, _moveMagnitude);
        }
        else
        {
            enemy.BossObjFlipX(true);
            if (isMove) enemy.MoveEnemyX(false, _moveMagnitude);
        }
    }

    public void ChackDistancePlayer(EnemyBase enemy, bool isMove = true)
    {
        _distance = Mathf.Abs(enemy.transform.position.x) - Mathf.Abs(enemy.Player.transform.position.x);
        if (enemy.transform.position.x > enemy.Player.transform.position.x)
        {
            enemy.BossObjFlipX(false);
            if (isMove) enemy.MoveEnemyX(true, _moveMagnitude);
        }
        else
        {
            enemy.BossObjFlipX(true);
            if (isMove) enemy.MoveEnemyX(false, _moveMagnitude);
        }
    }
}
