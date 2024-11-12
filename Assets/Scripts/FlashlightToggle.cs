using System;
using Unity.Netcode;
using UnityEngine;

public class FlashlightToggle : NetworkBehaviour
{
    [SerializeField] private Light flashlight;
    [SerializeField] private FlashlightEventSO flashlightActivate;

    void Start()
    {
        flashlight.enabled = false;
        flashlightActivate.OnFlashlightEvent += ToggleFlashlight;
    }

    private void ToggleFlashlight(ulong networkID)
    {
        if (NetworkObjectId != networkID) { return; }
        flashlight.enabled = !flashlight.enabled;
    }

    public override void OnDestroy()
    {
        flashlightActivate.OnFlashlightEvent -= ToggleFlashlight;
    }

    private void OnDisable()
    {
        flashlightActivate.OnFlashlightEvent -= ToggleFlashlight;
    }
}
