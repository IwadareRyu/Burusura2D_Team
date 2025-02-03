using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class SpecialAttackInterface : MonoBehaviour
{
    public virtual void Init() { return; }
    public virtual void UseSkill(PlayerController player) { return; }
}
