using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconManager : MonoBehaviour
{
    [SerializeField] private Image _attackIcon;
    [SerializeField] private Image _avoidIcon;
    [SerializeField] private Image _jumpIcon;   
    [SerializeField] private Image _specialIcon;
    [SerializeField] private int _splitNumber = 12;
    public void Test()
    {
        UpdateIcon(1, "Attack");
    }
    public void UpdateIcon(float waitingTime,string action)
    {
        switch (action)
        {
            case "Attack":
                StartCoroutine(WaitingTimeIcon(waitingTime, _attackIcon));
                break;
            case "Avoid":
                StartCoroutine(WaitingTimeIcon(waitingTime, _avoidIcon));
                break;
            case "Jump":
                StartCoroutine(WaitingTimeIcon(waitingTime, _jumpIcon));
                break;
            case "Special":
                //StartCoroutine(WaitingTimeIcon(waitingTime,_specialIcon));

                break;
        }
    }
    private IEnumerator WaitingTimeIcon(float waitingTime,Image icon)
    {
        icon.fillAmount = 0;
        float Increase = waitingTime / _splitNumber;
        for (int i = 0; i < _splitNumber; i++)
        {
            yield return  WaitforSecondsCashe.Wait(Increase);
            icon.fillAmount += Increase;
        }
        icon.fillAmount = 1;
        yield break;
    }

}
