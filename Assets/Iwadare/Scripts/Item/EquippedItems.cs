using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippedItems : SingletonMonovihair<EquippedItems>
{
    [SerializeField] int _equipPointCount = 3;
    static EquipItem[] _equipPoints;
    public EquipItem[] EquipPoints => _equipPoints;
    TotalPlusStatus _totalStatus;
    public TotalPlusStatus TotalStatus => _totalStatus;

    void Start()
    {
        if (!ObjectOnLoad()) return;

        _equipPoints = new EquipItem[_equipPointCount];
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
        _totalStatus = new TotalPlusStatus();
        for (var i = 0; i < _equipPoints.Length; i++)
        {
            if (_equipPoints[i] != null)
            {
                _totalStatus.TotalHP += _equipPoints[i].HPValue;
                _totalStatus.TotalATK += _equipPoints[i].AttackValue;
                _totalStatus.TotalDEF += _equipPoints[i].DiffenceValue;
            }
        }
    }
}

[Serializable]
public struct TotalPlusStatus
{
    public int TotalHP;
    public int TotalATK;
    public int TotalDEF;
}
