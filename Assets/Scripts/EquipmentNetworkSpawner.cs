using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EquipmentNetworkSpawner : NetworkBehaviour
{
    [SerializeField] private EquipmentListSO equipmentList;
    [SerializeField] private List<Transform> equipmentListTransform;
    private List<EquipmentSO> equipmentSOs = new List<EquipmentSO>();
    public override void OnNetworkSpawn()
    {
        for (int i = 0; i < equipmentList.equipmentSOList.Count; i++)
        {
            equipmentSOs.Add(equipmentList.equipmentSOList[i]);
        }

        if (!IsServer) return;

        for (int i = 0; i < equipmentSOs.Count; i++)
        {
            GameObject spawnedObject = Instantiate(equipmentSOs[i].equipmentNetworkPrefab.gameObject, equipmentListTransform[i].position, equipmentListTransform[i].rotation);
            spawnedObject.GetComponent<ObjectGrabbable>().equipmentSO = equipmentSOs[i];
            spawnedObject.GetComponent<NetworkObject>().Spawn();
        }

    }
}
