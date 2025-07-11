﻿using Cinemachine;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class PlayerSpawn : MonoBehaviour
{
    [SerializeField] PlayerController _player;
    [SerializeField] EnemyBase _enemy;
    public PlayerController _currentPlayer;
    [SerializeField] SetPlayerStruct _setPlayerStruct;
    [SerializeField] CinemachineTargetGroup _targetGroup;
    [SerializeField] ParticleSystem _spawnParticle;
    [SerializeField] float _spawnDelayTime = 1.5f;
    PlayerStatusSystems playersStatusSystems;
    bool _isDeath = false;
    bool _isNotRemain = false;


    private void Start()
    {
        playersStatusSystems = PlayerStatusSystems.Instance;
        playersStatusSystems.SetTotalStatus();
        SpawnPlayer();
    }

    private void Update()
    {
        if(!_isDeath &&_currentPlayer && (int)(_currentPlayer._playerState & PlayerState.DeathState) != 0)
        {
            _isDeath = true;
            InGameManager.Instance.AddRemain(-1);
            StartCoroutine(PlayerDelaySpawn());
        }
    }

    private IEnumerator PlayerDelaySpawn()
    {
        yield return WaitforSecondsCashe.Wait(_spawnDelayTime);
        ResponceManager.Instance?.PlayerDeathResponce();
        if (!_isNotRemain && InGameManager.Instance._currentPlayerRemain <= 0)
        {
            _isNotRemain = true;
            GameStateManager.Instance.EndBattle(false);
            yield break;
        }
        SpawnPlayer();
        _isDeath = false;
    }

    public void SpawnPlayer()
    {
        if (_currentPlayer) _targetGroup.RemoveMember(_currentPlayer.transform);
        _currentPlayer = Instantiate(_player, transform.position, Quaternion.identity);
        _targetGroup.AddMember(_currentPlayer.transform,3,0);
        //_currentPlayer.SetStatus();
        _currentPlayer.Init(_setPlayerStruct);
        _enemy.PlayerSet(_currentPlayer);
        if(_spawnParticle) _spawnParticle.Play();
        ResponceManager.Instance?.PlayerDeathResponce();
    }
}

[Serializable]
public struct SetPlayerStruct
{
    public Slider _playerHpSlider;
    public BulletPoolActive _missBulletPool;
    public BulletPoolActive _hitParticlePool;
    public BulletPoolActive _reflectParticlePool;
    public Image _shieldImage;
    public BulletPoolActive _numberPool;
}
