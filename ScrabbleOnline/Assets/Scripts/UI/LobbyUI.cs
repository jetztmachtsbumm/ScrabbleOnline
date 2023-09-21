using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{

    [SerializeField] Button _backButton;
    [SerializeField] Button _createLobbyButton;
    [SerializeField] Button _quickJoinButton;
    [SerializeField] Button _JoinWithCodeButton;
    [SerializeField] Transform _mainMenuUI;
    [SerializeField] Transform _lobbyCreateUI;
    [SerializeField] Transform _lobbyListContainer;
    [SerializeField] Transform _listedLobbyTemplate;
    [SerializeField] TMP_InputField _playerNameInputField;
    [SerializeField] TMP_InputField _lobbyCodeInputField;

    void Start()
    {
        _backButton.onClick.AddListener(() =>
        {
            _mainMenuUI.gameObject.SetActive(true);
            gameObject.SetActive(false);
        });
        _createLobbyButton.onClick.AddListener(() =>
        {
            LobbyManager.Instance.PlayerName = _playerNameInputField.text;
            _lobbyCreateUI.gameObject.SetActive(true);
        });
        _quickJoinButton.onClick.AddListener(() =>
        {
            LobbyManager.Instance.PlayerName = _playerNameInputField.text;
            LobbyManager.Instance.QuikJoin(); 
        });
        _JoinWithCodeButton.onClick.AddListener(() =>
        {
            LobbyManager.Instance.PlayerName = _playerNameInputField.text;
            LobbyManager.Instance.JoinWithCode(_lobbyCodeInputField.text);
        });

        LobbyManager.Instance.OnLobbyListChanged += LobbyManager_OnLobbyListChanged;
    }

    void LobbyManager_OnLobbyListChanged(object sender, List<Lobby> lobbies)
    {
        UpdateLobbyList(lobbies);
    }

    void UpdateLobbyList(List<Lobby> lobbyList)
    {
        foreach(Transform child in _lobbyListContainer)
        {
            Destroy(child.gameObject);
        }

        foreach(Lobby lobby in lobbyList)
        {
            Transform listedLobby = Instantiate(_listedLobbyTemplate, _lobbyListContainer);
            listedLobby.Find("LobbyNameText").GetComponent<TextMeshProUGUI>().text = lobby.Name;
            listedLobby.Find("LobbyPlayerCountText").GetComponent<TextMeshProUGUI>().text = lobby.Players.Count + "/" + lobby.MaxPlayers;
            listedLobby.GetComponent<ListedLobbyTemplate>().Lobby = lobby;
        }
    }

}
