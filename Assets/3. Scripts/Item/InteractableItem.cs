using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class InteractableItem : MonoBehaviour
{
    [SerializeField] private ItemData item;
    [SerializeField] private string guid = "";
    
    [Button("Generate New Guid")]
    private void GenerateNewGuid()
    {
        guid = Guid.NewGuid().ToString();
    }

    private void Start()
    {
        if (item == null || 
            guid.Equals("") || 
            EncryptedPlayerPrefs.GetInt($"Interactable@{guid}", 0) != 0)
        {
            gameObject.SetActive(false);
        }
    }
    
    public void AddItem()
    {
        if (item == null || 
            guid.Equals("") || 
            EncryptedPlayerPrefs.GetInt($"Interactable@{guid}", 0) != 0)    
            return;

        VariableManager.Instance.AddOneItem(item.ID);
        EncryptedPlayerPrefs.SetInt($"Interactable@{guid}", 1);
        
        gameObject.SetActive(false);
    }
}
