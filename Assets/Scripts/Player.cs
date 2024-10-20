using UnityEngine;
using Unity.Netcode;

public class Player : NetworkBehaviour
{
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
        }
        else
        {
            int layerNumber = LayerMask.NameToLayer("Player");
            SetLayerRecursively(playerVisuals.gameObject, layerNumber);
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
