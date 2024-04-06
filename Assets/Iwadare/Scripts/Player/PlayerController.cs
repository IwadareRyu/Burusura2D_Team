using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerMove))]
public class PlayerController : MonoBehaviour
{
    [Tooltip("�v���C���[�̃A�j���[�V����")]
    public Animator _playerAnim;
    [Tooltip("�v���C���[�̍��E���]������X�v���C�g")]
    [SerializeField] Transform _playerSprite;
    public Transform PlayerSprite => _playerSprite;

    /// <summary>�ړ��n�̕ϐ�</summary>
    PlayerMove _moveScript;

    [Tooltip("x�����̈ړ�")]
    float _x = 0;
    public float X => _x;
    bool _isGround;
    float _jumpCount;
    public bool IsGround => _isGround;

    /// <summary>�U���n�̕ϐ�</summary>
    [Tooltip("Player�̒e���o��������ݒ肷��Scripts"), Header("ArrowRota�̃I�u�W�F�N�g������B")]
    [SerializeField] AttackTargetArrow _targetArrowScript;
    bool _isAttackTime = false;

    // Start is called before the first frame update
    void Start()
    {
        _targetArrowScript.Init(this);
        _moveScript = GetComponent<PlayerMove>();
        _moveScript.Init();
    }

    // Update is called once per frame
    void Update()
    {
        _x = Input.GetAxisRaw("Horizontal");
        FlipX(_x);
        _moveScript.MoveUpdate(this);
        if (Input.GetButton("Fire1") && !_isAttackTime)
        {
            _isAttackTime = true;
            StartCoroutine(Attack());
        }
    }

    private void FixedUpdate()
    {
        _moveScript.MoveFixedUpdate(this);
    }

    // �U������
    IEnumerator Attack()
    {
        yield return StartCoroutine(_targetArrowScript.AttackTime(1));
        _isAttackTime = false;
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




    // �U�����o������
    void InstansAttack()
    {
        //�����Ă�������ɍ��킹�Ēe��ł���
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            _isGround = true;
            Debug.Log("�ڒn");
            _targetArrowScript.ResetDirection();
            _jumpCount = 1;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            _isGround = false;
            _jumpCount = 0;
        }
    }
}
