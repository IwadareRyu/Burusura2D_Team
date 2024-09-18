using System;
using UnityEngine;

public class TimeBossController : EnemyBase
{
    float _elapTime;
    
    [SerializeField]TimeAction[] _action;
    int _currentActionNumber = 0;
    AttackInterface _currentAction;

    [SerializeField] AttackStatesBoss1 _attackStatesBoss1 = new();
    private Coroutine _currentAttackCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        BaseInit();
        ChangeAction();
    }

    void ChangeAction()
    {
        // ChoiceActionでenumのAttackStateを選び、ChoiceAttackでAttackInterdfaceを継承している攻撃パターンを設定する。
        _currentAction = ChoiceAttack(_action[_currentActionNumber]._attackState);
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

    private void Update()
    {
        _currentHP -= Time.deltaTime;
        _elapTime += Time.deltaTime;
        DisplayHP();
        HPChack();

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
        if (_action.Length > _currentActionNumber + 1)
        {
            //現在の時間が次のアクションの指定時間になったら次に移行する処理。
            if (_elapTime >= _action[_currentActionNumber + 1]._hpPersent)
            {
                _bossState = BossState.NextActionState;
                if (_currentAttackCoroutine != null)
                {
                    StopCoroutine(_currentAttackCoroutine);
                    _currentAttackCoroutine = null;
                }
                _currentAction.ActionReset(this);
                _currentActionNumber++;
                //特殊攻撃の場合特殊攻撃に移行。
                if (_action[_currentActionNumber]._specialAction)
                {
                    // 特殊攻撃
                    SpecialAction();
                }
                // 次に行動するアクションを決める。
                ChangeAction();
            }
        }
        else
        {
            if (_currentHP < 0f)
            {
                Destroy(gameObject);
            }   // HPが0になった時の処理
        }
    }

    void SpecialAction()
    {
        return;
    }

    [Serializable]
    struct TimeAction
    {

        [Tooltip("経過時間"), Header("経過時間")]
        public float _hpPersent;

        [Tooltip("特殊アクション"), Header("特殊アクション")]
        public bool _specialAction;

        [Tooltip("攻撃のState"), Header("攻撃のState")]
        public AttackStatesBoss1.AttackStatesList _attackState;
    }
}
