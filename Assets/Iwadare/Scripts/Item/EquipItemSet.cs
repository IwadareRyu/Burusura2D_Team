using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EquipItemSet : MonoBehaviour
{
    [SerializeField] Button _itemButton;
    [SerializeField] int _displayItems = 7;
    Button[] _itemButtons;
    [SerializeField] Transform _constants;
    [SerializeField] Transform _tmpBottonPos;
    [SerializeField] Text _numText;
    [SerializeField] Text _infoNameText;
    [SerializeField] Text _infoEvalateText;
    [SerializeField] Text _infoAvilityText;
    int _currentPage = 1;
    int _maxPage;
    EquipInventorySystem _inventorySystem;

    private void Start()
    {
        _inventorySystem = EquipInventorySystem.Instance;
        _itemButtons = new Button[_displayItems];
        _currentPage = 1;
        SetItemButton();
    }

    void SetItemButton()
    {
        var currentCount = _displayItems * (_currentPage - 1);
        for (var i = 0; i < _displayItems; i++)
        {
            _itemButtons[i] = Instantiate(_itemButton, transform.position, Quaternion.identity);
            if (currentCount + i < _inventorySystem._equipItemInvantory.Count)
            {
                var itemData = _inventorySystem._equipItemInvantory[currentCount + i];
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
                _itemButtons[i].onClick.AddListener(() => SelectItemButton(itemData));
            }
            else
            {
                _itemButtons[i].transform.SetParent(_tmpBottonPos);
                _itemButtons[i].transform.position = _tmpBottonPos.position;
                _itemButton.interactable = false;
            }
        }
        // ちょうど_displayItemsの値と同じ場合、_maxPageのカウントが上がってしまうので限りなく小さい値で切り捨て調整。
        float num = ((float)_inventorySystem._equipItemInvantory.Count / _displayItems) - 0.01f;
        _maxPage = num > 0 ? (int)num + 1 : 1;
        _numText.text = $"{_currentPage}/{_maxPage}";
    }

    public void SelectItemButton(EquipItem item)
    {
        _infoNameText.text = item.ItemData._itemName;
        _infoEvalateText.text = "評価値:" + item.EvaluateValue.ToString();
        _infoAvilityText.text = 
            $"攻撃: {item.AttackValue}({item.ItemData._maxAttack}) " +
            $"防御: {item.DiffenceValue}({item.ItemData._maxDiffence}) \r\n" +
            $"HP: {item.HPValue}({item.ItemData._maxHP})";
    }

    public void ResetItemButton()
    {
        foreach (var button in _itemButtons)
        {
            Destroy(button.gameObject);
        }
    }

    public void InstanceItemSet()
    {
        _inventorySystem.InstanceItem();
        ResetItemButton();
        SetItemButton();
    }
}
