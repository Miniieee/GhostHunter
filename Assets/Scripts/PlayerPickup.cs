
using Unity.Netcode;
using UnityEngine;

public class PlayerPickup : NetworkBehaviour
{
    private PlayerControls playerControls;
    private HandEquipmentInventory handEquipmentInventory;
    private int selectedEquipmentIndex;

    private GameObject pickedUpObject;
    GameObject spawnedObject;

    [SerializeField] private int maxNumberOfEquipments = 3;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform objectGrabPointTransform;
    [SerializeField] private LayerMask pickupLayer;
    [SerializeField] private float pickupRange = 2f;


    public override void OnNetworkSpawn()
    {
        playerControls.Player.Interact.performed += ctx => OnPickup();
        playerControls.Player.Drop.performed += ctx => OnDrop();

        selectedEquipmentIndex = 0;
    }

    public override void OnNetworkDespawn()
    {
        playerControls.Player.Interact.performed -= ctx => OnPickup();
        playerControls.Player.Drop.performed -= ctx => OnDrop();
    }


    private void OnPickup()
    {
        if (!IsOwner) return;
        if (IsInventoryFull()) return;

        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit raycastHit, pickupRange, pickupLayer))
        {
            if (raycastHit.transform.TryGetComponent(out ObjectGrabbable interactableObject))
            {
                Debug.Log("Pickup");

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
                AddToHandInventory();
            }
        }

    }

    public void SpawnPlaceholderObject(GameObject _objectToPickup)
    {
        pickedUpObject = Instantiate(_objectToPickup, objectGrabPointTransform.position, objectGrabPointTransform.rotation, objectGrabPointTransform);
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
        AddToHandInventory();
    }

    private void AddToHandInventory()
    {
        handEquipmentInventory.SelectEquipment(selectedEquipmentIndex);
        selectedEquipmentIndex++;
    }

    private void OnDrop()
    {
        if (!IsOwner) return;
        
        if(selectedEquipmentIndex == 0) { return; }

        selectedEquipmentIndex--;
        
        pickedUpObject = handEquipmentInventory.ActiveHandEquipment();

        OnDropServerRpc();
        Destroy(pickedUpObject);
        handEquipmentInventory.SelectEquipment(selectedEquipmentIndex);
    }

    [ServerRpc]
    public void OnDropServerRpc()
    {
        pickedUpObject = handEquipmentInventory.ActiveHandEquipment();
        GameObject objecttospawn = pickedUpObject.GetComponent<ObjectGrabbable>().equipmentSO.equipmentNetworkPrefab;
        spawnedObject = Instantiate(objecttospawn, objectGrabPointTransform.position, objectGrabPointTransform.rotation, objectGrabPointTransform);

        spawnedObject.GetComponent<NetworkObject>().Spawn();
        spawnedObject.GetComponent<Rigidbody>().AddForce(spawnedObject.transform.forward * 10f, ForceMode.Impulse);

        Debug.Log("on the server drop " + selectedEquipmentIndex);
        
        OnDropClientRpc();
    }

    [ClientRpc]
    public void OnDropClientRpc()
    {
        if (IsOwner) return;
        selectedEquipmentIndex--;
        Destroy(pickedUpObject);
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
        handEquipmentInventory = GetComponent<HandEquipmentInventory>();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private bool IsInventoryFull()
    {
        return selectedEquipmentIndex >= maxNumberOfEquipments;
    }

}
