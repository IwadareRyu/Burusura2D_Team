using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftScripts : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private float _moveRange = 2f;

    private float _startY;

    // Start is called before the first frame update
    void Start()
    {
        _startY = transform.position.y;
    }
    private void FixedUpdate()
    {
        float newY = _startY + Mathf.Sin(Time.time * _moveSpeed) * _moveRange;
        transform.position = new Vector3(transform.position.x,newY,transform.position.z);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision is BoxCollider2D)
        {
            collision.transform.SetParent(transform);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision)
        {
            if (collision.tag == "Player" && collision is BoxCollider2D)
            {
                collision.transform.parent = null;
            }
        }
    }
}
