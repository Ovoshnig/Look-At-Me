using UnityEngine;
using UnityEngine.UI;
using Zenject;

public abstract class MenuHandler : MonoBehaviour
{
    [SerializeField] protected GameObject _optionsPanel;
    [SerializeField] protected Slider _sensitivitySlider;
    [SerializeField] protected Slider _globalVolumeSlider;

    protected LevelSwitch LevelSwitch;
    protected LookSettings LookSettings;
    protected AudioSettings AudioSettings;
    protected Settings Settings;

    [Inject]
    protected void Construct(LevelSwitch levelSwitch, LookSettings lookSettings, 
        AudioSettings audioSettings, Settings settings)
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

        _globalVolumeSlider.value = AudioSettings.GlobalVolume;
        _globalVolumeSlider.maxValue = Settings.MaxVolume;
    }

    public void SetSensitivity() => LookSettings.Sensitivity = _sensitivitySlider.value;

    public void SetVolume() => AudioSettings.GlobalVolume = _globalVolumeSlider.value;
}
