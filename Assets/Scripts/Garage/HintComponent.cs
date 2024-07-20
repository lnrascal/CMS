using System.Collections;
using System.Collections.Generic;
using EPOOutline;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class HintComponent : MonoBehaviour, IInteractable
{
    [field: SerializeField]
    public int ItemId { get; set; }
    [field: SerializeField]
    private int NodeKey { get; set; }
    
    [SerializeField]
    private Vehicle vehicle;
    
    [SerializeField]
    private Outlinable outline;
    private static Outlinable.OutlineProperties hintDefaultOutline;
    private static Outlinable.OutlineProperties hintHoverOutline;

    public static void InitializeOutline(Outlinable.OutlineProperties defaultOutl, Outlinable.OutlineProperties hoverOutl)
    {
        hintDefaultOutline = defaultOutl;
        hintHoverOutline = hoverOutl;
    }
    public void Interact(PlayerInteract player)
    {
        ChildDetail detail = player.InHandItem.GetComponent<ChildDetail>();
        player.Drop();
        vehicle.InstallDetail(detail, NodeKey);
    }

    public void Highlight(bool toHighlight)
    {
        if(toHighlight)
            outline.SetOutlineParameters(hintHoverOutline);
        else
            outline.SetOutlineParameters(hintDefaultOutline);
    }

    public void SetFields(int itemId, Outlinable outl)
    {
        ItemId = itemId;
        outline = outl;
        gameObject.layer = LayerMask.NameToLayer("Detail");
        this.GetComponent<Rigidbody>().isKinematic = true;

        Collider cld = this.GetComponent<Collider>();
        if (cld != null)
        {
            cld.enabled = true;
            if (cld is MeshCollider)
            {
                ((MeshCollider)cld).convex = false;
            }
        }
        
        Highlight(false);
    }

    public void SetVehicleAndNode(Vehicle vhcl, int nodeKey)
    {
        vehicle = vhcl;
        NodeKey = nodeKey;
    }
    public void Drop()
    {
        return;
    }
}
