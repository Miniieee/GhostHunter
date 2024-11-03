using UnityEngine;
using Unity.Netcode;

public class Player : NetworkBehaviour
{
    [SerializeField] private Transform firstPersonTarget;
    [SerializeField] private Transform thirdPersonTarget;
    [SerializeField] private SetObjectPosition ref_EquipmentRig;

    private CameraRotationFollower playerVisuals;

    private void Awake()
    {
        playerVisuals = GetComponentInChildren<CameraRotationFollower>();
    }

    private void Start()
    {
        if (IsOwner)
        {
            int layerNumber = LayerMask.NameToLayer("Hide");
            SetLayerRecursively(playerVisuals.gameObject, layerNumber);
            ref_EquipmentRig.SetTargetPosition(firstPersonTarget);
        }
        else
        {
            int layerNumber = LayerMask.NameToLayer("Player");
            SetLayerRecursively(playerVisuals.gameObject, layerNumber);

            ref_EquipmentRig.SetTargetPosition(thirdPersonTarget);
        }

    }

    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (obj == null)
        {
            return;
        }

        // Set the layer of the current object
        obj.layer = newLayer;

        // Recursively set the layer on all child objects
        foreach (Transform child in obj.transform)
        {
            if (child == null)
            {
                continue;
            }
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }


}
