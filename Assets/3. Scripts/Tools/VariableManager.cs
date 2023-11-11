using System;
using System.Collections.Generic;
using System.Linq;
using Quest;
using Sirenix.OdinInspector;
using UnityEngine;

public class VariableManager : MonoBehaviour
{
    [SerializeField] private bool resetVariablesOnStart;
    
    [TabGroup("Variables", "Stickers"), SerializeField]
    private List<StickerData> stickerData;
    
    [TabGroup("Variables", "Items"), SerializeField]
    private List<ItemData> itemData;
    
    public static VariableManager Instance;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (!resetVariablesOnStart)
            return;

        foreach (var data in stickerData)
        {
            PlayerPrefs.DeleteKey(data.ID);
        }
        foreach (var data in itemData)
        {
            PlayerPrefs.DeleteKey(data.ID);
        }
    }

    public List<StickerData> GetStickersList()
    {
        return stickerData;
    }

    public List<ItemData> GetItemsList()
    {
        return itemData;
    }

    public int GetItemValue(string id)
    {
        var firstOrDefault = itemData.FirstOrDefault(item => item.ID == id);
        if (firstOrDefault == null)
            return 0;
        return firstOrDefault.GetValue();
    }
    
    public int GetVariableValue(string id)
    {
        var sticker = stickerData.FirstOrDefault(obj => obj.ID == id);
        if (sticker != null)
        {
            return sticker.GetValue();
        }

        var item = itemData.FirstOrDefault(obj => obj.ID == id);
        if (item != null)
        {
            return item.GetValue();
        }

        return PlayerPrefs.GetInt(id, 0);
    }
    
    public void AddItems(string id, int value)
    {
        var sticker = stickerData.FirstOrDefault(obj => obj.ID == id);
        if (sticker != null)
        {
            sticker.SetValue(sticker.GetValue() + value);
            if (value > 0)
            {
                ItemCollect.Instance.ShowPopup($"(<u>{sticker.Name} <size=50><color=red>x 1</color></size></u>) 스티커 획득!", sticker.Sprite);
            }
            return;
        }

        var item = itemData.FirstOrDefault(obj => obj.ID == id);
        if (item != null)
        {
            item.SetValue(item.GetValue() + value);
            if (value > 0)
            {
                ItemCollect.Instance.ShowPopup($"(<u>{item.Name} <size=50><color=red>x 1</color></size></u>) 아이템 획득!", item.Sprite);
            }
            return;
        }
    }

    public void AddOneItem(string id)
    {
        var sticker = stickerData.FirstOrDefault(obj => obj.ID == id);
        if (sticker != null)
        {
            sticker.SetValue(sticker.GetValue() + 1);
            ItemCollect.Instance.ShowPopup($"(<u>{sticker.Name} <size=50><color=red>x 1</color></size></u>) 스티커 획득!", sticker.Sprite);
            return;
        }

        var item = itemData.FirstOrDefault(obj => obj.ID == id);
        if (item != null)
        {
            item.SetValue(item.GetValue() + 1);
            ItemCollect.Instance.ShowPopup($"(<u>{item.Name} <size=50><color=red>x 1</color></size></u>) 아이템 획득!", item.Sprite);
            return;
        }
        
        PlayerPrefs.SetInt(id, PlayerPrefs.GetInt(id, 0) + 1);
    }
}
