using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBGM : MonoBehaviour
{
    [SerializeField] AudioClip bgmClip;
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlayBGM(bgmClip.name);
    }
}
