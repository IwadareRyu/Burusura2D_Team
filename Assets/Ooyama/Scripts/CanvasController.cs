using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    [SerializeField] private List<Canvas> _charactorList;
    private int _targetIndex = 0;

    public void MoveLeft()
    {
        _targetIndex--;

        if (_targetIndex < 0) _targetIndex = _charactorList.Count - 1;

        UpdateUI(_targetIndex);
    }
    public void MoveRight()
    {
        _targetIndex++;

        _targetIndex = _targetIndex % _charactorList.Count;

        UpdateUI(_targetIndex);

    }
    private void UpdateUI(int DisplayCharactorID)
    {
        for (int i = 1; i < _charactorList.Count; i++)
        {
            _charactorList[i].gameObject.SetActive(false);
        }

        for (int i = 0; i <= DisplayCharactorID; i++)
        {
            _charactorList[i].gameObject.SetActive(true);
        }
    }
}
