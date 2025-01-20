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
    public void SceneLoad(string SceneName,float WaitTime)
    {
        StartCoroutine(WaitTimer(SceneName,WaitTime));
    }
    IEnumerator WaitTimer(string NextSceneName,float WaitTime)
    {
        var async = SceneManager.LoadSceneAsync(NextSceneName);
        async.allowSceneActivation = false;
        yield return new WaitForSeconds(WaitTime);
        async.allowSceneActivation = true;
    }
    public void Test()
    {
        SceneLoad("YuaiScene", 0.7f);
    }
}
