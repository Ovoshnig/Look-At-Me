using UnityEngine;
using UnityEngine.UI;
using Zenject;

public abstract class MenuHandler : MonoBehaviour
{
    [SerializeField] protected GameObject _optionsPanel;
    [SerializeField] protected Slider _sensitivitySlider;
    [SerializeField] protected Slider _volumeSlider;

    protected LevelSwitch LevelSwitch;
    protected SensitivityKeeper SensitivityKeeper;
    protected VolumeKeeper VolumeKeeper;
    protected Settings Settings;

    [Inject]
    protected void Construct(LevelSwitch levelSwitch, SensitivityKeeper sensitivityKeeper, 
        VolumeKeeper volumeKeeper, Settings settings)
    {
        LevelSwitch = levelSwitch;
        SensitivityKeeper = sensitivityKeeper;
        VolumeKeeper = volumeKeeper;
        Settings = settings;
    }

    protected void Start()
    {
        InitializeSettings();
        InitializeSliders();
    }

    protected abstract void InitializeSettings();

    protected void InitializeSliders()
    {
        float sensitivity = SensitivityKeeper.Value;
        _sensitivitySlider.value = sensitivity;
        _sensitivitySlider.maxValue = Settings.MaxSensitivity;

        float volume = VolumeKeeper.Value;
        _volumeSlider.value = volume;
        _volumeSlider.maxValue = Settings.MaxVolume;
    }

    public void SetSensitivity()
    {
        float sensitivity = _sensitivitySlider.value;
        SensitivityKeeper.Value = sensitivity;
    }

    public void SetVolume()
    {
        float volume = _volumeSlider.value;
        VolumeKeeper.Value = volume;
        AudioListener.volume = volume;
    }
}
