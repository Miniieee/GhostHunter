using System;
using Interfaces;
using UnityEngine;
using TMPro;
using UnityEditor.Experimental.GraphView;

namespace Equipments
{
    public class ThermometerActivate : MonoBehaviour, IActivatable
    {
        private const string TemperatureString = "Temperature";

        private TextMeshProUGUI temperatureText;
        private bool temperatureToggle = false;
        private SphereCollider thermometerCollider;
        private float temperature;
        private int temperatureLayer;

        private void Awake()
        {
            temperatureText = GetComponentInChildren<TextMeshProUGUI>();
            thermometerCollider = GetComponent<SphereCollider>();
            thermometerCollider.isTrigger = true;

            // Cache the layer index for performance
            temperatureLayer = LayerMask.NameToLayer(TemperatureString);
        }

        private void Start()
        {
            thermometerCollider.enabled = false;
        }

        public void Activate(ulong networkObjectId)
        {
            temperatureToggle = !temperatureToggle;

            if (temperatureToggle)
            {
                Debug.Log("Thermometer is on player ID: " + networkObjectId);
                thermometerCollider.enabled = temperatureToggle;
            }
            else
            {
                Debug.Log("Thermometer is off player ID: " + networkObjectId);
                thermometerCollider.enabled = temperatureToggle;
            }

            //check the collider for temperature
            //show the temperature on the UI
            //add variation to the temperature, like 0 to +2 degrees for every second
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.layer == temperatureLayer)
            {
                if (other.TryGetComponent<AreaTemperature>(out AreaTemperature areaTemp) && temperatureToggle)
                {
                    temperature = areaTemp.GetTemperature();
                    temperatureText.text = $"{temperature:0.0}";
                }
            }
        }
    }
}