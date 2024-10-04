using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnShotLine : MonoBehaviour
{
    [SerializeField] BulletPoolActive _shotpool;
    bool _isRay = false;
    float _shotLineCoolTime = 1f;
    float _currentShotCoolTime = 0f;
    ShotLine _shotLine;

    private void Start()
    {

    }

    // Update is called once per frame
    public bool RayUpdate()
    {
        if(_isRay)
        {
            _currentShotCoolTime += Time.deltaTime;
            _shotLine.LineUpdate(_currentShotCoolTime / _shotLineCoolTime);
            if(_currentShotCoolTime > _shotLineCoolTime)
            {
                _isRay = false;
                _shotLine.gameObject.SetActive(false);
                return true;
            }
        }
        return false;
    }

    public void SetRayStart(float direction)
    {
        _shotLine = _shotpool.GetBullet().GetComponent<ShotLine>();
        _shotLine.SetLine(transform.position,EndLine(direction));
        _currentShotCoolTime = 0f;
        _isRay = true;
    }

    Vector3 EndLine(float direction)
    {
        var ray = Physics2D.RaycastAll(
            transform.position, 
            new Vector3(Mathf.Cos(Mathf.Deg2Rad * (direction + 90f)), Mathf.Sin(Mathf.Deg2Rad * (direction + 90f)))
            );
        foreach (var hit in ray)
        {
            if (hit.collider.tag == "Ground" || hit.collider.tag == "Wall")
            {
                return hit.point;
            }
        }
        return transform.position;
    }

    public void ResetShotLine()
    {
        if(_isRay)
        {
            _isRay = false;
        }
    }
}
