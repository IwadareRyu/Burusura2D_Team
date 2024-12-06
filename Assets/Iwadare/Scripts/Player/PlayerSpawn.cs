﻿using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField] PlayerController _player;
    [SerializeField] EnemyBase _enemy;
    PlayerController _currentPlayer;
    [SerializeField] SetPlayerStruct _setPlayerStruct;
    [SerializeField] CinemachineTargetGroup _targetGroup;
    [SerializeField] ParticleSystem _spawnParticle;
    [SerializeField] float _spawnDelayTime = 1.5f;
    bool _isDeath = false;

    private void Start()
    {
        SpawnPlayer();
    }

    private void Update()
    {
        if(!_isDeath &&_currentPlayer && (int)(_currentPlayer._playerState & PlayerState.DeathState) != 0)
        {
            _isDeath = true;
            StartCoroutine(PlayerDelaySpawn());
            InGameManager.Instance.PlayerDeath();
        }
    }

    private IEnumerator PlayerDelaySpawn()
    {
        yield return WaitforSecondsCashe.Wait(_spawnDelayTime);
        SpawnPlayer();
        _isDeath = false;
    }

    public void SpawnPlayer()
    {
        if (_currentPlayer) _targetGroup.RemoveMember(_currentPlayer.transform);
        _currentPlayer = Instantiate(_player, transform.position, Quaternion.identity);
        _targetGroup.AddMember(_currentPlayer.transform,1,0);
        _currentPlayer.Init(_setPlayerStruct);
        _enemy.PlayerSet(_currentPlayer);
        if(_spawnParticle) _spawnParticle.Play();
        ResponceManager.Instance.PlayerDeathResponce();
    }
}

[Serializable]
public struct SetPlayerStruct
{
    public Slider _playerHpSlider;
    public BulletPoolActive _missBulletPool;
    public BulletPoolActive _hitParticlePool;
    public BulletPoolActive _reflectParticlePool;

}
