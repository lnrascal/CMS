using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "CMS/Vehicle Structure")]
public class VehicleStructure : SerializedScriptableObject
{
    [field: SerializeField] public string VehicleName { get; private set; }

    [SerializeField] private List<DetailNode> nodes = new();

    [SerializeField] private Dictionary<int, List<int>> itemToNodes = new();

#if UNITY_EDITOR

    private void Init(string vehicleName)
    {
        VehicleName = vehicleName;
        nodes = new();
    }

    public int NodeCount()
    {
        return nodes.Count;
    }
    public int AddNode(int parentKey, string detailName, Vector3 position, Vector3 rotation)
    {
        int nodeKey = nodes.Count;
        DetailNode detailNode = DetailNode.Create(nodeKey, parentKey, detailName, VehicleName, position, rotation);
        nodes.Add(detailNode);

        if (parentKey >= 0)
            nodes[parentKey].AddChild(nodeKey);

        return nodeKey;
    }

    public void AddItemToNode(int nodeId, int itemId)
    {
        nodes[nodeId].AddItem(itemId);

        if (!itemToNodes.ContainsKey(itemId))
        {
            List<int> nodes = new();
            nodes.Add(nodeId);
            itemToNodes.Add(itemId, nodes);
        }
        else
        {
            itemToNodes[itemId].Add(nodeId);
        }
    }

    public static VehicleStructure Create(string vehicleName)
    {
        var vehicle = ScriptableObject.CreateInstance<VehicleStructure>();

        vehicle.Init(vehicleName);

        string path = "Assets/Cars/" + vehicleName + "/" + vehicleName + ".asset";
        
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

    public List<int> GetNodesByItem(int itemId)
    {
        List<int> nodesWithItem = new();
        if(itemToNodes.TryGetValue(itemId, out var node))
            nodesWithItem.AddRange(node);

        return nodesWithItem;
    }
}