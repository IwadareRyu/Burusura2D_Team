using Cinemachine;
using System;
using System.Collections;
using UnityEngine;

public class YuaiSpecialAttack : MonoBehaviour, AttackInterface, PauseTimeInterface
{
    [SerializeField] float _thinkingTime = 30f;
    [SerializeField] float _stayTime = 1f;
    [SerializeField] float _moveTime = 3f;
    [SerializeField] float _disAttackTime = 0.1f;
    [SerializeField] float _attackCoolTime = 2f;
    [SerializeField] Canvas _uiCanvas;
    [SerializeField] Canvas _specialUI;
    [SerializeField] CinemachineVirtualCamera _BossUpCamera;
    CinemachineBrain _cameraBrain;
    float _currentTime = 0f;

    public void UnityActionSet()
    {
        TimeScaleManager.ChangeTimeScaleAction += TimeScaleChange;
        TimeScaleManager.StartPauseAction += StartPause;
        TimeScaleManager.EndPauseAction += EndPause;
    }

    private void UnityActionReset()
    {
        TimeScaleManager.ChangeTimeScaleAction -= TimeScaleChange;
        TimeScaleManager.StartPauseAction -= StartPause;
        TimeScaleManager.EndPauseAction -= EndPause;
    }


    public void Init()
    {
        if (_specialUI) _specialUI.enabled = false;
        gameObject.SetActive(false);
        _cameraBrain = Camera.main.GetComponent<CinemachineBrain>();
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
        if (_uiCanvas) _uiCanvas.enabled = false;
        if (_BossUpCamera) _BossUpCamera.Priority = 20;
        if (_specialUI) _specialUI.enabled = true;
        yield return new WaitForSeconds(2f);
        if (_BossUpCamera) _BossUpCamera.Priority = 0;
        if (_uiCanvas) _uiCanvas.enabled = true;
        if (_specialUI) _specialUI.enabled = false;
        yield return null;
    }

    public IEnumerator Attack(EnemyBase enemy)
    {
        throw new System.NotImplementedException();
    }

    public void ActionReset(EnemyBase enemy)
    {
        throw new System.NotImplementedException();
    }

    public void TimeScaleChange(float timeScale)
    {
        throw new NotImplementedException();
    }

    public void StartPause()
    {
        throw new NotImplementedException();
    }

    public void EndPause()
    {
        throw new NotImplementedException();
    }
}
