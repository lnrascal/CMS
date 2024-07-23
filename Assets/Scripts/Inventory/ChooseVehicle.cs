using UnityEngine;
using UnityEngine.UI;
using TMPro;

//UI Element That Represents Button In Inventory Through Which Player Will Only Get Details For A Particular Vehicle
public class ChooseVehicle : MonoBehaviour
{
    [SerializeField] private Button button;
    [field:SerializeField] public string VehicleName { get; set; }
    public TMP_Text title;
    
    //Setting Up Fields And Button
    public void SetBUtton(InventoryDisplay inventoryDisplay, string vehicle)
    {
        title.text = vehicle;
        VehicleName = vehicle;
        
        button.onClick.AddListener(delegate {inventoryDisplay.SetVehicle(VehicleName);});
    }
}
