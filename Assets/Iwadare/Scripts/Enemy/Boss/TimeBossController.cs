using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TimeBossController : EnemyBase, PauseTimeInterface
{
    float _elapTime;
    int _minute = 0;
    AttackInterface _currentAction;
    public Text _timeText;
    [SerializeField] float _specialAttackHP = 4;
    float _currentSpecialAttackHP;
    [SerializeField] SpecialAttackUI _specialAttackUI;
    ChoiceActionInterface _enemyActions;
    IEnumerator _currentCoroutine;
    Vector3 _tmpVelocity;
    float _tmpGravity;
    bool _isSpecialAttackMode = false;
    bool _gameEnd = false;
    public override bool SpecialHPChack() => _isSpecialAttackMode;
    [SerializeField] Animator _endAnimator;
    bool _death = false;
    [SerializeField] Transform _shakeObj;
    [SerializeField] float _shakeTime = 0.5f;
    [SerializeField] float _shakePower = 5f;
    Tween _damageTween;

    // Start is called before the first frame update
    void Start()
    {
        BaseInit();
        ChangeAction();
        HPChangeMinite();
        SetTime();
    }

    void ChangeAction()
    {
        ResetState();
        if (_isSpecialAttackMode) _currentAction = _enemyActions.SelectSpecialAttack();
        else _currentAction = _enemyActions.ChoiceAttack();
        _bossState = BossState.StayState;
    }


    void HPChangeMinite()
    {
        _minute = Mathf.Max(0, (int)_currentHP / 60);
    }

    void SetTime()
    {
        int setSecond = Mathf.Max(0, (int)_currentHP % 60);
        _timeText.text = $"{_minute.ToString("00")}:{(setSecond).ToString("00")}";
    }



    private void Update()
    {
        _currentHP -= Time.deltaTime;
        if (_minute > 0 && _currentHP - 60 * _minute < 0) _minute--;
        _elapTime += Time.deltaTime;
        DisplayHP();
        HPChack();
        SetTime();
    }

    private void FixedUpdate()
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
                if (!_isMove)
                {
                    _isMove = true;
                    _currentCoroutine = _currentAction.Move(this);
                    StartCoroutine(_currentCoroutine);
                }
                break;
            case BossState.AttackState:
                if (!_isAttack)
                {
                    _isAttack = true;
                    _currentCoroutine = _currentAction.Attack(this);
                    StartCoroutine(_currentCoroutine);
                }
                break;
            case BossState.ChangeActionState:
                ChangeAction();
                break;
        }
    }

    public override void HPChack()
    {
        if (_death) return;

        if (_damageTween != null && _damageTween.IsActive()) _shakeObj.DOComplete();
        _damageTween = _shakeObj.DOShakePosition(_shakeTime, _shakePower).SetLink(_shakeObj.gameObject);

        if (_enemyActions.ChackHP(_currentHP / MaxHP * 100))
        {
            if (!_isSpecialAttackMode)
            {
                _bossState = BossState.NextActionState;
                if (_currentCoroutine != null)
                {
                    StopCoroutine(_currentCoroutine);
                    _currentCoroutine = null;
                }
                _currentAction.ActionReset(this);
                if (_enemyActions.ChackSpecial())
                {
                    // スペシャルアタック発動
                    SpecialAttack();
                }
                else
                {
                    // 次に行動するアクションを決める。
                    ChangeAction();
                }
            }
        }
        if (_currentHP <= 0)
        {
            ChangeAction();
            //死ぬ
            if (_enemyAnim._animType == AnimationType.SkeletonAnimator)
            {
                _enemyAnim.ChangeAnimationSpain(AnimationName.Damage);
                _endAnimator.Play("End");
            }
            else
            {
                _enemyAnim.ChangeAnimationAnimator(AnimationName.Damage);
                _endAnimator.Play("End");
            }
            GameStateManager.Instance.EndBattle(true);
            _death = true;
        }
    }

    private void SpecialAttack()
    {
        return;
    }

    public void TimeScaleChange(float timeScale)
    {
        throw new NotImplementedException();
    }

    public void StartPause()
    {
        throw new NotImplementedException();
    }

    public void EndPause()
    {
        throw new NotImplementedException();
    }

    [Serializable]
    struct TimeAction
    {

        [Tooltip("経過時間"), Header("経過時間")]
        public float _hpPersent;

        [Tooltip("特殊アクション"), Header("特殊アクション")]
        public bool _specialAction;

        [Tooltip("攻撃のState"), Header("攻撃のState")]
        public AttackStatesList _attackState;
    }

    public enum AttackStatesList
    {
        DashAttack,
        Attack2,
    }
}
