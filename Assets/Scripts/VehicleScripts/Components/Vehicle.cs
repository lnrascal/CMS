using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Sirenix.OdinInspector;

public class Vehicle : MonoBehaviour
{
    [SerializeField] private VehicleStructure structure;

    //Tracks each installed detail, item order in list represents node key
    [SerializeField] private List<Installation> installations;

    void Start()
    {
        Garage.Instance.AddVehicle(this);
    }

    private void OnDestroy()
    {
        Garage.Instance.RemoveVehicle(this);
    }

    public List<GameObject> CreateHints(HintComponent hint)
    {
        List<GameObject> hints = new();
        List<int> nodes = structure.GetNodesByItem(hint.ItemId);

        foreach (int nodeKey in nodes)
        {
            //if given node has no details and its parent is installed
            DetailNode node = structure.GetNode(nodeKey);

            if (!installations[nodeKey].IsInstalled && installations[node.ParentKey].IsInstalled)
            {
                hints.Add(CreateHint(hint, nodeKey));
            }
        }

        return hints;
    }

    private GameObject CreateHint(HintComponent hint, int nodeKey)
    {
        hint.SetVehicleAndNode(this, nodeKey);

        GameObject newHint = Instantiate(hint.gameObject);
        Destroy(newHint.GetComponent<ChildDetail>());
        SetTransformToNode(newHint.transform, nodeKey);

        return newHint;
    }

    public void InstallDetail(ChildDetail detail, int nodeKey)
    {
        detail.SetInstalledNode(nodeKey);
        installations[nodeKey].Install(detail);
        SetTransformToNode(detail.transform, nodeKey);
    }

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