using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipItemInventory : MonoBehaviour
{
    List<KeyValuePair<int,ItemScriptable>> _equipItemInvantory = new List<KeyValuePair<int, ItemScriptable>>();
    [SerializeField] Button _itemButton;


    private void Start()
    {
        
    }
}
