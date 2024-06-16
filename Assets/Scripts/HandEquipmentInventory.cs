using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HandEquipmentInventory : NetworkBehaviour
{
    private PlayerControls playerControls;
    private List<GameObject> handEquipmentList = new List<GameObject>();

    private int selectedEquipmentIndex = 0;
    private int maxNumberOfEquipments = 3;
    private int currentlySelectedEquipmentIndex;

    [SerializeField] private Transform handEquipmentTransform;
    
    
    public override void OnNetworkSpawn()
    {
        playerControls.Player.EquipmentSwich.performed += ctx => SelectEquipment(playerControls.Player.EquipmentSwich.ReadValue<float>());
    }

    public override void OnNetworkDespawn()
    {
        playerControls.Player.EquipmentSwich.performed -= ctx => SelectEquipment(playerControls.Player.EquipmentSwich.ReadValue<float>());
    }


    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void Start() {
        selectedEquipmentIndex = 0;
    }

    public void SelectEquipment(float selectedEquipmentChangeDiretctionValue)
    {
        if(handEquipmentTransform.childCount == 0) { return; }

        int i = 0;
        currentlySelectedEquipmentIndex = GetSelectedEquipmentIndex(selectedEquipmentChangeDiretctionValue);

        foreach (Transform equipment in handEquipmentTransform)
        {
            equipment.gameObject.SetActive(i == currentlySelectedEquipmentIndex);
            i++;
        }
    }

    public GameObject ActiveHandEquipment()
    {
        int i = 0;

        foreach (Transform equipment in handEquipmentTransform)
        {
            if (i == currentlySelectedEquipmentIndex)
            {
                return equipment.gameObject;
            }
            i++;
        }

        return null;
    }

    private int GetSelectedEquipmentIndex(float selectedEquipmentChangeDiretctionValue)
    {
        int equipmentIndexChange = Mathf.RoundToInt(Mathf.Sign(selectedEquipmentChangeDiretctionValue));

        //TODO something wrong is here

        selectedEquipmentIndex += equipmentIndexChange;

        maxNumberOfEquipments = handEquipmentTransform.childCount;

        if(selectedEquipmentIndex >= maxNumberOfEquipments)
        {
            selectedEquipmentIndex = 0;
        }
        else if(selectedEquipmentIndex < 0)
        {
            selectedEquipmentIndex = maxNumberOfEquipments - 1;
        }

        Debug.Log(selectedEquipmentIndex);
        return selectedEquipmentIndex;
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

    private void OnEnable() {
        playerControls.Enable();

    }

    private void OnDisable() {
        playerControls.Disable();
    }
}
