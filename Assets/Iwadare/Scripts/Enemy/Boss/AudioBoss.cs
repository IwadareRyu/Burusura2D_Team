using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBoss : MonoBehaviour
{
    [SerializeField] AudioClip _catInAudio;
    [SerializeField] AudioClip _attackAudio;
    [SerializeField] AudioClip _damageAudio;
    [SerializeField] AudioClip _deadAudio;
    [SerializeField] AudioClip _shieldAudio;
    [SerializeField] AudioClip _shieldBreakAudio;
    [SerializeField] AudioClip _parryAudio;

    public void CatInAudioPlay()
    {
        AudioManager.Instance.PlaySE(_catInAudio.name);
    }

    public void AttackAudioPlay()
    {
        AudioManager.Instance.PlaySE(_attackAudio.name);
    }

    public void DamageAudioPlay()
    {
        AudioManager.Instance.PlaySE(_damageAudio.name);
    }

    public void DeadAudioPlay()
    {
        AudioManager.Instance.PlaySE(_deadAudio.name);
    }

    public void ShieldAudioPlay()
    {
        AudioManager.Instance.PlaySE(_shieldAudio.name);
    }

    public void ShieldBreakAudioPlay()
    {
        AudioManager.Instance.PlaySE(_shieldBreakAudio.name);
    }

    public void ParryAudio()
    {
        AudioManager.Instance.PlaySE(_parryAudio.name);
    }
}
