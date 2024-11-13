using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerSlashing : MonoBehaviour
{
    Rigidbody2D _rb;
    PlayerSpecialGuage _specialGuage;
    [SerializeField] float _moveSpeed = 10f;
    [SerializeField] float _moveTime = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.velocity = transform.up * _moveSpeed;
        StartCoroutine(MoveTime());
    }

    IEnumerator MoveTime()
    {
        yield return WaitforSecondsCashe.Wait(_moveTime);
        _rb.velocity = Vector2.zero;
    }


}
