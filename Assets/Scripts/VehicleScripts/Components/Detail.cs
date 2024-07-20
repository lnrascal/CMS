using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Detail : MonoBehaviour
{
    [field: SerializeField]
    public int ItemID { get; set; }
    
    public abstract int InstalledNode { get; set; }

    public virtual bool IsInstalled
    {
        get
        {
            if (InstalledNode == -1)
                return false;

            return true;
        }
    }

    public abstract bool IsUninstallable { get; }
    
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected Collider cld;
    
    
#if UNITY_EDITOR

    public virtual void SetComponents()
    {
        rb = GetComponent<Rigidbody>();
        cld = GetComponent<Collider>();
    }
#endif
    
    public virtual bool Remove()
    {
        if (!IsInstalled)
            return false;

        return true;
    }

    public virtual void SetInstalledNode(int installedNode)
    {
        InstalledNode = installedNode;
        rb.isKinematic = true;

        if (cld is MeshCollider)
            ((MeshCollider)cld).convex = false;
    }

    public void SetItem(int id)
    {
        ItemID = id;
    }

}
