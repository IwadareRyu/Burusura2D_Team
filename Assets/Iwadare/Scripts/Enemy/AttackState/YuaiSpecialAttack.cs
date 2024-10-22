using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class YuaiSpecialAttack : MonoBehaviour, AttackInterface, PauseTimeInterface
{
    [SerializeField] float _thinkingTime = 30f;
    [SerializeField] float _stayTime = 1f;
    [SerializeField] float _moveTime = 3f;
    [SerializeField] float _disAttackTime = 0.1f;
    [SerializeField] float _attackCoolTime = 2f;
    [SerializeField] Transform _bossPos;
    [SerializeField] Canvas _uiCanvas;
    [SerializeField] Canvas _specialUI;
    [SerializeField] Image _fadeImage;
    [SerializeField] Animator _bossFadeAnim;
    [SerializeField] AnimationClip _bossFadeAnimation;
    [SerializeField] CinemachineVirtualCamera _BossUpCamera;
    [SerializeField] MimicryPos[] _mimicryPos;
    CinemachineBrain _cameraBrain;
    float _currentTime = 0f;
    int randomNumber = 0;

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
        _bossFadeAnim.gameObject.SetActive(false);
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
        enemy.transform.DOMove(_bossPos.position,_moveTime);
        if (_uiCanvas) _uiCanvas.enabled = false;
        if (_BossUpCamera) _BossUpCamera.Priority = 20;
        if (_specialUI) _specialUI.enabled = true;
        yield return new WaitForSeconds(_moveTime);
        if (_BossUpCamera) _BossUpCamera.Priority = 0;
        if (_uiCanvas) _uiCanvas.enabled = true;
        if (_specialUI) _specialUI.enabled = false;
        yield return new WaitForSeconds(1f);
        _bossFadeAnim.gameObject.SetActive(true);
        yield return StartCoroutine(FadeManager.Instance.CustomFadeIn(_fadeImage,0.5f));
        _bossFadeAnim.Play(_bossFadeAnimation.name);
        yield return StartCoroutine(FadeManager.Instance.FadeIn());

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

    public int RandomNumber(int max)
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
