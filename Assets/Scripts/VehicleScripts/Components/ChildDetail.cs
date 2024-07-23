using EPOOutline;
using UnityEngine;

//Detail Subclass That Can Be Installed To Other Detail (i.e. almost any other Detail except Frame)
public class ChildDetail : Detail, IInteractable
{
    //If Detail Is Installed To Vehicle InstalledNode = -1, otherwise it contains Id Of A Node It Is Installed To
    [field: SerializeField] public override int InstalledNode { get; set; }
    [SerializeField] protected Outlinable outlinable;
    
    //Needed To Identify Whether Detail Should Be Uninstalled From Vehicle On PickUp Or Not
    public bool IsInstalled
    {
        get
        {
            if (InstalledNode == -1) 
            {
                return false;
            }

            return true;
        }
    }
    
    //Condition On Which Detail CAN Be Uninstalled
    public bool IsUninstallable
    {
        get
        {
            if (transform.childCount > 0 || transform.GetComponentInParent<Vehicle>() == null)
            {
                return false;
            }

            return true;
        }
    }

#if UNITY_EDITOR
    //Used In VehicleCreator To SetUp Prefab
    public void SetComponents()
    {
        rb = GetComponent<Rigidbody>();
        cld = GetComponent<Collider>();
        outlinable = GetComponent<Outlinable>();
        outlinable.enabled = false;
    }
#endif
    
    //PickUp Detail On Interaction
    public void Interact(PlayerInteract player)
    {
        PickUp(player);
    }

    public void Highlight(bool toHighlight)
    {
        outlinable.enabled = toHighlight;
    }

    public virtual void PickUp(PlayerInteract player)
    {
        if (transform.childCount > 0)
            return;
        
        //Uninstall Detail First
        if (IsInstalled)
        {
            Vehicle vehicle = GetComponentInParent<Vehicle>();
            if(vehicle != null)
                vehicle.UninstallDetail(InstalledNode);
        }
        
        //Try To Create Hints For Picked Up Object
        Garage.Instance.CreateHints(this);
        InstalledNode = -1;
        
        //Setting Layer To InHandItem So That It Is Always Seen Even If It Collides With Other Objects While In Hand
        gameObject.layer = LayerMask.NameToLayer("InHandItem");
        rb.isKinematic = true;
        
        player.Pickup(gameObject);
    }

    public void Drop()
    {
        gameObject.layer = LayerMask.NameToLayer("Detail");
        transform.SetParent(null);
        rb.isKinematic = false;
        
        Garage.Instance.RemoveHints();
    }
    
    //Create Hint For This Detail
    public HintComponent CreateHint()
    {
        GameObject copy = Instantiate(gameObject);

        ChildDetail copyDetailComponent = copy.GetComponent<ChildDetail>();
        MeshRenderer mr = copy.GetComponent<MeshRenderer>();
        mr.sharedMaterials = new Material[] { };
        Outlinable outline = copyDetailComponent.GetOutline();
        outline.enabled = true;
        HintComponent hint = copy.AddComponent<HintComponent>();
        hint.SetFields(copyDetailComponent.ItemID, outline);
        
        Destroy(copyDetailComponent);

        return hint;
    }

    public Outlinable GetOutline()
    {
        return outlinable;
    }
}