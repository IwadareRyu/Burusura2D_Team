using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusSystems : SingletonMonovihair<PlayerStatusSystems>
{
    [SerializeField] float _defaultHP;
    [SerializeField] int _defaultAttack;
    [SerializeField] int _defaultDiffence;
    public EquippedItems _equippedItems;
    TotalPlusStatus _totalStatus;
    public TotalPlusStatus TotalStatus => _totalStatus;

    protected override void Awake()
    {
        if (!ObjectOnLoad()) return;
    }

    private void Start()
    {
        _totalStatus = new TotalPlusStatus();
    }

    public void SetTotalStatus()
    {
        _totalStatus.TotalHP = _defaultHP;
        _totalStatus.TotalATK = _defaultAttack;
        _totalStatus.TotalDEF = _defaultDiffence;
        if(_equippedItems != null)
        {
            var totalEquippedStatus = _equippedItems.TotalEquipStatus;
            _totalStatus.TotalHP += totalEquippedStatus.TotalHP;
            _totalStatus.TotalATK += totalEquippedStatus.TotalATK;
            _totalStatus.TotalDEF += totalEquippedStatus.TotalDEF;
        }

        Debug.Log($"HP:{_totalStatus.TotalHP} ATK:{_totalStatus.TotalATK}");
    }
}
