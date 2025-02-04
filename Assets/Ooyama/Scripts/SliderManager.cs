using UnityEngine;
using UnityEngine.UI;

public class SliderManager : MonoBehaviour
{
    [SerializeField] private Slider _bgmSlider;
    [SerializeField] private Slider _seSlider;

    private void OnEnable()
    {
        _bgmSlider.value = AudioManager.Instance.GetBGMVolume();
        _seSlider.value = AudioManager.Instance.GetSEVolume();
    }

    public void ApplyChangeVolume()
    {
        AudioManager.Instance.SetBGMVolume(_bgmSlider.value);
        AudioManager.Instance.SetSEVolume(_seSlider.value);
    }
}
