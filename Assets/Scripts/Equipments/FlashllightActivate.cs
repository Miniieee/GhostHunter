using UnityEngine;

public class FlashllightActivate : MonoBehaviour, IActivatable
{

    private PlayerControls playerControls;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void Start()
    {
        playerControls.Player.Activate.performed += ctx => Activate();
    }

    public void Activate()
    {
        Debug.Log("Flashlight Activated");
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
}