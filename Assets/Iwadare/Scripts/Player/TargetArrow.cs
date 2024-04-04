using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetArrow : MonoBehaviour
{
    PlayerController _controller;
    [SerializeField] float _rotateGap = 90f;

    public void Init(PlayerController playerController)
    {
        _controller = playerController;
        PlayerDirection(playerController.PlayerSprite.localScale);
    }

    // Update is called once per frame
    void Update()
    {
        var x = Input.GetAxisRaw("Horizontal");
        var y = Input.GetAxisRaw("Vertical");
        if(x != 0 || y != 0)
        {
            RotationArrow(x,y);
        }
    }

    void RotationArrow(float x,float y)
    {
        var rota = transform.eulerAngles;
        rota.z = ArrowDirection(x, y);
        transform.eulerAngles = rota;
    }

    float ArrowDirection(float x,float y)
    {
        var rad = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
        if(_controller.IsGround && rad < 0)
        {
            var dir = _controller.PlayerSprite.localScale.x;
            return _rotateGap * (dir / Mathf.Abs(dir));
        }
        else
        {
            return rad + _rotateGap;
        }
    }

    public void ResetDirection()
    {
        if(transform.eulerAngles.z < _rotateGap)
        {
            PlayerDirection(_controller.PlayerSprite.localScale);
        }
    }

    /// <summary>初期の向きを設定するときに呼ばれるメソッド</summary>
    /// <param name="playerScale"></param>
    void PlayerDirection(Vector3 playerScale)
    {
        var rota = transform.eulerAngles;
        if(playerScale.x > 0) { rota.z = -90f; }
        else { rota.z = 90f; }
        transform.eulerAngles = rota;
    }

    /// <summary>攻撃時に呼ばれるメソッド</summary>
    public void AttackSlash()
    {

    }
}