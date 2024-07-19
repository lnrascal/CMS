using System.Collections;
using System.Collections.Generic;
using EPOOutline;
using UnityEngine;

public class ChildDetail : Detail, IInteractable
{
    public override int InstalledNode { get; set; }
    [SerializeField] protected Outlinable outlinable;

    public override bool IsInstalled
    {
        get
        {
            if (transform.childCount > 0 || transform.GetComponentInParent<Vehicle>() == null)
            {
                Debug.Log("Detail is not installed to vehicle OR Child Details should be removed first");
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

    public void Interact(GameObject player)
    {
        throw new System.NotImplementedException();
    }

    public void Highlight(bool toHighlight)
    {
        outlinable.enabled = toHighlight;
    }

    public void PickUp(GameObject player)
    {
        InstalledNode = -1;
        gameObject.layer = LayerMask.NameToLayer("InHandItem");
    }

    public void Drop()
    {
        gameObject.layer = LayerMask.NameToLayer("Detail");
        rb.isKinematic = false;
        if (cld is MeshCollider)
        {
            ((MeshCollider)cld).convex = true;
            transform.SetParent(null);
        }
    }
}