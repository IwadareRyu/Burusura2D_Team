using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingPlayer : SpecialAttackInterface
{
    [SerializeField] float _healingTime = 2f;
    [SerializeField] int _healingPower = 10;
    [SerializeField] int _upperLifePower = 1;
    [SerializeField] float _upperLifePersents = 5f;

    public override void Init()
    {

    }

    public override void UseSkill(PlayerController player)
    {
        player.AddDamage(-_healingPower);
        if (RamdomMethod.RandomNumber99() < _upperLifePersents)
        {
            InGameManager.Instance.AddRemain(_upperLifePower);
        }
    }

}
