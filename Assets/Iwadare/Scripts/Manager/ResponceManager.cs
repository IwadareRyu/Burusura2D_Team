using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class ResponceManager : SingletonMonovihair<ResponceManager>
{
    protected override void Awake()
    {
        base.Awake();
    }


    public void PlayerDeathResponce()
    {
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
        var ct = this.GetCancellationTokenOnDestroy();
        StageClearResponceAsync(ct).Forget();
    }

    private async UniTask StageClearResponceAsync(CancellationToken ct)
    {
        /// サーバーに指令を送る。

        ///
        await UniTask.Delay(TimeSpan.FromSeconds(Time.deltaTime), cancellationToken: ct);
    }
}
