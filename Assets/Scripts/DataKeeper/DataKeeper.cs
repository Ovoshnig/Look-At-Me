using System;
using System.IO;
using UnityEngine;

public abstract class DataKeeper<T> : IDisposable where T : new()
{
    [SerializeField] protected T ValueField;

    protected string DataKey;
    protected T DefaultValue;

    public abstract T Value { get; set; }

    protected virtual string GetFilePath() => Path.Combine(Application.persistentDataPath, $"{DataKey}.json");

    public void Dispose() => SaveData();

    protected void SaveData()
    {
        string json = JsonUtility.ToJson(this);
        File.WriteAllText(GetFilePath(), json);
    }

    protected void LoadData()
    {
        string filePath = GetFilePath();
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            JsonUtility.FromJsonOverwrite(json, this);
        }
        else
        {
            ValueField = DefaultValue;
        }
    }
}
