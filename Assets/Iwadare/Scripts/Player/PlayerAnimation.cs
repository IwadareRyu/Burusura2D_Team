using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] Animator _downAnim;
    [SerializeField] Animator _upAnim;
    bool _isMove = false;
    bool _isGround = true;
    bool _isAvoid = false;
    bool _isAttack = false;
    bool _attackTrigger = false;
    bool _isDeath = false;
    private void Start()
    {
        _downAnim.SetBool("IsGround",_isGround);
    }

    public void IsMove()
    {
        if (!_isMove)
        {
            _downAnim.SetFloat("MoveSpeed", 1);
            _isMove = true;
        }
        else
        {
            _downAnim.SetFloat("MoveSpeed", 0);
            _isMove = false;
        }
    }

    public void FlipX()
    {
        var scale = _downAnim.transform.localScale;
        scale.x *= -1;
        _downAnim.transform.localScale = scale;
    }

    public void IsJump()
    {
        if (_isGround)
        {
            var ct = this.GetCancellationTokenOnDestroy();
            JumpTime(ct).Forget();
        }
    }

    async UniTask JumpTime(CancellationToken ct)
    {
        _isGround = false;
        _downAnim.SetBool("IsGround",_isGround);
        await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: ct);
        _isGround = true;
        _downAnim.SetBool("IsGround", _isGround);
    }

    public void IsAvoid()
    {
        if (!_isAvoid)
        {
            var ct = this.GetCancellationTokenOnDestroy();
            AvoidTime(ct).Forget();
        }
    }

    async UniTask AvoidTime(CancellationToken ct)
    {
        _isAvoid = true;
        _downAnim.SetBool("Avoid",_isAvoid);
        await UniTask.Delay(TimeSpan.FromSeconds(1f),cancellationToken: ct);
        _isAvoid = false;
        _downAnim.SetBool("Avoid", _isAvoid);
    }

    public void IsAttack()
    {
        if(!_isAttack)
        {
            var ct = this.GetCancellationTokenOnDestroy();
            AttackTime(ct).Forget();
        }
        else
        {
            _attackTrigger = true;
        }
    }

    async UniTask AttackTime(CancellationToken ct)
    {
        _isAttack = true;
        _downAnim.SetBool("Attack", _isAttack);
        _upAnim.SetBool("IsAttack", _isAttack);
        await AttackTrigger(ct);
        if (ct.IsCancellationRequested) return;
        _isAttack = false;
        _downAnim.SetBool("Attack", _isAttack);
        _upAnim.SetBool("IsAttack", _isAttack);
    }

    async UniTask AttackTrigger(CancellationToken ct,int count = 1)
    {
        _upAnim.SetTrigger("AttackTrigger");
        for (var time = 0f;time < 1f && !ct.IsCancellationRequested;time += Time.deltaTime)
        {
            if(_attackTrigger && count < 3)
            {
                _attackTrigger = false;
                await AttackTrigger(ct, count + 1);
                return;
            }
            await UniTask.Delay(TimeSpan.FromSeconds(Time.deltaTime), cancellationToken: ct);
        }
    }

    public void Death()
    {
        _isDeath = true;
        _downAnim.Play("Death");
    }
}
