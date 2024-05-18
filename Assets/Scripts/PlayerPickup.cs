using System;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    private PlayerControls playerControls;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform objectGrabPointTransform;
    [SerializeField] private LayerMask pickupLayer;
    [SerializeField] private float pickupRange = 2f;

    private GameObject currentObject;

    private void Awake() {
        playerControls = new PlayerControls();
    }
    
    void Start() 
    {
        playerControls.Player.Interact.performed += ctx => OnPickup();
        playerControls.Player.Drop.performed += ctx => OnDrop();
    }


    private void OnPickup() 
    {

        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit raycastHit, pickupRange, pickupLayer))
        {
            if(raycastHit.transform.TryGetComponent(out IInteractable interactableObject))
            {
                currentObject = raycastHit.transform.gameObject;
                interactableObject.StartInteraction(objectGrabPointTransform);
            }
        }
        
    }

    private void OnDrop()
    {
        currentObject.GetComponent<IInteractable>().EndInteraction();
        currentObject = null;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, pickupRange);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(cameraTransform.position, cameraTransform.forward * pickupRange);
        
    }

    private void OnEnable() 
    {
        playerControls.Enable();
    }

    private void OnDisable() 
    {
        playerControls.Disable();
    }
    
}
