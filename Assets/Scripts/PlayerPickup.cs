using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerPickup : NetworkBehaviour
{
    private PlayerControls playerControls;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform objectGrabPointTransform;
    [SerializeField] private GameObject objectToPickup;
    [SerializeField] private LayerMask pickupLayer;
    [SerializeField] private float pickupRange = 2f;




    public override void OnNetworkSpawn()
    {
        playerControls.Player.Interact.performed += ctx => OnPickup();
    }

    public override void OnNetworkDespawn()
    {
        playerControls.Player.Interact.performed -= ctx => OnPickup();
    }

    private void OnPickup()
    {
        if (!IsOwner) return;

        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit raycastHit, pickupRange, pickupLayer))
        {
            if (raycastHit.transform.TryGetComponent(out ObjectGrabbable interactableObject))
            {
                NetworkObject networkObject = interactableObject.gameObject.GetComponent<NetworkObject>();

                ulong networkObjectId = networkObject.NetworkObjectId;

                DespawnNetworkItemsServerRpc(networkObjectId);
                SpawnPlaceholderObject();
            }
        }

    }

    public void SpawnPlaceholderObject()
    {
        Instantiate(objectToPickup, objectGrabPointTransform.position, objectGrabPointTransform.rotation, objectGrabPointTransform);
    }

    [ServerRpc]
    public void DespawnNetworkItemsServerRpc(ulong networkObjectId)
    {
        NetworkObject networkObject = NetworkManager.SpawnManager.SpawnedObjects[networkObjectId];
        networkObject.Despawn();

        SpawnPlaceholderItemsClientRpc();
    }

    [ClientRpc]
    public void SpawnPlaceholderItemsClientRpc()
    {
        if (IsOwner) return;

        SpawnPlaceholderObject();
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, pickupRange);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(cameraTransform.position, cameraTransform.forward * pickupRange);

    }


    private void Awake()
    {
        playerControls = new PlayerControls();
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
