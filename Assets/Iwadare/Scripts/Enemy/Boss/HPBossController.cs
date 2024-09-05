using System;
using UnityEngine;

public class HPBossController : EnemyBase
{

    [Tooltip("HPに応じた行動"), Header("HPに応じた行動")]
    [SerializeField] HPAction[] _action;
    [SerializeField] int _currentHPAction = 0;
    BossState _currentActionState = 0;
    AttackInterface _currentAction;
    bool _guard = false;

    [SerializeField] AttackStates _attackStates = new();

    void Start()
    {
        _enemyRb = GetComponent<Rigidbody2D>();
        if (_enemyRb && _enemyRb.gravityScale != 0) _useGravity = true;
        _currentHP = MaxHP;
        DisplayHP();
        if (_action[_currentHPAction]._attackState.Length != 0)
        {
            // ChoiceActionでenumのAttackStateを選び、ChoiceAttackでAttackInterdfaceを継承している攻撃パターンを設定する。
            _currentAction = ChoiceAttack(_action[_currentHPAction]._attackState[ChoiceAction(_action[_currentHPAction]._attackState.Length)]);
        }
    }


    void Update()
    {
        switch (_bossState)
        {
            case BossState.StayState:
                _currentAction.StayUpdate(this);
                break;
            case BossState.MoveState:
                _currentAction.MoveUpdate(this);
                break;
            case BossState.AttackState:
                if (!_isAttack)
                {
                    _isAttack = true;
                    StartCoroutine(_currentAction.Attack(this));
                }
                break;
        }
    }

    public override void HPChack()
    {
        if (_currentHP <= 0)
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
                // ChoiceActionでenumのAttackStateを選び、ChoiceAttackでAttackInterdfaceを継承している攻撃パターンを設定する。
                _currentAction = ChoiceAttack(_action[_currentHPAction]._attackState[ChoiceAction(_action[_currentHPAction]._attackState.Length)]);
            }
        }
    }

    public void SpecialAttack()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PAttack")
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
        public AttackStates.AttackStatesList[] _attackState;
    }

}
