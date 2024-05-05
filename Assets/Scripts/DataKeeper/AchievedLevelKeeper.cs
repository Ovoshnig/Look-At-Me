using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public sealed class AchievedLevelKeeper : DataKeeper<int>
{
    private AchievedLevelKeeper()
    {
        DataKey = "AchievedLevel";
        ValueField = PlayerPrefs.GetInt(DataKey, 1);
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
