using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class FadeManager : SingletonMonovihair<FadeManager>,PauseTimeInterface
{
    [SerializeField] Image _defaultFadeImage;
    [SerializeField] float _fadeInTime = 1f;
    [SerializeField] float _fadeOutTime = 1f;
    Sequence _fadeSequence;
    Tweener _currrentFadeTween;

    protected override void Awake()
    {
        base.Awake();
        _fadeSequence = DOTween.Sequence();
        var color = _defaultFadeImage.color;
        color.a = 0;
        _defaultFadeImage.color = color;
        _fadeSequence
            .Append(_defaultFadeImage.DOFade(1f, _fadeInTime))
            .Append(_defaultFadeImage.DOFade(0f, _fadeOutTime));
        _fadeSequence.Pause();
    }

    private void OnEnable()
    {
        TimeScaleManager.ChangeTimeScaleAction += TimeScaleChange;
        TimeScaleManager.StartPauseAction += StartPause;
        TimeScaleManager.EndPauseAction += EndPause;
    }

    private void OnDisable()
    {
        TimeScaleManager.ChangeTimeScaleAction -= TimeScaleChange;
        TimeScaleManager.StartPauseAction -= StartPause;
        TimeScaleManager.EndPauseAction -= EndPause;
    }

    public IEnumerator SceneChangeFade(string nextSceneName)
    {
        yield return _defaultFadeImage.DOFade(1f,_fadeInTime);
        //ロード処理完了的な何か
        SceneLoader.Instance.SceneLoad(nextSceneName,0.5f);
        yield return _defaultFadeImage.DOFade(0f,_fadeOutTime);
    }

    public IEnumerator Fade()
    {
        Debug.Log("fade開始");
        yield return _fadeSequence.Play();
        yield return new WaitForSeconds(_fadeInTime + _fadeOutTime);
    }

    public IEnumerator FadeIn()
    {
        _currrentFadeTween = _defaultFadeImage.DOFade(1f, _fadeInTime);
        yield return _currrentFadeTween.Play();
        yield return new WaitForSeconds(_fadeInTime);
    }

    public IEnumerator FadeOut()
    {
        _currrentFadeTween = _defaultFadeImage.DOFade(0f, _fadeOutTime);
        _currrentFadeTween.Play();
        yield return new WaitForSeconds(_fadeOutTime);
    }

    public IEnumerator CustomFadeIn(Image image,float fadeInTime)
    {
        _currrentFadeTween = image.DOFade(1f, fadeInTime);
        _currrentFadeTween.Play();
        yield return new WaitForSeconds(fadeInTime);
    }

    public IEnumerator CustomFadeOut(Image image,float fadeOutTime)
    {
        _currrentFadeTween = image.DOFade(0f, fadeOutTime);
        _currrentFadeTween.Play();
        yield return new WaitForSeconds(fadeOutTime);
    }

    public void TimeScaleChange(float timeScale)
    {
        if(_currrentFadeTween.IsActive())
        {
            _currrentFadeTween.timeScale = timeScale;
        }
    }

    public void StartPause()
    {
        if (_currrentFadeTween.IsActive())
        {
            _currrentFadeTween.timeScale = 0f;
        }
    }

    public void EndPause()
    {
        if (_currrentFadeTween.IsActive())
        {
            _currrentFadeTween.timeScale = 1f;
        }
    }
}
