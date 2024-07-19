using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "CMS/Catalog")]
public class Catalog : SerializedScriptableObject
{
    [field: SerializeField] private Dictionary<int, CatalogItem> _catalogItems = new();

    [field: SerializeField] private Dictionary<string, HashSet<int>> itemsByVehicle = new();
    [field: SerializeField] private Dictionary<ItemCategory, HashSet<int>> itemsByCategory = new();
    
    
    public CatalogItem GetItem(int id)
    {
        if (!_catalogItems.ContainsKey(id))
            throw new KeyNotFoundException("No Such Id In Catalog " + id);

        return _catalogItems[id];
    }
    
    #if UNITY_EDITOR

    public void AddNewItem(int id, CatalogItem catalogItem)
    {
        _catalogItems.Add(id, catalogItem);
        
    }
    public int GetNewItemId()
    {
        return _catalogItems.Count;
    }
    #endif
}
