using System;
using UnityEngine;
using UnityEngine.UI;

public class EquipMenuScripts : MonoBehaviour
{
    TotalPlusStatus _totalStatus;
    public TotalPlusStatus TotalStatus => _totalStatus;

    [Tooltip("装備アイテムメニュー"), Header("装備アイテムメニュー")]
    [SerializeField] Button _itemButton;
    [SerializeField] int _displayItems = 7;
    Button[] _itemButtons;
    [SerializeField] Transform _constants;
    [SerializeField] Transform _tmpBottonPos;
    [SerializeField] Text _numText;
    int _maxPage;
    [SerializeField] EquipItemSetInfo _equipItemSet;
    EquipInventorySystem _inventorySystem;
    int _currentPage = 1;
    [SerializeField] Image _inventoryPanel;
    bool _isItemSetFase;


    [Tooltip("装備メニュー"), Header("装備メニュー")]
    [SerializeField] Button[] _equipButton;
    [SerializeField] Text _totalStatusText;

    EquippedItems _equippedItems;
    //EquipItem[] _equipPoints;
    //public EquipItem[] EquipPoints => _equipPoints;
    EquipItem _selectEquip;
    public EquipItem SelectEquip => _selectEquip;
    int _currentSelectIndex;

    PlayerInput _input;
    int _moveMemuInput;

    // Start is called before the first frame update
    void Start()
    {
        _inventorySystem = EquipInventorySystem.Instance;
        _equippedItems = PlayerStatusSystems.Instance._equippedItems;
        _selectEquip = null;
        _equipItemSet.Init();
        _itemButtons = new Button[_displayItems];
        _currentPage = 1;
        SetItemButton(_currentPage);
        _inventoryPanel.gameObject.SetActive(false);
        _equippedItems.TotalStatusCal();
    }

    private void OnEnable()
    {
        SetInput();
    }

    void SetInput()
    {
        _input = new PlayerInput();
        _input.UI.Navigate.performed += ct =>
        {
            _moveMemuInput = (int)ct.ReadValue<Vector2>().x;
        };

        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    void Update()
    {
        if(_moveMemuInput < 1)
        {
            MoveMenu(false);
        }
        else if(_moveMemuInput > 1)
        {
            MoveMenu(true);
        }
    }

    // 次回
    public void MoveMenu(bool dirLeft)
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

    public void SelectItemButton(EquipItem itemData, int selectIndex)
    {
        var equipBool = _equipItemSet.SelectItemButtonInfo(itemData, selectIndex);
        if (equipBool) EquipItem(itemData, selectIndex);
    }

    public void EquipItem(EquipItem itemData, int selectIndex)
    {
        Debug.Log(selectIndex);
        _equipItemSet.EquipItemInfo(itemData);
        if (_equippedItems.EquipPoints[_currentSelectIndex] != null)
        {
            EquipItem tmpItemData = _equippedItems.EquipPoints[_currentSelectIndex];
            _inventorySystem._equipItemInvantory.Add(tmpItemData);
        }
        _equippedItems.SetEquip(_currentSelectIndex,itemData);
        _inventorySystem._equipItemInvantory.Remove(itemData);
        _equippedItems.TotalStatusCal();
        _totalStatusText.text = 
            $"HP+ {_equippedItems.TotalEquipStatus.TotalHP}  " +
            $"ATK+ {_equippedItems.TotalEquipStatus.TotalATK}  ";
        ResetItemButton();
        SetItemButton(_currentPage);
        _inventoryPanel.gameObject.SetActive(false);
        foreach (var equipButton in _equipButton)
        {
            equipButton.interactable = true;
        }
        ShowEquipButtonText(_equipButton[_currentSelectIndex], _equippedItems.EquipPoints[_currentSelectIndex].ItemData._itemName);
        _isItemSetFase = false;
        _currentSelectIndex = -1;
    }

    public void ShowEquipButtonText(Button button,string name)
    {
        var text = button.GetComponentInChildren<Text>();
        text.text = name;
    }


    public void SelectEquipPoint(int selectIndex)
    {
        _inventoryPanel.gameObject.SetActive(true);
        _isItemSetFase = true;
        _currentSelectIndex = selectIndex;
        foreach (var equipButton in _equipButton)
        {
            equipButton.interactable = false;
        }
    }

    public void SetSelectButton()
    {
        for (var i = 0; i < _equipButton.Length; i++)
        {
            var buttonText = GetComponentInChildren<Text>();
            if (_equippedItems.EquipPoints[i] != null)
            {
                buttonText.text = _equippedItems.EquipPoints[i].ItemData._itemName;
            }
        }
    }

    public void CancelSelect()
    {
        _equipItemSet.ResetSelect();
        _inventoryPanel.gameObject.SetActive(false);
        foreach (var equipButton in _equipButton)
        {
            equipButton.interactable = true;
        }
    }

    public void InstanceItemSet()
    {
        _inventorySystem.InstanceItem();
        _equipItemSet.ResetSelect();
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

    public void SubmitEquipped()
    {
        PlayerStatusSystems.Instance.SetTotalStatus();
        gameObject.SetActive(false);
    }
}
