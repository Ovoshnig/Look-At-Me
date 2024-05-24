using System;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "Settings", menuName = "ScriptableObjects/Settings")]
public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
{
    public GameSettings Settings;

    [Serializable]
    public class GameSettings
    {
        public uint FirstGameplayLevel;
        public float MaxSensitivity;
        public float MaxVolume;
        public float DefaultSliderCoefficient;
    }

    public override void InstallBindings()
    {
        Container.BindInstance(Settings);
    }
}
