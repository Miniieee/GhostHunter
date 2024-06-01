using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentSO", menuName = "ScriptableObjects/EquipmentSO")]
public class EquipmentSO : ScriptableObject
{
    public GameObject equipmentNetworkPrefab;
    public GameObject equipmentPrefab;
    public string equipmentName;

}
