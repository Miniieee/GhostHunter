using System;
using UnityEngine;

[CreateAssetMenu(fileName = "FlashlightEventSO", menuName = "EventBuses/FlashlightEventSO")]
public class FlashlightEventSO : ScriptableObject
{
    public event Action<ulong> OnFlashlightEvent;

    public void ToggleFlashlight(ulong networkID)
    {
        OnFlashlightEvent?.Invoke(networkID);
    }
}
