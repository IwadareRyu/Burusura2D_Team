using System;
using UnityEngine;
using UnityEngine.UI;

public class EquipScripts : MonoBehaviour
{
    EquipItem[] _equipPoints;
    public EquipItem[] EquipPoints => _equipPoints;

    [SerializeField] Button[] _equipButton;
    EquipItem _selectEquip;
    public EquipItem SelectEquip => _selectEquip;

    // Start is called before the first frame update
    void Start()
    {
        _equipPoints = new EquipItem[_equipButton.Length];
        _selectEquip = null;
    }

    // Update is called once per frame
    void Update()
    {

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
}

[Serializable]
public struct EquipButton
{
    Button _equipButton;
    Text _evalateText;
}
