using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIAttack : MonoBehaviour,AttackInterface, PauseTimeInterface
{
    [SerializeField] UIPosition[] _uiPos;
    [SerializeField] Image _timerPanel;
    Text _timerText;
    [SerializeField] Transform _centerPos;
    float _currentTime = 0f;
    [SerializeField] float _stayTime = 0.1f;
    [SerializeField] float _moveTime = 1.0f;
    [SerializeField] int _lookingAroundCount = 2;

    public void Init()
    {
        _timerText = _timerPanel.GetComponentInChildren<Text>();
        gameObject.SetActive(false);
    }

    public void StayUpdate(EnemyBase enemy)
    {
        _currentTime += Time.deltaTime * enemy._timeScale;
        if (_currentTime > _stayTime)
        {
            _currentTime = 0f;
            if (enemy._useGravity)
            {
                enemy._enemyRb.gravityScale = 0;
                enemy._enemyRb.velocity = Vector2.zero;
            }
            enemy._bossState = EnemyBase.BossState.MoveState;
        }
    }

    public IEnumerator Move(EnemyBase enemy)
    {
        SetEnemyAngle(enemy);
        
        yield return enemy.transform.DOMove(_centerPos.position, _moveTime).SetLink(enemy.gameObject).WaitForCompletion();
        enemy.ObjSetRotation(0);
        for (var i = 0; i < _lookingAroundCount; i++)
        {
            enemy.BossObjFlipX(false);
            yield return WaitforSecondsCashe.Wait(0.5f);
            enemy.BossObjFlipX(true);
            yield return WaitforSecondsCashe.Wait(0.5f);
        }
        Debug.Log("移動しました");
        yield return null;
    }

    void SetEnemyAngle(EnemyBase enemy)
    {
        var angle = GetAngle(enemy.transform, _centerPos);
        Debug.Log(angle);
        if(angle <= 90 && angle >= -90)
        {
            enemy.BossObjFlipX(false);
            enemy.ObjSetRotation(angle);
        }
        else
        {
            enemy.BossObjFlipX(true);
            enemy.ObjSetRotation(angle >= 0 ? -(180 - angle) : -(-180 - angle));
        }
    }

    float GetAngle(Transform enemyPos,Transform targetPos)
    {
        var distance = enemyPos.position - targetPos.position;
        return Mathf.Atan2(distance.y,distance.x) * Mathf.Rad2Deg;
    }

    public IEnumerator Attack(EnemyBase enemy)
    {
        yield return null;
    }

    public void ActionReset(EnemyBase enemy)
    {

    }

    public void StartPause()
    {

    }


    public void EndPause()
    { 

    }


    public void TimeScaleChange(float timeScale)
    {
        
    }

    [Serializable]
    struct UIPosition
    {
        [SerializeField]UIPositionState _uiPosState;
        [SerializeField]BulletSpawnEnemy[] _spawnBullet;
        [SerializeField] Transform _movePoint;
        [SerializeField] Text _yuaiText;
    }

    enum UIPositionState
    {
        LeftUp,
        RightUp,
        LeftDown,
        RightDown
    }
}
