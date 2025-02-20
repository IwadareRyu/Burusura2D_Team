using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public bool _isNetwork = true;
    [SerializeField] float _debugChatTime = 30f;
    float _currentChatTime;
    bool _isResponceActive = false;


    protected override void Awake()
    {
        base.Awake();
        VantanConnect.RegisterEventReceiver(this);
        ResponceStart();
    }

    private void Update()
    {
        if(!_isNetwork)
        {
            _currentChatTime += Time.deltaTime;
            if(_currentChatTime > _debugChatTime)
            {
                var ram = RamdomMethod.RandomNumber99();
                if(ram < 25)
                {
                    GoodChatResponce("頑張れワン！");
                }
                else if(ram < 50)
                {
                    BadChatResponce("ダメダメワン！");
                }
                else if (ram < 75)
                {
                    CoinResponce(RamdomMethod.RandomNumber99());
                }
                else
                {
                    BombStart();
                }
                _currentChatTime = 0f;
            }
        }
    }

    public bool IsActive => true;

    public void OnEventCall(EventData data)
    {
        if (!_isNetwork) return;
        switch (data.EventCode)
        {
            case EventDefine.Cheer:
                CheerEvent cheer = new CheerEvent(data);
                if (cheer.GetEmotion() > 0)
                {
                    GoodChatResponce(cheer.GetMessage());
                }
                else if (cheer.GetEmotion() < 0)
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
        if (_isNetwork)
        {
            VantanConnect.GameStart();
        }
    }

    public void GameEnd(bool win)
    {
        if (!_isNetwork) return;
        if (SceneManager.GetActiveScene().name == "YuaiScene")
        {
            if (!win)
            {
                GameEpisode epic = VantanConnect.CreateEpisode(EpisodeCode.BADead);
                epic.SetEpisode("ユウアイに心を奪われてしまった"); // エピソードを設定する
                epic.DataPack("心を奪われた位置", Camera.main.transform.position); // エピソードの補足を設定する
                VantanConnect.SendEpisode(epic);
            }
            else
            {
                GameEpisode epic = VantanConnect.CreateEpisode(EpisodeCode.BADefeatBoss);
                epic.SetEpisode("ユウアイを倒した！"); // エピソードを設定する
                epic.DataPack("ユウアイを倒した位置", Camera.main.transform.position); // エピソードの補足を設定する
                VantanConnect.SendEpisode(epic);
            }
        }
        if (SceneManager.GetActiveScene().name == "DataraBossStage")
        {
            if (!win)
            {
                GameEpisode epic = VantanConnect.CreateEpisode(EpisodeCode.BADead);
                epic.SetEpisode("ダタラの肉に叩きつけられた"); // エピソードを設定する
                epic.DataPack("肉で叩きつけられた位置", Camera.main.transform.position); // エピソードの補足を設定する
                VantanConnect.SendEpisode(epic);
            }
            else
            {
                GameEpisode epic = VantanConnect.CreateEpisode(EpisodeCode.BADefeatBoss);
                epic.SetEpisode("ダタラの肉を奪った！"); // エピソードを設定する
                epic.DataPack("肉を奪った位置", Camera.main.transform.position); // エピソードの補足を設定する
                VantanConnect.SendEpisode(epic);
            }
        }
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
        if (!_isNetwork) return;
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
        if (InGameManager.Instance.BombSystem())
        {
            StartCoroutine(_ultraChatScripts.ExplosionChat());
        }
    }
}
