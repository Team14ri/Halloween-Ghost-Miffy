using UnityEngine;
using UnityEngine.UI;

public class StickerItemButton : MonoBehaviour
{
    [SerializeField] private Image imageField;

    private StickerData _itemData;
    
    public void SetItem(StickerData itemData)
    {
        _itemData = itemData;

        imageField.sprite = itemData.Sprite;
    }
    
    public void ClickItem()
    {
        
    }
}
