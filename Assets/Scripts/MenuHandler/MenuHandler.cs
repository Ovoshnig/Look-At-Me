using UnityEngine;
using UnityEngine.UI;
using Zenject;

public abstract class MenuHandler : MonoBehaviour
{
    [SerializeField] protected GameObject _optionsPanel;

    [SerializeField] private Slider _sensitivitySlider;
    [SerializeField] private Slider _soundsVolumeSlider;
    [SerializeField] private Slider _musicVolumeSlider;

    protected LevelSwitch LevelSwitch;
    protected LookSettings LookSettings;
    protected AudioSettings AudioSettings;
    protected GameSettingsInstaller.GameSettings Settings;

    [Inject]
    protected void Construct(LevelSwitch levelSwitch, LookSettings lookSettings, 
        AudioSettings audioSettings, GameSettingsInstaller.GameSettings settings)
    {
        LevelSwitch = levelSwitch;
        LookSettings = lookSettings;
        AudioSettings = audioSettings;
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
        _sensitivitySlider.value = LookSettings.Sensitivity;
        _sensitivitySlider.maxValue = Settings.MaxSensitivity;

        _soundsVolumeSlider.value = AudioSettings.SoundsVolume;
        _soundsVolumeSlider.maxValue = Settings.MaxVolume;

        _musicVolumeSlider.value = AudioSettings.MusicVolume;
        _musicVolumeSlider.maxValue = Settings.MaxVolume;
    }

    public void SetSensitivity() => LookSettings.Sensitivity = _sensitivitySlider.value;

    public void SetSoundsVolume() => AudioSettings.SoundsVolume = _soundsVolumeSlider.value;

    public void SetMusicVolume() => AudioSettings.MusicVolume = _musicVolumeSlider.value;
}
