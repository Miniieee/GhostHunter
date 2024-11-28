using Interfaces;
using UnityEngine;

namespace Equipments
{
    public class EmfActivate : MonoBehaviour, IActivatable
    {
        bool emfToggle = false;
        public void Activate(ulong networkObjectId)
        {
            emfToggle = !emfToggle;
            if (emfToggle)
            {
                Debug.Log("Emf is on player ID: " + networkObjectId);
                //turn on emf lights
                //activates sphere collider
                //on collision with ghost, ghost will be detected based on its distance and activity level
            }
            else
            {
                Debug.Log("Emf is off player ID: " + networkObjectId);
                //turn off emf lights
                //deactivates sphere collider
            }
        }
    }
}
