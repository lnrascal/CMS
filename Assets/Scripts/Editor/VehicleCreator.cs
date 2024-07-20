#if UNITY_EDITOR

using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets;
using DG.Tweening;
using EPOOutline;

public class VehicleCreator : OdinEditorWindow
{
    [MenuItem("Tools/Vehicle Creator")]
    public static void ShowWindow()
    {
        GetWindow<VehicleCreator>("Vehicle Creator");
    }

    public Catalog Catalog;
    [field: SerializeField] public string VehicleName { get; set; }
    public GameObject vehicleGo;

    //Needed to track same items spread to same nodes
    private Dictionary<string, int> meshNamesToItemId = new();

    [Button]
    public void CreateHierarchy()
    {
        //Creating Necessary Folders
        AssetDatabase.CreateFolder("Assets/Cars", VehicleName);
        AssetDatabase.CreateFolder("Assets/Cars/" + VehicleName, "DetailNodes");
        AssetDatabase.CreateFolder("Assets/Cars/" + VehicleName, "CatalogItems");
        AssetDatabase.CreateFolder("Assets/Cars/" + VehicleName, "Prefabs");

        foreach (ItemCategory ctg in Enum.GetValues(typeof(ItemCategory)))
        {
            AssetDatabase.CreateFolder("Assets/Cars/" + VehicleName + "/CatalogItems", ctg.ToString());
            AssetDatabase.CreateFolder("Assets/Cars/" + VehicleName + "/Prefabs", ctg.ToString());
        }

        VehicleStructure structure = VehicleStructure.Create(VehicleName);
        
        //Create Root Node
        structure.AddNode(-1, "Frame", Vector3.zero, Vector3.zero);

        //Create Vehicle Prefab
        GameObject frameGo = Instantiate(vehicleGo);
        DeleteChildren(frameGo.transform);
        MeshCollider mc = frameGo.AddComponent<MeshCollider>();
        mc.convex = true;
        Rigidbody rb = frameGo.AddComponent<Rigidbody>();
        rb.mass = 2000;
        rb.isKinematic = true;
        mc.sharedMesh = frameGo.GetComponent<MeshFilter>().sharedMesh;
        
        Vehicle vehicle = frameGo.AddComponent<Vehicle>();
        vehicle.SetStructure(structure);


        //Creating Details Prefab and Detail Ndoes
        Transform rootTransform = vehicleGo.transform;
        meshNamesToItemId = new();
        AddChildren(structure, rootTransform, 0);

        vehicle.PrefabSetup(frameGo.AddComponent<Frame>());

        string localPath = "Assets/Cars/" + VehicleName + "/" + VehicleName + ".prefab";
        localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);
        PrefabUtility.SaveAsPrefabAssetAndConnect(frameGo, localPath, InteractionMode.UserAction);

        AddPrefabToAddressables(localPath, VehicleName);
        
        EditorUtility.SetDirty(structure);
        EditorUtility.SetDirty(Catalog);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        DestroyImmediate(frameGo);
    }

    private void AddChildren(VehicleStructure vehicleStructure, Transform parentTransform, int parentKey)
    {
        foreach (Transform child in parentTransform)
        {
            //divide gameObject name to two parts: 1. Category of an Item 2. Node Name
            string[] splitStrings = child.name.Split('_');
            string meshName = child.GetComponent<MeshFilter>().sharedMesh.name;

            //Creating TreeNode
            int childNode = vehicleStructure.AddNode(
                parentKey,
                splitStrings[1],
                child.localPosition,
                child.localEulerAngles
            );

            //if there is already Item with the same mesh, new Item will not be created
            if (meshNamesToItemId.TryGetValue(meshName, out var itemId))
            {
                vehicleStructure.AddItemToNode(childNode, itemId);
            }
            else //New Item Scriptable and Prefab Creation
            {
                //Getting available item id
                int newItemId = Catalog.GetNewItemId();

                //Identifying Item Category

                ItemCategory category = IdentifyCategory(splitStrings[0]);

                //Creating copy of an object and destroying child objects
                GameObject copyGo = PrefabSetup(newItemId, meshName, category, child, out var localPath);

                //Add Item To Catalog
                CatalogItem catalogItem = CatalogItem.Create(newItemId, meshName, category, localPath, VehicleName);
                Catalog.AddNewItem(newItemId, catalogItem, VehicleName);

                vehicleStructure.AddItemToNode(childNode, newItemId);

                DestroyImmediate(copyGo);
            }


            AddChildren(vehicleStructure, child, childNode);
        }
    }

    #region Add Children Submethods

    private ItemCategory IdentifyCategory(string objectName)
    {
        ItemCategory category = ItemCategory.EngineBay;

        foreach (ItemCategory ctg in Enum.GetValues(typeof(ItemCategory)))
        {
            if (objectName.Equals(ctg.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                category = ctg;
                break;
            }
        }

        return category;
    }

    private GameObject PrefabSetup(int newItemId, string meshName, ItemCategory category, Transform child,
        out string localPath)
    {
        GameObject copyGo = Instantiate(child.gameObject);

        DeleteChildren(copyGo.transform);

        //Adding and setting necessary components
        copyGo.layer = LayerMask.NameToLayer("Detail");
        MeshCollider mc = copyGo.AddComponent<MeshCollider>();
        Rigidbody rb = copyGo.AddComponent<Rigidbody>();

        copyGo.AddComponent<Outlinable>();

        // copyGo.AddComponent<>
        mc.convex = true;
        Mesh mesh = child.GetComponent<MeshFilter>().sharedMesh;
        meshNamesToItemId.Add(mesh.name, newItemId);
        mc.sharedMesh = mesh;
        rb.isKinematic = false;

        ChildDetail cd = copyGo.AddComponent<ChildDetail>();
        cd.InstalledNode = -1;

        cd.SetItem(newItemId);
        cd.SetComponents();

        //Creating Prefab and adding Addressable
        localPath = "Assets/Cars/" + VehicleName + "/" + "Prefabs" + "/" + category.ToString() + "/" + meshName +
                    ".prefab";
        localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);
        PrefabUtility.SaveAsPrefabAssetAndConnect(copyGo, localPath, InteractionMode.UserAction);

        AddPrefabToAddressables(localPath, VehicleName);

        return copyGo;
    }

    private void DeleteChildren(Transform parent)
    {
        Transform[] children = new Transform[parent.childCount];
        for (int i = 0; i < parent.childCount; i++)
            children[i] = parent.GetChild(i);

        foreach (Transform childCopy in children)
            DestroyImmediate(childCopy.gameObject);
    }

    private void AddPrefabToAddressables(string prefabPath, string groupName)
    {
        AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;

        AddressableAssetGroup group = settings.FindGroup(groupName);
        if (group == null)
        {
            group = settings.CreateGroup(
                groupName,
                false,
                false,
                false,
                null,
                typeof(BundledAssetGroupSchema)
            );
        }

        string assetGuid = AssetDatabase.AssetPathToGUID(prefabPath);

        AddressableAssetEntry existingEntry = settings.FindAssetEntry(assetGuid);
        if (existingEntry == null)
        {
            var entry = settings.CreateOrMoveEntry(assetGuid, group, false, false);
            settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entry, true, true);

            Debug.Log("Prefab added to Addressables group: " + groupName);
        }
        else
        {
            Debug.Log("Prefab already exists in Addressables group: " + groupName);
        }
    }

    #endregion
}

#endif