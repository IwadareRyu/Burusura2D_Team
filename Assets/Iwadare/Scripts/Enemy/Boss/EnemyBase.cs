using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBase : MonoBehaviour
{
    [Tooltip("敵のMaxHP"), Header("敵のMaxHP")]
    [SerializeField] float _maxHP = 100;
    public float MaxHP => _maxHP;
    [Tooltip("敵の現在HP")]
    [NonSerialized]public float _currentHP;
    [Tooltip("HP表示"), Header("HP表示")]
    [SerializeField] Slider _hpSlider;
    public BossState _bossState = BossState.StayState;
    public Rigidbody2D _enemyRb;
    public bool _useGravity = false;
    [SerializeField] float _stayTime = 0.5f;
    public float StayTime => _stayTime;

    [SerializeField] float _moveTime = 0.5f;
    public float MoveTime => _moveTime;

    [NonSerialized]public float _timeScale = 1f;

    public bool _isMove = false;
    public bool _isAttack = false;
    public bool _guard = false;

    public void BaseInit()
    {
        _enemyRb = GetComponent<Rigidbody2D>();
        if (_enemyRb && _enemyRb.gravityScale != 0) _useGravity = true;
        _currentHP = MaxHP;
        DisplayHP();
    }

    public void AddDamage(float damage = 1f)
    {
        if (_guard) return;
        _currentHP -= damage;
        DisplayHP();
        HPChack();
    }

    public void DisplayHP()
    {
        if (_hpSlider != null)
        {
            _hpSlider.value = _currentHP / _maxHP;
        }
    }

    public virtual void HPChack() { return; }

    public void SpawnBulletRef(BulletSpawnEnemy spawnBulletEnemy)
    {
        StartCoroutine(spawnBulletEnemy.BulletSpawn());
    }

    public void ResetState()
    {
        _isMove = false;
        _isAttack = false;
    }

    public void GuardMode()
    {
        _guard = true;
    }

    public void BreakGuardMode()
    {
        _guard = false;
    }

    public enum BossState
    {
        StayState,
        MoveState,
        AttackState,
        ChangeActionState,
        NextActionState,
        DeathState,
    }
}
