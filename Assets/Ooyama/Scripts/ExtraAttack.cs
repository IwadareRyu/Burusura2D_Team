using System.Collections;
using UnityEngine;

public class ExtraAttack : MonoBehaviour
{
    [SerializeField] private GameObject _shadowPrefab;  // 攻撃オブジェクト
    [SerializeField] private float _rotationInterval = 0.3f;
    [SerializeField] private float _rotationStep = 15f;
    [SerializeField] private int _maxAttackCount = 10;
    [SerializeField] private float _attackSpeed = 5f;
    [SerializeField] private float _lifeTime = 1.0f;
    [SerializeField] private float _maxDis = 3.0f;
    [SerializeField] private Transform enemy;

    public void TriggerExtraAttack(int attackPower)
    {
        int attackCount = Mathf.Min(attackPower / 10, _maxAttackCount);

        float startAngle = Random.Range(0f, 360f);

        int direction = Random.value < 0.5f ? 1 : -1;

        StartCoroutine(PerformExtraAttack(enemy, attackCount, startAngle, direction));
    }

    private IEnumerator PerformExtraAttack(Transform enemy, int attackCount, float startAngle, int direction)
    {
        float currentAngle = startAngle;

        for (int i = 0; i < attackCount; i++)
        {
            float radians = currentAngle * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0f) * 1.5f;

            GameObject shadow = Instantiate(_shadowPrefab, enemy.position + offset, Quaternion.identity);

            Vector3 directionToEnemy = (enemy.position - shadow.transform.position).normalized;

            float angle = Mathf.Atan2(directionToEnemy.y, directionToEnemy.x) * Mathf.Rad2Deg;
            shadow.transform.rotation = Quaternion.Euler(0, 0, angle);

            //StartCoroutine(MoveAndDestroy(shadow, directionToEnemy, _attackSpeed, _lifeTime));
            StartCoroutine(MoveUntilOpposite(shadow, directionToEnemy, _attackSpeed, _maxDis));  

            currentAngle += _rotationStep * direction;
            yield return new WaitForSeconds(_rotationInterval);
        }
    }
    private IEnumerator MoveAndDestroy(GameObject obj, Vector3 direction, float speed, float lifetime)
    {
        float timer = 0f;

        while (timer < lifetime)
        {
            if (obj != null)
            {
                obj.transform.position += direction * speed * Time.deltaTime;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        if (obj != null)
        {
            Destroy(obj);
        }
    }
    private IEnumerator MoveUntilOpposite(GameObject obj, Vector3 direction, float speed, float maxDistance)
    {
        Vector3 startPosition = obj.transform.position;
        float movedDistance = 0f;

        while (movedDistance < maxDistance)
        {
            if (obj == null) yield break;

            Vector3 delta = direction * speed * Time.deltaTime;
            obj.transform.position += delta;

            movedDistance = Vector3.Distance(startPosition, obj.transform.position);
            yield return null;
        }

        if (obj != null)
        {
            Destroy(obj);
        }
    }
}
