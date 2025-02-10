using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveAction_Tutorial : MonoBehaviour,AttackInterface
{
    [SerializeField] Image _movePanel;
    [SerializeField] Collider2D _chackPoint;

    public void Start()
    {
        _movePanel.gameObject.SetActive(false);
        _chackPoint.gameObject.SetActive(false);
    }

    public void Init()
    {
        
    }
    public void StayUpdate(EnemyBase enemy)
    {
        _movePanel.gameObject.SetActive(true);
        _chackPoint.gameObject.SetActive(true);
        enemy._bossState = EnemyBase.BossState.MoveState;
    }

    public IEnumerator Move(EnemyBase enemy)
    {
        
        yield return null;
    }

    public IEnumerator Attack(EnemyBase enemy)
    {
        yield return null;
    }

    public void ActionReset(EnemyBase enemy)
    {
        
    }
}
