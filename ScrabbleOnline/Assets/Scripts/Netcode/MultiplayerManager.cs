using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MultiplayerManager : NetworkBehaviour
{

    public static MultiplayerManager Instance { get; private set; }

    ulong _clientInTurn;

    public void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("There is more than one MultiplayerManager object active in the scene!");
            Destroy(gameObject);
        }
        Instance = this;

        StartGame();
    }

    public void StartGame()
    {
        if (IsServer)
        {
            SetClientInTurnServerRpc(NetworkManager.LocalClientId);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetClientInTurnServerRpc(ulong clientInTurn)
    {
        SetClientInTurnClientRpc(clientInTurn);
    }

    [ClientRpc]
    public void SetClientInTurnClientRpc(ulong clientInTurn)
    {
        _clientInTurn = clientInTurn;
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetNextPlayerInTurnServerRpc()
    {
        for(int i = 0; i < NetworkManager.ConnectedClientsIds.Count; i++)
        {
            if (NetworkManager.ConnectedClientsIds[i] == _clientInTurn)
            {
                if (i + 1 == NetworkManager.ConnectedClientsIds.Count)
                {
                    SetClientInTurnClientRpc(NetworkManager.ConnectedClientsIds[0]);
                    return;
                }
                else
                {
                    SetClientInTurnClientRpc(NetworkManager.ConnectedClientsIds[i + 1]);
                    return;
                }
            }
        }
    }

    public bool IsClientInTurn()
    {
        return _clientInTurn == NetworkManager.LocalClientId;
    }

}
