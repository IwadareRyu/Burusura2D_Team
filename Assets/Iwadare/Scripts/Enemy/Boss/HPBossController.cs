using UnityEngine;
using UnityEngine.UI;

public class HPBossController : EnemyBase
{
    BossState _currentActionState = BossState.StayState;
    AttackInterface _currentAction;
    Coroutine _currentCoroutine;
    [SerializeField] Image _timePanel;
    ChoiceActionInterface _enemyActions;

    void Start()
    {
        BaseInit();
        _enemyActions = GetComponent<ChoiceActionInterface>();
        ChangeAction();
        if(_timePanel) _timePanel.gameObject.SetActive(false);
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
                    _currentCoroutine = StartCoroutine(_currentAction.Move(this));
                }
                break;
            case BossState.AttackState:
                if (!_isAttack)
                {
                    _isAttack = true;
                    _currentCoroutine = StartCoroutine(_currentAction.Attack(this));
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
}
