using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    [Tooltip("x方向のSpeed")]
    [SerializeField] float _dashSpeed = 2f;
    [SerializeField] float _jumpPower = 1f;
    [SerializeField] int MaxJumpCount = 2;

    public void MoveUpdate(PlayerController controller)
    {
        Jump(controller,controller._playerRb);
    }

    public void MoveFixedUpdate(PlayerController controller)
    {
        if (!Input.GetButton("StopMove"))
        {
            Move(controller.X,controller._playerRb);
        }
    }

    //キャラを左右に動かす処理
    private void Move(float x,Rigidbody2D rb)
    {
        var y = rb.velocity.y;
        var move = (Vector2.right * x).normalized * _dashSpeed;
        var dir = new Vector2(move.x, y);
        rb.velocity = dir;
    }

    // ジャンプの処理
    void Jump(PlayerController controller,Rigidbody2D rb)
    {
        if (Input.GetButtonDown("Jump") && controller._currentJumpCount < MaxJumpCount)
        {
            Debug.Log("ジャンプ！");
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * _jumpPower,ForceMode2D.Impulse);
            controller._currentJumpCount++;
        }
    }
}
