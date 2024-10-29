using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YuaiSpecialAttack_Col : MonoBehaviour
{
    [NonSerialized] public bool _isHit = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "PAttack")
        {
            Debug.Log("当たった");
            _isHit = true;
        }
    }
}
