using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EllipseSpawn :BulletSpawnClass
{
    [Tooltip("速さの割合"), Header("速さの割合(実数)")]
    [SerializeField] float _speedDistance = 1f;
    [SerializeField] bool _isVerticalEllipse;
    [SerializeField] float _minPersents = 0.3f;
    float _speedPersents = 0f;
    public void Spawn(BulletSpawnEnemy bulletSpawn)
    {
        float j = 0f;
        for (float i = bulletSpawn.BulletDistance; i < 360 + bulletSpawn.BulletDistance; i += bulletSpawn.BulletRange)
        {
            if (!_isVerticalEllipse) _speedPersents = Mathf.Abs(Mathf.Cos(Mathf.Deg2Rad * j));
            else _speedPersents = Mathf.Abs(Mathf.Sin(Mathf.Deg2Rad * j));
            if (_speedPersents < _minPersents)
            {
                _speedPersents = _minPersents;
            }
            bulletSpawn.InitBullet(i, bulletSpawn.DefaultBulletSpeed + _speedDistance * _speedPersents, bulletSpawn.BulletActiveTime);
            j += bulletSpawn.BulletRange;
        }
    }
}
