using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class DataSaver : IDisposable
{
    private const string SaveFileName = "playerData.json";

    private readonly string _filePath;
    private Dictionary<string, string> _dataStore;

    public DataSaver()
    {
        _filePath = Path.Combine(Application.persistentDataPath, SaveFileName);
        LoadDataStore();
    }

    public void Dispose() => SaveDataStore();

    public T LoadData<T>(string key, T defaultValue = default)
    {
        if (_dataStore.TryGetValue(key, out string storedValue))
            return JsonConvert.DeserializeObject<T>(storedValue);
        return defaultValue;
    }

    public void SaveData<T>(string key, T value)
    {
        string serializedValue = JsonConvert.SerializeObject(value);
        _dataStore[key] = serializedValue;
    }

    private void LoadDataStore()
    {
        if (File.Exists(_filePath))
        {
            string json = File.ReadAllText(_filePath);
            _dataStore = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }
        else
        {
            _dataStore = new Dictionary<string, string>();
        }
    }

    private void SaveDataStore()
    {
        string json = JsonConvert.SerializeObject(_dataStore, Formatting.Indented);
        File.WriteAllText(_filePath, json);
    }
}
