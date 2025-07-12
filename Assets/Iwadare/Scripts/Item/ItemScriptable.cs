using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MasterDataClass;

[CreateAssetMenu(fileName = "SkillObjects", menuName = "Item/ItemObjects")]

public class ItemScriptable : ScriptableObject
{
    [SerializeField] Item _itemData;

    [SerializeField] SpecialpParts parts;
    public Item ItemData => _itemData;

    public void ItemDataLoad(ItemData itemData)
    {
        _itemData._itemID = itemData.ID;
        _itemData._itemName = itemData.ItemName;
        _itemData._descrition = itemData.Description;
        _itemData._baseAttack = itemData.BaseAttack;
        _itemData._maxAttack = itemData.MaxAttack;
        _itemData._baseDiffence = itemData.BaseDiffence;
        _itemData._maxDiffence = itemData.MaxDiffence;
        _itemData._baseHP = itemData.BaseHP;
        _itemData._maxHP = itemData.MaxHP;
    }
}

[Serializable]
public struct Item
{
    [Header("アイテムID"), Tooltip("アイテムID")]
    public int _itemID;

    [Header("アイテムの名前"),Tooltip("アイテムの名前")]
    public string _itemName;

    [Header("アイテム説明"),Tooltip("アイテム説明")]
    [TextArea(10,10)]
    public string _descrition;

    [Header("攻撃力")]
    [Tooltip("基礎攻撃力"), Header("基礎攻撃力")]
    public int _baseAttack;

    [Tooltip("最大攻撃力"), Header("最大攻撃力")]
    public int _maxAttack;


    [Header("防御力")]
    [Tooltip("基礎防御力"), Header("基礎防御力")]
    public int _baseDiffence;

    [Tooltip("最大防御力"), Header("最大防御力")]
    public int _maxDiffence;

    [Tooltip("基礎HP"), Header("基礎HP")]
    public int _baseHP;

    [Tooltip("最大HP"), Header("最大HP")]
    public int _maxHP;

    //[Tooltip("評価値"), Header("評価値")]
    //public float evelation;
}
