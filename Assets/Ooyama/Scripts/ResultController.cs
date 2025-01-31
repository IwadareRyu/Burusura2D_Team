using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultController : MonoBehaviour
{
    [SerializeField] List<string> _winCommentList;
    [SerializeField] List<string> _loseCommentList;
    [SerializeField] Text _targetText;
    [SerializeField] float _printCharSpeed = 0.1f;
    [SerializeField] int _maxLineLength;
    [SerializeField] Image _nekomataImage;
    [SerializeField] Sprite _nekomataWinSprite;
    [SerializeField] Sprite _nekomataLoseSprite;
    private bool _canMoveScene = false;
    void Start()
    {
        _canMoveScene = true;
        _targetText.text = "";
        ResultPrinter(GameStateManager.Instance._isWin);
    }
    private void Update()
    {
        if (_canMoveScene && (Input.anyKey || Input.GetButtonDown("Fire1")))
        {
            _canMoveScene = false;
            FadeManager.Instance.SceneChangeStart("Title");
        }
    }
    private void ResultPrinter(bool Win)
    {
        if (Win)
        {
            _nekomataImage.sprite = _nekomataWinSprite;
            StartCoroutine(Printer(_winCommentList[Random.Range(0, _winCommentList.Count)]));
        }
        else
        {
            _nekomataImage.sprite = _nekomataLoseSprite;
            StartCoroutine(Printer(_loseCommentList[Random.Range(0, _loseCommentList.Count)]));
        }
    }
    IEnumerator Printer(string PrintText)
    {
        for (int i = 0; i < PrintText.Length; i++)
        {
            _targetText.text += PrintText[i];
            if ((i + 1) % _maxLineLength == 0)
            {
                _targetText.text += "\n";
            }
            yield return WaitforSecondsCashe.Wait(_printCharSpeed);
        }
    }
}
