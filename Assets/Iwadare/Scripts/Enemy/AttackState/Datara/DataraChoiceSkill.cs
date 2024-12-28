﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataraChoiceSkill : MonoBehaviour,ChoiceActionInterface
{
    [SerializeField] DataraActions _dataraActions = new();
    [Tooltip("HPに応じた行動"), Header("HPに応じた行動")]
    [SerializeField] DataraActionStruct[] _action;
    [SerializeField] int _currentHPAction = 0;

    void Start()
    {

    }

    void Update()
    {

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

    public AttackInterface ChoiceAttack()
    {
        return _dataraActions._normalAttack;
    }

    public AttackInterface SelectSpecialAttack()
    {
        return _dataraActions._normalAttack;
    }

    [Serializable]
    struct DataraActionStruct
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
        Attack,
    }
}
