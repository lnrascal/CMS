using System;
using System.Collections;
using System.Collections.Generic;
using EPOOutline;
using UnityEngine;

public class ChildDetail : Detail, IInteractable
{
    [field: SerializeField]
    public override int InstalledNode { get; set; }
    [SerializeField] protected Outlinable outlinable;

    public override bool IsInstalled
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

    public override bool IsUninstallable
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

    public override void SetComponents()
    {
        rb = GetComponent<Rigidbody>();
        cld = GetComponent<Collider>();
        outlinable = GetComponent<Outlinable>();
        outlinable.enabled = false;
    }
#endif

    public void Interact(PlayerInteract player)
    {
        PickUp(player);
    }

    public void Highlight(bool toHighlight)
    {
        outlinable.enabled = toHighlight;
    }

    public void PickUp(PlayerInteract player)
    {
        if (transform.childCount > 0)
            return;

        if (IsInstalled)
        {
            Vehicle vehicle = GetComponentInParent<Vehicle>();
            if(vehicle != null)
                vehicle.UninstallDetail(InstalledNode);
        }
        
        Garage.Instance.CreateHints(this);
        InstalledNode = -1;
        gameObject.layer = LayerMask.NameToLayer("InHandItem");
        rb.isKinematic = true;
        cld.enabled = false;
        
        player.Pickup(gameObject);
    }

    public void Drop()
    {
        gameObject.layer = LayerMask.NameToLayer("Detail");
        rb.isKinematic = false;
        cld.enabled = true;
        
        if (cld is MeshCollider)
        {
            ((MeshCollider)cld).convex = true;
            transform.SetParent(null);
        }
        
        Garage.Instance.RemoveHints();
    }
    
    public virtual HintComponent CreateHint()
    {
        GameObject copy = Instantiate(gameObject);

        ChildDetail copyDetailComponent = copy.GetComponent<ChildDetail>();
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