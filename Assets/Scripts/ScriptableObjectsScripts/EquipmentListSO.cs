using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentListSO", menuName = "ScriptableObjects/EquipmentListSO", order = 1)]
public class EquipmentListSO : ScriptableObject
{
    public List<EquipmentSO> equipmentSOList;
}
