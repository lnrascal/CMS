using TMPro;
using UnityEngine;
using UnityEngine.UI;

//UI Element That Will Represent Each Detail In Inventory
public class ChooseItem : MonoBehaviour
{
    [SerializeField] private int item;
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private Button button;
    
    //Setting Up Fields And Button Component
    public void SetItem(CatalogItem catalogItem, InventoryDisplay inventoryDisplay)
    {
        item = catalogItem.Id;
        itemName.text = catalogItem.ItemName;
        
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(delegate { inventoryDisplay.SpawnItem(item);});
    }
}
