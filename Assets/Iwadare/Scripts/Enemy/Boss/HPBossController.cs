using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HPBossController : EnemyBase,PauseTimeInterface
{
    BossState _currentActionState = BossState.StayState;
    AttackInterface _currentAction;
    public Image _timePanel;
    [SerializeField] float _specialAttackHP = 4;
    float _currentSpecialAttackHP;
    [SerializeField] SpecialAttackUI _specialAttackUI;
    IEnumerator _currentCoroutine;
    ChoiceActionInterface _enemyActions;
    Vector3 _tmpVelocity;
    float _tmpGravity;
    bool _isSpecialAttackMode = false;
    bool _gameEnd = false;
    public override bool SpecialHPChack() => _isSpecialAttackMode;
    [SerializeField] Animator _endAnimator;
    bool _death = false;

    void Start()
    {
        BaseInit();
        _specialAttackUI.InitHPView(_specialAttackHP);
        _specialAttackUI.gameObject.SetActive(false);
        _currentSpecialAttackHP = _specialAttackHP;
        _enemyActions = GetComponent<ChoiceActionInterface>();
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
        if (_timePanel) _timePanel.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        TimeScaleManager.ChangeTimeScaleAction += TimeScaleChange;
        TimeScaleManager.StartPauseAction += StartPause;
        TimeScaleManager.EndPauseAction += EndPause;
    }

    private void OnDisable()
    {
        TimeScaleManager.ChangeTimeScaleAction -= TimeScaleChange;
        TimeScaleManager.StartPauseAction -= StartPause;
        TimeScaleManager.EndPauseAction -= EndPause;
    }


    void Update()
    {

        if (GameStateManager.Instance.GameState != GameState.StayState
            && GameStateManager.Instance.GameState != GameState.BattleEndState)
        {
            StateUpdate();
        }

        if(!_gameEnd && GameStateManager.Instance.GameState == GameState.BattleEndState)
        {
            _gameEnd = true;
            if (_currentCoroutine != null)
            {
                StopCoroutine(_currentCoroutine);
                _currentCoroutine = null;
            }
            _currentAction.ActionReset(this);
        }
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

        if(_isSpecialAttackMode)
        {
            AddSpecialHPDamage();
        }
        else if (_enemyActions.ChackHP(_currentHP / MaxHP * 100))
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
        if (_currentHP <= 0)
        {
            ChangeAction();
            //死ぬ
            if(_enemyAnim._animType == AnimationType.SkeletonAnimator)
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

    public void AddSpecialHPDamage()
    {
        _currentSpecialAttackHP--;
        _specialAttackUI.HPDamageView();
        if (_currentSpecialAttackHP <= 0f)
        {
            _isSpecialAttackMode = false;
        }
    }

    void ChangeAction()
    {
        ResetState();
        if (_isSpecialAttackMode) _currentAction = _enemyActions.SelectSpecialAttack();
        else _currentAction = _enemyActions.ChoiceAttack();
        _bossState = BossState.StayState;
    }

    public void SpecialAttack()
    {
        _isSpecialAttackMode = true;
        GuardMode();
        _specialAttackUI.gameObject.SetActive(true);
        ChangeAction();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PAttack")
        {
            AddDamage(1);
        }
    }

    public void TimeScaleChange(float timeScale)
    {
        _timeScale = timeScale;
    }


    public void StartPause()
    {
        _timeScale = 0f;
        _tmpVelocity = _enemyRb.velocity;
        _enemyRb.velocity = Vector2.zero;
        _tmpGravity = _enemyRb.gravityScale;
        _enemyRb.gravityScale = 0f;
        //_enemyAnim._objAnimator.speed = 0f;
    }

    public void EndPause()
    {
        _timeScale = 1f;
        _enemyRb.velocity = _tmpVelocity;
        _enemyRb.gravityScale = _tmpGravity;
        //_enemyAnim._objAnimator.speed = 1f;
    }
}
