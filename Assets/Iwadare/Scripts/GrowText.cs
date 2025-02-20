using UnityEngine;
using UnityEngine.UI;

public class GrowText : MonoBehaviour
{
    [SerializeField] Material _textMaterial;
    float _glowIntensity = 0.5f;
    bool _increasing = true;
    [SerializeField] Color _defaultColor;
    [SerializeField] Color _afterColor;
    [SerializeField] float _growPower = 5f;

    private void Start()
    {
        _textMaterial.SetColor("Tint Color", _defaultColor);
    }
    void Update()
    {
        // 発光の強さを変化
        _glowIntensity += (_increasing ? Time.deltaTime : -Time.deltaTime) * _growPower;

        // 範囲を制限
        if (_glowIntensity >= _growPower) _increasing = false;
        if (_glowIntensity <= _growPower / 5) _increasing = true;

        // Emission Color を更新
        Color glowColor = _afterColor * _glowIntensity;
        _textMaterial.SetColor("_EmissionColor", glowColor);
    }
}