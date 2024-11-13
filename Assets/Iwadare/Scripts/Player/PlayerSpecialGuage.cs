using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpecialGuage : MonoBehaviour
{
    [SerializeField] Slider _guageSlider;
    [SerializeField] float _maxGuage = 100f;
    [SerializeField] float _avoidBulletAddGuage = 0.1f;
    public float AvoidBulletAddGuage => _avoidBulletAddGuage;
    [SerializeField] float _breakAddGuage = 1f;
    public float BreakAddGuage => _breakAddGuage;

    float _currentGuage;

    public void Init()
    {
        _currentGuage = 0f;
        SetSlider(_maxGuage, _currentGuage);
    }

    public void AddGuage(float addNumber)
    {
        _currentGuage = Mathf.Min(_maxGuage,_currentGuage + addNumber);
        SetSlider(_maxGuage,_currentGuage);
    }

    public bool CostGuage(float cost)
    {
        if(_currentGuage < cost) return false;

        _currentGuage -= cost;
        SetSlider(_maxGuage,_currentGuage);
        return true;
    }

    private void SetSlider(float maxNumber,float currentNumber)
    {
        var num = currentNumber / maxNumber;
        _guageSlider.value = num;
    }

}
