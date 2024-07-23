using System.Collections.Generic;
using UnityEngine;


public class Vehicle : MonoBehaviour
{
    //Reference To VehicleStructure, To Identify What Type Of Car It Is
    [SerializeField] private VehicleStructure structure;

    //Tracks each installed detail, item order in list represents node key
    [SerializeField] private List<Installation> installations;

    void Start()
    {
        //Add To Garage's Vehicle List
        Garage.Instance.AddVehicle(this);
    }

    private void OnDestroy()
    {
        //Remove From Garage's Vehicle List
        Garage.Instance.RemoveVehicle(this);
    }
    
    //Creting Copy Of Hint For Each Node On Which Detail's Hint Can Be Installed
    public List<GameObject> CreateHints(HintComponent hint)
    {
        List<GameObject> hints = new();
        List<int> nodes = structure.GetNodesByItem(hint.ItemId);
        
        foreach (int nodeKey in nodes)
        {
            //if given node has no details and its parent is installed
            DetailNode node = structure.GetNode(nodeKey);
            
            //Create Hint If Detail Is NOT Already Installed AND its Parent Already IS
            if (!installations[nodeKey].IsInstalled && installations[node.ParentKey].IsInstalled)
                hints.Add(CreateHint(hint, nodeKey));
        }

        return hints;
    }
    
    //Creating Hint For A Particular Node
    private GameObject CreateHint(HintComponent hint, int nodeKey)
    {
        hint.SetVehicleAndNode(this, nodeKey);

        GameObject newHint = Instantiate(hint.gameObject);
        Destroy(newHint.GetComponent<ChildDetail>());
        SetTransformToNode(newHint.transform, nodeKey);

        return newHint;
    }
    
    //Set Detail As INSTALLED In Installations
    public void InstallDetail(ChildDetail detail, int nodeKey)
    {
        detail.SetInstalledNode(nodeKey);
        installations[nodeKey].Install(detail);
        SetTransformToNode(detail.transform, nodeKey);
    }
    
    //Set Detail As UNINSTALLED In Installations
    public void UninstallDetail(int nodeKey)
    {
        Installation installation = installations[nodeKey];
        if (installation.IsInstalled)
        {
            installation.Uninstall();
        }
    }
    
    private void SetTransformToNode(Transform detailTransform, int nodeKey)
    {
        DetailNode node = structure.GetNode(nodeKey);
        Transform parentTransform = installations[node.ParentKey].Detail.transform;

        detailTransform.SetParent(parentTransform);
        detailTransform.SetLocalPositionAndRotation(node.Position, Quaternion.Euler(node.Rotation));
    }
#if UNITY_EDITOR
    //Used In VehicleCreator Script To Create And Set Vehicle Prefab
    public void PrefabSetup(Frame frame)
    {
        installations = new(new Installation[structure.NodeCount()]);
        for (int i = 0; i < installations.Count; i++)
        {
            installations[0] = new Installation();
        }

        installations[0].Install(this.GetComponent<Frame>());
    }

    public void SetStructure(VehicleStructure vehicleStructure)
    {
        structure = vehicleStructure;
    }
#endif
}