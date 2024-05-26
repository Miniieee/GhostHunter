using UnityEngine;

public class Selfparenting : MonoBehaviour
{
    void Start()
    {
        Reparent();
    }

    void Reparent()
    {
        // Get the current parent of this object
        Transform currentParent = transform.parent;

        // Check if the current parent is not null
        if (currentParent != null)
        {
            // Find the target child object by name
            Transform targetChild = currentParent.GetComponentInChildren<HandHelper>().transform;

            // Check if the target child exists
            if (targetChild != null)
            {
                // Reparent this object to the target child
                transform.SetParent(targetChild);
                Debug.Log($"Reparented {gameObject.name} to {targetChild.name}");
            }
            else
            {
                Debug.LogWarning($"Child with name MainCamera/ObjectGrabTransform not found under {currentParent.name}");
            }
        }
        else
        {
            Debug.LogWarning("This object does not have a parent");
        }
    }
}
