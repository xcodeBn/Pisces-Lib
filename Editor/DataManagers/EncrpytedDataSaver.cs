using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace PiData
{
    public static class EncryptedDataSaver
    {
        private static string GetFilePath<T>(string fileName)
        {
            return Path.Combine(Application.persistentDataPath, fileName + ".json");
        }

        public static void SaveData<T>(T data, string fileName, string encryptionKey)
        {
            try
            {
                string filePath = GetFilePath<T>(fileName);
                string json = JsonUtility.ToJson(data);
                string encryptedJson = Encrypt(json, encryptionKey);
                File.WriteAllText(filePath, encryptedJson);
                Debug.Log($"Data of type {typeof(T)} saved successfully to {filePath}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to save data of type {typeof(T)}: {e.Message}");
            }
        }

        public static T LoadData<T>(string fileName, string encryptionKey) where T : new()
        {
            try
            {
                string filePath = GetFilePath<T>(fileName);
                if (File.Exists(filePath))
                {
                    string encryptedJson = File.ReadAllText(filePath);
                    string json = Decrypt(encryptedJson, encryptionKey);
                    T data = JsonUtility.FromJson<T>(json);
                    Debug.Log($"Data of type {typeof(T)} loaded successfully from {filePath}");
                    return data;
                }
                else
                {
                    Debug.LogWarning($"Save file not found for {filePath}, returning new {typeof(T)} instance.");
                    return new T();
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load data of type {typeof(T)}: {e.Message}");
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
                    Debug.LogWarning($"Save file not found for {filePath}");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to delete data of type {typeof(T)}: {e.Message}");
            }
        }

        private static string Encrypt(string plainText, string key)
        {
            using (Aes aes = Aes.Create())
            {
                byte[] keyBytes = Encoding.UTF8.GetBytes(key);
                Array.Resize(ref keyBytes, 16); // Ensure the key is 16 bytes long
                aes.Key = keyBytes;

                aes.GenerateIV();
                byte[] iv = aes.IV;
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream())
                {
                    ms.Write(iv, 0, iv.Length); // Write IV to the start of the stream
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(plainText);
                        }
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        private static string Decrypt(string cipherText, string key)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes aes = Aes.Create())
            {
                byte[] keyBytes = Encoding.UTF8.GetBytes(key);
                Array.Resize(ref keyBytes, 16); // Ensure the key is 16 bytes long
                aes.Key = keyBytes;

                byte[] iv = new byte[16];
                Array.Copy(cipherBytes, iv, iv.Length);
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, iv);

                using (MemoryStream ms = new MemoryStream(cipherBytes, iv.Length, cipherBytes.Length - iv.Length))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
