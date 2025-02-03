using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillSet : MonoBehaviour
{
    [SerializeField] SpecialAttackInterface _halfSkill;
    public SpecialAttackInterface HalfSkill => _halfSkill;
    [SerializeField] SpecialAttackInterface _allSkill;
    public SpecialAttackInterface AllSkill => _allSkill;

    public void Start()
    {
        _halfSkill?.Init();
        _allSkill?.Init();
    }
}
