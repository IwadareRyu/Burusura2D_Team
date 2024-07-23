using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeTimeParticle : MonoBehaviour
{
    [SerializeField] float _lifeTime = 2f; 
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject,_lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
