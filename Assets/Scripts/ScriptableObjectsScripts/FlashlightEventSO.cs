using System;
using UnityEngine;

[CreateAssetMenu(fileName = "FlashlightEventSO", menuName = "EventBuses/FlashlightEventSO")]
public class FlashlightEventSO : ScriptableObject
{
    public event Action OnFlashlightEvent;

    public void ToggleFlashlight()
    {
        OnFlashlightEvent?.Invoke();
    }
}
