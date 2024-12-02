using UnityEngine;

namespace ScriptableObjectsScripts
{
    [CreateAssetMenu(fileName = "EquipmentSO", menuName = "ScriptableObjects/EquipmentSO")]
    public class EquipmentSO : ScriptableObject
    {
        public GameObject equipmentNetworkPrefab;
        public GameObject equipmentThirdPersonPrefab;
        public GameObject equipmentFirstPersonPrefab;
        public string equipmentName;

        public float detectionRange;
        public float activeTime;
    }
}
