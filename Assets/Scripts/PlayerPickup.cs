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


    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    /*void Start()
    {
        playerControls.Player.Interact.performed += ctx => OnPickup();
    }*/



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
                SpawnPlaceholderObject();
                NetworkObject networkObject = interactableObject.gameObject.GetComponent<NetworkObject>();

                ulong networkObjectId = networkObject.NetworkObjectId;
                ulong networkPlayerID = this.gameObject.GetComponent<NetworkObject>().NetworkObjectId;
                Debug.Log("Player networkobject: " + networkPlayerID);

                DespawnNetworkItemsServerRpc(networkObjectId);

            }
        }

    }

    private void SpawnPlaceholderObject()
    {
        Instantiate(objectToPickup, objectGrabPointTransform.position, objectGrabPointTransform.rotation, objectGrabPointTransform);
    }

    [ServerRpc]
    public void DespawnNetworkItemsServerRpc(ulong networkObjectId)
    {
        ulong networkPlayerID = NetworkManager.Singleton.LocalClientId;
        Debug.Log("Player networkobject: " + networkPlayerID);

        NetworkObject networkObject = NetworkManager.SpawnManager.SpawnedObjects[networkObjectId];
        networkObject.Despawn();

        SpawnPlaceholderItemsClientRpc(networkPlayerID);
    }

    [ClientRpc]
    public void SpawnPlaceholderItemsClientRpc(ulong networkPlayerID)
    {
        Transform playerHand = NetworkManager.Singleton.ConnectedClients[networkPlayerID].PlayerObject.transform.Find("ObjectGrabTransform");
        if (playerHand == null)
        {
            Debug.Log("Player hand not found");
        }

        GameObject visualItem = Instantiate(objectToPickup);

        visualItem.transform.parent = playerHand;

    }


    private void OnDrawGizmos()
    {
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
