using System;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBase : MonoBehaviour
{
    PlayerController _player;
    public AnimationController_Enemy _enemyAnim;
    [NonSerialized]public AudioBoss _bossAudio;
    public PlayerController Player => _player;
    [Tooltip("敵のMaxHP"), Header("敵のMaxHP")]
    [SerializeField] float _maxHP = 100;
    public float MaxHP => _maxHP;
    [Tooltip("敵の現在HP")]
    [NonSerialized] public float _currentHP;
    [Tooltip("HP表示"), Header("HP表示")]
    [SerializeField] Slider _hpSlider;
    public BossState _bossState = BossState.StayState;
    public Rigidbody2D _enemyRb;
    public bool _useGravity = false;
    [SerializeField] float _stayTime = 0.5f;
    public float StayTime => _stayTime;

    [SerializeField] float _moveTime = 0.5f;
    public float MoveTime => _moveTime;

    [NonSerialized] public float _timeScale = 1f;
    [SerializeField] BulletPoolActive _slashEffect;
    [SerializeField] float _defaultMoveSpeed = 2f;
    [SerializeField] SpriteRenderer _shieldRenderer;
    [SerializeField] Image _shieldImage;
    public ParticleSystem _parryParticle;
    public Text _attackText;
    public Transform[] _movePoint;
    [NonSerialized] public int _minMovePointIndex;

    [NonSerialized] public bool _isFlip = false;
    [NonSerialized] public bool _isWaitDamage = false;
    [NonSerialized] public bool _isTrueDamage = false;
    [NonSerialized] public bool _isMove = false;
    [NonSerialized] public bool _isAttack = false;
    [NonSerialized] public bool _guard = false;

    public void BaseInit()
    {
        _enemyRb = GetComponent<Rigidbody2D>();
        _bossAudio = GetComponent<AudioBoss>();
        if (_enemyRb && _enemyRb.gravityScale != 0) _useGravity = true;
        _currentHP = MaxHP;
        DisplayHP();
        _shieldRenderer.enabled = false;
        _shieldImage.enabled = false;
        if (_attackText) _attackText.enabled = false;
        if (_enemyAnim)
        {
            _enemyAnim.gameObject.SetActive(true);
            _enemyAnim.ChangeAnimationSpain(_enemyAnim._initialName);
        }
        GameStateManager.Instance.InBattle();
    }

    public void PlayerSet(PlayerController player)
    {
        _player = player;
    }

    public void AddDamage(float damage = 1f, HitEffect effect = HitEffect.Slash)
    {
        if (_isWaitDamage) _isTrueDamage = true;
        if (_guard) return;

        if (effect == HitEffect.Slash)
        {
            var effectObj = _slashEffect.GetPool().GetComponent<ParticleSystem>();
            if (effectObj != null)
            {
                effectObj.transform.position = transform.position;
                effectObj.Play();
            }
            _bossAudio.DamageAudioPlay();
            
        }
        _currentHP -= damage;
        DisplayHP();
        HPChack();
    }

    public void PerforateDamage(float damage, HitEffect effect = HitEffect.Slash)
    {
        _currentHP -= damage;
        if (effect == HitEffect.Slash)
        {
            var effectObj = _slashEffect.GetPool().GetComponent<ParticleSystem>();
            if (effectObj != null)
            {
                effectObj.transform.position = transform.position;
                effectObj.Play();
            }
            _bossAudio.DamageAudioPlay();

        }
        DisplayHP();
        HPChack();
    }


    public void DisplayHP()
    {
        if (_hpSlider != null)
        {
            _hpSlider.value = _currentHP / _maxHP;
        }
    }

    public virtual void HPChack() { return; }

    public virtual bool SpecialHPChack() => false;

    public virtual void NextAction() { return; }

    public void SpawnBulletRef(BulletSpawnEnemy spawnBulletEnemy)
    {
        StartCoroutine(spawnBulletEnemy.BulletSpawn());
    }

    public void SpawnBulletRefDangerous(BulletSpawnEnemy spawnBulletEnemy, float _time = 1f)
    {
        StartCoroutine(spawnBulletEnemy.BulletSpawnDangerous(_time));
    }

    /// <summary>BossSpriteのFlipの設定</summary>
    /// <param name="flip">左ならfalse右ならtrueを渡す。</param>
    public void BossObjFlipX(bool flip)
    {
        var scale = _enemyAnim.transform.localScale;
        if (flip)
        {
            scale.x = -Mathf.Abs(scale.x);
            _isFlip = true;
        }
        else
        {
            scale.x = Mathf.Abs(scale.x);
            _isFlip = false;
        }
        _enemyAnim.transform.localScale = scale;
    }

    public void MoveEnemyX(bool leftvertical, float moveMag = 1f)
    {
        var velocity = _enemyRb.velocity;
        velocity.x = leftvertical ? Vector2.left.x * (_defaultMoveSpeed * moveMag) : Vector2.right.x * (_defaultMoveSpeed * moveMag);
        _enemyRb.velocity = velocity;
    }

    /// <summary>BossSpriteのRotationの設定。</summary>
    /// <param name="angle"></param>
    public void ObjSetRotation(float angle)
    {
        var rota = _enemyAnim.transform.localEulerAngles;
        rota.z = angle;
        _enemyAnim.transform.localEulerAngles = rota;
    }

    public void ResetState()
    {
        _isMove = false;
        _isAttack = false;
    }

    public void GuardMode()
    {
        if (_guard) return;
        _bossAudio.ShieldAudioPlay();
        _guard = true;
        _shieldRenderer.enabled = true;
        _shieldImage.enabled = true;
    }

    public void BreakGuardMode()
    {
        if (!_guard) return;
        _bossAudio.ShieldBreakAudioPlay();
        _guard = false;
        _shieldRenderer.enabled = false;
        _shieldImage.enabled = false;
    }

    public void ChangeAnimationObject(AnimationController_Enemy enemyAnim)
    {
        _enemyAnim.gameObject.SetActive(false);
        _enemyAnim = enemyAnim;
        _enemyAnim.gameObject.SetActive(true);
    }

    public enum BossState
    {
        StayState,
        MoveState,
        AttackState,
        ChangeActionState,
        NextActionState,
        DeathState,
    }
}

public enum HitEffect
{
    None,
    Slash,
    Blow,
}
