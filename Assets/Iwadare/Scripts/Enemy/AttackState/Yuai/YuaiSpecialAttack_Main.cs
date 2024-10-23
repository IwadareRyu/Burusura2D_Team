using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(YuaiSpecialAttack_UI))]
public class YuaiSpecialAttack_Main : MonoBehaviour, AttackInterface, PauseTimeInterface
{
    YuaiSpecialAttack_UI _yuaiUI;
    [SerializeField] float _thinkingTime = 30f;
    [SerializeField] float _stayTime = 1f;
    [SerializeField] float _moveTime = 3f;
    [SerializeField] float _disAttackTime = 0.1f;
    [SerializeField] float _attackCoolTime = 2f;
    [SerializeField] Transform _bossPos;
    [SerializeField] Transform _bossHidePos;
    [SerializeField] CinemachineVirtualCamera _BossUpCamera;
    [SerializeField] MimicryPos[] _mimicryPos;
    CinemachineBrain _cameraBrain;
    float _currentTime = 0f;
    int _randomNumber = 0;
    public int RandomNumber => _randomNumber;

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
        _yuaiUI = GetComponent<YuaiSpecialAttack_UI>();
        _yuaiUI.Init();
        _cameraBrain = Camera.main.GetComponent<CinemachineBrain>();
        foreach (var mimic in _mimicryPos)
        {
            mimic._hitPosColider.gameObject.SetActive(false);
            mimic._hitPosImage.enabled = false;
        }
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
        GameStateManager.Instance.ChangeState(GameState.BattleStopState);
        enemy.transform.DOMove(_bossPos.position, _moveTime);
        if (_BossUpCamera) _BossUpCamera.Priority = 20;
        _yuaiUI.BossUpStart();
        yield return new WaitForSeconds(_moveTime);
        if (_BossUpCamera) _BossUpCamera.Priority = 0;
        _yuaiUI.BossUpEnd(_thinkingTime);
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(_yuaiUI.FadeInBoss());
        SetYuaiUI();
        enemy.transform.position = _bossHidePos.position;
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(_yuaiUI.FadeOutBoss());
        GameStateManager.Instance.ChangeState(GameState.InBattleState);
        yield return null;
    }

    public void SetYuaiUI()
    {
        _randomNumber = RandomNumberSet(_mimicryPos.Length);
        for(var i = 0;i < _mimicryPos.Length;i++)
        {
            _mimicryPos[i]._hitPosColider.gameObject.SetActive(true);
            if (i == _randomNumber)
            {
                _mimicryPos[i]._hitPosImage.enabled = true;
            }
        }
    }

    public IEnumerator Attack(EnemyBase enemy)
    {
        throw new System.NotImplementedException();
    }

    public void ActionReset(EnemyBase enemy)
    {
        throw new System.NotImplementedException();
    }

    public int RandomNumberSet(int max)
    {
        UnityEngine.Random.InitState(DateTime.Now.Millisecond);
        return UnityEngine.Random.Range(0, max);
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

    [Serializable]
    struct MimicryPos
    {
        public Collider2D _hitPosColider;
        public Text _hitPosImage;
    }
}
