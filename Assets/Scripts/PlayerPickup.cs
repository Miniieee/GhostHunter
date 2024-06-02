
using Unity.Netcode;
using UnityEngine;

public class PlayerPickup : NetworkBehaviour
{
    private PlayerControls playerControls;
    
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform objectGrabPointTransform;
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
                NetworkObject pickedupNetworkObject = interactableObject.gameObject.GetComponent<NetworkObject>();

                EquipmentSO pickedUpEquipmentSO = interactableObject.equipmentSO;
                GameObject objectToPickup = pickedUpEquipmentSO.equipmentPrefab;

                if (pickedUpEquipmentSO == null)
                {
                    Debug.LogError("EquipmentSO is null");
                    return;
                }

                ulong pickedUpObjectNetworkID = pickedupNetworkObject.NetworkObjectId;

                SpawnPlaceholderObjectServerRpc(pickedUpObjectNetworkID);
                SpawnPlaceholderObject(objectToPickup);
            }
        }

    }

    public void SpawnPlaceholderObject(GameObject _objectToPickup)
    {
        Instantiate(_objectToPickup, objectGrabPointTransform.position, objectGrabPointTransform.rotation, objectGrabPointTransform);
    }

    [ServerRpc]
    public void SpawnPlaceholderObjectServerRpc(ulong pickedUpNetworkObjectId)
    {
        NetworkObject pickedUpNetworkObjectOnServer = NetworkManager.SpawnManager.SpawnedObjects[pickedUpNetworkObjectId];
        SpawnPlaceholderObjectClientRpc(pickedUpNetworkObjectOnServer);

        //despawn at last to have a reference in ClientRpc
        pickedUpNetworkObjectOnServer.Despawn();
    }

    [ClientRpc]
    public void SpawnPlaceholderObjectClientRpc(NetworkObjectReference PickedUpNetworkObjectReference)
    {   
        if (IsOwner) return;
        
        PickedUpNetworkObjectReference.TryGet(out NetworkObject objectToSpawnNetworkObject);

        GameObject objectToSpawnGameObject = objectToSpawnNetworkObject.GetComponent<NetworkObject>().gameObject;
        GameObject objectToSpawnPrefabSO = objectToSpawnGameObject.GetComponent<ObjectGrabbable>().equipmentSO.equipmentPrefab;

        SpawnPlaceholderObject(objectToSpawnPrefabSO);
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
