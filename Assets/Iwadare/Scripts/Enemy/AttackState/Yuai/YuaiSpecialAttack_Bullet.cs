using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YuaiSpecialAttack_Bullet : MonoBehaviour
{

    public void Init()
    {
        
    }

    public void RefDangerousSign(BulletSpawnEnemy bulletSpawn,float dangerousTime)
    {
        bulletSpawn.DangerousSign(dangerousTime);
    }

    public void RefSpawnBullet(BulletSpawnEnemy bulletSpawn)
    {
        StartCoroutine(bulletSpawn.BulletSpawn());
    }

}
