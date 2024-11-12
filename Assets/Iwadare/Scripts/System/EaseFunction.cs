using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EaseFunction
{
    public static float EaseOutExpo(float maxTime,float currentTime)
    {
        var x = currentTime / maxTime;
        return x == 1 ? 1 : 1 - Mathf.Pow(2, -10 * x);
    }

    public static float EaseInExpoReverse(float maxTime,float currentTime)
    {
        var x = 1 - currentTime / maxTime;
        return x == 0 ? 0 : Mathf.Pow(2, 10 * x - 10);
    }
}
