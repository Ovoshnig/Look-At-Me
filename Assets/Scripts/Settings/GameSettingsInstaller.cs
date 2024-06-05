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
        public uint GameplayLevelsCount;
        public float LevelTransitionDuration;
        public float MaxSensitivity;
        public float MinVolume;
        public float MaxVolume;
        public float DefaultSliderCoefficient;

        public uint LastGameplayLevel => FirstGameplayLevel + GameplayLevelsCount - 1;
    }

    public override void InstallBindings()
    {
        Container.BindInstance(Settings);
    }
}
