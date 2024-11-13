using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRateManager : SingletonMonovihair<FrameRateManager>
{
    [SerializeField] int _frameRate = 60;
    [SerializeField] bool _vSinc = false;
    void Awake()
    {
        Application.targetFrameRate = _frameRate;
        if (_vSinc) QualitySettings.vSyncCount = 1;
        else QualitySettings.vSyncCount = 0;
    }
}
