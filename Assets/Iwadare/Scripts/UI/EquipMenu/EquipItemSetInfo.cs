using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EquipItemSetInfo : MonoBehaviour
{
    [SerializeField] Text _infoNameText;
    [SerializeField] Text _infoEvalateText;
    [SerializeField] Text _infoAvilityText;
    int _selectIndex = -1;

    public void Init()
    {
    }



    public bool SelectItemButtonInfo(EquipItem item,int selectIndex)
    {
        Debug.Log(selectIndex);
        if (_selectIndex != selectIndex)
        {
            _selectIndex = selectIndex;
            _infoNameText.text = item.ItemData._itemName;
            _infoEvalateText.text = "評価値:" + item.EvaluateValue.ToString();
            _infoAvilityText.text =
                $"攻撃: {item.AttackValue}({item.ItemData._maxAttack}) " +
                $"防御: {item.DiffenceValue}({item.ItemData._maxDiffence}) \r\n" +
                $"HP: {item.HPValue}({item.ItemData._maxHP})";
            return false;
        }
        return true;
    }
}
