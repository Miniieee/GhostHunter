using System.Collections.Generic;
using UnityEngine;

public class HandEquipmentInventory : MonoBehaviour
{
    //private List<GameObject> handEquipmentList = new List<GameObject>();

    public void SelectEquipment(int selectedEquipmentIndex)
    {
        int i = 0;

        foreach (Transform equipment in transform)
        {
            equipment.gameObject.SetActive(i == selectedEquipmentIndex);
            i++;
        }
    }





    /*public void AddHandEquipment(GameObject equipment)
    {   
        if (!isEquipmentToPickupAvaiable())
        {
            return;
        }

        foreach(Transform transforms in transform)
        {
            
        }    

        handEquipmentList.Add(equipment);
        currentNumberOfEquipment++;
    }

    public bool isEquipmentToPickupAvaiable(){
        return currentNumberOfEquipment < maxNumberOfEquipment;}

    public void SetActiveHandEquipment(int index)
    {
        for (int i = 0; i < handEquipmentList.Count; i++)
        {
            handEquipmentList[i].SetActive(i == index);
        }
    }

    public void RemoveHandEquipment(GameObject equipment)
    {
        handEquipmentList.Remove(equipment);
        currentNumberOfEquipment--;
    }*/
}
