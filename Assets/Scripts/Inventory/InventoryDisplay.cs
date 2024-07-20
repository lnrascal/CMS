using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class InventoryDisplay : MonoBehaviour
{
    [SerializeField] private Catalog catalog;
    [SerializeField] private PlayerInteract player;
    
    [SerializeField] private string currentVehicle;
    [SerializeField] private ItemCategory itemCategory;

    [SerializeField] private ChooseVehicle vehicleButtonPrefab;
    [SerializeField] private Transform vehicleButtonContainer;

    [SerializeField] private List<ChooseItem> itemButtonPool = new();
    [SerializeField] private CanvasManager canvasManager;
    void Start()
    {
        string[] vehicleNames = catalog.GetVehicleNames();
        
        foreach (string vehicleName in vehicleNames)
        {
            ChooseVehicle vehicleButton = Instantiate(vehicleButtonPrefab, vehicleButtonContainer);
            vehicleButton.SetBUtton(this, vehicleName);
            
        }
        
        SetVehicle(vehicleNames[0]);
    }
    
    public void SetVehicle(string vehicle)
    {
        if (vehicle == currentVehicle)
            return;

        currentVehicle = vehicle;
        
        RefreshInventory();
    }

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
        canvasManager.OpenInventory(false);
    }
    
    public void SpawnDetail(string address)
    {
        Addressables.InstantiateAsync(address).Completed += OnDetailInstantiated;
    }

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
