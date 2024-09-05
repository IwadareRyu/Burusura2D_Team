using System;

[Serializable]
public class AttackStates
{
    public enum AttackStatesList
    {
        DashAttack,
        Attack2,
    }
    public DashAttack dashAttack = new();
    public Attack2 at2 = new();

}
