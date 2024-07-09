using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;

public class Player : NetworkBehaviour
{
    [SerializeField] List<Vector3> playerSpawnPoints;
    private PlayerControls inputActions;
    private void Awake()
    {
        inputActions = new PlayerControls();
        inputActions.Player.Enable();
    }

    void Start()
    {
        if (!IsOwner) { return; }

        transform.position = playerSpawnPoints[(int)OwnerClientId];
        transform.rotation = Quaternion.identity;
        Debug.Log("this client is spawned at: " + transform.position + " with id: " + (int)OwnerClientId);
    }

    public override void OnNetworkSpawn()
    {
        
    }
}
