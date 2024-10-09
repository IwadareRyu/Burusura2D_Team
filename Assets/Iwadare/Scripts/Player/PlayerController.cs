using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerMove))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour,HitStopInterface
{
    [Tooltip("PlayerのHP"), Header("PlayerのHP")]
    [SerializeField]float _playerDefaultHP = 100;
    float _currentPlayerHP;
    [SerializeField] Slider _playerHPSlider;
    [Tooltip("プレイヤーのアニメーション")]
    public Animator _playerAnim;
    [Tooltip("プレイヤーの左右反転させるスプライト")]
    [SerializeField] Transform _playerSprite;
    public Transform PlayerSprite => _playerSprite;

    [NonSerialized]public Rigidbody2D _playerRb;

    [Tooltip("Playerのジャンプ時、着地時のFric"), Header("Playerのジャンプ時、着地時のFric")]
    [SerializeField] PhysicsMaterial2D _playerPhysicFric;
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

    [Tooltip("Playerの状態")]
    PlayerState _playerState;
    public PlayerState PlayerState => _playerState;

    [SerializeField] float _invisibleTime = 0.5f;
    float _currentInvisibleTime = 0f;
    

    [SerializeField] bool _isInvisible;

    // Start is called before the first frame update
    void Start()
    {
        _currentPlayerHP = _playerDefaultHP;
        _targetArrowScript.Init(this);
        _moveScript = GetComponent<PlayerMove>();
        _playerRb = GetComponent<Rigidbody2D>();
        HitStopManager.instance._speedHitStopActionStart += HitStopStart;
        HitStopManager.instance._speedHitStopActionEnd += HitStopEnd;
        Init();
    }

    public void Init()
    {
        _playerState = PlayerState.NormalState;
    }

    private void OnDisable()
    {
        HitStopManager.instance._speedHitStopActionStart -= HitStopStart;
        HitStopManager.instance._speedHitStopActionEnd -= HitStopEnd;
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerState != PlayerState.DeathState && GameStateManager.instance.GameState == GameState.InBattleState)
        {
            _x = Input.GetAxisRaw("Horizontal");
            FlipX(_x);
            _moveScript.MoveUpdate(this);
            if (Input.GetButton("Fire1") && !_isAttackTime)
            {
                _isAttackTime = true;
                StartCoroutine(Attack());
            }
            if(_playerState == PlayerState.InvisibleState)
            {
                _currentInvisibleTime += Time.deltaTime;
                if(_currentInvisibleTime >= _invisibleTime)
                {
                    _currentInvisibleTime = 0;
                    _playerState = PlayerState.NormalState;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (_playerState != PlayerState.DeathState && GameStateManager.instance.GameState == GameState.InBattleState)
        {
            _moveScript.MoveFixedUpdate(this);
        }
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

    public void AddDamage(int damage)
    {
        if (_isInvisible || _playerState == PlayerState.InvisibleState) return;
        _currentPlayerHP -= damage;
        if(_currentPlayerHP <= 0)
        {
            _currentPlayerHP = 0;
            Death();
        }
        else
        {
            PlayerInvisible();
        }
        _playerHPSlider.value = _currentPlayerHP / _playerDefaultHP;
    }

    public void Death()
    {
        _playerState = PlayerState.DeathState;
    }

    public void PlayerInvisible()
    {
        _playerState = PlayerState.InvisibleState;
    }

    public void HitStopStart(float _hitStopPower)
    {

    }

    public void HitStopEnd()
    {

    }
}

public enum PlayerState
{
    NormalState,
    InvisibleState,
    DeathState,
}
