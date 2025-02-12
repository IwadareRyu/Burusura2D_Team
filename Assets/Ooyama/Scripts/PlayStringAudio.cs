using UnityEngine;

public class PlayStringAudio : MonoBehaviour
{
    [SerializeField] private string _audioName;
    public void PlayNameAudio()
    {
        AudioManager.Instance.PlaySE(_audioName);
    }
}
