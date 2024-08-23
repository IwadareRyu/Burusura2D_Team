/// <summary>Bulletに攻撃したときの処理を決めるState</summary>
public enum BulletBreakType
{
    NotBreak,
    Break,
    Counter,
    Homing,
}

/// <summary>Bulletの動きの処理を決めるState</summary>
public enum BulletMoveType
{
    Forward,
    TargetPlayer,
    DelayTargetPlayer,
    Rotate,
}

/// <summary>Bulletのスポーン方法を決めるState</summary>
public enum BulletSpawnType
{
    ForwardOnceSpawn,
    ForwardAfterSlowSpawn,
    CircleSpawn,
    DelayCircleSpawn,
    WaveSpawn,
    WaitWaveSpawn,
    ElipseSpawn,
}

/// <summary>スポーン回数を大まかに決めるState</summary>
public enum SpawnCountType
{
    /// <summary>一回のみ</summary>
    OneShot,
    /// <summary>指定回数</summary>
    Count,
    /// <summary>ループ</summary>
    Loop,
}