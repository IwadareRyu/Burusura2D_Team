using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    //ファイル内のSE数
    private int _seCount = 5;

    //音量設定保存用のKey及び各音声のデフォルト音量
    private const string BGM_VOLUME_KEY = "BGM_VOLUME";
    private const string SE_VOLUME_KEY = "SE_VOLUME";
    [SerializeField,Range(0f,1.0f)] private float _defaultBGMVolume = 1.0f;
    [SerializeField,Range(0f,1.0f)] private float _defaultSEVolume = 1.0f;

    //各ファイルのパス
    private const string BGM_PATH = "Audio/BGM";
    private const string SE_PATH = "Audio/SE";

    //各AudioSource
    private AudioSource _bgmSource;
    private List<AudioSource> _seSourcesLis;

    //ファイル内を名称検索するためのDictionary
    private Dictionary<string, AudioClip> _bgmDic;
    private Dictionary<string, AudioClip> _seDic;

    public static AudioManager Instance = null;

    [SerializeField] private AudioMixer _audioMixer = null;

    private void Awake()
    {

        if (Instance == null) Instance = this;
        else Destroy(this);

        DontDestroyOnLoad(Instance.gameObject);

        _bgmDic = new();
        _seDic = new();

        var bgmList = Resources.LoadAll(BGM_PATH);
        foreach (AudioClip bgm in bgmList)
        {
            _bgmDic[bgm.name] = bgm;
        }

        var seList = Resources.LoadAll(SE_PATH);
        foreach (AudioClip se in seList)
        {
            _seDic[se.name] = se;
        }

        _seCount = seList.Length;
        for (int i = 0; i <= _seCount; i++)
        {
            gameObject.AddComponent<AudioSource>();
        }

        AudioSource[] audioSources = GetComponents<AudioSource>();
        _seSourcesLis = new();


        audioSources[0].loop = true;
        _bgmSource = audioSources[0];
        _bgmSource.volume = GetBGMVolume();
        _bgmSource.outputAudioMixerGroup = _audioMixer.FindMatchingGroups("BGM")[0];

        for (int i = 1; i < audioSources.Length; i++)
        {
            audioSources[i].playOnAwake = false;
            _seSourcesLis.Add(audioSources[i]);
            audioSources[i].volume = GetSEVolume();
            audioSources[i].outputAudioMixerGroup = _audioMixer.FindMatchingGroups("SE")[0];
        }
    }
    public void ChangeVolume(float BGMVolume, float SEVolume)
    {
        _bgmSource.volume = BGMVolume;
        foreach (AudioSource source in _seSourcesLis)
        {
            source.volume = SEVolume;
        }

        SetBGMVolume(BGMVolume);
        SetSEVolume(SEVolume);
    }
    public void PlayBGM(string bgmName)
    {
        if (!_bgmDic.ContainsKey(bgmName))
        {
            Debug.Log(bgmName + "が存在しません");
            return;
        }

        if (!_bgmSource.isPlaying)
        {
            _bgmSource.clip = _bgmDic[bgmName];
            _bgmSource.Play();
        }
        else
        {
            StopBGM();
            PlayBGM(bgmName);
        }
    }
    public void PlaySE(string seName)
    {
        if (!_seDic.ContainsKey(seName))
        {
            Debug.Log(seName + "が存在しません");
            return;
        }

        foreach (AudioSource source in _seSourcesLis)
        {

            Debug.Log("Play" + seName);
            source.PlayOneShot(_seDic[seName]);
            return;
        }
    }
    public void TestBGM()
    {
        PlayBGM("Test");
    }
    public void TestSE()
    {
        PlaySE("GetItem");
    }
    public void TestSE2()
    {
        PlaySE("se_powerup");
    }
    public void StopBGM()
    {
        _bgmSource.Stop();
    }
    public float GetBGMVolume()
    {
        return PlayerPrefs.GetFloat(BGM_VOLUME_KEY, _defaultBGMVolume);
    }
    public float GetSEVolume()
    {
        return PlayerPrefs.GetFloat(SE_VOLUME_KEY, _defaultSEVolume);
    }
    public void SetBGMVolume(float BGMVolume)
    {
        PlayerPrefs.SetFloat(BGM_VOLUME_KEY, BGMVolume);
    }
    public void SetSEVolume(float SEVolume)
    {
        PlayerPrefs.SetFloat(SE_VOLUME_KEY, SEVolume);
    }
}