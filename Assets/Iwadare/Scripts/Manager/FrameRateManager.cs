using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRateManager : MonoBehaviour
{
    [SerializeField] int _frameRate = 60;
    void Start()
    {
        Application.targetFrameRate = _frameRate;
    }
}
