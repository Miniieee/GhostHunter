using System;
using UnityEngine;

public class FlashlightToggle : MonoBehaviour
{
    [SerializeField] private Light flashlight;
    [SerializeField] private FlashlightEventSO flashlightActivate;

    void Start()
    {
        flashlight.enabled = false;
        flashlightActivate.OnFlashlightEvent += ToggleFlashlight;
    }

    private void ToggleFlashlight(object sender, EventArgs e)
    {
        flashlight.enabled = !flashlight.enabled;
    }

    public void OnDestroy()
    {
        if (flashlightActivate == null) return;
        flashlightActivate.OnFlashlightEvent -= ToggleFlashlight;
    }
}
