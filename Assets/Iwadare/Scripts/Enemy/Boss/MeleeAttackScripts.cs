using System;
using System.Collections;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class MeleeAttackScripts
{
    [SerializeField] float _meleeWaitTime = 0.5f;
    float _meleeCurrentTime = 0f;
    [SerializeField] float _blowPowerX = 3f;
    [SerializeField] float _blowPowerY = 10f;
    Outline _attackTextOutLine;
    [SerializeField] float _disParryTime = 0.1f;
    float _currentDisParryTime;
    bool _isNotmeleeContinue;
    public void StartParryTime(EnemyBase enemy)
    {
        enemy._attackText.enabled = true;
        if (!_attackTextOutLine) _attackTextOutLine = enemy._attackText.GetComponent<Outline>();
        _currentDisParryTime = 0f;
    }

    public void MeleeUpdate()
    {
        if (_isNotmeleeContinue)
        {
            _meleeCurrentTime += Time.deltaTime;
            if (_meleeCurrentTime > _meleeWaitTime)
            {
                _meleeCurrentTime = 0f;
                _isNotmeleeContinue = false;
            }
        }
    }

    public void ParryTimeUpdate()
    {
        _currentDisParryTime += Time.deltaTime;
        if (_currentDisParryTime > _disParryTime)
        {
            SwitchOutLine();
            _currentDisParryTime = 0f;
        }
    }

    public void SwitchOutLine()
    {
        _attackTextOutLine.enabled = !_attackTextOutLine.enabled;
    }

    public void EndParryTime(EnemyBase enemy)
    {
        _attackTextOutLine.enabled = true;
        enemy._attackText.enabled = false;
    }

    public void MeleeAttackWait(EnemyBase enemy, Vector2 size, Vector2 pos, int damage)
    {
        if (!_isNotmeleeContinue)
        {
            MeleeAttack(enemy, size, pos, damage);
            _isNotmeleeContinue = true;
        }
    }

    public void MeleeAttack(EnemyBase enemy, Vector2 size, Vector2 pos, int damage)
    {
        var attackTargets = Physics2D.OverlapBoxAll(pos, size, 0f);
        foreach (var target in attackTargets)
        {
            if (target.transform == enemy.Player.transform)
            {
                enemy.Player.AddBulletDamage(damage);
                BlowPlayer(enemy);
                break;
            }
        }
    }

    public void BlowPlayer(EnemyBase enemy)
    {
        if ((enemy.Player._playerState & PlayerState.DeathState) != PlayerState.DeathState)
        {
            enemy.Player._playerState |= PlayerState.ImpactState;
            Debug.Log(enemy.Player._playerState);
            var playerVelocity = enemy.Player._playerRb;

            if (enemy._isFlip)
            {
                playerVelocity.AddForce(Vector2.right * _blowPowerX + Vector2.up * _blowPowerY, ForceMode2D.Impulse);
            }
            else
            {
                playerVelocity.AddForce(Vector2.left * _blowPowerX + Vector2.up * _blowPowerY, ForceMode2D.Impulse);
            }

        }
    }

    public void Reset()
    {
        _meleeCurrentTime = 0;
    }

}
