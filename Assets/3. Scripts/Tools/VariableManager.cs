using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class VariableManager : MonoBehaviour
{
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
}
