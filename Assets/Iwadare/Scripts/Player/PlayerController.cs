using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerMove))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour, PauseTimeInterface
{
    [Tooltip("PlayerのHP"), Header("PlayerのHP")]
    [SerializeField] float _playerDefaultHP = 100;
    float _currentPlayerHP;
    [SerializeField] Slider _playerHPSlider;
    [Tooltip("プレイヤーのアニメーション")]
    public Animator _playerAnim;
    [Tooltip("プレイヤーの左右反転させるスプライト")]
    [SerializeField] SpriteRenderer _playerSprite;
    public SpriteRenderer PlayerSprite => _playerSprite;

    [NonSerialized] public Rigidbody2D _playerRb;

    [Tooltip("Playerのジャンプ時、着地時のFric"), Header("Playerのジャンプ時、着地時のFric")]
    [SerializeField] PhysicsMaterial2D _playerPhysicFric;
    [SerializeField] PhysicsMaterial2D _playerPhysicNonFric;

    [SerializeField] BulletPoolActive _missParticlePool;
    [SerializeField] BulletPoolActive _hitParticlePool;
    [SerializeField] BulletPoolActive _reflectHitPool;
    /// <summary>移動系の変数</summary>
    PlayerMove _moveScript;
    PlayerSpecialGuage _specialGuage;

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
    [NonSerialized] public PlayerState _playerState;

    [SerializeField] float _invisibleTime = 0.5f;
    float _currentInvisibleTime = 0f;


    [SerializeField] bool _isInvisible;
    [SerializeField] SpriteRenderer _guardSprite;
    bool _isGuard = false;
    [NonSerialized] public bool _isAvoidCoolTime = false;

    float _timeScale;
    public float TimeScale => _timeScale;

    Vector3 _tmpVelocity;
    float _tmpGravity;
    // Start is called before the first frame update
    void Start()
    {
        _currentPlayerHP = _playerDefaultHP;
        _targetArrowScript.Init(this);
        _moveScript = GetComponent<PlayerMove>();
        _specialGuage = InGameManager.Instance._playerSpecialGuage;
        _specialGuage.Init();
        _playerRb = GetComponent<Rigidbody2D>();
        Init();
    }

    public void Init()
    {
        if (!_isInvisible) _guardSprite.enabled = false;
        _playerState |= PlayerState.NormalState;
        _timeScale = TimeScaleManager.Instance.DefaultTimeScale;
        TimeScaleManager.ChangeTimeScaleAction += TimeScaleChange;
        TimeScaleManager.StartPauseAction += StartPause;
        TimeScaleManager.EndPauseAction += EndPause;
    }

    private void OnDisable()
    {
        TimeScaleManager.ChangeTimeScaleAction -= TimeScaleChange;
        TimeScaleManager.StartPauseAction -= StartPause;
        TimeScaleManager.EndPauseAction -= EndPause;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(_playerState);
        if ((int)(_playerState & PlayerState.DeathState) == 0 && GameStateManager.Instance.GameState == GameState.InBattleState)
        {
            if ((int)(_playerState & PlayerState.AvoidState) == 0)
            {
                _x = Input.GetAxisRaw("Horizontal");
                FlipX(_x);
                _moveScript.MoveUpdate(this);
            }

            if (Input.GetButton("Fire1") && (int)(_playerState & PlayerState.AttackState) == 0)
            {
                _playerState |= PlayerState.AttackState;
                StartCoroutine(Attack());
            }

            if (Input.GetButton("Avoid")
                && (int)(_playerState & (PlayerState.AttackState | PlayerState.AvoidState)) == 0
                && !_isAvoidCoolTime)
            {
                _playerState |= PlayerState.AvoidState;
                _playerState &= ~PlayerState.NormalState;
                StartCoroutine(_moveScript.Avoidance(this, _playerRb, _playerSprite));
            }

            if ((int)(_playerState & PlayerState.InvisibleState) != 0)
            {
                _currentInvisibleTime += Time.deltaTime;
                if (_currentInvisibleTime >= _invisibleTime)
                {
                    _currentInvisibleTime = 0;
                    _playerState &= ~PlayerState.InvisibleState;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if ((int)(_playerState & (PlayerState.DeathState | PlayerState.AvoidState)) == 0
            && GameStateManager.Instance.GameState == GameState.InBattleState)
        {
            _moveScript.MoveFixedUpdate(this);
        }
    }

    // 攻撃処理
    IEnumerator Attack()
    {
        yield return StartCoroutine(_targetArrowScript.AttackTime(1));
        _playerState &= ~PlayerState.AttackState;
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

        if (collision.tag == "Enemy")
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

    public void AddBulletDamage(int damage)
    {
        if (_isInvisible || _isGuard
            || (int)(_playerState & (PlayerState.InvisibleState | PlayerState.DeathState | PlayerState.AvoidState)) != 0)
        {
            var missParticle = _missParticlePool.GetPool().GetComponent<ParticleDestroy>();
            var reflectHit = _reflectHitPool.GetPool().GetComponent<ParticleDestroy>();
            {
                missParticle.transform.position = reflectHit.transform.position = transform.position;
                missParticle.Init();
                reflectHit.Init();
                _specialGuage.AddGuage(_specialGuage.AvoidBulletAddGuage);
            }
            return;
        }

        var hitParticle = _hitParticlePool.GetPool().GetComponent<ParticleDestroy>();
        if (hitParticle)
        {
            hitParticle.transform.position = transform.position;
            hitParticle.Init();
        }
        _currentPlayerHP -= damage;
        if (_currentPlayerHP <= 0)
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
        _playerState |= PlayerState.DeathState;
        _playerState &= ~PlayerState.NormalState;
        GameStateManager.Instance.ChangeState(GameState.BattleEndState);
    }

    public void PlayerInvisible()
    {
        _playerState |= PlayerState.InvisibleState;
        _playerState |= PlayerState.NormalState;
    }

    public void TimeScaleChange(float timeScale)
    {
        _timeScale = timeScale;
    }

    public void StartPause()
    {
        _tmpVelocity = _playerRb.velocity;
        _playerRb.velocity = Vector2.zero;
        _tmpGravity = _playerRb.gravityScale;
        _playerRb.gravityScale = 0f;
    }

    public void EndPause()
    {
        _playerRb.velocity = _tmpVelocity;
        _playerRb.gravityScale = _tmpGravity;
    }

    public void StartGuardMode()
    {
        _guardSprite.enabled = true;
        _isGuard = true;
    }

    public void EndGuardMode()
    {
        if (!_isInvisible) _guardSprite.enabled = false;
        _isGuard = false;
    }

    public void HitStopStart(float _hitStopPower)
    {
        throw new NotImplementedException();
    }

    public void HitStopEnd()
    {
        throw new NotImplementedException();
    }
}

[Flags]
public enum PlayerState
{
    NormalState = 1,
    AttackState = 1 << 1,
    InvisibleState = 1 << 2,
    AvoidState = 1 << 3,
    DeathState = 1 << 4,
}
