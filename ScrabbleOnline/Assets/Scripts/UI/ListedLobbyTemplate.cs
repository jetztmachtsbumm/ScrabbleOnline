using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class ListedLobbyTemplate : MonoBehaviour
{

    public Lobby Lobby { get; set; }

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            LobbyManager.Instance.JoinWithCode(Lobby.Id);
        });
    }

}
