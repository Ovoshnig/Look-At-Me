using UnityEngine.SceneManagement;
using System;

public sealed class AchievedLevelKeeper : DataKeeper<int>
{
    private const int StartLevel = 1;

    private AchievedLevelKeeper()
    {
        DefaultValue = StartLevel;
        DataKey = "AchievedLevel";
        LoadData();
    }

    public override int Value 
    {
        get => ValueField;
        set
        {
            if (value > 0 && value < SceneManager.sceneCountInBuildSettings - 1)
                ValueField = value;
            else
                throw new InvalidOperationException($"Invalid achieved level: {value}");
        }
    }
}
