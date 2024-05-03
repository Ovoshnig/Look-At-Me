using UnityEngine;

public class VolumeKeeper
{
    private float _volume;
    private const string _volume_key = "Volume";

    private VolumeKeeper() => _volume = PlayerPrefs.GetFloat(_volume_key, 0.5f);

    public float Get() => _volume;

    public void Set(float volume) => _volume = Mathf.Clamp01(volume);
    
    public void Save() => PlayerPrefs.SetFloat(_volume_key, _volume);
}
