using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using UnityEngine;
using DG.Tweening;

public class PlayerMove : MonoBehaviour
{

    [Tooltip("x方向のSpeed")]
    [SerializeField] float _dashSpeed = 2f;
    [SerializeField] float _avoidSpeed = 10f;
    [SerializeField] float _avoidTime = 1f;
    [SerializeField] float _avoidCoolTime = 3f;
    [SerializeField] float _jumpPower = 1f;
    [SerializeField] int MaxJumpCount = 2;
    [SerializeField] Color _avoidColor = Color.white;
    [SerializeField] Color _normalColor = Color.white;
    [SerializeField] Color _avoidCompleteColor = Color.black;

    public void MoveUpdate(PlayerController controller)
    {
        Jump(controller,controller._playerRb);
    }

    public void MoveFixedUpdate(PlayerController controller)
    {
        if (!Input.GetButton("StopMove"))
        {
            Move(controller.X,controller._playerRb,controller.TimeScale);
        }
    }

    //キャラを左右に動かす処理
    private void Move(float x,Rigidbody2D rb,float _timeScale)
    {
        var y = rb.velocity.y;
        var move = (Vector2.right * x).normalized * _dashSpeed;
        var dir = new Vector2(move.x, y);
        rb.velocity = dir * _timeScale;
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

    public IEnumerator Avoidance(PlayerController controller,Rigidbody2D rb,SpriteRenderer playerSprite)
    {
        playerSprite.color = _avoidColor;
        var dirScale = playerSprite.transform.localScale.x;
        var tmpGravity = rb.gravityScale;
        rb.gravityScale = 0;
        for (float currentTime = 0; currentTime < _avoidTime; currentTime += Time.deltaTime)
        {
            Vector2 dir;
            if (dirScale >= 0) dir = Vector2.right * _avoidSpeed;
            else dir = Vector2.left * _avoidSpeed;
            rb.velocity = dir * EaseFunction.EaseInExpoReverse(_avoidTime,currentTime);
            yield return WaitforSecondsCashe.Wait(Time.deltaTime);  
        }
        rb.velocity = Vector2.zero;
        rb.gravityScale = tmpGravity;
        controller._isAvoidCoolTime = true;
        controller._playerState &= ~PlayerState.AvoidState;
        controller._playerState |= PlayerState.NormalState;
        playerSprite.color = _normalColor;
        yield return WaitforSecondsCashe.Wait(_avoidCoolTime);
        playerSprite.color = _avoidCompleteColor;
        yield return WaitforSecondsCashe.Wait(0.1f);
        playerSprite.color = _normalColor;
        controller._isAvoidCoolTime = false;
    }
}
