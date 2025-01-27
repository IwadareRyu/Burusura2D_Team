using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    [SerializeField] AudioClip _audio;

    public void AudioPlay()
    {
        AudioManager.Instance.PlaySE(_audio.name);
    }
}
