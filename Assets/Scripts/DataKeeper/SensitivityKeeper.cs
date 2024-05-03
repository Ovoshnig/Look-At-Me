using UnityEngine;

public class SensitivityKeeper
{
    private float _sensitivity;
    private const string _sensitivity_key = "Sensitivity";

    private SensitivityKeeper() => _sensitivity = PlayerPrefs.GetFloat(_sensitivity_key, 5f);

    public float Get() => _sensitivity;

    public void Set(float sensitivity) => _sensitivity = Mathf.Clamp(sensitivity, 0f, 10f);

    public void Save() => PlayerPrefs.SetFloat(_sensitivity_key, _sensitivity);
}
