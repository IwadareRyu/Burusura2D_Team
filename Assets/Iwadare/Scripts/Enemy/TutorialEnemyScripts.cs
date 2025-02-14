using System.Collections;

public class TutorialEnemyScripts : EnemyBase
{
    ChoiceActionInterface _enemyActions;
    AttackInterface _currentAction;
    IEnumerator _currentCoroutine;
    bool _gameEnd;
    float _nextAction = 0;

    void Start()
    {
        BaseInit();
        _enemyActions = GetComponent<ChoiceActionInterface>();
        // 次に行動するアクションを決める。
        ChangeAction();
        GuardMode();
    }

    void Update()
    {

        if (GameStateManager.Instance.GameState != GameState.StayState
            && GameStateManager.Instance.GameState != GameState.BattleEndState)
        {
            StateUpdate();
        }

        if (!_gameEnd && GameStateManager.Instance.GameState == GameState.BattleEndState)
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

    public override void NextAction()
    {
        _nextAction++;
        _enemyActions.ChackHP(_nextAction);
    }

    void ChangeAction()
    {
        ResetState();
        _currentAction = _enemyActions.ChoiceAttack();
        _bossState = BossState.StayState;
    }
}
