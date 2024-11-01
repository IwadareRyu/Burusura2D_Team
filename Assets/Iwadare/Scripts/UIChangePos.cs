using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChangePos : MonoBehaviour
{
    [SerializeField] Transform _trans;
    [SerializeField] Transform _targetUI;
    [SerializeField] RectTransform _targetPanel;
    Vector2 _panelSize;
    Vector3 _tmpTrans;
    Camera _camera;
    [SerializeField] Vector2 _cameraMax = new Vector2(5,3);
    [SerializeField] Vector2 _cameraMin = new Vector2(-8,-3);

    void Start()
    {
        _tmpTrans = new Vector3(0, 0, 10);
        _camera = Camera.main;
        _panelSize = _targetPanel.sizeDelta;
    }

    private void Update()
    {
        _panelSize = _targetPanel.sizeDelta;
        _tmpTrans.x = Mathf.Max(Mathf.Min(_trans.position.x,_camera.transform.position.x + _cameraMax.x - _panelSize.x / 110f), _camera.transform.position.x + _cameraMin.x);
        _tmpTrans.y = Mathf.Max(Mathf.Min(_trans.position.y, _camera.transform.position.y + _cameraMax.y), _camera.transform.position.y + _cameraMin.y);
        _targetUI.position = _tmpTrans;
    }
}
