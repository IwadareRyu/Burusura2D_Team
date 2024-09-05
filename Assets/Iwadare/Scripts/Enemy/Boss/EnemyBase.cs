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

    public bool _isMove = false;
    public bool _isAttack = false;

    [SerializeField] AttackStates _attackStates = new();
    public AttackStates AttackStates => _attackStates;

    public void AddDamage(float damage)
    {
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

    public int ChoiceAction(int maxActionCount)
    {
        ResetState();
        UnityEngine.Random.InitState(DateTime.Now.Millisecond);
        int ans = UnityEngine.Random.Range(0,100);
        return ans % maxActionCount;
    }

    public AttackInterface ChoiceAttack(AttackStates.AttackStatesList attackStates)
    {
        switch(attackStates)
        {
            case AttackStates.AttackStatesList.DashAttack:
                return _attackStates.dashAttack;
            case AttackStates.AttackStatesList.Attack2:
                return _attackStates.at2;
        }
        return _attackStates.at2;
    }

    void ResetState()
    {
        _isMove = false;
        _isAttack = false;
    }

    public enum BossState
    {
        StayState,
        MoveState,
        AttackState,
        DeathState,
    }
}
