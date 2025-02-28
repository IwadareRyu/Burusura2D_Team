using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    float _attacksize;
    BulletPoolActive _slashPool;

    public void Start()
    {
        Destroy(gameObject, 0.5f);
        var attackcolider = GetComponent<CircleCollider2D>();
        _attacksize = attackcolider.radius;
    }

    public void Init(BulletPoolActive pool)
    {
        _slashPool = pool;
    }

    // Update is called once per frame
    void Update()
    {
        var enemys = Physics2D.OverlapCircleAll(transform.position, _attacksize);
        foreach (var enemy in enemys)
        {
            if (enemy.gameObject.tag == "Enemy") 
            {
                var slash = _slashPool.GetPool().GetComponent<ParticleDestroy>();
                slash.transform.position = enemy.transform.position;
                slash.Init();
                Destroy(enemy.gameObject); 
            }
        }
    }


}
