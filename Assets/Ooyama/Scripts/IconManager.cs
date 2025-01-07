﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconManager : MonoBehaviour
{
    [SerializeField] private Image _attackIcon;
    [SerializeField] private Image _avoidIcon;
    [SerializeField] private Image _jumpIcon;
    [SerializeField] private Image _specialIcon;
    [SerializeField] private float _splitNumber = 12.0f;
    public static IconManager Instance = null;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        _attackIcon.fillAmount = 1;
        _avoidIcon.fillAmount = 1;
        _jumpIcon.fillAmount = 1;
        _specialIcon.fillAmount = 1;
    }
    public void Test()
    {
        UpdateIcon(0.5f, TargetIcon.Special);
    }
    public void Test2()
    {
        UpdateIcon(1, TargetIcon.Special);
    }
    public void Test3()
    {
        UpdateIcon(0, TargetIcon.Jump);
    }
    public void UpdateIcon(float reciveValue, TargetIcon icon)
    {
        switch (icon)
        {
            case TargetIcon.Attack:
                StartCoroutine(WaitingTimeIcon(reciveValue, _attackIcon));
                break;
            case TargetIcon.Avoid:
                StartCoroutine(WaitingTimeIcon(reciveValue, _avoidIcon));
                break;
            case TargetIcon.Jump:
                JumpIconChanger(reciveValue);
                break;
            case TargetIcon.Special:
                SpecialIconChanger(reciveValue);
                break;
        }
    }
    private IEnumerator WaitingTimeIcon(float waitingTime, Image icon)
    {
        icon.fillAmount = 0;
        float dividedNum = waitingTime / _splitNumber;
        for (int i = 0; i < _splitNumber && this.gameObject; i++)
        {
            yield return WaitforSecondsCashe.Wait(dividedNum);
            icon.fillAmount += 1.0f / _splitNumber;
        }
        icon.fillAmount = 1;
        yield break;
    }
    private void SpecialIconChanger(float reciveValue)
    {
        if (reciveValue == 1.0f)
        {
            _specialIcon.gameObject.SetActive(true);
        }
        else
        {
            _specialIcon.gameObject.SetActive(false);
        }
    }
    private void JumpIconChanger(float reciveValue)
    {
        if (reciveValue == 2.0)
        {
            _jumpIcon.gameObject.SetActive(false);
        }
        else if (reciveValue == 0.0)
        {
            _jumpIcon.gameObject.SetActive(true);
        }
        else
        {
            return;
        }
    }
}
public enum TargetIcon
{
    Attack,
    Avoid,
    Jump,
    Special
}

