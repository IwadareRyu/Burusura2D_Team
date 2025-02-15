using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataraSpecialAttack_UI : MonoBehaviour
{
    [SerializeField] Canvas _uiCanvas;
    [SerializeField] Canvas _specialUI;
    [SerializeField] Animator _bossUpFadeAnim;
    [SerializeField] AnimationClip _bossUpFadeAnimClip;
    [SerializeField] bool _isBossUpFade;


    public void Init()
    {
        if (_specialUI) _specialUI.enabled = false;
        _bossUpFadeAnim.gameObject.SetActive(false);
    }

    public void ChangeMainUIEnable() { _uiCanvas.enabled = true; }
    public void ChangeMainUIDisable() { _uiCanvas.enabled = false; }

    public float BossUpStart()
    {
        if (_specialUI && !_isBossUpFade) _specialUI.enabled = true;
        else
        {
            _bossUpFadeAnim.gameObject.SetActive(true);
            _bossUpFadeAnim.Play(_bossUpFadeAnimClip.name);
        }
        ChangeMainUIDisable();
        return _bossUpFadeAnimClip.length;
    }

    public void BossUpEnd()
    {
        ChangeMainUIEnable();
        if (_specialUI && !_isBossUpFade) _specialUI.enabled = false;
    }
}
