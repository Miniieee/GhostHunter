using Unity.Netcode;
using UnityEngine;

public class EquipmentNetworkSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject equipmentPrefab;
    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        GameObject spawnedObject = Instantiate(equipmentPrefab, transform.position, transform.rotation);
        spawnedObject.GetComponent<NetworkObject>().Spawn();

    }
}
