using MasterDataClass;
using System;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class EquipMenuScripts : MonoBehaviour
{

    [Tooltip("装備アイテムメニュー"), Header("装備アイテムメニュー")]
    [SerializeField] Button _itemButton;
    [SerializeField] int _displayItems = 7;
    Button[] _itemButtons;
    [SerializeField] Transform _constants;
    [SerializeField] Transform _tmpBottonPos;
    [SerializeField] Text _numText;
    int _maxPage;
    [SerializeField]EquipItemSetInfo _equipItemSet;
    EquipInventorySystem _inventorySystem;
    int _currentPage = 1;


    [Tooltip("装備メニュー"), Header("装備メニュー")]
    [SerializeField] Button[] _equipButton;
    EquipItem[] _equipPoints;
    public EquipItem[] EquipPoints => _equipPoints;
    EquipItem _selectEquip;
    public EquipItem SelectEquip => _selectEquip;

    // Start is called before the first frame update
    void Start()
    {
        _inventorySystem = EquipInventorySystem.Instance;
        _equipPoints = new EquipItem[_equipButton.Length];
        _selectEquip = null;
        _equipItemSet.Init();
        _itemButtons = new Button[_displayItems];
        _currentPage = 1;
        SetItemButton(_currentPage);
    }

    void Update()
    {

    }

    public void SetItemButton(int currentPage)
    {
        var currentCount = _displayItems * (currentPage - 1);
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
                var count = currentCount + i;
                _itemButtons[i].onClick.AddListener(() => SelectItemButton(itemData, count));
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
        _numText.text = $"{currentPage}/{_maxPage}";
    }

    public void SelectItemButton(EquipItem itemData,int selectIndex)
    {
        _equipItemSet.SelectItemButtonInfo(itemData,selectIndex);
    }


    public void SelectEquipPoint()
    {

    }

    public void SetSelectButton()
    {
        for (var i = 0; i < _equipButton.Length; i++)
        {
            var buttonText = GetComponentInChildren<Text>();
            if (_equipPoints[i] != null)
            {
                buttonText.text = _equipPoints[i].ItemData._itemName;
            }
        }
    }

    public void InstanceItemSet()
    {
        _inventorySystem.InstanceItem();
        ResetItemButton();
        SetItemButton(_currentPage);
    }

    public void ResetItemButton()
    {
        foreach (var button in _itemButtons)
        {
            Destroy(button.gameObject);
        }
    }
}

[Serializable]
public struct EquipButton
{
    Button _equipButton;
    Text _evalateText;
}
