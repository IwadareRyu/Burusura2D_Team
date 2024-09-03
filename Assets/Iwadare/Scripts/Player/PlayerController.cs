using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerMove))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour,HitStopInterface
{
    [Tooltip("プレイヤーのアニメーション")]
    public Animator _playerAnim;
    [Tooltip("プレイヤーの左右反転させるスプライト")]
    [SerializeField] Transform _playerSprite;
    public Transform PlayerSprite => _playerSprite;

    [NonSerialized]public Rigidbody2D _playerRb;
    [SerializeField]PhysicsMaterial2D _playerPhysicFric;
    [SerializeField] PhysicsMaterial2D _playerPhysicNonFric;


    /// <summary>移動系の変数</summary>
    PlayerMove _moveScript;

    [Tooltip("x方向の移動")]
    float _x = 0;
    public float X => _x;
    bool _isGround;
    [NonSerialized] public float _currentJumpCount;
    public bool IsGround => _isGround;

    /// <summary>攻撃系の変数</summary>
    [Tooltip("Playerの弾を出す向きを設定するScripts"), Header("ArrowRotaのオブジェクトを入れる。")]
    [SerializeField] AttackTargetArrow _targetArrowScript;
    bool _isAttackTime = false;

    // Start is called before the first frame update
    void Start()
    {
        _targetArrowScript.Init(this);
        _moveScript = GetComponent<PlayerMove>();
        _playerRb = GetComponent<Rigidbody2D>();
        HitStopManager.instance._speedHitStopActionStart += HitStopStart;
        HitStopManager.instance._speedHitStopActionEnd += HitStopEnd;
    }

    private void OnDisable()
    {
        HitStopManager.instance._speedHitStopActionStart -= HitStopStart;
        HitStopManager.instance._speedHitStopActionEnd -= HitStopEnd;
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

    // 攻撃処理
    IEnumerator Attack()
    {
        yield return StartCoroutine(_targetArrowScript.AttackTime(1));
        _isAttackTime = false;
    }

    // 左右にキャラを向ける処理
    void FlipX(float x)
    {
        if (x != 0)
        {
            var scale = _playerSprite.transform.localScale;
            scale.x = x * Mathf.Abs(scale.x);
            _playerSprite.transform.localScale = scale;
        }
    }




    // 攻撃を出す処理
    void InstansAttack()
    {
        //向いている方向に合わせて弾を打つ処理
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            _isGround = true;
            Debug.Log("接地");
            _targetArrowScript.ResetDirection();
            _currentJumpCount = 0;
            _playerRb.sharedMaterial = _playerPhysicFric;
        }

        if(collision.tag == "Enemy")
        {
            //Damage判定
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            _isGround = false;
            _currentJumpCount = 1;
            _playerRb.sharedMaterial = _playerPhysicNonFric;
        }
    }

    public void HitStopStart(float _hitStopPower)
    {
        
    }

    public void HitStopEnd()
    {

    }
}
