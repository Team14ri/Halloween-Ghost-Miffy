using System.Collections.Generic;
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
}
