using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CatalogItem : SerializedScriptableObject
{
    [field: SerializeField] public int Id { get; set; }

    [field: SerializeField] public string ItemName { get; set; }

    public ItemCategory Category { get; set; }

    [field: SerializeField] public string AddressablePath { get; set; }

    public static CatalogItem Create(int id, string itemName, ItemCategory category, string addressablePath, string vehicleName)
    {
        var catalogItem = ScriptableObject.CreateInstance<CatalogItem>();

        catalogItem.Init(id, itemName, category, addressablePath);
        
        string path = "Assets/Cars/" + vehicleName+ "/CatalogItems/" + category + "/" + itemName + ".asset";

        AssetDatabase.CreateAsset(catalogItem, path);
        AssetDatabase.SaveAssets();
        
        AssetDatabase.Refresh();

        return catalogItem;
    }

    private void Init(int id, string itemName, ItemCategory category, string addressablePath)
    {
        Id = id;
        ItemName = itemName;
        Category = category;
        AddressablePath = addressablePath;
    }
    
}

public enum ItemCategory
{
    EngineBay,
    Transmission,
    Exhaust,
    Exterior,
    Interior,
    Wheel
}