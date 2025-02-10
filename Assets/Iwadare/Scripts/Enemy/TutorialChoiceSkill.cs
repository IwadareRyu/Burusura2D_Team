using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialChoiceSkill : ChoiceActionInterface
{
    [SerializeField] TutorialActions _tutorialAction = new();
    [Tooltip("HPに応じた行動"), Header("HPに応じた行動")]
    [SerializeField] TutorialActionStruct[] _action;
    [SerializeField] int _currentHPAction = 0;


    public bool ChackHP(float currentHpPersent)
    {
        _currentHPAction++;
        return true;
    }

    public bool ChackSpecial()
    {
        return false;
    }

    public AttackInterface ChoiceAttack()
    {
        switch (_action[_currentHPAction]._attackState[ChoiceAction(_action[_currentHPAction]._attackState.Length)])
        {
            case AttackStatesList.Move:
                return _tutorialAction._moveAction;
            case AttackStatesList.Attack:
                return _tutorialAction._attackAction;
            case AttackStatesList.Special1:
                return _tutorialAction._specialOne;
            case AttackStatesList.Special2:
                return _tutorialAction._specialTwo;
            case AttackStatesList.FinalSpecial:
                return _tutorialAction._finalSpecial;
        }
        return _tutorialAction._moveAction;
    }

    int ChoiceAction(int maxActionCount)
    {
        UnityEngine.Random.InitState(DateTime.Now.Millisecond);
        int ans = UnityEngine.Random.Range(0, 100);
        return ans % maxActionCount;
    }

    public AttackInterface SelectSpecialAttack()
    {
        return _tutorialAction._moveAction;
    }


    [Serializable]
    struct TutorialActionStruct
    {
        [Tooltip("攻撃のState"), Header("攻撃のState")]
        public AttackStatesList[] _attackState;
    }

    public enum AttackStatesList
    {
        Move,
        Attack,
        Special1,
        Special2,
        FinalSpecial,
    }
}
