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

    public void CatInAudioPlay()
    {
        AudioManager.Instance.PlayBGM(_catInAudio.name);
    }

    public void AttackAudioPlay()
    {
        AudioManager.Instance.PlayBGM(_attackAudio.name);
    }

    public void DamageAudioPlay()
    {
        AudioManager.Instance.PlayBGM(_damageAudio.name);
    }

    public void DeadAudioPlay()
    {
        AudioManager.Instance.PlayBGM(_deadAudio.name);
    }

    public void ShieldAudioPlay()
    {
        AudioManager.Instance.PlayBGM(_shieldAudio.name);
    }

    public void ShieldBreakAudioPlay()
    {
        AudioManager.Instance.PlayBGM(_shieldBreakAudio.name);
    }
}
