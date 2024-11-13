using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class ParticleDestroy : MonoBehaviour
{
    ParticleSystem _particle;
    [SerializeField] float _particleTime = 2f;
    float _currentTime;

    private void Awake()
    {
        _particle = GetComponent<ParticleSystem>();
    }

    public void Init()
    {
        _particle.Play();
        //_particleTime = _particle.time;
        //Debug.Log(_particleTime);
    }

    void Update()
    {
        _currentTime += Time.deltaTime;
        if (_currentTime >= _particleTime)
        {
            _currentTime = 0f;
            gameObject.SetActive(false);
        }
    }
}
