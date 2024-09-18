using System;
using UnityEngine;

public class HPBossController : EnemyBase
{

    [Tooltip("HPに応じた行動"), Header("HPに応じた行動")]
    [SerializeField] HPAction[] _action;
    [SerializeField] int _currentHPAction = 0;
    BossState _currentActionState = 0;
    AttackInterface _currentAction;
    Coroutine _currentAttackCoroutine;

    [SerializeField] AttackStatesBoss1 _attackStatesBoss1 = new();


    void Start()
    {
        BaseInit();
        ChangeAction();
    }


    void Update()
    {
        StateUpdate();
    }

    void StateUpdate()
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
                    _currentAttackCoroutine = StartCoroutine(_currentAction.Attack(this));
                }
                break;
            case BossState.ChangeActionState:
                ChangeAction();
                break;
        }
    }

    public override void HPChack()
    {
        if (_action.Length > _currentHPAction + 1)
        {
            //現在の体力が次のアクションに移行する体力を下回ったら次に移行する処理
            if (_action[_currentHPAction + 1]._hpPersent >= _currentHP / MaxHP * 100)
            {
                _bossState = BossState.NextActionState;
                if (_currentAttackCoroutine != null)
                {
                    StopCoroutine(_currentAttackCoroutine);
                    _currentAttackCoroutine = null;
                }
                _currentAction.ActionReset(this);
                _currentHPAction++;
                //特殊攻撃の場合特殊攻撃に移行。
                if (_action[_currentHPAction]._specialAction)
                {
                    // 特殊攻撃
                    SpecialAttack();
                }
                // 次に行動するアクションを決める。
                ChangeAction();
            }
        }
        else
        {
            if (_currentHP <= 0)
            {
                //死ぬ
                Destroy(gameObject);
            }
        }
    }

    void ChangeAction()
    {
        if (_action[_currentHPAction]._attackState.Length != 0)
        {
            // ChoiceActionでenumのAttackStateを選び、ChoiceAttackでAttackInterdfaceを継承している攻撃パターンを設定する。
            _currentAction = ChoiceAttack(_action[_currentHPAction]._attackState[ChoiceAction(_action[_currentHPAction]._attackState.Length)]);
        }
        _bossState = BossState.StayState;
    }

    public AttackInterface ChoiceAttack(AttackStatesBoss1.AttackStatesList attackStates)
    {
        switch (attackStates)
        {
            case AttackStatesBoss1.AttackStatesList.DashAttack:
                return _attackStatesBoss1.dashAttack;
            case AttackStatesBoss1.AttackStatesList.Attack2:
                return _attackStatesBoss1.at2;
        }
        return _attackStatesBoss1.at2;
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
        public AttackStatesBoss1.AttackStatesList[] _attackState;
    }

}
