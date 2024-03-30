using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEditor.Tilemaps;
using UnityEngine;

//[RequireComponent<>]
public class PlayerController : MonoBehaviour
{
    [SerializeField] Animator _playerAnim;
    [Tooltip("x������Speed")]
    [SerializeField] float _dashSpeed = 2f;
    [SerializeField] float _jumpPower = 1f;
    [SerializeField] float _attackCoolTime = 2f;
    [SerializeField] float MaxAttackCount = 3f;

    [Tooltip("x�����̈ړ�")]
    float _x = 0;
    [Tooltip("y�����̈ړ�")]
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

    //�L���������E�ɓ���������
    private void Move()
    {
        _y = _playerRb.velocity.y;
        var move = (Vector2.right * _x).normalized * _dashSpeed;
        var dir = new Vector2(move.x, _y);
        _playerRb.velocity = dir;
    }

    // ���E�ɃL�����������鏈��
    void FlipX(float x)
    {
        if (x != 0)
        {
            var scale = transform.localScale;
            scale.x = x;
            transform.localScale = scale;
        }
    }

    // �W�����v�̏���
    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("�W�����v�I");
            _playerRb.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
        }
    }

    // �U������
    IEnumerator Attack()
    {
        yield return StartCoroutine(AttackTime(1));
        _isAttackTime = false;
    }

    // �U���̃N�[���^�C���̏���
    IEnumerator AttackTime(int count)
    {
        Debug.Log($"{count}��ڂ̍U���I");
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

    // �U�����o������
    void InstansAttack()
    {
        //�����Ă�������ɍ��킹�Ēe��ł���
    }
}
