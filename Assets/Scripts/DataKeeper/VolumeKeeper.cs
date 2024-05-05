using System;
using UnityEngine;

public class VolumeKeeper : DataKeeper<float>
{
    private VolumeKeeper()
    {
        DataKey = "Volume";
        ValueField = PlayerPrefs.GetFloat(DataKey, 0.5f);
    }

    public override float Value 
    { 
        get => ValueField;
        set
        {
            if (value is >= 0 and <= 1)
                ValueField = value;
            else
                throw new InvalidOperationException("Invalid volume");
        }
    }
}
