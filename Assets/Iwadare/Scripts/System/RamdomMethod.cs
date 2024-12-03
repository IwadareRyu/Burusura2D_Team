using System;

public static class RamdomMethod
{
    public static int RamdomNumber(int maxNum)
    {
        UnityEngine.Random.InitState(DateTime.Now.Millisecond);
        return UnityEngine.Random.Range(0, maxNum);
    }

}
