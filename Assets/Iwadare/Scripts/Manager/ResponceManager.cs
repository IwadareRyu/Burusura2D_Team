using Cysharp.Threading.Tasks;
using System;
using System.Collections;
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
    [SerializeField] float _eventWaitTime = 0.5f;
    int _ramdomEventNumber = 4;
    PlayerSpawn _playerSpawn;
    EnemyBase _enemy;
    [SerializeField] bool _network = true;
    bool _isResponceActive = false;


    protected override void Awake()
    {
        base.Awake();
        VantanConnect.RegisterEventReceiver(this);
        ResponceStart();
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
            case EventDefine.DefeatBomb:
                BombStart();
                break;

        }
    }

    public void ResponceStart()
    {
        _isResponceActive = true;
    }

    public void ResponceStop()
    {
        _isResponceActive = false;
    }

    public void GameStart()
    {
        VantanConnect.GameStart();
    }

    public void GameEnd(bool win)
    {
        VantanConnect.GameEnd(win);
        //VantanConnect.SystemReset();
    }


    public void GetPlayerEnemy()
    {
        _playerSpawn = GameObject.FindObjectOfType<PlayerSpawn>();
        _enemy = GameObject.FindObjectOfType<EnemyBase>();
    }

    public IEnumerator RamdomGoodEvent()
    {
        _isResponceActive = false;
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
        yield return WaitforSecondsCashe.Wait(_eventWaitTime);
        _isResponceActive = true;
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


    //public void StageClearResponce()
    //{
    //    if (!_network) return;
    //    var ct = this.GetCancellationTokenOnDestroy();
    //    StageClearResponceAsync(ct).Forget();
    //}

    //private async UniTask StageClearResponceAsync(CancellationToken ct)
    //{
    //    /// サーバーに指令を送る。

    //    ///
    //    await UniTask.Delay(TimeSpan.FromSeconds(Time.deltaTime), cancellationToken: ct);
    //}

    public void CoinResponce(float coin)
    {
        if (GameStateManager.Instance.GameState != GameState.InBattleState || !_isResponceActive) return;
        StartCoroutine(_ultraChatScripts.CoinChatCoroutine(coin));
        StartCoroutine(RamdomGoodEvent());
    }

    public void GoodChatResponce(string goodChat)
    {
        if (GameStateManager.Instance.GameState != GameState.InBattleState || !_isResponceActive) return;
        StartCoroutine(_ultraChatScripts.ChatCoroutine(goodChat, true));
        StartCoroutine(RamdomGoodEvent());
    }

    public void BadChatResponce(string badChat)
    {
        if (GameStateManager.Instance.GameState != GameState.InBattleState || !_isResponceActive) return;
        StartCoroutine(_ultraChatScripts.ChatCoroutine(badChat, false));
        StartCoroutine(RamdomGoodEvent());
    }


    public void BombStart()
    {
        if (GameStateManager.Instance.GameState != GameState.InBattleState || !_isResponceActive) return;
        if(InGameManager.Instance.BombSystem())
        {
            StartCoroutine(_ultraChatScripts.ExplosionChat());
        }
    }
}
