using UnityEngine;

public class TitleController : MonoBehaviour
{
    [SerializeField] private AudioClip _titleBGM;
    private void Start()
    {
        AudioManager.Instance.PlayBGM(_titleBGM.name);
    }
    public static void StartGame(string nextSceneName)
    {
        FadeManager.Instance.SceneChangeStart(nextSceneName);
    }
}
