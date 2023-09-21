using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WaitingForPlayersUI : NetworkBehaviour
{

    [SerializeField] TextMeshProUGUI _lobbyNameText;
    [SerializeField] TextMeshProUGUI _lobbyCodeText;
    [SerializeField] TextMeshProUGUI _playerAmountText;
    [SerializeField] TextMeshProUGUI _joinMessageText;
    [SerializeField] Button _leaveButton;
    [SerializeField] Button _startButton;

    int _joinedPlayers;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        _lobbyNameText.text = "Lobby: " + LobbyManager.Instance.GetJoinedLobby().Name;
        _lobbyCodeText.text = "Lobbycode: " + LobbyManager.Instance.GetJoinedLobby().LobbyCode;
        SetPlayerAmountTextServerRpc(false);
        AddJoinMessageServerRpc(LobbyManager.Instance.PlayerName, false);

        if (!NetworkManager.Singleton.IsHost) Destroy(_startButton.gameObject);

        _leaveButton.onClick.AddListener(() =>
        {
            SetPlayerAmountTextServerRpc(true);
            AddJoinMessageServerRpc(LobbyManager.Instance.PlayerName, true);
            LobbyManager.Instance.LeaveLobby();
            NetworkManager.Singleton.Shutdown();
            SceneManager.LoadScene(0); //Main menu
        });

        _startButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.SceneManager.LoadScene("Game", LoadSceneMode.Single);
        });
    }

    [ServerRpc(RequireOwnership = false)]
    void SetPlayerAmountTextServerRpc(bool leftGame)
    {
        if (leftGame)
        {
            _joinedPlayers--;
            SetPlayerAmountTextClientRpc(_joinedPlayers);
        }
        else
        {
            _joinedPlayers++;
            SetPlayerAmountTextClientRpc(_joinedPlayers);
        }
    }

    [ClientRpc]
    void SetPlayerAmountTextClientRpc(int playerAmount)
    {
        _playerAmountText.text = "Players: " + playerAmount + "/" + LobbyManager.MAX_PLAYERS;
    }

    [ServerRpc(RequireOwnership = false)]
    void AddJoinMessageServerRpc(string playerName, bool leftGame)
    {
        AddJoinMessageClientRpc(playerName, leftGame);
    }

    [ClientRpc]
    void AddJoinMessageClientRpc(string playerName, bool leftGame)
    {
        if (leftGame)
        {
            _joinMessageText.text += "\n" + playerName + " left the game!";
        }
        else
        {
            if(_joinMessageText.text == "")
            {
                _joinMessageText.text += playerName + " joined the game!";
            }
            else
            {
                _joinMessageText.text += "\n" + playerName + " joined the game!";
            }
        }
    }

}
