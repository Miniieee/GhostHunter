using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    private Transform targetTransform;
    [SerializeField] Vector3 offset;

    public void SetTargetTransform(Transform targetTransform)
    {
        this.targetTransform = targetTransform;
    }

    private void LateUpdate()
    {   
        if (targetTransform == null) return;

        transform.position = targetTransform.position;
        transform.rotation = targetTransform.rotation;
    }
}
