using UnityEngine;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour
{
    [SerializeField] protected GameObject _optionsPanel;
    [SerializeField] protected Slider _sensitivitySlider;
    [SerializeField] protected Slider _volumeSlider;

    protected float _sensitivity;
    protected float _volume;

    protected const string SensitivityKey = "Sensitivity";
    protected const string VolumeKey = "Volume";

    protected void Start()
    {
        InitializeSettings();
    }

    protected virtual void InitializeSettings()
    {
        _sensitivitySlider.value = PlayerPrefs.GetFloat(SensitivityKey, _sensitivitySlider.maxValue / 2);
        _volumeSlider.value = PlayerPrefs.GetFloat(VolumeKey, _volumeSlider.maxValue / 2);
    }

    public void SetSensitivity()
    {
        _sensitivity = _sensitivitySlider.value;
    }

    public void SetVolume()
    {
        _volume = _volumeSlider.value;
        AudioListener.volume = _volume;
    }

    protected void SaveSensitivityAndVolume()
    {
        PlayerPrefs.SetFloat(SensitivityKey, _sensitivity);
        PlayerPrefs.SetFloat(VolumeKey, _volume);
    }
}
