using UnityEngine;
using UnityEngine.UI;

public class InventoryItemButton : MonoBehaviour
{
    [SerializeField] private InventoryViewer viewer;
    [SerializeField] private Image imageField;

    private ItemData _itemData;
    
    public void SetItem(ItemData itemData)
    {
        _itemData = itemData;

        imageField.sprite = itemData.Sprite;
    }
    
    public void ClickItem()
    {
        viewer.ShowItemDetail(_itemData);
    }
}
