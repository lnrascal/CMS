using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("Cameras")] [SerializeField] private Camera characterCamera;

    [SerializeField] private Camera itemCamera;

    [Header("Interacted Objects")]
    //object in front of the player view
    [SerializeField]
    private IInteractable targetInteractable;

    [SerializeField] private GameObject targetComponent;

    [SerializeField] private LayerMask hitLayers;

    [Header("Object Pickup")]
    [SerializeField] private Transform itemContainer;

    public GameObject InHandItem;
    

    void Awake()
    {
    }

    private Vector3 rayPosition;
    private Quaternion rayRotation;

    private Vector3 rayDirection;
    RaycastHit hit;
    private GameObject newtarget;

    void Update()
    {
        rayPosition = characterCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
        rayRotation = characterCamera.transform.rotation;
        rayDirection = rayRotation * Vector3.forward;

        if (Physics.Raycast(rayPosition, rayDirection, out hit, 3f, hitLayers))
        {
            //if target is the same do nothing
            newtarget = hit.collider.gameObject;
            if (!(targetComponent != null && newtarget == targetComponent))
            {
                //Unhighlight previous target
                Highlight(false);

                targetComponent = hit.collider.gameObject;
                targetInteractable = targetComponent.GetComponent<IInteractable>();
                Highlight(true);
            }
        }
        else
        {
            //Unhighlight previous target
            Highlight(false);
            targetComponent = null;
            targetInteractable = null;
        }
        
        
        //Interaction
        if (Input.GetKeyDown(KeyCode.E) && targetInteractable != null)
        {
            targetInteractable.Interact(this);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Drop();
        }
    }

    private void Highlight(bool toHighlight)
    {
        if (targetInteractable != null)
        {
            targetInteractable.Highlight(toHighlight);
        }
    }

    public void Pickup(GameObject pickupGo)
    {
        if(InHandItem != null)
            Drop();

        InHandItem = pickupGo;
        itemCamera.enabled = true;
        
        pickupGo.transform.SetParent(itemContainer);
        pickupGo.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(Vector3.zero));
    }

    public void Drop()
    {
        if (InHandItem == null)
            return;

        
        itemCamera.enabled = false;
        
        InHandItem.GetComponent<IInteractable>().Drop();
        InHandItem = null;
    }
}