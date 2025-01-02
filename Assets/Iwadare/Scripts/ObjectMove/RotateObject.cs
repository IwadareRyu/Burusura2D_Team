using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour,PauseTimeInterface
{
    [SerializeField] float _rotateSpeed = 1f;
    [SerializeField] float _memoRotate;

    private void Awake()
    {
        _memoRotate = _rotateSpeed;
    }

    private void OnEnable()
    {
        TimeScaleManager.ChangeTimeScaleAction += TimeScaleChange;
        TimeScaleManager.StartPauseAction += StartPause;
        TimeScaleManager.EndPauseAction += EndPause;
    }

    private void OnDisable()
    {
        TimeScaleManager.ChangeTimeScaleAction -= TimeScaleChange;
        TimeScaleManager.StartPauseAction -= StartPause;
        TimeScaleManager.EndPauseAction -= EndPause;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * _rotateSpeed);
    }

    public void EndPause()
    {
        _rotateSpeed = _memoRotate;
    }

    public void StartPause()
    {
        _rotateSpeed = 0f;
    }

    public void TimeScaleChange(float timeScale)
    {
        _rotateSpeed = _memoRotate * timeScale;
    }

}
