using MasterDataClass;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipInventorySystem : SingletonMonovihair<EquipInventorySystem>
{
    public List<EquipItem> _equipItemInvantory;
    [SerializeField] int _instanceCount = 5;
    [SerializeField] ItemScriptable[] _itemDatas;

    protected override void Awake()
    {
        if(!ObjectOnLoad()) return;

        _equipItemInvantory = new List<EquipItem>();

        for (var i = 0; i < _instanceCount; i++)
        {
            var ram = RamdomMethod.RamdomNumber0Max(_itemDatas.Length);
            EquipItem item = new EquipItem(_itemDatas[ram]);
            _equipItemInvantory.Add(item);
        }
    }

    public void InstanceItem()
    {
        var ram = RamdomMethod.RamdomNumber0Max(_itemDatas.Length);
        EquipItem item = new EquipItem(_itemDatas[ram]);
        _equipItemInvantory.Add(item);
    }
}
