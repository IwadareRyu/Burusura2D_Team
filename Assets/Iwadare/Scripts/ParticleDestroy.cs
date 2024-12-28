using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class ParticleDestroy : MonoBehaviour
{
    ParticleSystem _particle;
    [SerializeField] float _particleTime = 2f;
    float _currentTime;
    [SerializeField] bool _poolEffect = true;
    [SerializeField] bool _mixParticle = false;
    [SerializeField] ParticleSystem[] _mixParticleArray;

    private void Awake()
    {
        _particle = GetComponent<ParticleSystem>();
    }

    public void Init()
    {
        _particle.Play();
        if (_mixParticle)
        {
            for (var i = 0; i < _mixParticleArray.Length; i++)
            {
                _mixParticleArray[i].Play();
            }
        }
    }

    void Update()
    {
        _currentTime += Time.deltaTime;
        if (_currentTime >= _particleTime)
        {
            _currentTime = 0f;
            if(_poolEffect)gameObject.SetActive(false);
            else Destroy(gameObject);
        }
    }
}
