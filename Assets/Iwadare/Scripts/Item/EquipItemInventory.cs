using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EquipItemInventory : MonoBehaviour
{
    List<EquipItem> _equipItemInvantory;
    [SerializeField] Button _itemButton;
    [SerializeField] int _displayItems = 7;
    Button[] _itemButtons;
    [SerializeField] Transform _constants;
    [SerializeField] Transform _tmpBottonPos;
    [SerializeField] Text _numText;
    [SerializeField] int _instanceCount = 5;
    [SerializeField] ItemScriptable[] _itemDatas;
    int _currentPage = 1;
    int _maxPage;


    private void Start()
    {
        _equipItemInvantory = new List<EquipItem>();
        _itemButtons = new Button[_displayItems];
        
        for(var i = 0;i < _instanceCount;i++)
        {
            var ram = RamdomMethod.RamdomNumber0Max(_itemDatas.Length);
            EquipItem item = new EquipItem(_itemDatas[ram]);
            _equipItemInvantory.Add(item);
        }

        _currentPage = 1;
        SetItem();
        _maxPage = _equipItemInvantory.Count / 7 + 1;
        _numText.text = $"{_currentPage}/{_maxPage}";
    }

    void SetItem()
    {
        var currentCount = _displayItems * (_currentPage - 1);
        for (var i = 0; i < _itemButtons.Length; i++)
        {
            _itemButtons[i] = Instantiate(_itemButton, transform.position, Quaternion.identity);
            if (currentCount + i < _equipItemInvantory.Count)
            {
                var itemData = _equipItemInvantory[currentCount + i];
                _itemButtons[i].transform.SetParent(_constants);
                _itemButtons[i].interactable = true;
                Text nameText = null;
                Text evaluateText = null;
                foreach (var text in _itemButtons[i].GetComponentsInChildren<Text>())
                {
                    if (text.gameObject.tag == "ItemName") nameText = text;
                    if (text.gameObject.tag == "ItemEvalation") evaluateText = text;
                }
                if (nameText != null && evaluateText != null)
                {
                    nameText.text = itemData.ItemData._itemName;
                    evaluateText.text = itemData.EvaluateValue.ToString();
                }
                _itemButtons[i].onClick.AddListener(() => SelectItem(itemData));
            }
            else
            {
                _itemButtons[i].transform.SetParent(_tmpBottonPos);
                _itemButtons[i].transform.position = _tmpBottonPos.position;
                _itemButton.interactable = false;
            }
        }
    }

    public void SelectItem(EquipItem item)
    {
        Debug.Log($"名前{item.ItemData._itemName} 評価値{item.EvaluateValue} \n" +
            $"攻撃{item.AttackValue}({item.ItemData._maxAttack}) " +
            $"防御{item.DiffenceValue}({item.ItemData._maxDiffence}) " +
            $"HP{item.HPValue}({item.ItemData._maxHP})");
    }
}
