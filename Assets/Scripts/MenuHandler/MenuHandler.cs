using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MenuHandler : MonoBehaviour
{
    [SerializeField] protected GameObject _optionsPanel;
    [SerializeField] protected Slider _sensitivitySlider;
    [SerializeField] protected Slider _volumeSlider;

    [Inject] protected readonly ProgressKeeper _progressKeeper;
    [Inject] protected readonly SensitivityKeeper _sensitivityKeeper;
    [Inject] protected readonly VolumeKeeper _volumeKeeper;

    protected void Start()
    {
        InitializeSettings();
        DataLoad();
    }

    protected virtual void InitializeSettings()
    {

    }

    public void DataLoad()
    {
        var sensitivity = _sensitivityKeeper.Get();
        _sensitivitySlider.value = sensitivity;

        var volume = _volumeKeeper.Get();
        _volumeSlider.value = volume;
    }

    public void SetSensitivity()
    {
        var sensitivity = _sensitivitySlider.value;
        _sensitivityKeeper.Set(sensitivity);
    }

    public void SetVolume()
    {
        var volume = _volumeSlider.value;
        _volumeKeeper.Set(volume);
        AudioListener.volume = volume;
    }

    protected void SaveData()
    {
        _sensitivityKeeper.Save();
        _volumeKeeper.Save();
        PlayerPrefs.Save();
    }
}
