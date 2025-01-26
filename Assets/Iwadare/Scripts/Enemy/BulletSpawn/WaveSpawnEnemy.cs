using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawnEnemy : MonoBehaviour
{
    [Header("WaveSpawn変数")]
    [Tooltip("波スポーンの幅")]
    [SerializeField] int _waveWidth = 4;
    [SerializeField] float _waveWidthTime = 0.05f;
    [Tooltip("波スポーンの回数")]
    [SerializeField] int _waveCount = 2;
    [SerializeField] float _waveCountTime = 0.5f;
    [Tooltip("波スポーンの変える角度")]
    [SerializeField] float _waveDistance = 5f;
    float _tmpDistance;

    public void Init(BulletSpawnEnemy bulletSpawnEnemy)
    {
        _tmpDistance = bulletSpawnEnemy.BulletDistance;
    }

    public IEnumerator WaveSpawn(BulletSpawnEnemy bulletSpawn,CircleSpawn circleSpawn)
    {
        bulletSpawn._bulletDistance = _tmpDistance;
        for (var i = 0; i < _waveCount; i++)
        {
            for (var j = 0; j < _waveWidth; j++)
            {
                if (!bulletSpawn.IsManualMove && bulletSpawn.SpawnBulletMoveStruct._bulletMoveType != BulletMoveType.DelayFastLazer)
                {
                    bulletSpawn.AttackAudio();
                }
                circleSpawn.Spawn(bulletSpawn);
                yield return new WaitForSeconds(_waveWidthTime);
                bulletSpawn._bulletDistance += _waveDistance;
            }
            yield return new WaitForSeconds(_waveCountTime);
            _waveDistance = -_waveDistance;
        }
    }
}
