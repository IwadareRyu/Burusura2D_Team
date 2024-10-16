using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ChoiceActionInterface
{
    abstract public bool ChackHP(float currentHpPersent);
    abstract public bool ChackSpecial();
    abstract public AttackInterface ChoiceAttack();
    abstract public AttackInterface SelectSpecialAttack();
}
