using System.Globalization;
using UnityEngine;
using System.Security.Cryptography;
using System.Text;
 
public class EncryptedPlayerPrefs  
{
    private static string privateKey = "gTka5rtXihyF8JdMA7Qi";

    private static string[] _keys =
    {
        "zOJZyFN9Tv",
        "SFTpMIE6uv",
        "J3idArPGsV",
        "hGAj0N51PV",
        "BN0HQoLsS8",
        "nHTD6f39Ui",
        "gjbRbf00ZF",
        "fZ2u9lzIsS",
        "R6oLvTF5Tp",
        "3GNf3v43ox"
    };

    private static string Md5(string input)
    {
        using MD5 md5 = MD5.Create();
        byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(input));

        StringBuilder builder = new StringBuilder();
        foreach (var t in bytes)
        {
            builder.Append(t.ToString("x2"));
        }
        return builder.ToString();
    }

    private static string Sha256(string input)
    {
        using SHA256 sha256Hash = SHA256.Create();
        byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

        StringBuilder builder = new StringBuilder();
        foreach (var t in bytes)
        {
            builder.Append(t.ToString("x2"));
        }
        return builder.ToString();
    }

    private static void SaveEncryption(string key, string type, string value) 
    {
        int keyIndex = Random.Range(0, _keys.Length);
        string secretKey = _keys[keyIndex];
        string check = Sha256($"{key}_{type}_{privateKey}_{secretKey}_{value}");
        PlayerPrefs.SetString($"{key}@CheckSum", check);
        PlayerPrefs.SetInt($"{key}@Key", keyIndex);
    }

    private static bool CheckEncryption(string key, string type, string value) 
    {
        int keyIndex = PlayerPrefs.GetInt($"{key}@Key");
        string secretKey = _keys[keyIndex];
        string check = Sha256($"{key}_{type}_{privateKey}_{secretKey}_{value}");
        if (!PlayerPrefs.HasKey($"{key}@CheckSum")) 
            return false;
        string storedCheck = PlayerPrefs.GetString($"{key}@CheckSum");
        return storedCheck == check;
    }

    public static void SetKeys(string[] keys)
    {
        _keys = keys;
    }
   
    public static void SetInt(string key, int value)
    {
        string hashKey = Md5(key);
        PlayerPrefs.SetInt(hashKey, value);
        SaveEncryption(hashKey, "int", value.ToString());
    }
   
    public static void SetFloat(string key, float value) 
    {
        string hashKey = Md5(key);
        PlayerPrefs.SetFloat(hashKey, value);
        SaveEncryption(hashKey, "float", Mathf.Floor(value * 1000).ToString(CultureInfo.InvariantCulture));
    }
   
    public static void SetString(string key, string value) 
    {
        string hashKey = Md5(key);
        PlayerPrefs.SetString(hashKey, value);
        SaveEncryption(hashKey, "string", value);
    }

    public static int GetInt(string key, int defaultValue = 0) 
    {
        string hashKey = Md5(key);
        int value = PlayerPrefs.GetInt(hashKey);
        if (!CheckEncryption(hashKey, "int", value.ToString())) 
            return defaultValue;
        return value;
    }
   
    public static float GetFloat(string key, float defaultValue = 0f) 
    {
        string hashKey = Md5(key);
        float value = PlayerPrefs.GetFloat(hashKey);
        if (!CheckEncryption(hashKey, "float", Mathf.Floor(value * 1000).ToString(CultureInfo.InvariantCulture))) 
            return defaultValue;
        return value;
    }
   
    public static string GetString(string key, string defaultValue = "") 
    {
        string hashKey = Md5(key);
        string value = PlayerPrefs.GetString(hashKey);
        if (!CheckEncryption(hashKey, "string", value)) 
            return defaultValue;
        return value;
    }
   
    public static bool HasKey(string key) 
    {
        string hashKey = Md5(key);
        return PlayerPrefs.HasKey(hashKey);
    }
   
    public static void DeleteKey(string key) 
    {
        string hashKey = Md5(key);
        PlayerPrefs.DeleteKey(hashKey);
        PlayerPrefs.DeleteKey($"{hashKey}@CheckSum");
        PlayerPrefs.DeleteKey($"{hashKey}@Key");
    }
}