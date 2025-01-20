using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using Spine.Unity.Examples;
[RequireComponent(typeof(YuaiSpecialAttack_UI),typeof(YuaiSpecialAttack_Bullet))]
public class YuaiSpecialAttack_Main : MonoBehaviour, AttackInterface, PauseTimeInterface
{
    YuaiSpecialAttack_UI _yuaiUI;
    YuaiSpecialAttack_Bullet _yuaiAttack;
    [SerializeField] float _thinkingTime = 30f;
    [SerializeField] float _stayTime = 1f;
    [SerializeField] float _moveTime = 3f;
    [SerializeField] float _disAttackTime = 0.1f;
    [SerializeField] float _attackCoolTime = 2f;
    [SerializeField] float _dangerousTime = 1f;
    [SerializeField] float _attackTime = 10f;
    [SerializeField] Transform _bossPos;
    [SerializeField] Transform _bossHidePos;
    [SerializeField] CinemachineVirtualCamera _bossUpCamera;
    [SerializeField] CinemachineVirtualCamera _centerUpCamera;
    [SerializeField] MimicryPos[] _mimicryPos;
    [SerializeField] AnimationController_Enemy _yureiAnim;
    float _currentTime = 0f;
    int _randomNumber = 0;
    int _answerNumber = -1;
    bool _isAnswer = false;
    bool _isSelectUI = false;
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
        _yuaiAttack = GetComponent<YuaiSpecialAttack_Bullet>();
        _yuaiAttack.Init();
        foreach (var mimic in _mimicryPos)
        {
            mimic._hitPosColider.gameObject.SetActive(false);
            mimic._hitPosImage.SetActive(false);
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
        enemy._enemyAnim.ChangeAnimationSpain(AnimationName.Run);
        enemy.transform.DOMove(_bossPos.position, _moveTime);
        if (_bossUpCamera) _bossUpCamera.Priority = 20;
        _yuaiUI.BossUpStart();
        yield return WaitforSecondsCashe.Wait(_moveTime);
        enemy._enemyAnim.ChangeAnimationSpain(AnimationName.Idle);
        if (_bossUpCamera) _bossUpCamera.Priority = 0;
        _yuaiUI.BossUpEnd(_thinkingTime);
        yield return WaitforSecondsCashe.Wait(1f);
        yield return StartCoroutine(_yuaiUI.FadeInBoss());
        SetYuaiUI();
        enemy.transform.position = _bossHidePos.position;
        yield return WaitforSecondsCashe.Wait(0.5f);
        yield return StartCoroutine(_yuaiUI.FadeOutBoss());
        GameStateManager.Instance.ChangeState(GameState.InBattleState);
        enemy._bossState = EnemyBase.BossState.AttackState;
    }

    public void SetYuaiUI()
    {
        _randomNumber = RandomNumberSet(_mimicryPos.Length);
        for (var i = 0; i < _mimicryPos.Length; i++)
        {
            _mimicryPos[i]._hitPosColider.gameObject.SetActive(true);
            if (i == _randomNumber)
            {
                _mimicryPos[i]._hitPosImage.SetActive(true);
            }
        }
    }

