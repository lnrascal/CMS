using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEditor;

[Serializable]
public class DetailNode: SerializedScriptableObject
{
    
    [field: SerializeField]
    public int DetailKey { get; private set; }
    [field: SerializeField]
    public int ParentKey { get; private set; }
    
    [field:SerializeField]
    public List<int> Children { get; private set; } = new();
    [field:SerializeField]
    public HashSet<int> Items { get; private set; } = new();
    
    [field: SerializeField] public Vector3 Position { get; set; }

    [field: SerializeField] public Vector3 Rotation { get; set; }

    private void Init(int detailKey, int parentKey, Vector3 position, Vector3 rotation)
    {
        DetailKey = detailKey;
        ParentKey = parentKey;
        Children = new();
        
        Items = new();
        Position = position;
        Rotation = rotation;
    }

    public static DetailNode Create(int detailKey, int parentKey, string detailName, string vehicleName, Vector3 position, Vector3 rotation)
    {
        var detaiLNode = ScriptableObject.CreateInstance<DetailNode>();

        detaiLNode.Init(detailKey, parentKey, position, rotation);
        
        string path = "Assets/Cars/" + vehicleName+ "/DetailNodes/" + detailName + ".asset";

        AssetDatabase.CreateAsset(detaiLNode, path);
        AssetDatabase.SaveAssets();
        
        AssetDatabase.Refresh();

        return detaiLNode;
    }

    public void AddChild(int childKey)
    {
        if(!Children.Contains(childKey))
            Children.Add(childKey);
    }

    public void AddItem(int id)
    {
        Items.Add(id);
    }
    public bool ContainsItem(int id)
    {
        if (Items.Contains(id))
            return true;

        return false;
    }
}
