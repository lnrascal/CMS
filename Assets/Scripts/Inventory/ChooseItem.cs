using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ChooseItem : MonoBehaviour
{
    [SerializeField] private int item;
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private Button button;
    
    public void SetItem(CatalogItem catalogItem, InventoryDisplay inventoryDisplay)
    {
        item = catalogItem.Id;
        itemName.text = catalogItem.ItemName;
        
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(delegate { inventoryDisplay.SpawnItem(item);});
    }
}
