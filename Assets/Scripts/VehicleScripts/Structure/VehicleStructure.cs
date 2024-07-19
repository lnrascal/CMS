using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "CMS/Vehicle Structure")]
public class VehicleStructure: SerializedScriptableObject
{
    [field:SerializeField]
    public string VehicleName { get; private set; }
    [SerializeField]
    private List<DetailNode> nodes = new();
    
#if UNITY_EDITOR
    
    private void Init(string vehicleName)
    {
        VehicleName = vehicleName;
        nodes = new();
    }

    public int AddNode(int parentKey, string detailName, Vector3 position, Vector3 rotation)
    {
        int nodeKey = nodes.Count;
        DetailNode detailNode = DetailNode.Create(nodeKey, parentKey, detailName, VehicleName, position, rotation);
        nodes.Add(detailNode);
        
        nodes[parentKey].AddChild(nodeKey);
        
        return nodeKey;
    }

    public static VehicleStructure Create(string vehicleName)
    {
        var vehicle = ScriptableObject.CreateInstance<VehicleStructure>();

        vehicle.Init(vehicleName);

        string path = "Assets/Cars/" + vehicleName + ".asset";

        AssetDatabase.CreateAsset(vehicle, path);
        AssetDatabase.SaveAssets();

        AssetDatabase.Refresh();

        return vehicle;
    }
    #endif

    public DetailNode GetNode(int key)
    {
        if (key < 0 || key >= nodes.Count)
        {
            throw new ArgumentOutOfRangeException("No such node");
        }
        
        return nodes[key];
    }

    public bool IsItemInstallable(int nodeKey, int itemId)
    {
        DetailNode detailNode = GetNode(nodeKey);

        return detailNode.ContainsItem(itemId);
    }
    
    
}
