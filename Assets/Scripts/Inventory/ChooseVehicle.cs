using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ChooseVehicle : MonoBehaviour
{
    [SerializeField] private Button button;
    [field:SerializeField] public string VehicleName { get; set; }
    public TMP_Text title;
    
    public void SetBUtton(InventoryDisplay inventoryDisplay, string vehicle)
    {
        title.text = vehicle;
        VehicleName = vehicle;
        
        button.onClick.AddListener(delegate {inventoryDisplay.SetVehicle(VehicleName);});
    }
}
