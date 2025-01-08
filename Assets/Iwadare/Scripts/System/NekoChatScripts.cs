using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NekoChatScripts : MonoBehaviour
{
    bool _isResponceUltraChat;
    [SerializeField] Animator _ultraChatPanel;
    [SerializeField] AnimationClip _ultraChatMoveAnimClip;
    [SerializeField] Image _nekoImage;
    [SerializeField] Text _ultraChatText;
    [SerializeField] Transform _initialPosition;
    [SerializeField] float _testResponceCoolTime = 5f;
    [SerializeField] Sprite[] _nekoSprite;
    [SerializeField] String[] _chat;
    float _testResponceCurrentTime;

    private void Awake()
    {
        _isResponceUltraChat = true;
        _testResponceCurrentTime = 0f;
        _ultraChatPanel.transform.position = _initialPosition.position;
    }

    private void Update()
    {
        _testResponceCurrentTime += Time.deltaTime;
        if (_isResponceUltraChat && _testResponceCurrentTime > _testResponceCoolTime)
        {
            StartCoroutine(UltraChatCoroutine(RamdomMethod.RandomNumber99() + 1));
            _testResponceCurrentTime = 0f;
        }
    }

    public IEnumerator UltraChatCoroutine(float coinNumber)
    {
        _isResponceUltraChat = false;
        var ram = RamdomMethod.RamdomNumber(_nekoSprite.Length);
        _ultraChatText.text = "";
        var str = _chat[ram].Split("\\n");
        for (int i = 0; i < str.Length; i++)
        {
            _ultraChatText.text += str[i].Contains("yen") ? str[i].Replace("yen", coinNumber.ToString()) : str[i];
            _ultraChatText.text += Environment.NewLine;
        }
        _nekoImage.sprite = _nekoSprite[ram];
        _ultraChatPanel.Play(_ultraChatMoveAnimClip.name);
        yield return WaitforSecondsCashe.Wait(_ultraChatMoveAnimClip.length);
        Debug.Log("ResponceOK");
        _isResponceUltraChat = true;
    }
}
