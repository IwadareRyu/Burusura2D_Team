using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [ContextMenuItem("CreateAudioSources", "CreateAudioSources")]
    [ContextMenuItem("ResetAudios", "ResetAudios")]
    [SerializeField] GameObject _bgmTarget;
    [ContextMenuItem("CreateAudioSources", "CreateAudioSources")]
    [ContextMenuItem("ResetAudios", "ResetAudios")]
    [SerializeField] GameObject _seTarget;
    //ファイル内のSE数
    private int _seCount = 5;

    //音量設定保存用のKey及び各音声のデフォルト音量
    private const string BGM_VOLUME_KEY = "BGM";
    private const string SE_VOLUME_KEY = "SE";
    //デフォルト音量
    [SerializeField, Range(0f, 1.0f)] private float _defaultBGMVolume = 0.5f;
    [SerializeField, Range(0f, 1.0f)] private float _defaultSEVolume = 0.5f;

    //各ファイルのパス
    private const string BGM_PATH = "Audio/BGM";
    private const string SE_PATH = "Audio/SE";
    private const string MIXER_PATH = "Audio/Mixer";

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
        else Destroy(this.gameObject);
        DontDestroyOnLoad(Instance.gameObject);
        CreateAudioSources();
    }

    public void Start()
    {
        ChangeVolume(_defaultBGMVolume, _defaultSEVolume);
    }

    void CreateAudioSources()
    {
        _bgmDic = new();
        _seDic = new();
        _seSourcesLis = new();

        var bgmList = Resources.LoadAll(BGM_PATH);
        var seList = Resources.LoadAll(SE_PATH);
        if (_audioMixer == null) _audioMixer = (AudioMixer)Resources.Load(MIXER_PATH);

        if (_bgmTarget.GetComponent<AudioSource>() == null)
        {
            _bgmTarget.AddComponent<AudioSource>();
        }

        var oldSeList = _seTarget.GetComponents<AudioSource>();

        _seCount = seList.Length;
        for (int i = 0; i < _seCount - oldSeList.Length; i++)
        {
            _seTarget.AddComponent<AudioSource>();
        }

        AudioSource[] audioSources = GetComponentsInChildren<AudioSource>();

        audioSources[0].loop = true;
        _bgmSource = audioSources[0];
        _bgmSource.outputAudioMixerGroup = _audioMixer.FindMatchingGroups("BGM")[0];

        for (int i = 1; i < audioSources.Length; i++)
        {
            audioSources[i].playOnAwake = false;
            audioSources[i].outputAudioMixerGroup = _audioMixer.FindMatchingGroups("SE")[0];
            _seSourcesLis.Add(audioSources[i]);
        }

        foreach (AudioClip bgm in bgmList)
        {
            _bgmDic[bgm.name] = bgm;
        }
        foreach (AudioClip se in seList)
        {
            _seDic[se.name] = se;
        }

        for (int i = 0; i < _seSourcesLis.Count; i++)
        {
            _seSourcesLis[i].clip = (AudioClip)seList[i];
        }
    }

    void ResetAudios()
    {
        AudioSource[] audioSources = GetComponentsInChildren<AudioSource>();
        if (audioSources.Length == 0)
        {
            return;
        }
        foreach (var audio in audioSources)
        {
            DestroyImmediate(audio);
        }
    }

    public void ChangeVolume(float BGMVolume, float SEVolume)
    {
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
    public void PlaySE(string seName, float Volume = 1.1f)
    {
        if (!_seDic.ContainsKey(seName))
        {
            Debug.Log(seName + "が存在しません");
            return;
        }

        foreach (AudioSource source in _seSourcesLis)
        {
            if (source.clip.name == seName)
            {
                if (Volume != 1.1f)
                {
                    source.volume = Volume;
                }
                if (source.isPlaying)
                {
                    source.Stop();
                }
                source.Play();
                return;
            }
        }
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
        _audioMixer.SetFloat(BGM_VOLUME_KEY, Mathf.Lerp(-40f, 0f, BGMVolume));
        PlayerPrefs.SetFloat(BGM_VOLUME_KEY, BGMVolume);
    }
    public void SetSEVolume(float SEVolume)
    {
        _audioMixer.SetFloat(SE_VOLUME_KEY, Mathf.Lerp(-40f, 0f, SEVolume));
        PlayerPrefs.SetFloat(SE_VOLUME_KEY, SEVolume);
    }
}