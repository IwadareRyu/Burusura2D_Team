using System;
using Unity.VisualScripting;

public static class RamdomMethod
{
    public static int RamdomNumber0Max(int maxNum)
    {
        UnityEngine.Random.InitState(DateTime.Now.Millisecond);
        return UnityEngine.Random.Range(0, maxNum);
    }

    public static int RamdomNumberMinMax(int minNum, int maxNum)
    {
        UnityEngine.Random.InitState(DateTime.Now.Millisecond);
        return UnityEngine.Random.Range(minNum, maxNum);
    }

    public static float RamdomNumberMinMax(float minNum,float maxNum)
    {
        UnityEngine.Random.InitState(DateTime.Now.Millisecond);
        return UnityEngine.Random.Range(minNum,maxNum);
    }


    public static int RandomNumber99()
    {
        UnityEngine.Random.InitState(DateTime.Now.Millisecond);
        return UnityEngine.Random.Range(0, 99);
    }
}
