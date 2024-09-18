using System;

[Serializable]
public class AttackStatesBoss1
{
    public enum AttackStatesList
    {
        DashAttack,
        Attack2,
    }
    public DashAttack dashAttack = new();
    public SampleAttack at2 = new();

}
