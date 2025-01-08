using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class ResponceManager : SingletonMonovihair<ResponceManager>
{
    [SerializeField] NekoChatScripts _ultraChatScripts;
    [SerializeField] bool _network = true;
    protected override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {

    }


    public void PlayerDeathResponce()
    {
        if (!_network) return;
        var ct = this.GetCancellationTokenOnDestroy();
        PlayerDeathResponceAsync(ct).Forget();
    }

    public async UniTask PlayerDeathResponceAsync(CancellationToken ct)
    {
        /// サーバーに指令を送る。

        ///
        await UniTask.Delay(TimeSpan.FromSeconds(Time.deltaTime), cancellationToken: ct);
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
