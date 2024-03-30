using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEditor.Tilemaps;
using UnityEngine;

//[RequireComponent<>]
public class PlayerController : MonoBehaviour
{
    [SerializeField] Animator _playerAnim;
    [Tooltip("x方向のSpeed")]
    [SerializeField] float _dashSpeed = 2f;
    [SerializeField] float _jumpPower = 1f;
    [SerializeField] float _attackCoolTime = 2f;
    [SerializeField] float MaxAttackCount = 3f;

    [Tooltip("x方向の移動")]
    float _x = 0;
    [Tooltip("y方向の移動")]
    float _y = 0;

    bool _isAttackTime = false;
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
        if (Input.GetButtonDown("Fire1") && !_isAttackTime)
        {
            _isAttackTime = true;
            StartCoroutine(Attack());
        }
    }

    private void FixedUpdate()
    {
        Move();
        Jump();
    }

    //キャラを左右に動かす処理
    private void Move()
    {
        _y = _playerRb.velocity.y;
        var move = (Vector2.right * _x).normalized * _dashSpeed;
        var dir = new Vector2(move.x, _y);
        _playerRb.velocity = dir;
    }

    // 左右にキャラを向ける処理
    void FlipX(float x)
    {
        if (x != 0)
        {
            var scale = transform.localScale;
            scale.x = x;
            transform.localScale = scale;
        }
    }

    // ジャンプの処理
    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("ジャンプ！");
            _playerRb.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
        }
    }

    // 攻撃処理
    IEnumerator Attack()
    {
        yield return StartCoroutine(AttackTime(1));
        _isAttackTime = false;
    }

    // 攻撃のクールタイムの処理
    IEnumerator AttackTime(int count)
    {
        Debug.Log($"{count}回目の攻撃！");
        InstansAttack();
        yield return new WaitForFixedUpdate();
        for (var time = 0f;time < _attackCoolTime;time += Time.deltaTime)
        {
            if(Input.GetButtonDown("Fire1") && count < MaxAttackCount)
            {
                yield return StartCoroutine(AttackTime(count + 1));
                break;
            }
            yield return null;
        }
    }

    // 攻撃を出す処理
    void InstansAttack()
    {
        //向いている方向に合わせて弾を打つ処理
    }
}
