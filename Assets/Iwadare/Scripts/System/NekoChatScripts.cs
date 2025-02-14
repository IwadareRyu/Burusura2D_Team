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
    [SerializeField] Chat[] _goodChat;
    [SerializeField] Chat[] _badChat;
    [SerializeField] Chat[] _chat;
    [SerializeField] Chat[] _explosionChat;
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
        if (GameStateManager.Instance.GameState != GameState.InBattleState || !_isResponceUltraChat) yield break;
        _isResponceUltraChat = false;
        _ultraChatText.text = "";
        if (chat.Length > 10)
        {
            chat = chat[..10];
            chat += "...";
        }
        _ultraChatText.text += "『" + chat + "』";
        _ultraChatText.text += Environment.NewLine;
        var ram = RamdomMethod.RamdomNumber0Max(_goodChat.Length);
        if (isGood)
        {
            var str = _goodChat[ram]._chat.Split("\\n");
            TextFill(str);
            _nekoImage.sprite = _goodChat[ram]._sprite;
        }
        else
        {
            var str = _badChat[ram]._chat.Split("\\n");
            TextFill(str);
            _nekoImage.sprite = _badChat[ram]._sprite;
        }

        _ultraChatPanel.Play(_ultraChatMoveAnimClip.name);
        yield return WaitforSecondsCashe.Wait(_ultraChatMoveAnimClip.length);
        Debug.Log("ResponceOK");
        _isResponceUltraChat = true;
    }

    public IEnumerator CoinChatCoroutine(float coinNumber)
    {
        if (GameStateManager.Instance.GameState != GameState.InBattleState || !_isResponceUltraChat) yield break;
        _isResponceUltraChat = false;
        _ultraChatText.text = "";
        if (coinNumber <= _maxCount)
        {
            var ram = RamdomMethod.RamdomNumber0Max(_chat.Length);
            var str = _chat[ram]._chat.Split("\\n");
            for (int i = 0; i < str.Length; i++)
            {
                _ultraChatText.text += str[i].Contains("yen") ? str[i].Replace("yen", coinNumber.ToString()) : str[i];
                _ultraChatText.text += Environment.NewLine;
            }

            _nekoImage.sprite = _chat[ram]._sprite;
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

    public IEnumerator ExplosionChat()
    {
        if (GameStateManager.Instance.GameState != GameState.InBattleState || !_isResponceUltraChat) yield break;
        _isResponceUltraChat = false;
        _ultraChatText.text = "";
        var ram = RamdomMethod.RamdomNumber0Max(_explosionChat.Length);
        var str = _explosionChat[ram]._chat.Split("\\n");
        TextFill(str);
        _nekoImage.sprite = _explosionChat[ram]._sprite;
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

    [Serializable]
    struct Chat
    {
        public Sprite _sprite;
        public string _chat;
    }
}
