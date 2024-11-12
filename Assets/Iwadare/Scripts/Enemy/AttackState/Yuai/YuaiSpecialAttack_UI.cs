using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class YuaiSpecialAttack_UI : MonoBehaviour
{
    [SerializeField] Canvas _uiCanvas;
    [SerializeField] Canvas _specialUI;
    [SerializeField] Image _fadeImage;
    [SerializeField] Text _yuaiSearchText;
    [SerializeField] Text _worldYuaiText;
    [SerializeField] Image _timerPanel;
    [SerializeField] Animator _bossFadeAnim;
    [SerializeField] AnimationClip _bossFadeAnimationClip;
    [SerializeField] Animator _bossUpFadeAnim;
    [SerializeField] AnimationClip _bossUpFadeAnimClip;
    [SerializeField] bool _isBossUpFade;
    

    public void Init()
    {
        if (_specialUI) _specialUI.enabled = false;
        _bossFadeAnim.gameObject.SetActive(false);
        _bossUpFadeAnim.gameObject.SetActive(false);
        _yuaiSearchText.gameObject.SetActive(false);
        _worldYuaiText.gameObject.SetActive(false);
    }

    public void ChangeMainUIEnable() { _uiCanvas.enabled = true; }
    public void ChangeMainUIDisable() { _uiCanvas.enabled = false; }

    public void BossUpStart()
    {
        if (_specialUI && !_isBossUpFade) _specialUI.enabled = true;
        else
        {
            _bossUpFadeAnim.gameObject.SetActive(true);
            _bossUpFadeAnim.Play(_bossUpFadeAnimClip.name);
        }
        ChangeMainUIDisable();
    }

    public void BossUpEnd(float thinkingTime)
    {
        _timerPanel.enabled = true;
        ChangeMainUIEnable();
        _timerPanel.gameObject.SetActive(true);
        TimerSet(thinkingTime);
        if (_specialUI && !_isBossUpFade) _specialUI.enabled = false;
    }

    public void TimerSet(float thinkingTime)
    {
        var _timerText = _timerPanel.GetComponentInChildren<Text>();
        if (!_timerText) return;

        _timerText.text = $"00:{thinkingTime.ToString("00")}";
    }

    public IEnumerator FadeInBoss()
    {
        _bossFadeAnim.gameObject.SetActive(true);
        yield return StartCoroutine(FadeManager.Instance.CustomFadeIn(_fadeImage, 0.5f));
        _bossFadeAnim.Play(_bossFadeAnimationClip.name);
        yield return StartCoroutine(FadeManager.Instance.FadeIn());
        _bossFadeAnim.gameObject.SetActive(false);
    }

    public IEnumerator FadeOutBoss()
    {
        _yuaiSearchText.gameObject.SetActive(true);
        yield return StartCoroutine(FadeManager.Instance.FadeOut());
        yield return WaitforSecondsCashe.Wait(1f);
        yield return StartCoroutine(FadeManager.Instance.CustomFadeOut(_fadeImage,0.5f));
    }

    public void UIReset()
    {
        _timerPanel.gameObject.SetActive(false);
        _yuaiSearchText.gameObject.SetActive(false);
        _worldYuaiText.gameObject.SetActive(false);
        _bossUpFadeAnim.gameObject.SetActive(false);
        _bossFadeAnim.gameObject.SetActive(false);
    }

    public void ChangeYuaiSearchText()
    {
        _yuaiSearchText.gameObject.SetActive(false);
        _worldYuaiText.gameObject.SetActive(true);
    }

    public void ChangeSearchText(string text)
    {
        _worldYuaiText.text = text;
    }

}
