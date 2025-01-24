using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using VTNConnect;

public class ResponceManager : SingletonMonovihair<ResponceManager>, IVantanConnectEventReceiver
{
    [SerializeField] NekoChatScripts _ultraChatScripts;
    [SerializeField] int _plusRemain = 3;
    [SerializeField] int _playerDamage = -10;
    [SerializeField] int _enemyDamage = 10;
    PlayerSpawn _playerSpawn;
    EnemyBase _enemy;
    [SerializeField] bool _network = true;

    public bool IsActive => throw new NotImplementedException();

    protected override void Awake()
    {
        base.Awake();
    }


    public void OnEventCall(EventData data)
    {
        switch (data.EventId)
        {
            case (int)EventDefine.DefeatBomb:

                break;
        }
    }

    public void GetPlayerEnemy()
    {
        _playerSpawn = GameObject.FindObjectOfType<PlayerSpawn>();
        _enemy = GameObject.FindObjectOfType<EnemyBase>();
    }

    private void Update()
    {
    }

    public void RemainUp()
    {
        if (_playerSpawn)
        {
            _playerSpawn.AddRemain(_plusRemain);
        }
    }

    public void PlayerDamage()
    {
        if (_playerSpawn)
        {
            _playerSpawn._currentPlayer.AddDamage(_playerDamage);
        }
    }

    public void EnemyDamage()
    {
        if (_enemy)
        {
            _enemy.AddDamage(_enemyDamage);
        }
    }


    public void PlayerDeathResponce()
    {
        if (!_network) return;
        EventData data = new EventData(EventDefine.DeathStack);
         VantanConnect.SendEvent(data);
    }


    public void StageClearResponce()
    {
        if (!_network) return;
        var ct = this.GetCancellationTokenOnDestroy();
        StageClearResponceAsync(ct).Forget();
    }

    private async UniTask StageClearResponceAsync(CancellationToken ct)
    {
        /// サーバーに指令を送る。

        ///
        await UniTask.Delay(TimeSpan.FromSeconds(Time.deltaTime), cancellationToken: ct);
    }

    public void CoinResponce(float coin)
    {
        StartCoroutine(_ultraChatScripts.UltraChatCoroutine(coin));
    }
}