    public IEnumerator Attack(EnemyBase enemy)
    {
        for (var time = 0f; time < _thinkingTime; time += Time.deltaTime * enemy._timeScale)
        {
            for (var i = 0; i < _mimicryPos.Length; i++)
            {
                if (_mimicryPos[i]._hitPosColider._isHit == true)
                {
                    _answerNumber = i;
                    _yuaiUI.ChangeYuaiSearchText();
                    _yuaiUI.ChangeSearchText($"選択\n{_mimicryPos[i]._hitPosText}");
                    _isSelectUI = true;
                    break;
                }
            }
            if (_isSelectUI) break;
            _yuaiUI.TimerSet(_thinkingTime - time);
            yield return new WaitForFixedUpdate();
        }
        if(!_isSelectUI)
        {
            _answerNumber = -1;
            _yuaiUI.ChangeYuaiSearchText();
            _yuaiUI.ChangeSearchText($"選択\nなし");
        }

        foreach (var mimic in _mimicryPos)
        {
            mimic._hitPosColider._isHit = false;
            mimic._hitPosColider.gameObject.SetActive(false);
            mimic._hitPosImage.SetActive(false);
        }
        _yuaiUI.ChangeMainUIDisable();
        _centerUpCamera.Priority = 20;
        yield return WaitforSecondsCashe.Wait(2f);
        enemy.transform.position = _mimicryPos[_randomNumber]._hitPosColider.transform.position;
        _centerUpCamera.Priority = 0;
        yield return WaitforSecondsCashe.Wait(1f);
        _yuaiUI.ChangeMainUIEnable();
        if (_answerNumber == _randomNumber)
        {
            _yuaiUI.ChangeSearchText("正解！");
            _isAnswer = true;
            enemy.Player.StartGuardMode();
            enemy.HPChack();
        }
        else
        {
            _yuaiUI.ChangeSearchText("不正解！");
        }
        yield return WaitforSecondsCashe.Wait(_attackCoolTime);
        for(var i = 0;i < _mimicryPos.Length;i++)
        {
            if(!_isAnswer || i != _answerNumber)
            {
                _yuaiAttack.RefDangerousSign(_mimicryPos[i]._bulletSpawn,_dangerousTime);
            }
        }
        enemy._enemyAnim.ChangeAnimationSpain(AnimationName.Attack);
        yield return WaitforSecondsCashe.Wait(_dangerousTime);
        for (var i = 0; i < _mimicryPos.Length; i++)
        {
            if (!_isAnswer || i != _answerNumber)
            {
                _yuaiAttack.RefSpawnBullet(_mimicryPos[i]._bulletSpawn);
            }
        }
        yield return WaitforSecondsCashe.Wait(_attackTime);
        enemy._enemyAnim.ChangeAnimationSpain(AnimationName.Idle);
        if (enemy.Player) enemy.Player.EndGuardMode();
        if(!enemy.SpecialHPChack())
        {
            yield return WaitforSecondsCashe.Wait(1f);
            enemy._enemyAnim.ChangeAnimationSpain(AnimationName.Change);
            yield return StartCoroutine(_yuaiUI.AttackEndFadeIn());
            enemy.ChangeAnimationObject(_yureiAnim);
            enemy._enemyAnim.ChangeAnimationSpain(AnimationName.Idle);
            yield return StartCoroutine(_yuaiUI.AttackEndFadeOut());
            enemy.BreakGuardMode();
            _yuaiUI.UIRenderChange();
        }
        enemy.ResetState();
        ResetAction(enemy);
        enemy._bossState = EnemyBase.BossState.ChangeActionState;
    }

    void ResetAction(EnemyBase enemy)
    {
        _isAnswer = false;
        _isSelectUI = false;
        _answerNumber = -1;
        _yuaiUI.UIReset();
        if (enemy._useGravity)
        {
            enemy._enemyRb.gravityScale = 1;
            enemy._enemyRb.velocity = Vector2.zero;
        }
    }

    public void ActionReset(EnemyBase enemy)
    {
        enemy.BreakGuardMode();
        ResetAction(enemy);
    }

    public int RandomNumberSet(int max)
    {
        UnityEngine.Random.InitState(DateTime.Now.Millisecond);
        return UnityEngine.Random.Range(0, max);
    }

    public void TimeScaleChange(float timeScale)
    {

    }

    public void StartPause()
    {

    }

    public void EndPause()
    {

    }

    [Serializable]
    struct MimicryPos
    {
        public YuaiSpecialAttack_Col _hitPosColider;
        public GameObject _hitPosImage;
        public string _hitPosText;
        public BulletSpawnEnemy _bulletSpawn; 
    }
}
