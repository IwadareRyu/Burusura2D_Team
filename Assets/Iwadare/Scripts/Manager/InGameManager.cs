using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameManager : MonoBehaviour
{
    public static InGameManager Instance;
    [NonSerialized] public PlayerSpecialGuage _playerSpecialGuage;
    [SerializeField] public Text _deathCountText;
    int _deathCount;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        _playerSpecialGuage = GetComponent<PlayerSpecialGuage>();
        _deathCount = 0;
        _deathCountText.text = $"死亡回数: {_deathCount}";
    }

    public void PlayerDeath()
    {
        _deathCount++;
        _deathCountText.text = $"死亡回数: {_deathCount}";
    }
}
