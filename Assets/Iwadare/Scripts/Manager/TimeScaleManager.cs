﻿using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms;

public class TimeScaleManager : SingletonMonovihair<TimeScaleManager>
{
    [SerializeField] float _defaultTimeScale = 1.0f;
    public float DefaultTimeScale => _defaultTimeScale;
    float _currentTimeScale;

    public static UnityAction<float> ChangeTimeScaleAction;
    public static UnityAction StartPauseAction;
    public static UnityAction EndPauseAction;

    protected override void Awake()
    {
        base.Awake();
        _currentTimeScale = _defaultTimeScale;
    }

    /// <summary>全体の時間のみを変えるときの処理</summary>
    /// <param name="timeScale"></param>
    public void TimeScaleChange(float timeScale)
    {
        WaitforSecondsCashe._waitTimeScale = timeScale;
        _currentTimeScale = timeScale;
        Time.timeScale = timeScale;
        ChangeTimeScaleAction.Invoke(_currentTimeScale);
    }

    /// <summary>時間を止めて処理を止める処理</summary>
    /// <param name="timeScale"></param>
    public void StartPauseManager()
    {
        WaitforSecondsCashe._waitTimeScale = 0f;
        Time.timeScale = 0f;
        StartPauseAction.Invoke();
    }

    /// <summary>時間を動かして処理を再開する処理</summary>
    public void EndPauseManager()
    {
        WaitforSecondsCashe._waitTimeScale = 1f;
        EndPauseAction.Invoke();
        Time.timeScale = 1f;
    }

    /// <summary>デフォルトのゲームスピードを上げる処理</summary>
    /// <param name="timeScale"></param>
    public void ChangeDefaultTimeScale(float timeScale)
    {
        _defaultTimeScale = _currentTimeScale = timeScale;
        ChangeTimeScaleAction?.Invoke(_currentTimeScale);
    }
}