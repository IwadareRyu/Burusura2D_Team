using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

[Serializable]
public class PlayerStatusClass : MonoBehaviour
{
    [SerializeField] float _playerDefaultHP = 100f;
    [SerializeField] int _playerDefaultAttack = 1;
    [SerializeField] int _playerDefaultDiffence = 1;
    [NonSerialized] public float _playerMaxHP;
    [NonSerialized] public int _playerMaxAttack;
    [NonSerialized] public int _playerMaxDiffence;
    public float _currentHP;
    public float CurrentHP 
    { 
        get { return _currentHP; }
        set { _currentHP = value; }
    }

    private int _currentAttack; 
    public int CurrentAttack 
    {
        get { return _currentAttack; }
        set { _currentAttack = value; }
    }

    private int _currentDiffence;
    public int CurrentDiffence
    {
        get { return _currentAttack; }
        set { _currentDiffence = value; }
    }

    public void InitStatus()
    {
        _currentHP = _playerMaxHP = _playerDefaultHP;
        _currentAttack = _playerMaxAttack = _playerDefaultAttack;
        _currentDiffence = _playerMaxDiffence = _playerDefaultDiffence;
    }

    public void SetStatus(TotalPlusStatus totalPlusStatus) 
    {
        _currentHP = _playerMaxHP = _playerDefaultHP + totalPlusStatus.TotalHP;
        _currentAttack = _playerMaxAttack = _playerDefaultAttack + totalPlusStatus.TotalATK;
        _currentDiffence = _playerMaxDiffence = _playerDefaultDiffence + totalPlusStatus.TotalDEF;
    }
}
