using System;
using Unity.VisualScripting;

public static class RamdomMethod
{
    static int _second = 0;
    public static int RamdomNumber0Max(int maxNum)
    {
        if (DateTimeChack())
        {
            UnityEngine.Random.InitState(_second);
        }
        return UnityEngine.Random.Range(0, maxNum);
    }

    public static int RamdomNumberMinMax(int minNum, int maxNum)
    {
        if (DateTimeChack())
        {
            UnityEngine.Random.InitState(_second);
        }
        return UnityEngine.Random.Range(minNum, maxNum);
    }

    public static float RamdomNumberMinMax(float minNum,float maxNum)
    {
        if (DateTimeChack())
        {
            UnityEngine.Random.InitState(_second);
        }
        return UnityEngine.Random.Range(minNum,maxNum);
    }


    public static int RandomNumber99()
    {
        if (DateTimeChack())
        {
            UnityEngine.Random.InitState(_second);
        }
        return UnityEngine.Random.Range(0, 99);
    }

    public static bool DateTimeChack()
    {
        if(_second != DateTime.Now.Millisecond)
        {
            _second = DateTime.Now.Millisecond;
            return true;
        }
        return false;
    }
}
