using MasterDataClass;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EquipItem
{
    Item _itemData;
    public Item ItemData => _itemData;

    int _attackValue = 0;
    public int AttackValue => _attackValue;

    int _diffenceValue = 0;
    public int DiffenceValue => _diffenceValue;

    int _hpValue = 0;
    public int HPValue => _hpValue;

    int _evaluateValue;
    public int EvaluateValue => _evaluateValue;

    public EquipItemState _itemState = EquipItemState.None;

    public EquipItem(ItemScriptable item)
    {
        AssignValue(item.ItemData);
    }

    void AssignValue(Item data)
    {
        _itemData = data;
        _evaluateValue = 0;
        var count = 0;
        _attackValue = ValueAssign(data._maxAttack,data._baseAttack,EquipItemState.Weapon,ref count);
        _diffenceValue = ValueAssign(data._maxDiffence,data._baseDiffence,EquipItemState.Armor,ref count);
        _hpValue = ValueAssign(data._maxHP,data._baseHP,EquipItemState.HP,ref count);
        if(count != 0)
        {
            _itemState &= ~EquipItemState.None;
            _evaluateValue = _evaluateValue / count;
        }
        Debug.Log(_itemState);
        Debug.Log(_evaluateValue);
    }

    int ValueAssign(int max,int min,EquipItemState state,ref int count)
    {
        if (max > 0)
        {
            var ram = RamdomMethod.RamdomNumberMinMax(min, max);
            var evalate = EvaluateCalculate(ram,max,min);
            Debug.Log(evalate);
            _evaluateValue += (int)(evalate * 100);
            count++;
            _itemState |= state;
            return ram;
        }
        return 0;
    }

    float EvaluateCalculate(int current,int max,int min)
    {
        if (max == min) return 1;
        else return (float)(current - min) / (float)(max - min);
    }
}
[Flags]
public enum EquipItemState
{
    None = 1,
    Weapon = 1 << 1,
    Armor = 1 << 2,
    HP = 1 << 3,
    SpecialEffect = 1 << 4
}
