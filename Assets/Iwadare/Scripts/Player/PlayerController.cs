using DG.Tweening;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerMove))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerAudio))]
public class PlayerController : MonoBehaviour, PauseTimeInterface
{
    [SerializeField] SpriteRenderer _currentPlayerArrowSprite;
    [SerializeField] Collider2D _playerCollition;
    [Tooltip("PlayerのHP"), Header("PlayerのHP")]
    [SerializeField] float _playerDefaultHP = 100;
    float _currentPlayerHP;
    [SerializeField] Slider _playerHPSlider;
    [Tooltip("プレイヤーのアニメーション(下半身)")]
    public Animator _downPlayerAnim;
    [Tooltip("プレイヤーのアニメーション(上半身)")]
    public Animator _upPlayerAnim;
    [SerializeField] TrailRenderer _katanaTrail;
    [Tooltip("プレイヤーの左右反転させるスプライト")]
    [SerializeField] Transform _playerObj;
    public Transform PlayerObj => _playerObj;

    [NonSerialized] public Rigidbody2D _playerRb;

    [Tooltip("Playerのジャンプ時、着地時のFric"), Header("Playerのジャンプ時、着地時のFric")]
    [SerializeField] PhysicsMaterial2D _playerPhysicFric;
    [SerializeField] PhysicsMaterial2D _playerPhysicNonFric;

    [SerializeField] BulletPoolActive _missParticlePool;
    [SerializeField] BulletPoolActive _hitParticlePool;
    public BulletPoolActive HitParticlePool => _hitParticlePool;
    [SerializeField] BulletPoolActive _reflectHitPool;
    BulletPoolActive _numberPool;
    PlayerSpecialGuage _specialGuage;
    [NonSerialized] public PlayerAudio _audio;
    PlayerSkillSet _playerSkillSet;
    /// <summary>移動系の変数</summary>
    PlayerMove _moveScript;
    
    [Tooltip("x方向の移動")]
    float _x = 0;
    public float X => _x;
    float _y = 0;
    public float Y => _y;
    bool _isGround;
    [NonSerialized] public float _currentJumpCount;
    public bool IsGround => _isGround;

    /// <summary>攻撃系の変数</summary>
    [Tooltip("Playerの弾を出す向きを設定するScripts"), Header("ArrowRotaのオブジェクトを入れる。")]
    [SerializeField] AttackTargetArrow _targetArrowScript;

    [Tooltip("Playerの状態")]
    [NonSerialized] public PlayerState _playerState;

    [SerializeField] float _invisibleTime = 0.5f;
    [SerializeField] float _attackWaitTime = 0.5f;
    float _currentInvisibleTime = 0f;


    [SerializeField] bool _isInvisible;
    [SerializeField] SpriteRenderer _guardSprite;
    [SerializeField] ParticleSystem _healingObj;
    [SerializeField] AudioClip _healingAudioClip;
    Image _guardImage;
    bool _isGuard = false;
    [NonSerialized] public bool _isAvoidCoolTime = false;

    float _timeScale;
    public float TimeScale => _timeScale;

    Vector3 _tmpVelocity;
    float _tmpGravity;

    [Tooltip("Input系")]
    public PlayerInput _playerInput;
    bool _isAttack = false;
    public bool IsAttack => _isAttack;
    [Tooltip("ダメージ時のシェイク設定"),Header("ダメージ時のシェイク設定")]
    [SerializeField] Transform _shakeObj;
    [SerializeField] float _shakeTime = 0.5f;
    [SerializeField] float _shakePower = 0.1f;
    Tween _shakeTween;

    public void Init(SetPlayerStruct setPlayer)
    {
        /// シーン上の変数Set
        _playerHPSlider = setPlayer._playerHpSlider;
        _hitParticlePool = setPlayer._hitParticlePool;
        _missParticlePool = setPlayer._missBulletPool;
        _reflectHitPool = setPlayer._reflectParticlePool;
        _guardImage = setPlayer._shieldImage;
        _numberPool = setPlayer._numberPool;
        _guardImage.enabled = false;
        _katanaTrail.enabled = false;
        /// GetComponent
        _audio = GetComponent<PlayerAudio>();
        _moveScript = GetComponent<PlayerMove>();
        _playerSkillSet = GetComponent<PlayerSkillSet>();
        _playerRb = GetComponent<Rigidbody2D>();
        /// Init
        if (!_isInvisible) _guardSprite.enabled = false;
        _playerState |= PlayerState.NormalState;
        _timeScale = TimeScaleManager.Instance.DefaultTimeScale;
        _currentPlayerHP = _playerDefaultHP;
        _playerHPSlider.value = _currentPlayerHP / _playerDefaultHP;
        _targetArrowScript.Init(this);
        _specialGuage = InGameManager.Instance._playerSpecialGuage;
        _specialGuage.Init();
        _moveScript.MoveInit();
        // InputSystem初期化
        InputSet();
        /// ActionSet
        TimeScaleManager.ChangeTimeScaleAction += TimeScaleChange;
        TimeScaleManager.StartPauseAction += StartPause;
        TimeScaleManager.EndPauseAction += EndPause;
    }

