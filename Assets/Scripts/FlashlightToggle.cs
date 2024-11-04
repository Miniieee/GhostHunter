using UnityEngine;

public class FlashlightToggle : MonoBehaviour
{
    [SerializeField] private Light flashlight;
    private FlashlightActivate flashlightActivate;

    void Start()
    {
        flashlight.enabled = false;
    }

    public void GetFlashlightReference()
    {
        flashlightActivate = GetComponentInChildren<FlashlightActivate>();
        flashlightActivate.OnActivated += ToggleFlashlight;
    }

    private void ToggleFlashlight()
    {
        flashlight.enabled = !flashlight.enabled;
    }

    public void OnDestroy()
    {
        if (flashlightActivate == null) return;
        flashlightActivate.OnActivated -= ToggleFlashlight;
    }
}
