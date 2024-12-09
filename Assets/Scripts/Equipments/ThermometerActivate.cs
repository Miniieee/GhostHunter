using Interfaces;
using UnityEngine;

namespace Equipments
{
    public class ThermometerActivate : MonoBehaviour, IActivatable
    {
        public void Activate(ulong networkObjectId)
        {
            Debug.Log("Thermometer is on player ID: " + networkObjectId);
        }
    }
}