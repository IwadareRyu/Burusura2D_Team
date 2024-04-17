using System.Collections;
public class BulletMoveClass
{
    public virtual void Init(MoveBulletEnemy bulletMove) { return; }

    public virtual IEnumerator BulletMove(MoveBulletEnemy bulletMove, float bulletSpeed) { yield return null; }

    public virtual IEnumerator BulletMove(MoveBulletEnemy bulletMove, float bulletSpeed, float bulletRota) { yield return null; }
}
