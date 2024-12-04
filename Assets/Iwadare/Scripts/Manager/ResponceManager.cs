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

    public async UniTask PlayerDeathResponceAsync(CancellationToken ct)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(Time.deltaTime), cancellationToken: ct);
    }
}
