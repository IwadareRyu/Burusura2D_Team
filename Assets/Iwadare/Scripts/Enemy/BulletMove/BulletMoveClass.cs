using System.Collections;
public class BulletMoveClass
{
    public virtual void Init(MoveBulletEnemy bulletMove) { return; }

    public virtual void BulletMove(float bulletSpeed) {  return; }

    public virtual void BulletMove(float bulletSpeed, float bulletRota) { return; }

    public virtual bool BulletMoveUpdate(MoveBulletEnemy bulletMove) { return false; }
}
