﻿using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBase : MonoBehaviour
{
    PlayerController _player;
    public AnimationController_Enemy _enemyObj;
    public PlayerController Player => _player;
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
    [SerializeField] BulletPoolActive _slashEffect;
    [SerializeField] float _moveSpeed = 2f;
    [SerializeField] SpriteRenderer _shieldRenderer;
    public Canvas _attackCanvas;

    public bool _isFlip = false;
    public bool _isWaitDamage = false;
    public bool _isTrueDamage = false;
    public bool _isMove = false;
    public bool _isAttack = false;
    public bool _guard = false;

    public void BaseInit()
    {
        _enemyRb = GetComponent<Rigidbody2D>();
        if (_enemyRb && _enemyRb.gravityScale != 0) _useGravity = true;
        _currentHP = MaxHP;
        DisplayHP();
        _shieldRenderer.enabled = false;
    }

    public void PlayerSet(PlayerController player)
    {
        _player = player;
    }

    public void AddDamage(float damage = 1f,HitEffect effect = HitEffect.Slash)
    {
        if (_isWaitDamage) _isTrueDamage = true;
        if (_guard) return;

        if (effect == HitEffect.Slash)
        {
            var effectObj = _slashEffect.GetPool().GetComponent<ParticleSystem>();
            if (effectObj != null)
            {
                effectObj.transform.position = transform.position;
                effectObj.Play();
            }
        }
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

    public virtual bool SpecialHPChack() => false;

    public void SpawnBulletRef(BulletSpawnEnemy spawnBulletEnemy)
    {
        StartCoroutine(spawnBulletEnemy.BulletSpawn());
    }

    public void SpawnBulletRefDangerous(BulletSpawnEnemy spawnBulletEnemy,float _time = 1f)
    {
        StartCoroutine(spawnBulletEnemy.BulletSpawnDangerous(_time));
    }

    /// <summary>BossSpriteのFlipの設定</summary>
    /// <param name="flip">左ならfalse右ならtrueを渡す。</param>
    public void BossObjFlipX(bool flip)
    {
        var scale = _enemyObj.transform.localScale;
        if (flip)
        {
            scale.x = -Mathf.Abs(scale.x);
            _isFlip = true;
        }
        else
        {
            scale.x = Mathf.Abs(scale.x);
            _isFlip = false;
        }
        _enemyObj.transform.localScale = scale;
    }

    public void MoveEnemyX(bool leftvertical)
    {
        var velocity = _enemyRb.velocity;
        velocity.x = leftvertical ? Vector2.left.x * _moveSpeed : Vector2.right.x * _moveSpeed;
        _enemyRb.velocity = velocity;
    }

    /// <summary>BossSpriteのRotationの設定。</summary>
    /// <param name="angle"></param>
    public void ObjSetRotation(float angle)
    {
        var rota = _enemyObj.transform.localEulerAngles;
        rota.z = angle;
        _enemyObj.transform.localEulerAngles = rota;
    }

    public void MeleeAttack(Vector2 size,Vector2 pos ,int damage)
    {
        var attackTargets = Physics2D.OverlapBoxAll(pos, size,0f);
        foreach(var target in attackTargets)
        {
            if(target.transform == _player.transform)
            {
                _player.AddBulletDamage(damage);
                break;
            }
        }
    }

    public void ResetState()
    {
        _isMove = false;
        _isAttack = false;
    }

    public void GuardMode()
    {
        _guard = true;
        _shieldRenderer.enabled = true;
    }

    public void BreakGuardMode()
    {
        _guard = false;
        _shieldRenderer.enabled = false;
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

public enum HitEffect
{
    None,
    Slash,
    Blow,
}
