using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HPBossController : EnemyBase,PauseTimeInterface
{
    BossState _currentActionState = BossState.StayState;
    AttackInterface _currentAction;
    [SerializeField] Image _timePanel;
    IEnumerator _currentCoroutine;
    ChoiceActionInterface _enemyActions;
    Vector3 _tmpVelocity;
    float _tmpGravity;

    void Start()
    {
        BaseInit();
        _enemyActions = GetComponent<ChoiceActionInterface>();
        ChangeAction();
        if(_timePanel) _timePanel.gameObject.SetActive(false);
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
        if (_enemyActions.ChackHP(_currentHP / MaxHP * 100))
        {
            _bossState = BossState.NextActionState;
            if (_currentCoroutine != null)
            {
                StopCoroutine(_currentCoroutine);
                _currentCoroutine = null;
            }
            _currentAction.ActionReset(this);
            // 次に行動するアクションを決める。
            ChangeAction();
        }
        if (_currentHP <= 0)
        {
            //死ぬ
            Destroy(gameObject);
        }
    }

    void ChangeAction()
    {
        ResetState();
        _currentAction = _enemyActions.ChoiceAttack();
        _bossState = BossState.StayState;
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

    public void TimeScaleChange(float timeScale)
    {
        _timeScale = timeScale;
    }

    public void StartPause()
    {
        if(_isMove || _isAttack)
        {
            StopCoroutine(_currentCoroutine);
            Debug.Log("Stopですわ！");
        }
        _timeScale = 0f;
        _tmpVelocity = _enemyRb.velocity;
        _enemyRb.velocity = Vector2.zero;
        _tmpGravity = _enemyRb.gravityScale;
        _enemyRb.gravityScale = 0f;
    }

    public void EndPause()
    {
        _timeScale = 1f;
        if (_isAttack || _isMove)
        {
            StartCoroutine(_currentCoroutine);
        }
        _enemyRb.velocity = _tmpVelocity;
        _enemyRb.gravityScale = _tmpGravity;
    }
}
