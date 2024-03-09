using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Animator _playerAnim;
    [Tooltip("x•ûŒü‚ÌSpeed")]
    [SerializeField] float _dashSpeed = 2f;

    [Tooltip("x•ûŒü‚ÌˆÚ“®")]
    float _x = 0;
    [Tooltip("y•ûŒü‚ÌˆÚ“®")]
    float _y = 0;

    Rigidbody2D _playerRb;
    // Start is called before the first frame update
    void Start()
    {
        _playerRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _x = Input.GetAxisRaw("Horizontal");
        FlipX(_x);
    }

    private void FixedUpdate()
    {
        _y = _playerRb.velocity.y;
        var move = (Vector2.right * _x).normalized * _dashSpeed;
        var dir = new Vector2(move.x, _y);
        _playerRb.velocity = dir;
    }

    void FlipX(float x)
    {

    }
}
