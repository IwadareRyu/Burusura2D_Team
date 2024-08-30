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
        UnityEngine.Random.InitState(DateTime.Now.Millisecond);
        int ans = UnityEngine.Random.Range(0,100);
        return ans % maxActionCount;
    }

    public enum BossState
    {
        StayState,
        MoveState,
        AttackState,
        DeathState,
    }
}
