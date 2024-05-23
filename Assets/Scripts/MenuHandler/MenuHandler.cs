using UnityEngine;
using UnityEngine.UI;
using Zenject;

public abstract class MenuHandler : MonoBehaviour
{
    [SerializeField] protected GameObject MenuPanel;
    [SerializeField] protected GameObject SettingsPanel;

    [SerializeField] private Button _openSettingsPanelButton;
    [SerializeField] private Button _closeSettingsPanelButton;
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

    protected void OnEnable()
    {
        AddButtonListeners();
        AddSliderListeners();
    }

    protected virtual void AddButtonListeners()
    {
        _openSettingsPanelButton.onClick.AddListener(OpenSettingsPanel);
        _closeSettingsPanelButton.onClick.AddListener(CloseSettingsPanel);
    }

    protected void AddSliderListeners()
    {
        _sensitivitySlider.onValueChanged.AddListener(SetSensitivity);
        _soundsVolumeSlider.onValueChanged.AddListener(SetSoundsVolume);
        _musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
    }

    protected void OpenSettingsPanel()
    {
        MenuPanel.SetActive(false);
        SettingsPanel.SetActive(true);
    }

    protected void CloseSettingsPanel()
    {
        MenuPanel.SetActive(true);
        SettingsPanel.SetActive(false);
    }

    protected void SetSensitivity(float value) => LookSettings.Sensitivity = value;

    protected void SetSoundsVolume(float value) => AudioSettings.SoundsVolume = value;

    protected void SetMusicVolume(float value) => AudioSettings.MusicVolume = value;
}
