using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryViewer : MonoBehaviour
{
    [BoxGroup("Inventory Right View Settings"), SerializeField]
    private Image imageField;
    [BoxGroup("Inventory Right View Settings"), SerializeField]
    private TMP_Text nameField;
    [BoxGroup("Inventory Right View Settings"), SerializeField]
    private TMP_Text descriptionField;

    [SerializeField] private List<InventoryItemButton> inventoryStorage;
    
    private void Init()
    {
        imageField.gameObject.SetActive(false);
        nameField.text = "";
        descriptionField.text = "";
        foreach (var storage in inventoryStorage)
        {
            storage.gameObject.SetActive(false);
        }
    }
    
    private void OnEnable()
    {
        Init();
        var itemData = VariableManager.Instance.GetItemsList()
            .Where(item => item.GetValue() > 0)
            .ToList();
        UpdateInventory(itemData);
    }
    
    private void UpdateInventory(List<ItemData> items)
    {
        for (int i = 0; i < items.Count; ++i)
        {
            inventoryStorage[i].gameObject.SetActive(true);
            inventoryStorage[i].SetItem(items[i]);
        }
    }

    public void ShowItemDetail(ItemData item)
    {
        imageField.gameObject.SetActive(true);
        imageField.sprite = item.Sprite;
        nameField.text = $"{item.Name} <size=30>x {item.GetValue()}</size>";
        descriptionField.text = item.Description;
    }
}
