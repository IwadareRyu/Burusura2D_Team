using System;
using UnityEngine;

[Serializable]
public struct SpawnBulletMoveStruct
{
    [Tooltip("弾の動きの設定"), Header("弾の動きの設定")]
    [SerializeField] public BulletMoveType _bulletMoveType;

    [Tooltip("弾を攻撃したときの起こることの設定"), Header("弾を攻撃したときの起こることの設定")]
    [SerializeField] public BulletBreakType _bulletBreakType;

    [Tooltip("毎秒弾の回る角度"), Header("毎秒弾の回る角度")]
    [SerializeField] public float _bulletRotation;

    [Tooltip("弾を回転させるか"), Header("弾を回転させるか")]
    [SerializeField] public bool _isRota;
}