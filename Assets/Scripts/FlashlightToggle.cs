using UnityEngine;

public class FlashlightToggle : MonoBehaviour
{
    [SerializeField] private Light flashlight;
    [SerializeField] private FlashlightEventSO flashlightActivate;

    void Start()
    {
        flashlight.enabled = false;
    }

    public void GetFlashlightReference()
    {
        flashlightActivate.OnFlashlightEvent += ToggleFlashlight;
    }

    private void ToggleFlashlight()
    {
        flashlight.enabled = !flashlight.enabled;
    }

    public void OnDestroy()
    {
        if (flashlightActivate == null) return;
        flashlightActivate.OnFlashlightEvent -= ToggleFlashlight;
    }
}
