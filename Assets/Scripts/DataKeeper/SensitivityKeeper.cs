using System;
using Zenject;

public class SensitivityKeeper : DataKeeper<float>
{
    private readonly Settings _settings;

    [Inject]
    public SensitivityKeeper(Settings settings)
    {
        _settings = settings;
        DefaultValue = _settings.MaxSensitivity / 2;

        DataKey = "Sensitivity";
        LoadData();
    }

    public override float Value 
    { 
        get => ValueField;
        set
        {
            if (value >= 0 && value <= _settings.MaxSensitivity)
                ValueField = value;
            else
                throw new InvalidOperationException($"Invalid sensitivity: {value}");
        }
    }

}
    