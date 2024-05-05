using System;
using UnityEngine;

public abstract class DataKeeper<T> : IDisposable
{
    protected string DataKey;
    protected T ValueField;

    public abstract T Value { get; set; }

    public void Dispose()
    {
        if (typeof(T) == typeof(int))
            PlayerPrefs.SetInt(DataKey, Convert.ToInt32(Value));
        else if (typeof(T) == typeof(float))
            PlayerPrefs.SetFloat(DataKey, Convert.ToSingle(Value));
        else if (typeof(T) == typeof(string))
            PlayerPrefs.SetString(DataKey, Convert.ToString(Value));
        else
            throw new InvalidOperationException("Unsupported type provided");

        PlayerPrefs.Save();
    }
}
