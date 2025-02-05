using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using VTNConnect;

public class ResponceManager : SingletonMonovihair<ResponceManager>, IVantanConnectEventReceiver
{
    [SerializeField] NekoChatScripts _ultraChatScripts;
    [SerializeField] int _plusRemain = 1;
    [SerializeField] int _playerDamage = -5;
    [SerializeField] int _enemyDamage = 5;
    int _ramdomEventNumber = 4;
    PlayerSpawn _playerSpawn;
    EnemyBase _enemy;
    [SerializeField] bool _network = true;


    protected override void Awake()
    {
        base.Awake();
        VantanConnect.RegisterEventReceiver(this);
    }

    public bool IsActive => true;

    public void OnEventCall(EventData data)
    {
        switch (data.EventCode)
        {
            case EventDefine.Cheer:
                CheerEvent cheer = new CheerEvent(data);
                if(cheer.GetEmotion() > 0)
                {
                    GoodChatResponce(cheer.GetMessage());
                }
                else if(cheer.GetEmotion() < 0)
                {
                    BadChatResponce(cheer.GetMessage());
                }
                break;
            case EventDefine.BonusCoin:
                int coin = data.GetIntData("GetCoin");
                CoinResponce(coin);
                break;

        }
    }

    public void GameStart()
    {
        VantanConnect.GameStart();
    }

    public void GameEnd(bool win)
    {
        VantanConnect.GameEnd(win);
    }


    public void GetPlayerEnemy()
    {
        _playerSpawn = GameObject.FindObjectOfType<PlayerSpawn>();
        _enemy = GameObject.FindObjectOfType<EnemyBase>();
    }

    private void Update()
    {
    }

    public void RamdomGoodEvent()
    {
        var ram = RamdomMethod.RandomNumber99();
        if (ram > 100 / _ramdomEventNumber * 3)
        {
            RemainUp();
        }
        else if (ram > 100 / _ramdomEventNumber * 2)
        {
            PlayerDamage();
        }
        else if (ram > 100 / _ramdomEventNumber)
        {
            EnemyDamage();
        }
        else
        {
            GuageUp(25);
        }
    }

    public void RemainUp()
    {
        if (_playerSpawn)
        {
            InGameManager.Instance.AddRemain(_plusRemain);
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
            _enemy.PerforateDamage(_enemyDamage);
        }
    }

    public void GuageUp(float guage)
    {
        if (InGameManager.Instance)
        {
            InGameManager.Instance._playerSpecialGuage.AddGuage(guage);
        }
    }


    public void PlayerDeathResponce()
    {
        if (!_network) return;
        //EventData data = new EventData(EventDefine.DeathStack);
        //VantanConnect.SendEvent(data);
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
        StartCoroutine(_ultraChatScripts.CoinChatCoroutine(coin));
    }

    public void GoodChatResponce(string goodChat)
    {
        StartCoroutine(_ultraChatScripts.ChatCoroutine(goodChat, true));
        RamdomGoodEvent();
    }

    public void BadChatResponce(string badChat)
    {
        StartCoroutine(_ultraChatScripts.ChatCoroutine(badChat, false));
        RamdomGoodEvent();
    }
}
