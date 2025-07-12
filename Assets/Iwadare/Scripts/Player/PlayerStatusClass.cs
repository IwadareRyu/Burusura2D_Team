using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

[Serializable]
public class PlayerStatusClass : MonoBehaviour
{
    [NonSerialized] public float _playerMaxHP;
    [NonSerialized] public int _playerMaxAttack;
    [NonSerialized] public int _playerMaxDiffence;
    [SerializeField] private float _currentHP;
    public float CurrentHP 
    { 
        get { return _currentHP; }
        set { _currentHP = value; }
    }

    [SerializeField] private int _currentAttack; 
    public int CurrentAttack 
    {
        get { return _currentAttack; }
        set { _currentAttack = value; }
    }

    [SerializeField] private int _currentDiffence;
    public int CurrentDiffence
    {
        get { return _currentDiffence; }
        set { _currentDiffence = value; }
    }

    public void SetStatus(TotalPlusStatus totalPlusStatus)
    {
        _currentHP = _playerMaxHP = totalPlusStatus.TotalHP;
        _currentAttack = _playerMaxAttack = totalPlusStatus.TotalATK;
        _currentDiffence = _playerMaxDiffence = totalPlusStatus.TotalDEF;
    }
}
