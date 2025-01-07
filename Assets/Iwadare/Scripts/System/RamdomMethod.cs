using System;
using Unity.VisualScripting;

public static class RamdomMethod
{
    public static int RamdomNumber(int maxNum)
    {
        UnityEngine.Random.InitState(DateTime.Now.Millisecond);
        return UnityEngine.Random.Range(0, maxNum);
    }

    public static int RandomNumber99()
    {
        UnityEngine.Random.InitState(DateTime.Now.Millisecond);
        return UnityEngine.Random.Range(0, 99);
    }
}
