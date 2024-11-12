using UnityEngine;

public class FlashlightActivate : MonoBehaviour, IActivatable
{
    [SerializeField] private FlashlightEventSO flashlightEvent;

    public void Activate(ulong networkObjectId)
    {
        flashlightEvent.ToggleFlashlight(networkObjectId);
    }

}