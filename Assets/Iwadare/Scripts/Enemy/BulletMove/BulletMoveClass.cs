using System.Collections;
public class BulletMoveClass
{
    public virtual void Init(MoveBulletEnemy bulletMove) { return; }

    public virtual void BulletMove() {  return; }

    public virtual bool BulletMoveUpdate(MoveBulletEnemy bulletMove, float bulletSpeed,float bulletRota = 0f) { return false; }
}
