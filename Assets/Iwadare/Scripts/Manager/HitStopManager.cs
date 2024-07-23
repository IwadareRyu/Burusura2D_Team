using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HitStopManager : MonoBehaviour
{
    public static HitStopManager instance;

    public UnityAction<float> _speedHitStopActionStart;

    public UnityAction _speedHitStopActionEnd;

    HitStopType _hitStopType;

    [SerializeField] float _speedhitStopTime = 0.5f;

    public float _speedHitStopPower = 0.8f;

    float _currentStopTime;

    bool _isHitStop = false;

    public bool _isSpeedHitStop = false;

    float _maxTimeScale;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        _maxTimeScale = Time.timeScale;
    }

    void Update()
    {
        if(_isHitStop)
        {
            HitStop();
        }
    }

    void HitStop()
    {
        _currentStopTime += Time.deltaTime;

        if (_currentStopTime >= _speedhitStopTime)
        {
            switch (_hitStopType)
            {
                case HitStopType.SpeedHitStop:
                    _isSpeedHitStop = false;
                    _speedHitStopActionEnd.Invoke();
                    _hitStopType = HitStopType.None;
                    _isHitStop = false;
                    break;
                case HitStopType.TimeScaleHitStop:
                    Time.timeScale = _maxTimeScale;
                    _hitStopType = HitStopType.None;
                    _isHitStop = false;
                    break;

            }
        }
    }

    public void SpeedHitStop()
    {
        if (!_isHitStop)
        {
            _isHitStop = true;
            _isSpeedHitStop = true;
            _hitStopType = HitStopType.SpeedHitStop;
            _speedHitStopActionStart.Invoke(_speedHitStopPower);
        }
        _currentStopTime = 0f;
    }

    public void TimeHitStop()
    {
        if (!_isHitStop)
        {
            _isHitStop = true;
            _hitStopType = HitStopType.TimeScaleHitStop;
            Time.timeScale = _maxTimeScale * _speedHitStopPower;
        }
        _currentStopTime = 0f;
    }

    enum HitStopType
    {
        None,
        SpeedHitStop,
        TimeScaleHitStop,
    }
}
