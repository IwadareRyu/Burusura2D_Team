using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    public static InGameManager Instance;
    [NonSerialized] public PlayerSpecialGuage _playerSpecialGuage;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        _playerSpecialGuage = GetComponent<PlayerSpecialGuage>();
    }
}
