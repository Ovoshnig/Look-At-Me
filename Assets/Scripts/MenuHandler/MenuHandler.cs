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

    private void InitializeSliders()
    {
        _sensitivitySlider.maxValue = Settings.MaxSensitivity;
        _sensitivitySlider.value = LookSettings.Sensitivity;

        _soundsVolumeSlider.minValue = Settings.MinVolume;
        _soundsVolumeSlider.maxValue = Settings.MaxVolume;
        _soundsVolumeSlider.value = AudioSettings.SoundsVolume;

        _musicVolumeSlider.minValue = Settings.MinVolume;
        _musicVolumeSlider.maxValue = Settings.MaxVolume;
        _musicVolumeSlider.value = AudioSettings.MusicVolume;
    }

    protected void OnEnable()
    {
        SubscribeToEvents();
        AddButtonListeners();
        AddSliderListeners();
    }

    protected void OnDisable()
    {
        UnsubscribeFromEvents();
        RemoveButtonListeners();
        RemoveSliderListeners();
    }

    protected virtual void SubscribeToEvents()
    {

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

    protected virtual void UnsubscribeFromEvents()
    {

    }

    protected virtual void RemoveButtonListeners()
    {
        _openSettingsPanelButton.onClick.RemoveListener(OpenSettingsPanel);
        _closeSettingsPanelButton.onClick.RemoveListener(CloseSettingsPanel);
    }

    protected void RemoveSliderListeners()
    {
        _sensitivitySlider.onValueChanged.RemoveListener(SetSensitivity);
        _soundsVolumeSlider.onValueChanged.RemoveListener(SetSoundsVolume);
        _musicVolumeSlider.onValueChanged.RemoveListener(SetMusicVolume);
    }

    private void OpenSettingsPanel()
    {
        MenuPanel.SetActive(false);
        SettingsPanel.SetActive(true);
    }

    private void CloseSettingsPanel()
    {
        MenuPanel.SetActive(true);
        SettingsPanel.SetActive(false);
    }

    private void SetSensitivity(float value) => LookSettings.Sensitivity = value;

    private void SetSoundsVolume(float value) => AudioSettings.SoundsVolume = value;

    private void SetMusicVolume(float value) => AudioSettings.MusicVolume = value;
}
