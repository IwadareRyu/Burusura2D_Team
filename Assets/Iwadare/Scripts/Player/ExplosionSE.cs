using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSE : MonoBehaviour
{
    [SerializeField] AudioClip _explosionSE;

    public void Explosion()
    {
        AudioManager.Instance.PlaySE(_explosionSE.name);
    }
}
