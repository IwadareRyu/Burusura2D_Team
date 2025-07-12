using Cysharp.Threading.Tasks.Triggers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippedItems : MonoBehaviour
{
    [SerializeField] int _equipPointCount = 3;
    static EquipItem[] _equipPoints;
    public EquipItem[] EquipPoints => _equipPoints;
    TotalPlusStatus _totalEquipStatus;
    public TotalPlusStatus TotalEquipStatus => _totalEquipStatus;

    protected void Awake()
    {
        _equipPoints = new EquipItem[_equipPointCount];
        TotalStatusCal();
    }

    public EquipItem GetEquip(int index)
    {
        return _equipPoints[index];
    }

    public void SetEquip(int index,EquipItem item)
    {
        _equipPoints[index] = item;
    }

    public void TotalStatusCal()
    {
        _totalEquipStatus = new TotalPlusStatus();
        for (var i = 0; i < _equipPoints.Length; i++)
        {
            if (_equipPoints[i] != null)
            {
                _totalEquipStatus.TotalHP += _equipPoints[i].HPValue;
                _totalEquipStatus.TotalATK += _equipPoints[i].AttackValue;
                _totalEquipStatus.TotalDEF += _equipPoints[i].DiffenceValue;
            }
        }
    }
}