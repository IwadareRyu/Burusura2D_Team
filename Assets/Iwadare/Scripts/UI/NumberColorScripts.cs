using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberColorScripts : MonoBehaviour
{
    [SerializeField] Text _numText;
    [SerializeField] float _lifeTime = 5f;
    [SerializeField] float _transY = 3f;
    public Color _damageColor = Color.red;
    public Color _healColor = Color.green;
    public Color _guageUpColor = Color.yellow;


    public void NumberColorChange(Color textColor)
    {
        _numText.color = textColor;
    }

    public void MoveNumber(int number)
    {
        _numText.text = number > 0 ? "+" + number.ToString() : number.ToString();
        StartCoroutine(NumberLifeTime());
    }

    public IEnumerator NumberLifeTime()
    {
        _numText.transform.DOMoveY(transform.position.y + _transY,_lifeTime).SetLink(gameObject);
        yield return WaitforSecondsCashe.Wait(_lifeTime - 1f);
        yield return _numText.DOFade(0,1f).SetLink(gameObject).WaitForCompletion();
        gameObject.SetActive(false);
    }
}
