using UnityEngine;

public class SetObjectPosition : MonoBehaviour
{
    [SerializeField] Transform targetPosition;

    void LateUpdate()
    {
        this.gameObject.transform.position = targetPosition.position;
    }
}
