using System;
using Unity.Netcode;
using UnityEngine;

public class FlashlightActivate : MonoBehaviour, IActivatable
{
    public event Action OnActivated;
    private FlashlightToggle flashlightToggle;

    private void Start()
    {
        flashlightToggle = GetComponentInParent<FlashlightToggle>();
        flashlightToggle.GetFlashlightReference();
    }

    public void Activate()
    {
        Debug.Log("Activated Flashlight event");
        OnActivated?.Invoke();
    }

}