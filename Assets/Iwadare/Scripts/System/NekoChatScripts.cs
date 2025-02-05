using System;
using System.Collections;
using Unity.Burst.Intrinsics;
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
    [SerializeField] String[] _goodChat;
    [SerializeField] String[] _badChat;
    [SerializeField] String[] _chat;
    [SerializeField] float _maxCount = 100000;
    [SerializeField] float _testDisCount = 1;
    [SerializeField] Sprite _maxSprite;
    [SerializeField] String _maxchat;
    float _testResponceCurrentTime;
    [SerializeField]bool _isDebug = false;

    private void Awake()
    {
        _isResponceUltraChat = true;
        _testResponceCurrentTime = 0f;
        _ultraChatPanel.transform.position = _initialPosition.position;
    }

    private void Update()
    {
        if (_isDebug)
        {
            _testResponceCurrentTime += Time.deltaTime;
            if (_isResponceUltraChat && _testResponceCurrentTime > _testResponceCoolTime)
            {
                StartCoroutine(CoinChatCoroutine(RamdomMethod.RandomNumber99() + _testDisCount));
                _testResponceCurrentTime = 0f;
            }
        }
    }

    public IEnumerator ChatCoroutine(string chat,bool isGood)
    {
        if (GameStateManager.Instance.GameState != GameState.InBattleState && _isResponceUltraChat) yield break;
        _isResponceUltraChat = false;
        _ultraChatText.text = "";
        if (chat.Length > 10)
        {
            chat = chat[..10];
            chat += "...";
        }
        _ultraChatText.text += "『" + chat + "』";
        _ultraChatText.text += Environment.NewLine;
        var ram = RamdomMethod.RamdomNumber0Max(_nekoSprite.Length);
        if (isGood)
        {
            var str = _goodChat[ram].Split("\\n");
            TextFill(str);
            _nekoImage.sprite = _nekoSprite[ram];
        }
        else
        {
            var str = _badChat[ram].Split("\\n");
            TextFill(str);
            _nekoImage.sprite = _nekoSprite[3];
        }

        _ultraChatPanel.Play(_ultraChatMoveAnimClip.name);
        yield return WaitforSecondsCashe.Wait(_ultraChatMoveAnimClip.length);
        Debug.Log("ResponceOK");
    }

    public IEnumerator CoinChatCoroutine(float coinNumber)
    {
        if (GameStateManager.Instance.GameState != GameState.InBattleState && _isResponceUltraChat) yield break;
        _isResponceUltraChat = false;
        _ultraChatText.text = "";
        if (coinNumber <= _maxCount)
        {
            var ram = RamdomMethod.RamdomNumber0Max(_nekoSprite.Length);
            var str = _chat[ram].Split("\\n");
            for (int i = 0; i < str.Length; i++)
            {
                _ultraChatText.text += str[i].Contains("yen") ? str[i].Replace("yen", coinNumber.ToString()) : str[i];
                _ultraChatText.text += Environment.NewLine;
            }

            _nekoImage.sprite = _nekoSprite[ram];
        }
        else
        {
            var str = _maxchat.Split("\\n");
            TextFill(str);
            _nekoImage.sprite = _maxSprite;
        }
        _ultraChatPanel.Play(_ultraChatMoveAnimClip.name);
        yield return WaitforSecondsCashe.Wait(_ultraChatMoveAnimClip.length);
        Debug.Log("ResponceOK");
        _isResponceUltraChat = true;
    }

    public void TextFill(string[] str)
    {
        for (int i = 0; i < str.Length; i++)
        {
            _ultraChatText.text += str[i];
            _ultraChatText.text += Environment.NewLine;
        }
    }
}
