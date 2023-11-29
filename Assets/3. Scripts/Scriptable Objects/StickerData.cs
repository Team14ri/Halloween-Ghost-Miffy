using Quest;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public enum StickerType
{
    Normal,
    Big
}

[System.Serializable]
public enum StickerChapter
{
    Ch1,
    Ch2
}

[System.Serializable]
[CreateAssetMenu(fileName = "Sticker@Name", menuName = "Scriptable Objects/Sticker Data")]
public class StickerData : ScriptableObject
{
    [BoxGroup("Default Settings"), Title("Variable ID")]
    [SerializeField] private string id = "Sticker@Name";
    public string ID => id;

    [BoxGroup("Sticker Settings"), Title("Sticker Name")]
    [SerializeField] private new string name;
    public string Name => name;
    
    [BoxGroup("Sticker Settings"), Title("Sticker Type")]
    [SerializeField] private StickerType type;
    public StickerType Type => type;
    
    [BoxGroup("Sticker Settings"), Title("Sticker Chapter")]
    [SerializeField] private StickerChapter chapter;
    public StickerChapter Chapter => chapter;

    [BoxGroup("Sticker Settings"), Title("Sticker Description")]
    [SerializeField, TextArea(3, 10)] private string description;
    public string Description => description;
    
    [BoxGroup("Sticker Settings"), Title("Sticker Image")]
    [SerializeField] private Sprite sprite;
    public Sprite Sprite => sprite;
    
    [BoxGroup("Sticker Settings"), Title("Sticker Default Value")]
    [SerializeField] private int defaultValue;
    public int DefaultValue => defaultValue;

    public void SetValue(int value)
    {
        EncryptedPlayerPrefs.SetInt(id, value);
    }
    
    public int GetValue()
    {
        return EncryptedPlayerPrefs.GetInt(id, defaultValue);
    }
}