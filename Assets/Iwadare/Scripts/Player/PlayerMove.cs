using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Rigidbody2D _playerRb;
    [Tooltip("x方向のSpeed")]
    [SerializeField] float _dashSpeed = 2f;
    [SerializeField] float _jumpPower = 1f;
    [SerializeField] int MaxJumpCount = 2;
    public void Init()
    {
        _playerRb = GetComponent<Rigidbody2D>();
    }

    public void MoveUpdate(PlayerController controller)
    {
        Jump(controller);
    }

    public void MoveFixedUpdate(PlayerController controller)
    {
        if (!Input.GetButton("StopMove"))
        {
            Move(controller.X);
        }
    }

    //キャラを左右に動かす処理
    private void Move(float x)
    {
        var y = _playerRb.velocity.y;
        var move = (Vector2.right * x).normalized * _dashSpeed;
        var dir = new Vector2(move.x, y);
        _playerRb.velocity = dir;
    }

    // ジャンプの処理
    void Jump(PlayerController controller)
    {
        if (Input.GetButtonDown("Jump") && controller._currentJumpCount < MaxJumpCount)
        {
            Debug.Log("ジャンプ！");
            _playerRb.velocity = Vector2.zero;
            _playerRb.AddForce(Vector2.up * _jumpPower,ForceMode2D.Impulse);
            controller._currentJumpCount++;
        }
    }
}
