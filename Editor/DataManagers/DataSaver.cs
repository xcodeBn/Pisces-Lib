using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;

namespace PiData
{
    public static class DataSaver
    {
        private static string GetFilePath<T>(string fileName)
        {
            return Path.Combine(Application.persistentDataPath, fileName + ".json");
        }

        public static void SaveData<T>(T data, string fileName)
        {
            try
            {
                string filePath = GetFilePath<T>(fileName);
                string contents = JsonConvert.SerializeObject(data, Formatting.Indented);
                File.WriteAllText(filePath, contents);
                Debug.Log($"Data of type {typeof(T)} saved successfully to {filePath}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to save data of type {typeof(T)}: {ex.Message}");
            }
        }

        public static T LoadData<T>(string fileName) where T : new()
        {
            try
            {
                string filePath = GetFilePath<T>(fileName);
                if (File.Exists(filePath))
                {
                    string json = File.ReadAllText(filePath);
                    T result = JsonConvert.DeserializeObject<T>(json);
                    Debug.Log($"Data of type {typeof(T)} loaded successfully from {filePath}");
                    return result;
                }

                Debug.LogWarning($"Save file not found for {filePath}, returning new {typeof(T)} instance.");
                return new T();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to load data of type {typeof(T)}: {ex.Message}");
                return new T();
            }
        }

        public static void DeleteData<T>(string fileName)
        {
            try
            {
                string filePath = GetFilePath<T>(fileName);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    Debug.Log($"Data of type {typeof(T)} deleted successfully from {filePath}");
                }
                else
                {
                    Debug.LogWarning("Save file not found for " + filePath);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to delete data of type {typeof(T)}: {ex.Message}");
            }
        }

        public static void SaveFloat(float data, string fileName)
        {
            SaveData(data, fileName);
        }

        public static float LoadFloat(string fileName)
        {
            return LoadData<float>(fileName);
        }
    }
}
