using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingPlayer : SpecialAttackInterface
{
    [SerializeField] ParticleSystem _healingObj;
    ParticleSystem.MainModule _mainParticle;
    [SerializeField] AudioClip _healingAudioClip;
    [SerializeField] float _healingTime = 2f;
    [SerializeField] int _healingPower = 10;
    [SerializeField] int _upperLifePower = 1;

    public override void Init()
    {
        _mainParticle = _healingObj.main;
    }

    public override void UseSkill(PlayerController player)
    {
        _healingObj.Play();
        player.AddDamage(-_healingPower);
        InGameManager.Instance.AddRemain(_upperLifePower);
        AudioManager.Instance.PlaySE(_healingAudioClip.name);
    }

}
