using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HPBossController : EnemyBase
{

    [Tooltip("HPに応じた行動"), Header("HPに応じた行動")]
    [SerializeField] HPAction[] _action;
    [SerializeField] int _currentHPAction = 0;
    BossState _currentActionState = 0;
    Transform _currentAction;
    bool _guard = false;
    AttackStateBase _attackStateBase = new();

    void Start()
    {
        _currentHP = MaxHP;
        DisplayHP();
        if (_action[_currentHPAction]._attackState.Length != 0)
        {
            _currentAction = _action[_currentHPAction]._attackState[ChoiceAction(_action[_currentHPAction]._attackState.Length)];
        }
    }


    void Update()
    {

    }

    public override void HPChack()
    {
        if(_currentHP <= 0)
        {
            //死ぬ
            Destroy(gameObject);
        }

        if (_action.Length <= _currentHPAction + 1) return;
        
        //現在の体力が次のアクションに移行する体力を下回ったら次に移行する処理
        if (_action[_currentHPAction + 1]._hpPersent >= _currentHP / MaxHP * 100)
        {
            _currentHPAction++;
            //特殊攻撃の場合特殊攻撃に移行。
            if (_action[_currentHPAction]._specialAction)
            {
                // 特殊攻撃
                SpecialAttack();
            }
            // 次に行動するアクションを決める。
            if (_action[_currentHPAction]._attackState.Length != 0)
            {
                _currentAction = _action[_currentHPAction]._attackState[ChoiceAction(_action[_currentHPAction]._attackState.Length)];
            }
        }
    }

    public void SpecialAttack()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "PAttack")
        {
            AddDamage(1);
        }
    }

    [Serializable]
    struct HPAction
    {

        [Tooltip("HPの％"), Header("HPの％")]
        public float _hpPersent;

        [Tooltip("特殊アクション"), Header("特殊アクション")]
        public bool _specialAction;

        [Tooltip("攻撃のState"), Header("攻撃のState")]
        public Transform[] _attackState;
    }

}
