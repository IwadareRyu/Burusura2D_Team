using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ChoiceActionInterface
{
    abstract public bool ChackHP(float currentHpPersent);
    abstract public AttackInterface ChoiceAttack();
}
