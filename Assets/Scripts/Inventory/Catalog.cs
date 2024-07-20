using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "CMS/Catalog")]
public class Catalog : SerializedScriptableObject
{
    [SerializeField] private Dictionary<int, CatalogItem> _catalogItems = new();

    [SerializeField] private Dictionary<string, HashSet<int>> itemsByVehicle = new();
    [SerializeField] private Dictionary<ItemCategory, HashSet<int>> itemsByCategory = new();

    void Awake()
    {
        
    }
    public CatalogItem GetItem(int id)
    {
        if (!_catalogItems.ContainsKey(id))
            throw new KeyNotFoundException("No Such Id In Catalog " + id);

        return _catalogItems[id];
    }

    public string[] GetVehicleNames()
    {
        string[] vehicleNames = itemsByVehicle.Keys.ToArray();
        
        return vehicleNames;
    }

    public List<int> Intersect(string vehicleName, ItemCategory itemCategory)
    {
        HashSet<int> vehicleSet = itemsByVehicle[vehicleName];
        HashSet<int> categorySet = itemsByCategory[itemCategory];

        return vehicleSet.Intersect(categorySet).ToList();
        
    }
    
    #if UNITY_EDITOR

    [Button(ButtonSizes.Large)]
    public void ClearCatalog()
    {
        _catalogItems = new();
        itemsByVehicle = new();
        itemsByCategory = new();
    }
    public void AddNewItem(int id, CatalogItem catalogItem, string vehicleName)
    {
        _catalogItems.Add(id, catalogItem);

        if (!itemsByVehicle.ContainsKey(vehicleName))
            itemsByVehicle.Add(vehicleName, new HashSet<int>());
        
        if (!itemsByCategory.ContainsKey(catalogItem.Category))
            itemsByCategory.Add(catalogItem.Category, new HashSet<int>());
        
        itemsByVehicle[vehicleName].Add(id);
        itemsByCategory[catalogItem.Category].Add(id);
    }
    public int GetNewItemId()
    {
        return _catalogItems.Count;
    }
    #endif
}