    private void InputSet()
    {
        _playerInput = new PlayerInput();

        var player = _playerInput.Player;
        player.Move.performed += ct => 
        {
            _x = ct.ReadValue<Vector2>().x;
            if(-0.01f < _x && _x< 0.01f) _x = 0f;
        };
        player.Move.performed += ct => 
        {
            _y = ct.ReadValue<Vector2>().y;
            if (-0.01f < _y &&_y < 0.01f) _y = 0f;
        };
        player.Move.canceled += ct => _x = 0f;
        player.Move.canceled += ct => _y = 0f;
        player.Fire.performed += ct => _isAttack = true;
        player.Fire.canceled += ct => _isAttack = false;

        _playerInput.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
        ///ActionReset
        TimeScaleManager.ChangeTimeScaleAction -= TimeScaleChange;
        TimeScaleManager.StartPauseAction -= StartPause;
        TimeScaleManager.EndPauseAction -= EndPause;
    }

    // Update is called once per frame
    void Update()
    {
        if ((int)(_playerState & PlayerState.DeathState) == 0 && GameStateManager.Instance.GameState == GameState.InBattleState)
        {
            
            if ((int)(_playerState & PlayerState.AvoidState) == 0)
            {
                FlipX(_x);
                _moveScript.MoveUpdate(this);
            }

            if (_isAttack && (int)(_playerState & (PlayerState.AttackState | PlayerState.AvoidState | PlayerState.UseSkillState)) == 0)
            {
                _playerState |= PlayerState.AttackState;
                StartCoroutine(Attack());
            }

            if (_playerInput.Player.Avoid.WasPressedThisFrame()
                && (int)(_playerState & (PlayerState.AttackState | PlayerState.AvoidState | PlayerState.UseSkillState)) == 0
                && !_isAvoidCoolTime)
            {
                _playerState |= PlayerState.AvoidState;
                _playerState &= ~PlayerState.NormalState;
                StartCoroutine(_moveScript.Avoidance(this, _playerRb, _playerObj));
            }

            if(_playerInput.Player.Special.WasPressedThisFrame())
            {
                if(InGameManager.Instance._playerSpecialGuage.IsCostChack(InGameManager.Instance._playerSpecialGuage.MaxGuage))
                {
                    InGameManager.Instance._playerSpecialGuage.UseGuage();
                    _playerSkillSet.AllSkill?.UseSkill(this);
                    _playerSkillSet.HalfSkill?.UseSkill(this);
                }
                else if (InGameManager.Instance._playerSpecialGuage.IsCostChack(InGameManager.Instance._playerSpecialGuage.MaxGuage / 2))
                {
                    InGameManager.Instance._playerSpecialGuage.UseGuage();
                    _playerSkillSet.HalfSkill?.UseSkill(this);
                }
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
        _targetArrowScript.ArrowUpdate(this);
    }

    // 攻撃処理
    IEnumerator Attack()
    {
        _downPlayerAnim.SetBool("Attack", true);
        _upPlayerAnim.SetBool("Attack", true);
        _katanaTrail.enabled = true;
        yield return StartCoroutine(_targetArrowScript.AttackTime(this,1, _upPlayerAnim));
        IconManager.Instance.UpdateIcon(_attackWaitTime,TargetIcon.Attack);
        _katanaTrail.enabled = false;
        _downPlayerAnim.SetBool("Attack", false);
        _upPlayerAnim.SetBool("Attack", false);
        yield return WaitforSecondsCashe.Wait(_attackWaitTime);
        _playerState &= ~PlayerState.AttackState;

    }

    // 左右にキャラを向ける処理
    void FlipX(float x)
    {
        if (x != 0)
        {
            var scale = _playerObj.transform.localScale;
            if (x > 0)
            {
                scale.x = Mathf.Abs(scale.x);
            }
            else
            {
                scale.x = -Mathf.Abs(scale.x);
            }
            _playerObj.transform.localScale = scale;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            _isGround = true;
            //Debug.Log("接地");
            _targetArrowScript.ResetDirection();
            _currentJumpCount = 0;
            _playerRb.sharedMaterial = _playerPhysicFric;
            _downPlayerAnim.SetBool("IsGround", _isGround);
            IconManager.Instance.UpdateIcon(_currentJumpCount,TargetIcon.Jump);
            _playerState &= ~PlayerState.ImpactState;
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
            _downPlayerAnim.SetBool("IsGround", _isGround);
        }
    }

    public void AddDamage(int damage)
    {
        
        _currentPlayerHP = Mathf.Min(_playerDefaultHP,_currentPlayerHP -= damage);

        if (_currentPlayerHP <= 0)
        {
            _currentPlayerHP = 0;
            Death();
        }

        var number = _numberPool.GetPool().GetComponent<NumberColorScripts>();
        number.transform.position = transform.position;
        if(damage > 0)
        {
            var hitParticle = _hitParticlePool.GetPool().GetComponent<ParticleDestroy>();
            if (hitParticle)
            {
                hitParticle.transform.position = transform.position;
                hitParticle.Init();
            }
            _audio.DamageAudio();
            number.NumberColorChange(number._damageColor);
        }
        else
        {
            _healingObj.Play();
            AudioManager.Instance.PlaySE(_healingAudioClip.name);
            number.NumberColorChange(number._healColor);
        }
        number.MoveNumber(-damage);
        if (_shakeTween != null && _shakeTween.IsActive()) _shakeObj.DOComplete();
        _shakeTween = _shakeObj.DOShakePosition(_shakeTime,_shakePower).SetLink(_shakeObj.gameObject);

        BattleUISlider.Instance.PlayerHPSlider(_currentPlayerHP, _playerDefaultHP);
    }

    public void AddBulletDamage(int damage)
    {
        _audio.DamageAudio();
        if (_isInvisible || _isGuard
            || (int)(_playerState & (PlayerState.InvisibleState | PlayerState.DeathState | PlayerState.AvoidState | PlayerState.UseSkillState)) != 0
            || GameStateManager.Instance.GameState == GameState.BattleEndState)
        {
            var missParticle = _missParticlePool.GetPool().GetComponent<ParticleDestroy>();
            var reflectHit = _reflectHitPool.GetPool().GetComponent<ParticleDestroy>();
            missParticle.transform.position = reflectHit.transform.position = transform.position;
            missParticle.Init();
            reflectHit.Init();
            _specialGuage.AddGuage(_specialGuage.AvoidBulletAddGuage);

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

        var number = _numberPool.GetPool().GetComponent<NumberColorScripts>();
        number.transform.position = transform.position;
        number.NumberColorChange(number._damageColor);
        number.MoveNumber(-damage);

        if (_shakeTween != null && _shakeTween.IsActive()) _shakeObj.DOComplete();
        _shakeTween = _shakeObj.DOShakePosition(_shakeTime, _shakePower).SetLink(_shakeObj.gameObject);

        BattleUISlider.Instance.PlayerHPSlider(_currentPlayerHP,_playerDefaultHP);
    }

    public void Death()
    {
        _playerState |= PlayerState.DeathState;
        _playerState &= ~PlayerState.NormalState;
        StartCoroutine(DeathCoroutine());
    }

    public IEnumerator DeathCoroutine()
    {
        _downPlayerAnim.Play("Death");
        IconManager.Instance.UpdateIcon(0, TargetIcon.Jump);
        _playerRb.gravityScale = 0f;
        _playerRb.velocity = Vector3.zero;
        _currentPlayerArrowSprite.enabled = false;
        _playerCollition.enabled = false;
        yield return WaitforSecondsCashe.Wait(4f);
        Destroy(this.gameObject);
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
        _audio.ShieldAudio();
        _guardSprite.enabled = true;
        _guardImage.enabled = true;
        _isGuard = true;
    }

    public void EndGuardMode()
    {
        if (!_isInvisible) _guardSprite.enabled = false;
        _audio.ShieldBreakAudio();
        _guardImage.enabled = false;
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
    ImpactState = 1 << 5,
    UseSkillState = 1 << 6,
}
