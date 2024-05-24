using System;
using System.IO;
using UnityEngine;

public class DataKeeper<T> : IDisposable
{
    [SerializeField] public T Value;

    private readonly string _dataKey;
    private readonly T _defaultValue;

    public DataKeeper(string dataKey, T defaultValue)
    {
        _dataKey = dataKey;
        _defaultValue = defaultValue;

        LoadData();
    }

    private string GetFilePath() => Path.Combine(Application.persistentDataPath, $"{_dataKey}.json");

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
            Value = _defaultValue;
        }
    }

    private void SaveData()
    {
        string json = JsonUtility.ToJson(this);
        File.WriteAllText(GetFilePath(), json);
    }

    public void Dispose() => SaveData();
}
