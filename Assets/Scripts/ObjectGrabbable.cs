using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ObjectGrabbable : MonoBehaviour, IInteractable
{
    private Rigidbody rb;
    private float throwForce = 2f;
    private FollowTransform followTransform;

    public EquipmentSO equipmentSO;

    private void Awake() {
        followTransform = GetComponent<FollowTransform>();
        rb = GetComponent<Rigidbody>();
    }

    public void StartInteraction(Transform objectGrabPointTransform)
    {
        followTransform.SetTargetTransform(objectGrabPointTransform);
        
        rb.isKinematic = true;
    }

    public void EndInteraction()
    {
        transform.parent = null; // Detach from the player
        rb.isKinematic = false; // Allow physics to affect the object again

        // Apply a forward force to throw the object away
        rb.AddForce(transform.forward * throwForce, ForceMode.Impulse);
    }

}
