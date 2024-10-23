using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;
using System;

public class CustomNetworkManager : NetworkManager
{
    [SerializeField] private List<Vector3> spawnpoints = new List<Vector3>();

    private void Start()
    {   
        spawnpoints.Clear();

        Vector3 spawnPosition1 = new Vector3(0, 0, 0);
        Vector3 spawnPosition2 = new Vector3(1, 0, 0);
        Vector3 spawnPosition3 = new Vector3(2, 0, 0);
        Vector3 spawnPosition4 = new Vector3(3, 0, 0);

        spawnpoints.Add(spawnPosition1);
        spawnpoints.Add(spawnPosition2);
        spawnpoints.Add(spawnPosition3);
        spawnpoints.Add(spawnPosition4);

        this.NetworkConfig.ConnectionApproval = true;
        this.NetworkConfig.ConnectionData = new byte[0];

        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.ConnectionApprovalCallback -= ApprovalCheck;
        }
    }

    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        response.Approved = true;
        response.CreatePlayerObject = true;

        // Set the player's initial position based on their client ID
        Vector3 spawnPosition = spawnpoints[(int)request.ClientNetworkId];
        response.Position = spawnPosition;
        response.Rotation = Quaternion.identity;
    }
}
