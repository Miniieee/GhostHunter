using Unity.Netcode;
using UnityEngine;

public class FlashlightActivate : MonoBehaviour, IActivatable
{
    [SerializeField] private Light flashlight;


    public void Activate()
    {
        Debug.Log("Activated Flashlight");
        flashlight.enabled = !flashlight.enabled;
    }

}