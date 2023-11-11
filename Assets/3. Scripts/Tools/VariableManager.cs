using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class VariableManager : MonoBehaviour
{
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

    public List<ItemData> GetItemsList()
    {
        return itemData;
    }
}
