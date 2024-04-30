using UnityEngine;

public static class SensitivityKeeper
{
    private static float s_sensitivity;
    private const string _sensitivity_key = "Sensitivity";

    static SensitivityKeeper() => s_sensitivity = PlayerPrefs.GetFloat(_sensitivity_key, 5f);

    public static float Get() => s_sensitivity;

    public static void Set(float sensitivity) => s_sensitivity = Mathf.Clamp(sensitivity, 0f, 10f);

    public static void Save() => PlayerPrefs.SetFloat(_sensitivity_key, s_sensitivity);
}
