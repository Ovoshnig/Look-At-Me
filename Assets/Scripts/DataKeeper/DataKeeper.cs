using System;
using System.IO;
using UnityEngine;

public class DataKeeper<T> : IDisposable
{
    [SerializeField] public T Value;

    protected string DataKey;
    protected T DefaultValue;

    public DataKeeper(string dataKey, T defaultValue)
    {
        DataKey = dataKey;
        DefaultValue = defaultValue;

        LoadData();
    }

    private string GetFilePath() => Path.Combine(Application.persistentDataPath, $"{DataKey}.json");

    private void LoadData()
    {
        string filePath = GetFilePath();
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            JsonUtility.FromJsonOverwrite(json, this);
        }
        else
        {
            Value = DefaultValue;
        }
    }

    private void SaveData()
    {
        string json = JsonUtility.ToJson(this);
        File.WriteAllText(GetFilePath(), json);
    }

    public void Dispose() => SaveData();
}
