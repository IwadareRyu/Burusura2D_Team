using Cinemachine;
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
    public PlayerController _currentPlayer;
    [SerializeField] int _playerRemain = 3;
    int _currentPlayerRemain;
    [SerializeField] SetPlayerStruct _setPlayerStruct;
    [SerializeField] CinemachineTargetGroup _targetGroup;
    [SerializeField] ParticleSystem _spawnParticle;
    [SerializeField] float _spawnDelayTime = 1.5f;
    bool _isDeath = false;
    bool _isNotRemain = false;

    private void Start()
    {
        _currentPlayerRemain = _playerRemain;
        SpawnPlayer();
        InGameManager.Instance.PlayerRemain(_currentPlayerRemain);
    }

    private void Update()
    {
        if(!_isDeath &&_currentPlayer && (int)(_currentPlayer._playerState & PlayerState.DeathState) != 0)
        {
            _isDeath = true;
            _currentPlayerRemain--;
            StartCoroutine(PlayerDelaySpawn());
            InGameManager.Instance.PlayerRemain(_currentPlayerRemain);
        }
    }

    private IEnumerator PlayerDelaySpawn()
    {
        if(!_isNotRemain && _currentPlayerRemain <= 0)
        {
            yield return null;
            GameStateManager.Instance.ChangeState(GameState.BattleEndState);
            _isNotRemain = true;
            yield break;
        }
        yield return WaitforSecondsCashe.Wait(_spawnDelayTime);
        SpawnPlayer();
        _isDeath = false;
    }

    public void SpawnPlayer()
    {
        if (_currentPlayer) _targetGroup.RemoveMember(_currentPlayer.transform);
        _currentPlayer = Instantiate(_player, transform.position, Quaternion.identity);
        _targetGroup.AddMember(_currentPlayer.transform,3,0);
        _currentPlayer.Init(_setPlayerStruct);
        _enemy.PlayerSet(_currentPlayer);
        if(_spawnParticle) _spawnParticle.Play();
        ResponceManager.Instance.PlayerDeathResponce();
    }

    public void AddRemain(int plusRemain)
    {
        _currentPlayerRemain += plusRemain;
        InGameManager.Instance.PlayerRemain(_currentPlayerRemain);
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
}
