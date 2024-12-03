using System;
using UnityEngine;

public class YuaiChoiceAction : MonoBehaviour,ChoiceActionInterface
{
    [SerializeField] YuaiActions _yuaiActions = new();
    [Tooltip("HPに応じた行動"), Header("HPに応じた行動")]
    [SerializeField] YuaiActionStruct[] _action;
    [SerializeField] int _currentHPAction = 0;

    private void Start()
    {
        _yuaiActions._dropKunai.Init();
        _yuaiActions._yuaiSpecialAttack.Init();
        _yuaiActions._uiAttack.Init();
    }

    public AttackInterface ChoiceAttack()
    {
        switch (_action[_currentHPAction]._attackState[ChoiceAction(_action[_currentHPAction]._attackState.Length)])
        {
            case AttackStatesList.DashAttack:
                return _yuaiActions._dashAttack;
            case AttackStatesList.Attack2:
                return _yuaiActions._at2;
            case AttackStatesList.DropKunai:
                _yuaiActions._dropKunai.gameObject.SetActive(true);
                return _yuaiActions._dropKunai;
            case AttackStatesList.UIAttack:
                _yuaiActions._uiAttack.gameObject.SetActive(true);
                return _yuaiActions._uiAttack;
        }
        return _yuaiActions._at2;
    }

    public bool ChackHP(float currentHpPersent)
    {
        if (_action.Length <= _currentHPAction + 1) return false;
        //現在の体力が次のアクションに移行する体力を下回ったら次に移行する処理
        if (_action[_currentHPAction + 1]._hpPersent >= currentHpPersent)
        {
            _currentHPAction++;
            return true;
        }
        return false;
    }

    public bool ChackSpecial()
    {
        return _action[_currentHPAction]._specialAction;
    }

    int ChoiceAction(int maxActionCount)
    {
        UnityEngine.Random.InitState(DateTime.Now.Millisecond);
        int ans = UnityEngine.Random.Range(0, 100);
        return ans % maxActionCount;
    }

    public AttackInterface SelectSpecialAttack()
    {
        _yuaiActions._yuaiSpecialAttack.gameObject.SetActive(true);
        return _yuaiActions._yuaiSpecialAttack;
    }

    [Serializable]
    struct YuaiActionStruct
    {

        [Tooltip("HPの％"), Header("HPの％")]
        public float _hpPersent;

        [Tooltip("特殊アクション"), Header("特殊アクション")]
        public bool _specialAction;

        [Tooltip("攻撃のState"), Header("攻撃のState")]
        public AttackStatesList[] _attackState;
    }

    public enum AttackStatesList
    {
        DashAttack,
        Attack2,
        DropKunai,
        UIAttack,
    }
        
}
