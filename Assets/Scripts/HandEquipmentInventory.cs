using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HandEquipmentInventory : NetworkBehaviour
{
    private PlayerControls playerControls;

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
        if(!IsOwner) { return; }

        if (handEquipmentTransform.childCount == 0) { return; }
        ActivateSelectedEquipment(selectedEquipmentChangeDiretctionValue);
        ActivateSelectedEquipmentRpc(selectedEquipmentChangeDiretctionValue);
    }

    private void ActivateSelectedEquipment(float selectedEquipmentChangeDiretctionValue)
    {
        int i = 0;
        currentlySelectedEquipmentIndex = GetSelectedEquipmentIndex(selectedEquipmentChangeDiretctionValue);

        foreach (Transform equipment in handEquipmentTransform)
        {
            equipment.gameObject.SetActive(i == currentlySelectedEquipmentIndex);
            i++;
        }
    }

    [Rpc(SendTo.Everyone)]
    public void ActivateSelectedEquipmentRpc(float selectedEquipmentChangeDiretctionValue)
    {
        if(IsOwner) { return; }
        ActivateSelectedEquipment(selectedEquipmentChangeDiretctionValue);
    }

    public GameObject ActiveHandEquipment()
    {
        foreach (Transform equipment in handEquipmentTransform)
        {
            if (equipment.gameObject.activeSelf == true)
            {
                return equipment.gameObject;
            }
        }

        return null;
    }

    private int GetSelectedEquipmentIndex(float selectedEquipmentChangeDiretctionValue)
    {
        //round float to integer and then show change direction so the float value doesent matter 
        int equipmentIndexChange = Mathf.RoundToInt(Mathf.Sign(selectedEquipmentChangeDiretctionValue));


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

    private void OnEnable() {
        playerControls.Enable();

    }

    private void OnDisable() {
        playerControls.Disable();
    }
}
