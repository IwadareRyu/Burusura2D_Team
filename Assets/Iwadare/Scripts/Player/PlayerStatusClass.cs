using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerStatusClass : MonoBehaviour
{
    [SerializeField] float _playerDefaultHP = 100f;
    [SerializeField] int _playerDefaultAttack = 1;
    [SerializeField] int _playerDefaultDiffence = 1;

    public float _currentHP 
    { 
        get { return _currentHP; }
        set { _currentHP = value; }
    }

    public int _currentAttack 
    {
        get { return _currentAttack; }
        set { _currentAttack = value; }
    }

    public int _currentDiffence
    {
        get { return _currentDiffence; }
        set { _currentDiffence = value; }
    }


}
