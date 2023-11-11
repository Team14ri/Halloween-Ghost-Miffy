using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Item@Name", menuName = "Scriptable Objects/Item Data")]
public class ItemData : ScriptableObject
{
    [BoxGroup("Default Settings"), Title("Variable ID")]
    [SerializeField] private string id = "Item@Name";
    public string ID => id;

    [BoxGroup("Item Settings"), Title("Item Name")]
    [SerializeField] private new string name;
    public string Name => name;
    
    [BoxGroup("Item Settings"), Title("Item Description")]
    [SerializeField, TextArea(3, 10)] private string description;
    public string Description => description;
    
    [BoxGroup("Item Settings"), Title("Item Image")]
    [SerializeField] private Sprite sprite;
    public Sprite Sprite => sprite;
    
    [BoxGroup("Item Settings"), Title("Item Default Value")]
    [SerializeField] private int defaultValue;
    public int DefaultValue => defaultValue;

    public void SetValue(int value)
    {
        PlayerPrefs.SetInt(id, value);
    }
    
    public int GetValue()
    {
        return PlayerPrefs.GetInt(id, defaultValue);
    }
}