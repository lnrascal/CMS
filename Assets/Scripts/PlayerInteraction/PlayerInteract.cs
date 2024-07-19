using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("Cameras")]
    [SerializeField]
    private Camera characterCamera;
    
    [SerializeField]
    private Camera itemCamera;
    
    [Header("Interacted Objects")]
    //object in front of the player view
    [SerializeField] private IInteractable targetInteractable;
    [SerializeField] private GameObject targetComponent;

    [SerializeField] private LayerMask hitLayers;
    //picked up object
    [SerializeField] private GameObject inHanItem;
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
        
        if (Physics.Raycast(rayPosition, rayDirection, out hit, 2f, hitLayers))
        {
            
            //if target is the same do nothing
            newtarget = hit.collider.gameObject;
            if(targetComponent != null && newtarget == targetComponent)
                return;
            
            //Unhighlight previous target
            Highlight(false);
            
            targetComponent = hit.collider.gameObject;
            targetInteractable = targetComponent.GetComponent<IInteractable>();
            Highlight(true);
        }
        else
        {
            //Unhighlight previous target
            Highlight(false);
            targetComponent = null;
            targetInteractable = null;
        }
    }

    private void Highlight(bool toHighlight)
    {
        if (targetInteractable != null)
        {
            targetInteractable.Highlight(toHighlight);
        }
    }
    
    private void Pickup(GameObject pickupGo)
    {
        
    }

    private void Drop()
    {
        
    }
}
