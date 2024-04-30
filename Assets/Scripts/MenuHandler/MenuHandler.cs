using UnityEngine;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour
{
    [SerializeField] protected GameObject _optionsPanel;
    [SerializeField] protected Slider _sensitivitySlider;
    [SerializeField] protected Slider _volumeSlider;

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
        var sensitivity = SensitivityKeeper.Get();
        _sensitivitySlider.value = sensitivity;

        var volume = VolumeKeeper.Get();
        _volumeSlider.value = volume;
    }

    public void SetSensitivity()
    {
        var sensitivity = _sensitivitySlider.value;
        SensitivityKeeper.Set(sensitivity);
    }

    public void SetVolume()
    {
        var volume = _volumeSlider.value;
        VolumeKeeper.Set(volume);
        AudioListener.volume = volume;
    }

    protected void SaveData()
    {
        SensitivityKeeper.Save();
        VolumeKeeper.Save();
        PlayerPrefs.Save();
    }
}
