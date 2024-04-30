using UnityEngine;

public static class VolumeKeeper
{
    private static float s_volume;
    private const string _volume_key = "Volume";

    static VolumeKeeper() => s_volume = PlayerPrefs.GetFloat(_volume_key, 0.5f);

    public static float Get() => s_volume;

    public static void Set(float volume) => s_volume = Mathf.Clamp01(volume);
    
    public static void Save() => PlayerPrefs.SetFloat(_volume_key, s_volume);
}
