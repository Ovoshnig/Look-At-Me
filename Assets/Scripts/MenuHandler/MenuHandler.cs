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

    protected SceneSwitch SceneSwitch;

    private LookTuner _lookTuner;
    private AudioTuner _audioTuner;
    private GameSettingsInstaller.ControlSettings _controlSettings;
    private GameSettingsInstaller.AudioSettings _audioSettings;

    [Inject]
    protected void Construct(SceneSwitch sceneSwitch, LookTuner lookTuner, 
                             AudioTuner audioTuner, GameSettingsInstaller.ControlSettings controlSettings, 
                             GameSettingsInstaller.AudioSettings audioSettings)
    {
        SceneSwitch = sceneSwitch;
        _lookTuner = lookTuner;
        _audioTuner = audioTuner;
        _controlSettings = controlSettings;
        _audioSettings = audioSettings;
    }

    protected void Start()
    {
        InitializeSettings();
        InitializeSliders();
    }

    protected abstract void InitializeSettings();

    private void InitializeSliders()
    {
        _sensitivitySlider.maxValue = _controlSettings.MaxSensitivity;
        _sensitivitySlider.value = _lookTuner.Sensitivity;

        _soundsVolumeSlider.minValue = _audioSettings.MinVolume;
        _soundsVolumeSlider.maxValue = _audioSettings.MaxVolume;
        _soundsVolumeSlider.value = _audioTuner.SoundsVolume;

        _musicVolumeSlider.minValue = _audioSettings.MinVolume;
        _musicVolumeSlider.maxValue = _audioSettings.MaxVolume;
        _musicVolumeSlider.value = _audioTuner.MusicVolume;
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

    private void SetSensitivity(float value) => _lookTuner.Sensitivity = value;

    private void SetSoundsVolume(float value) => _audioTuner.SoundsVolume = value;

    private void SetMusicVolume(float value) => _audioTuner.MusicVolume = value;
}
