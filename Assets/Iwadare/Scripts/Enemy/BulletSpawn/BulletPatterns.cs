using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BulletPatterns
{
    [Tooltip("真っ直ぐ飛ばす弾をスポーンさせるメソッド")]
    public ForwardSpawn _forwardSpawn = new();
    [Tooltip("円状に弾をスポーンさせるメソッド")]
    public CircleSpawn _circleSpawn = new();
    [Tooltip("軌道上に速さの違う複数の弾をスポーンさせるメソッド")]
    public ForwardAfterSlowSpawn _forwardAfterSlowSpawn = new();
    [Tooltip("_circleSpawnを使って波上に弾をスポーンさせるメソッド")]
    public WaveSpawnEnemy _waveSpawnEnemy;
    [Tooltip("楕円型に弾をスポーンさせるメソッド")]
    public EllipseSpawn _ellipseSpawn = new();
}
