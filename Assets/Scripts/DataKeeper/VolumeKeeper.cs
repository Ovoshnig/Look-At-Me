using System;
using Zenject;

public class VolumeKeeper : DataKeeper<float>
{
    private readonly Settings _settings;

    [Inject]
    public VolumeKeeper(Settings settings)
    {
        _settings = settings;
        DefaultValue = _settings.MaxVolume / 2;

        DataKey = "Volume";
        LoadData();
    }

    public override float Value 
    { 
        get => ValueField;
        set
        {
            if (value >= 0 && value <= _settings.MaxVolume)
                ValueField = value;
            else
                throw new InvalidOperationException($"Invalid volume: {value}");
        }
    }
}
