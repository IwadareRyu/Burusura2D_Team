using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] AudioClip _zangekiClip;
    [SerializeField] AudioClip _damageClip;
    [SerializeField] AudioClip _explosionClip;
    [SerializeField] AudioClip _shieldClip;
    [SerializeField] AudioClip _shieldBreakClip;

    public void AttackPlayAudio()
    {
        AudioManager.Instance.PlaySE(_zangekiClip.name);
    }

    public void DamageAudio()
    {
        AudioManager.Instance.PlaySE(_damageClip.name);
    }

    public void ExplosionAudio()
    {
        AudioManager.Instance.PlaySE(_explosionClip.name);
    }

    public void ShieldAudio()
    {
        AudioManager.Instance.PlaySE(_shieldClip.name);
    }

    public void ShieldBreakAudio()
    {
        AudioManager.Instance.PlaySE(_shieldBreakClip.name);
    }

}
