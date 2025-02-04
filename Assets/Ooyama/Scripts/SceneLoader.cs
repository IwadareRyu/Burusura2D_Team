using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }
    public IEnumerator SceneLoad(string sceneName)
    {
        yield return WaitTimer(sceneName);
    }
    IEnumerator WaitTimer(string NextSceneName)
    {
        var async = SceneManager.LoadSceneAsync(NextSceneName);
        async.allowSceneActivation = false;
        while (async.progress < 0.9f) yield return null;
        async.allowSceneActivation = true;
        while (!async.isDone) yield return null;
    }
    public void Test()
    {
        SceneLoad("YuaiScene");
    }
}
