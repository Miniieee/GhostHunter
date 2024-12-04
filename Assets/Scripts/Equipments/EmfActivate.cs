using System;
using Interfaces;
using ScriptableObjectsScripts;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Equipments
{
    public class EmfActivate : MonoBehaviour, IActivatable
    {
        private bool emfToggle = false;
        private SphereCollider sphereCollider;
        private float emfValue = 0f;
        private float detectionRange;

        [Title("Equipment Data", "Drag the EMF Reader scriptable object here")] [SerializeField]
        private EquipmentSO emfEquipmentSo;
        
        
        private void Start()
        {
            detectionRange = emfEquipmentSo.range;
            
            sphereCollider = GetComponent<SphereCollider>();
            
            sphereCollider.isTrigger = true;
            sphereCollider.radius = detectionRange;
            
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
            }
            else
            {
                Debug.Log("Emf is off player ID: " + networkObjectId);
                sphereCollider.enabled = false;
                //turn off emf lights
            }
        }
        
        private void OnTriggerStay(Collider other)
        {
            if (!other.gameObject.CompareTag("Ghost")) return;

            float distance = Vector3.Distance(transform.position, other.transform.position);
            if (distance < detectionRange)
            {
                if (other.TryGetComponent<Ghost>(out Ghost ghost))
                {
                    emfValue = ghost.GetEmfValue();
                }

                //add sound effect
                //light up the emf reader
            }
        }
    }
}
