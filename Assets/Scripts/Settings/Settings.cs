using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "ScriptableObjects/Settings")]
public class Settings : ScriptableObject
{
    public uint FirstGameplayLevel;
    public float MaxSensitivity;
    public float MaxVolume;
    public float DefaultSliderCoefficient;
}
