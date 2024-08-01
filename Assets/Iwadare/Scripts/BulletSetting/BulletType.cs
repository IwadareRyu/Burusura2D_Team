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
    CircleSpawn,
    DelayCircleSpawn,
    WaveSpawn,
}