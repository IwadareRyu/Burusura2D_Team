using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouinAttackSkill : SpecialAttackInterface
{
    [SerializeField] RouinAttack _rouinObj;
    [SerializeField] float damage = 20f;

    public override void Init()
    {

    }

    public override void UseSkill(PlayerController player)
    {
        var rouin = Instantiate(_rouinObj, transform.position, Quaternion.identity);
        rouin.SetRouin(damage);
    }
}
