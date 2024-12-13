using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AreaTemperature : MonoBehaviour
{
    [SerializeField] private float temperature = 20.0f;

    public float GetTemperature()
    {
        return temperature;
    }

    public void SetTemperature(float newTemperature)
    {
        temperature = newTemperature;
    }

    private void Update()
    {
        temperature = Random.Range(0f, 20f);
    }
}