using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField] PlayerController _player;
    [SerializeField] EnemyBase _enemy;
    PlayerController _currentPlayer;
    [SerializeField] SetPlayerStruct _setPlayerStruct;

    private void Start()
    {
        SpawnPlayer();
    }

    private void Update()
    {
        if(_currentPlayer && (int)(_currentPlayer._playerState & PlayerState.DeathState) != 0)
        {
            SpawnPlayer();
            InGameManager.Instance.PlayerDeath();
        }
    }

    public void SpawnPlayer()
    {
        _currentPlayer = Instantiate(_player, transform.position, Quaternion.identity);
        _currentPlayer.Init(_setPlayerStruct);
        _enemy.PlayerSet(_currentPlayer);
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
