using System.Collections;
using UnityEngine;

public class JumpAttack : MonoBehaviour, AttackInterface
{
    [SerializeField] float _stayTime = 0.1f;
    [SerializeField] float _jumpWaitTime = 0.5f;
    [SerializeField] float _jumpTime = 1f;
    [SerializeField] float _damageWaitTime = 0.5f;
    [SerializeField] float _TrueAttackWaitTime = 1f;
    [SerializeField] Vector2 _attackPosition;
    [SerializeField] Vector2 _attackSize;
    [SerializeField] Vector2 _jumpJuageSize;
    [SerializeField] Vector2 _jumpPower = new Vector2(5, 10);
    [SerializeField] int _jumpDamage = 2;
    [SerializeField] int _attackDamage = 10;
    [SerializeField] Vector2 _attackBlowMag = new Vector2(2,2);
    [SerializeField] MeleeAttackScripts _meleeAttack;
    [SerializeField] Transform _attackParticle;
    [SerializeField] float _playerDistance = 10f;
    [SerializeField] float _distancePoint = 1f;
    [SerializeField] float _moveMagnitude = 2f;
    float _currentTime = 0f;
    float _distance;

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        var first = new Vector2(transform.position.x + _attackPosition.x, transform.position.y + _attackPosition.y);
        Gizmos.DrawWireCube(first, _attackSize);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, _jumpJuageSize);
    }

    public void Init()
    {

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
        float movePointX;

        /// 移動ポイントが壁の外かを判定する処理
        if (enemy.Player.transform.position.x - _playerDistance < enemy._movePoint[enemy._minMovePointIndex].position.x)
        {
            movePointX = enemy.Player.transform.position.x + _playerDistance;
        }
        else if (enemy.Player.transform.position.x + _playerDistance > enemy._movePoint[enemy._movePoint.Length - 1].position.x)
        {
            movePointX = enemy.Player.transform.position.x - _playerDistance;
        }
        else
        {
            movePointX = RamdomMethod.RamdomNumber(99) <= 50 ?
                enemy.Player.transform.position.x + _playerDistance : enemy.Player.transform.position.x - _playerDistance;
        }

        _distance = enemy.transform.position.x - movePointX;
        while (_distance > _distancePoint || _distance < -_distancePoint)
        {
            ChackDistance(enemy, movePointX);
            yield return new WaitForFixedUpdate();
        }
        enemy._bossState = EnemyBase.BossState.AttackState;
    }

    public IEnumerator Attack(EnemyBase enemy)
    {
        ChackDistance(enemy, enemy.Player.transform.position.x, false);
        enemy._enemyRb.velocity = Vector2.zero;
        yield return WaitforSecondsCashe.Wait(_jumpWaitTime);
        if (enemy._isFlip)
        {
            enemy._enemyRb.AddForce(Vector2.right * _jumpPower.x + Vector2.up * _jumpPower.y,ForceMode2D.Impulse);
        }
        else
        {
            enemy._enemyRb.AddForce(Vector2.left * _jumpPower.x + Vector2.up * _jumpPower.y,ForceMode2D.Impulse);
        }

        for (float i = 0; i < _jumpTime; i += Time.deltaTime)
        {
            _meleeAttack.MeleeAttackWait(enemy, _jumpJuageSize, transform.position, _jumpDamage);
            _meleeAttack.MeleeUpdate();
            yield return new WaitForFixedUpdate();
        }
        enemy.GuardMode();
        Debug.Log("攻撃待機");
        _meleeAttack.StartParryTime(enemy);
        enemy._isWaitDamage = true;
        for (var i = 0f; i < _damageWaitTime; i += Time.deltaTime)
        {
            _meleeAttack.MeleeAttackWait(enemy, _jumpJuageSize, transform.position, _jumpDamage);
            _meleeAttack.MeleeUpdate();
            _meleeAttack.ParryTimeUpdate();
            if (enemy._isTrueDamage)
            {
                break;
            }
            yield return new WaitForFixedUpdate();
        }
        enemy._isWaitDamage = false;
        _meleeAttack.EndParryTime(enemy);

        if (enemy._isTrueDamage)
        {
            enemy._enemyRb.velocity = Vector2.zero;
            enemy.BreakGuardMode();
            enemy._parryParticle.Play();
            TimeScaleManager.Instance.TimeScaleChange(TimeScaleManager.Instance.DefaultTimeScale * 0.8f);
            InGameManager.Instance._playerSpecialGuage.AddGuage(20);
            yield return WaitforSecondsCashe.Wait(_TrueAttackWaitTime);
            TimeScaleManager.Instance.TimeScaleChange(TimeScaleManager.Instance.DefaultTimeScale);
            enemy._isTrueDamage = false;
        }
        else
        {
            var position = _attackPosition;
            if (enemy._isFlip) position.x = -position.x;
            position = new Vector2(transform.position.x + position.x, transform.position.y + position.y);
            _meleeAttack.MeleeAttack(enemy, _attackSize, position, _attackDamage);
            var AttackParticle = Instantiate(_attackParticle, position, _attackParticle.transform.rotation).GetComponentInChildren<ParticleDestroy>();
            if (enemy._isFlip)
            {
                var particleSize = AttackParticle.transform.localScale;
                particleSize.z = -particleSize.z;
                AttackParticle.transform.localScale = particleSize;
            }
            AttackParticle.Init();
        }
        ChackDistance(enemy,enemy.Player.transform.position.x, false);
        enemy.BreakGuardMode();
        ActionReset(enemy);
        enemy._bossState = EnemyBase.BossState.ChangeActionState;
    }

    public void ActionReset(EnemyBase enemy)
    {
        _currentTime = 0;
        _meleeAttack.Reset();
    }

    public void ChackDistance(EnemyBase enemy, float movePointX, bool isMove = true)
    {
        _distance = Mathf.Abs(enemy.transform.position.x) - Mathf.Abs(movePointX);
        if (enemy.transform.position.x > movePointX)
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
