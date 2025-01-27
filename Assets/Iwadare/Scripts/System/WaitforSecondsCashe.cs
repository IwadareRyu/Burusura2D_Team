using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WaitforSecondsCashe
{
    public static Dictionary<float, WaitForSeconds> _secondsCashe = new Dictionary<float, WaitForSeconds>();

    public static float _waitTimeScale = 1f;

    public static WaitForSeconds Get(float waitTime)
    {
        if (!_secondsCashe.ContainsKey(waitTime))
        {
            _secondsCashe.Add(waitTime, new WaitForSeconds(waitTime));
        }

        return _secondsCashe[waitTime];
    }

    public static IEnumerator Wait(float waitTime)
    {
        while (_waitTimeScale == 0f) yield return null;
        var wait = Get(waitTime * _waitTimeScale);
        yield return wait;
    }
}
