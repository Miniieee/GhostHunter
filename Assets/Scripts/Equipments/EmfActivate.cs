using System;
using Interfaces;
using UnityEngine;

namespace Equipments
{
    public class EmfActivate : MonoBehaviour, IActivatable
    {
        private bool emfToggle = false;
        private SphereCollider sphereCollider;
        
        private void Start()
        {
            sphereCollider = GetComponent<SphereCollider>();
            sphereCollider.enabled = false;
        }

        public void Activate(ulong networkObjectId)
        {
            emfToggle = !emfToggle;
            if (emfToggle)
            {
                Debug.Log("Emf is on player ID: " + networkObjectId);
                sphereCollider.enabled = true;
                //turn on emf lights
                //activates sphere collider
                //on collision with ghost, ghost will be detected based on its distance and activity level
            }
            else
            {
                Debug.Log("Emf is off player ID: " + networkObjectId);
                sphereCollider.enabled = false;
                //turn off emf lights
                //deactivates sphere collider
            }
        }
        
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Ghost"))
            {
                Debug.Log("Ghost detected");
                float distance = Vector3.Distance(transform.position, other.transform.position);
                Debug.Log("Distance: " + distance);
            }
        }
    }
}
