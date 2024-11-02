using Unity.Netcode;
using UnityEngine;

public class PlayerEquipmentActivate : NetworkBehaviour
{
    private PlayerControls playerControls;
    private HandEquipmentInventory handEquipmentInventory;

    private void Awake()
    {
        playerControls = new PlayerControls();
        handEquipmentInventory = GetComponent<HandEquipmentInventory>();
    }

    void Start()
    {
        playerControls.Player.Activate.performed += ctx => ActivateEquipment();
    }

    private void ActivateEquipment()
    {
        if (!IsOwner) { return; }

        GameObject currentEquipment = handEquipmentInventory.ActiveHandEquipment();

        if (currentEquipment == null) { return; }

        if (currentEquipment.TryGetComponent(out IActivatable activatable))
        {
            activatable.Activate();
        }

        ActivateEquipmentRpc();
    }

    [Rpc(SendTo.Everyone)]
    private void ActivateEquipmentRpc()
    {
        if (IsOwner) { return; }

        GameObject currentEquipment = handEquipmentInventory.ActiveHandEquipment();

        if (currentEquipment == null) { return; }

        currentEquipment.GetComponent<IActivatable>().Activate();
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
