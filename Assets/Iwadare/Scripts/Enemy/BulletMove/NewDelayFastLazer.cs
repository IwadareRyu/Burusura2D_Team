using UnityEngine;

public class NewDelayFastLazer : BulletMoveClass
{
    Vector3 _hitPosition;
    float _currentTime = 0f;
    float _persent;
    public override void BulletMove()
    {
    }

    public override bool BulletMoveUpdate(MoveBulletEnemy bulletMove, float bulletSpeed, float bulletRota = 0f)
    {
        if (bulletMove.IsAudio) AudioManager.Instance.PlaySE(bulletMove._strongAttackAudio);
        bulletMove.NewAttackRay();
        return false;
    }
}
