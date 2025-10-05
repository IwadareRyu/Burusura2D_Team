//using DG.Tweening;
//using System;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class TimeBossController : EnemyBase, PauseTimeInterface
//{
//    float _elapTime;
//    int _minute = 0;
//    AttackInterface _currentAction;
//    public Image _timePanel;
//    [SerializeField] float _specialAttackHP = 4;
//    float _currentSpecialAttackHP;
//    [SerializeField] SpecialAttackUI _specialAttackUI;
//    ChoiceActionInterface _enemyActions;
//    Vector3 _tmpVelocity;
//    float _tmpGravity;
//    bool _isSpecialAttackMode = false;
//    bool _gameEnd = false;
//    public override bool SpecialHPChack() => _isSpecialAttackMode;
//    [SerializeField] Animator _endAnimator;
//    bool _death = false;
//    [SerializeField] Transform _shakeObj;
//    [SerializeField] float _shakeTime = 0.5f;
//    [SerializeField] float _shakePower = 5f;
//    Tween _damageTween;

//    // Start is called before the first frame update
//    void Start()
//    {
//        BaseInit();
//        ChangeAction();
//        HPChangeMinite();
//        SetTime();
//    }

//    void ChangeAction()
//    {
//        ResetState();
//        if (_isSpecialAttackMode) _currentAction = _enemyActions.SelectSpecialAttack();
//        else _currentAction = _enemyActions.ChoiceAttack();
//        _bossState = BossState.StayState;
//    }


//    void HPChangeMinite()
//    {
//        _minute = Mathf.Max(0,(int)_currentHP / 60);
//    }

//    //void SetTime()
//    //{
//    //    int setSecond = Mathf.Max(0,(int)_currentHP % 60);
//    //    _timeText.text = $"{_minute.ToString("00")}:{(setSecond).ToString("00")}";
//    //}

//    //public AttackInterface ChoiceAttack(AttackStatesList attackStates)
//    //{
//    //    switch (attackStates)
//    //    {
//    //        case AttackStatesList.DashAttack:
//    //            return _attackStatesBoss1._dashAttack;
//    //        case AttackStatesList.Attack2:
//    //            return _attackStatesBoss1._at2;
//    //    }
//    //    return _attackStatesBoss1._at2;
//    //}

//    private void Update()
//    {
//        _currentHP -= Time.deltaTime;
//        if(_minute > 0 && _currentHP - 60 * _minute < 0) _minute--;
//        _elapTime += Time.deltaTime;
//        DisplayHP();
//        HPChack();
//        SetTime();
//    }

//    private void FixedUpdate()
//    {
//        StateUpdate();
//    }

//    void StateUpdate()
//    {
//        switch (_bossState)
//        {
//            case BossState.StayState:
//                _currentAction.StayUpdate(this);
//                break;
//            case BossState.MoveState:
//                if (!_isMove)
//                {
//                    _isMove = true;
//                    _currentCoroutine = StartCoroutine(_currentAction.Move(this));
//                }
//                break;
//            case BossState.AttackState:
//                if (!_isAttack)
//                {
//                    _isAttack = true;
//                    _currentCoroutine = StartCoroutine(_currentAction.Attack(this));
//                }
//                break;
//            case BossState.ChangeActionState:
//                ChangeAction();
//                break;
//        }
//    }

//    public override void HPChack()
//    {
//        if (_action.Length > _currentActionNumber + 1)
//        {
//            //現在の時間が次のアクションの指定時間になったら次に移行する処理。
//            if (_elapTime >= _action[_currentActionNumber + 1]._hpPersent)
//            {
//                _bossState = BossState.NextActionState;
//                if (_currentCoroutine != null)
//                {
//                    StopCoroutine(_currentCoroutine);
//                    _currentCoroutine = null;
//                }
//                _currentAction.ActionReset(this);
//                _currentActionNumber++;
//                //特殊攻撃の場合特殊攻撃に移行。
//                if (_action[_currentActionNumber]._specialAction)
//                {
//                    // 特殊攻撃
//                    SpecialAction();
//                }
//                // 次に行動するアクションを決める。
//                ChangeAction();
//            }
//        }
//        else
//        {
//            if (_currentHP < 0f)
//            {
//                Destroy(gameObject);
//            }   // HPが0になった時の処理
//        }
//    }

//    void SpecialAction()
//    {
//        return;
//    }

//    public void TimeScaleChange(float timeScale)
//    {
//        throw new NotImplementedException();
//    }

//    public void StartPause()
//    {
//        throw new NotImplementedException();
//    }

//    public void EndPause()
//    {
//        throw new NotImplementedException();
//    }

//    [Serializable]
//    struct TimeAction
//    {

//        [Tooltip("経過時間"), Header("経過時間")]
//        public float _hpPersent;

//        [Tooltip("特殊アクション"), Header("特殊アクション")]
//        public bool _specialAction;

//        [Tooltip("攻撃のState"), Header("攻撃のState")]
//        public AttackStatesList _attackState;
//    }

//    public enum AttackStatesList
//    {
//        DashAttack,
//        Attack2,
//    }
//}
