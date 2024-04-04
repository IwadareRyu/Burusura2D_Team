using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEditor.Tilemaps;
using UnityEngine;

//[RequireComponent<>]
public class PlayerController : MonoBehaviour
{
    [Tooltip("Player�̒e���o��������ݒ肷��Scripts"),Header("ArrowRota�̃I�u�W�F�N�g������B")]
    [SerializeField] TargetArrow _targetArrowScript;
    [SerializeField] Animator _playerAnim;
    [SerializeField] Transform _playerSprite;
    public Transform PlayerSprite => _playerSprite;
    [Tooltip("x������Speed")]
    [SerializeField] float _dashSpeed = 2f;
    [SerializeField] float _jumpPower = 1f;
    [SerializeField] float _attackCoolTime = 2f;
    [SerializeField] int MaxAttackCount = 3;
    [SerializeField] int MaxJumpCount = 2;

    [Tooltip("x�����̈ړ�")]
    float _x = 0;
    [Tooltip("y�����̈ړ�")]
    float _y = 0;

    bool _isAttackTime = false;
    Rigidbody2D _playerRb;
    bool _isGround;
    public bool IsGround => _isGround;

    // Start is called before the first frame update
    void Start()
    {
        _targetArrowScript.Init(this);
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
            var scale = _playerSprite.transform.localScale;
            scale.x = x * Mathf.Abs(scale.x);
            _playerSprite.transform.localScale = scale;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _isGround = true;
        _targetArrowScript.ResetDirection();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _isGround = false;
    }
}
