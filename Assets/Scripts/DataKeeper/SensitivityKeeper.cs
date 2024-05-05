using System;
using UnityEngine;

public class SensitivityKeeper : DataKeeper<float>
{
    private SensitivityKeeper()
    {
        DataKey = "Sensitivity";
        ValueField = PlayerPrefs.GetFloat(DataKey, 5f);
    }

    public override float Value 
    { 
        get => ValueField;
        set
        {
            if (value is >= 0 and <= 10)
                ValueField = value;
            else
                throw new InvalidOperationException("Invalid sensitivity");
        }
    }
}
