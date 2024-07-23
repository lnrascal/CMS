using System.Collections.Generic;
using EPOOutline;
using UnityEngine;

public class Garage : MonoBehaviour
{
    //Singleton
    public static Garage Instance;
    
    //Tracks vehicles to create hints for each car
    [SerializeField] private List<Vehicle> vehicles = new();
    
    //Tracks each hint to destroy them when needed
    [SerializeField] private List<GameObject> hints = new();
    
    //Outline Parameters For Hints To Set Them As Static Field
    [SerializeField] private Outlinable.OutlineProperties hintDefaultOutline;
    [SerializeField] private Outlinable.OutlineProperties hintHoverOutline;
    
    private void Awake()
    {
        Instance = this;
        HintComponent.InitializeOutline(hintDefaultOutline, hintHoverOutline);
    }

    public void AddVehicle(Vehicle vehicle)
    {
        vehicles.Add(vehicle);
    }

    public void RemoveVehicle(Vehicle vehicle)
    {
        vehicles.Remove(vehicle);
    }
    public void CreateHints(ChildDetail detail)
    {
        HintComponent hint = detail.CreateHint();
        
        //Creating hints by iterating through each car
        foreach (Vehicle vehicle in vehicles)
        {
            hints.AddRange(vehicle.CreateHints(hint));
        }
        
        Destroy(hint.gameObject);
    }
    
    public void RemoveHints()
    {
        foreach (GameObject hint in hints)
        {
            Destroy(hint);
        }

        hints = new();
    }
}
