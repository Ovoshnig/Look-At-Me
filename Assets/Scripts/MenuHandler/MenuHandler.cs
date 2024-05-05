using UnityEngine;
using UnityEngine.UI;
using Zenject;

public abstract class MenuHandler : MonoBehaviour
{
    [SerializeField] protected GameObject _optionsPanel;
    [SerializeField] protected Slider _sensitivitySlider;
    [SerializeField] protected Slider _volumeSlider;

    protected LevelSwitch _levelSwitch;
    protected SensitivityKeeper _sensitivityKeeper;
    protected VolumeKeeper _volumeKeeper;

    [Inject]
    protected void Construct(LevelSwitch levelSwitch, SensitivityKeeper sensitivityKeeper, VolumeKeeper volumeKeeper)
    {
        _levelSwitch = levelSwitch;
        _sensitivityKeeper = sensitivityKeeper;
        _volumeKeeper = volumeKeeper;
    }

    protected void Start()
    {
        InitializeSettings();
        InitializeSliders();
    }

    protected abstract void InitializeSettings();

    public void InitializeSliders()
    {
        float sensitivity = _sensitivityKeeper.Value;
        _sensitivitySlider.value = sensitivity;

        float volume = _volumeKeeper.Value;
        _volumeSlider.value = volume;
    }

    public void SetSensitivity()
    {
        float sensitivity = _sensitivitySlider.value;
        _sensitivityKeeper.Value = sensitivity;
    }

    public void SetVolume()
    {
        float volume = _volumeSlider.value;
        _volumeKeeper.Value = volume;
        AudioListener.volume = volume;
    }
}
