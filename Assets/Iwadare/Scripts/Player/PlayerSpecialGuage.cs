﻿using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpecialGuage : MonoBehaviour
{
    [SerializeField] Slider _guageSlider;
    [SerializeField] float _maxGuage = 100f;
    public float MaxGuage => _maxGuage;
    [SerializeField] float _avoidBulletAddGuage = 0.1f;
    public float AvoidBulletAddGuage => _avoidBulletAddGuage;
    [SerializeField] float _breakAddGuage = 1f;
    public float BreakAddGuage => _breakAddGuage;
    [SerializeField] float _parryAddGuage = 25f;
    public float ParryAddGuage => _parryAddGuage;

    float _currentGuage;

    public void Init()
    {
        _currentGuage = 0f;
        SetSlider(_maxGuage, _currentGuage);
    }

    public void AddGuage(float addNumber)
    {
        _currentGuage = Mathf.Min(_maxGuage, _currentGuage + addNumber);
        SetSlider(_maxGuage, _currentGuage);
    }

    public bool IsCostChack(float cost)
    {
        if (_currentGuage < cost) return false;
        return true;
    }

    public void UseGuage()
    {
        _currentGuage = 0;
        SetSlider(_maxGuage, _currentGuage);
        IconManager.Instance.UpdateIcon(_currentGuage, TargetIcon.Special);
    }

    private void SetSlider(float maxNumber, float currentNumber)
    {
        var num = currentNumber / maxNumber;
        _guageSlider.value = num;
        IconManager.Instance.UpdateIcon(num, TargetIcon.Special);
    }
}
