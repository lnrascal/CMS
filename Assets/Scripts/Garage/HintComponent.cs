using EPOOutline;
using UnityEngine;

public class HintComponent : MonoBehaviour, IInteractable
{
    //Needed For Car To Identify If Hint Should Be Created For The Item
    [field: SerializeField]  public int ItemId { get; set; }
    //Node To Which Detail Will Be Installed On Interaction With This Hint
    [field: SerializeField] protected int NodeKey { get; set; }
    
    [SerializeField]
    protected Vehicle vehicle;
    
    [SerializeField]
    protected Outlinable outline;
    
    //Setting Outlinable Parameters For Hint
    private static Outlinable.OutlineProperties hintDefaultOutline;
    private static Outlinable.OutlineProperties hintHoverOutline;

    public static void InitializeOutline(Outlinable.OutlineProperties defaultOutl, Outlinable.OutlineProperties hoverOutl)
    {
        hintDefaultOutline = defaultOutl;
        hintHoverOutline = hoverOutl;
    }
    
    public virtual void Interact(PlayerInteract player)
    {
        //Getting Detail From Player Hand
        ChildDetail detail = player.InHandItem.GetComponent<ChildDetail>();
        player.Drop();
        
        //Pass Detail Reference To A Vehicle To Install It
        vehicle.InstallDetail(detail, NodeKey);
    }

    public void Highlight(bool toHighlight)
    {
        if(toHighlight)
            outline.SetOutlineParameters(hintHoverOutline);
        else
            outline.SetOutlineParameters(hintDefaultOutline);
    }
    
    //Setting Parameters And Fields To Place Hint On Vehicle
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
