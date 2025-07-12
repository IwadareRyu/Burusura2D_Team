using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    float _attackValue = 1f;
    float _attacksize;
    BulletPoolActive _slashPool;
    EnemyBase _target;
    bool _isAttack = false;
    bool _isOneShot = false;
    bool _isInterval = false;


    public void Start()
    {
        Destroy(gameObject, 0.5f);
        var attackcolider = GetComponent<CircleCollider2D>();
        _attacksize = attackcolider.radius;
    }

    public void Init(BulletPoolActive pool,float attackValue)
    {
        _slashPool = pool;
        _attackValue = attackValue;
        _isInterval = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isAttack || !_isInterval) return;

        _isAttack = false;
        if (_target != null)
        {
            _isOneShot = true;
            _target.AddDamage(_attackValue);
        }

        //var enemys = Physics2D.OverlapCircleAll(transform.position, _attacksize);
        //foreach (var enemy in enemys)
        //{
        //    if (enemy.gameObject.tag == "Enemy")
        //    {
        //        var slash = _slashPool.GetPool().GetComponent<ParticleDestroy>();
        //        slash.transform.position = enemy.transform.position;
        //        slash.Init();
        //        Destroy(enemy.gameObject);
        //    }
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isOneShot) return;

        if (collision.TryGetComponent<EnemyBase>(out var enemy))
        {
            _target = enemy;
            _isAttack = true;
        }
    }

}
