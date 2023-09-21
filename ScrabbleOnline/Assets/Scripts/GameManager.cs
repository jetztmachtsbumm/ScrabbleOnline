using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{

    public static GameManager Instance { get; private set; }

    public event EventHandler OnAvailableLettersChanged;

    ulong _clientInTurn;
    List<LetterData> _drawableLetters;
    List<LetterData> _availableLetters;

    public void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("There is more than one MultiplayerManager object active in the scene!");
            Destroy(gameObject);
        }
        Instance = this;

        _drawableLetters = new List<LetterData>();
        _availableLetters = new List<LetterData>();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        StartGame();
    }

    public void StartGame()
    {
        if (NetworkManager.IsServer)
        {
            FillDrawableLetters();
            SetClientInTurnServerRpc(NetworkManager.LocalClientId);
        }
    }

    void FillDrawableLetters()
    {
        foreach(LetterData letter in Letters.ABC)
        {
            for (int i = 0; i < letter.GetTotalAmount(); i++)
            {
                AddDrawableLetterServerRpc(letter);
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void DrawInitialLettersServerRpc(ulong client)
    {
        DrawInitialLettersClientRpc(client);
    }

    [ClientRpc]
    void DrawInitialLettersClientRpc(ulong client)
    {
        if (NetworkManager.LocalClientId != client) return;

        for (int i = 0; i < 7; i++)
        {
            DrawLetter();
        }
    }

    void DrawLetter()
    {
        int randomIndex = UnityEngine.Random.Range(0, _drawableLetters.Count);
        _availableLetters.Add(_drawableLetters[randomIndex]);
        OnAvailableLettersChanged?.Invoke(this, EventArgs.Empty);
        RemoveDrawableLetterServerRpc(_drawableLetters[randomIndex]);
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

    [ServerRpc]
    public void AddDrawableLetterServerRpc(LetterData letterData)
    {
        AddDrawableLetterClientRpc(letterData);
    }

    [ClientRpc]
    public void AddDrawableLetterClientRpc(LetterData letterData)
    {
        _drawableLetters.Add(letterData);
    }

    [ServerRpc(RequireOwnership = false)]
    public void RemoveDrawableLetterServerRpc(LetterData letterData)
    {
        RemoveDrawableLetterClientRpc(letterData);
    }

    [ClientRpc]
    public void RemoveDrawableLetterClientRpc(LetterData letterData)
    {
        _drawableLetters.Remove(letterData);
    }

    public bool IsClientInTurn()
    {
        return _clientInTurn == NetworkManager.LocalClientId;
    }

    public List<LetterData> GetAvailableLetters()
    {
        return _availableLetters;
    }

}
