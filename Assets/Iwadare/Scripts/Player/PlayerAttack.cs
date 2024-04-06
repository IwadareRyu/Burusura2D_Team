using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    float _attacksize;

    public void Start()
    {
        Destroy(gameObject, 0.5f);
        var attackcolider = GetComponent<CircleCollider2D>();
        _attacksize = attackcolider.radius;
    }

    // Update is called once per frame
    void Update()
    {
        var enemys = Physics2D.OverlapCircleAll(transform.position, _attacksize);
        foreach (var enemy in enemys)
        {
            if (enemy.tag == "Enemy") { Destroy(enemy); }
        }
    }


}
