using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

//Script That Controls UI Of An Inventory
public class InventoryDisplay : MonoBehaviour
{
    //Reference To A Catalog Scriptable
    [SerializeField] private Catalog catalog;
    
    //Reference To A PlayerScript To Spawn Items In Hand Of A Player
    [SerializeField] private PlayerInteract player;
    
    //Tracks Current Vehicle And Category To Display Items For Both
    [SerializeField] private string currentVehicle;
    [SerializeField] private ItemCategory itemCategory;
    
    //Prefab For Creating Button For Each Vehicle
    [SerializeField] private ChooseVehicle vehicleButtonPrefab;
    [SerializeField] private Transform vehicleButtonContainer;
    
    //Pool Of Item Objects, Used For Decreasing The Amount Of Instantiate And Destroy Calls
    [SerializeField] private List<ChooseItem> itemButtonPool = new();
    [SerializeField] private CanvasManager canvasManager;
    void Start()
    {
        //Creaing Buttons For Each Vehicle And Choosing First Of Them By Default
        string[] vehicleNames = catalog.GetVehicleNames();
        
        foreach (string vehicleName in vehicleNames)
        {
            ChooseVehicle vehicleButton = Instantiate(vehicleButtonPrefab, vehicleButtonContainer);
            vehicleButton.SetBUtton(this, vehicleName);
            
        }
        
        SetVehicle(vehicleNames[0]);
    }
    
    //Set Vehicle Chosen By Player
    public void SetVehicle(string vehicle)
    {
        if (vehicle == currentVehicle)
            return;

        currentVehicle = vehicle;
        
        RefreshInventory();
    }
    
    //Set Category Chosen By Player
    public void SetCategory(string categoryString)
    {
        ItemCategory category = (ItemCategory) Enum.Parse(typeof(ItemCategory), categoryString, true);
        if (category == itemCategory)
            return;

        itemCategory = category;
        
        RefreshInventory();
    }
    
    public void SpawnItem(int itemId)
    {
        SpawnDetail(catalog.GetItem(itemId).AddressablePath);
        //Close VehicleStore UI
        canvasManager.OpenInventory(false);
    }
    
    //Asynchronous Object Instantiation
    public void SpawnDetail(string address)
    {
        Addressables.InstantiateAsync(address).Completed += OnDetailInstantiated;
    }
    
    //After Instantiating Object Make Player To Pick It Up
    private void OnDetailInstantiated(AsyncOperationHandle<GameObject> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject detail = obj.Result;
            detail.GetComponent<ChildDetail>().Interact(player);
        }
        else
        {
            Debug.LogError("Failed to instantiate detail prefab");
        }
    }
    
    //Display New Items After Another Vehicle Or Category Is Chosen
    public void RefreshInventory()
    {
        List<int> intersect = catalog.Intersect(currentVehicle, itemCategory);

        int i;
        
        for (i = 0; i < intersect.Count; i++)
        {
            itemButtonPool[i].gameObject.SetActive(true);
            itemButtonPool[i].SetItem(catalog.GetItem(intersect[i]), this);
        }

        for (; i < itemButtonPool.Count; i++)
            itemButtonPool[i].gameObject.SetActive(false);
    }
}
