
using Unity.Netcode;
using UnityEngine;

public class PlayerPickup : NetworkBehaviour
{
    private PlayerControls playerControls;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform objectGrabPointTransform;

    [SerializeField] private LayerMask pickupLayer;
    [SerializeField] private float pickupRange = 2f;


    //public GameObject objectToPickup;

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

                EquipmentSO equipmentSO = interactableObject.equipmentSO;
                GameObject objectToPickup = equipmentSO.equipmentPrefab;

                if (equipmentSO == null)
                {
                    Debug.LogError("EquipmentSO is null");
                    return;
                }

                ulong networkObjectId = networkObject.NetworkObjectId;

                DespawnNetworkItemsServerRpc(networkObjectId);
                SpawnPlaceholderObject(objectToPickup);
            }
        }

    }

    public void SpawnPlaceholderObject(GameObject _objectToPickup)
    {
        Instantiate(_objectToPickup, objectGrabPointTransform.position, objectGrabPointTransform.rotation, objectGrabPointTransform);
    }

    [ServerRpc]
    public void DespawnNetworkItemsServerRpc(ulong networkObjectId)
    {
        NetworkObject networkObject = NetworkManager.SpawnManager.SpawnedObjects[networkObjectId];
        SpawnPlaceholderItemsClientRpc(networkObject);


        networkObject.Despawn();
    }

    [ClientRpc]
    public void SpawnPlaceholderItemsClientRpc(NetworkObjectReference networkObjectReference)
    {   
        Debug.Log("SpawnPlaceholderItemsClientRpc" + networkObjectReference);


        networkObjectReference.TryGet(out NetworkObject objectToSpawn);

        GameObject objectToSpawnGO = objectToSpawn.GetComponent<NetworkObject>().gameObject;
        GameObject objectToSpawnPrefab = objectToSpawnGO.GetComponent<ObjectGrabbable>().equipmentSO.equipmentPrefab;
        
        if (IsOwner) return;

        SpawnPlaceholderObject(objectToSpawnPrefab);
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
