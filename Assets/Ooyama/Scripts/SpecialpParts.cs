using UnityEngine;

public abstract class SpecialpParts : ScriptableObject
{
    public EffectType effectType;
    public abstract void Apply(GameObject target);
    public abstract string Description { get; }
}
public enum EffectType
{
    Poison,
    StatusUp,
    Invisivle,
    Normal,
}

