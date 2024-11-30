using ScriptableObjectsScripts;
using UnityEngine;
using Enums;

public class Ghost : MonoBehaviour
{
    private Player player;
    private PlayerData playerData;

    private float emfValue;
    private float temperatureValue;

    private void Awake()
    {
        player = GetComponent<Player>();
        playerData = player.GetPlayerData();
    }

    private void Start()
    {
        if (playerData.playerType != PlayerType.Ghost)
        {
            this.enabled = false;
        }
    }

    private void Initialize()
    {
        emfValue = playerData.emfIntensity;
        temperatureValue = playerData.temperatureAnomaly;
    }

    private void GetEmfValue()
    {
        Debug.Log("EMF Value: " + emfValue);
    }

    private void GetTemperatureValue()
    {
        Debug.Log("Temperature Value: " + temperatureValue);
    }
}