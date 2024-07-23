using UnityEngine;

//base class for details
public abstract class Detail : MonoBehaviour
{
    [field: SerializeField]
    public int ItemID { get; set; }
    
    public abstract int InstalledNode { get; set; }
    
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected Collider cld;

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
