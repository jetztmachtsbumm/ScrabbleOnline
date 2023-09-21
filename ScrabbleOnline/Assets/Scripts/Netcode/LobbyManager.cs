using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{

    public static LobbyManager Instance { get; private set; }

    public const int MAX_PLAYERS = 4;

    public string PlayerName { get; set; }

    public event EventHandler<List<Lobby>> OnLobbyListChanged;

    Lobby _joinedLobby;
    float _heartBeatTimer;
    float _listLobbiesTimer;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("There is more than one LobbyManager object active in the scene!");
            Destroy(Instance.gameObject);
        }

        DontDestroyOnLoad(gameObject);
        InitializeUnityAuthentication();
        Instance = this;
    }

    void Update()
    {
        HandleHeartBeat();
        HandleLobbyList();
    }

    void HandleHeartBeat()
    {
        if (IsLobbyHost())
        {
            _heartBeatTimer -= Time.deltaTime;
            if(_heartBeatTimer <= 0)
            {
                float heartBeatTimerMax = 15f;
                _heartBeatTimer = heartBeatTimerMax;

                LobbyService.Instance.SendHeartbeatPingAsync(_joinedLobby.Id);
            }
        }
    }

    void HandleLobbyList()
    {
        if (_joinedLobby != null || !AuthenticationService.Instance.IsSignedIn) return;

        _listLobbiesTimer -= Time.deltaTime;
        if(_listLobbiesTimer <= 0)
        {
            float listLobbiesTimerMax = 3f;
            _listLobbiesTimer = listLobbiesTimerMax;

            ListLobbies();
        }
    }

    bool IsLobbyHost()
    {
        return _joinedLobby != null && _joinedLobby.HostId == AuthenticationService.Instance.PlayerId;
    }

    async void ListLobbies()
    {
        try
        {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
            {
                Filters = new List<QueryFilter>
                {
                    new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT)
                }
            };

            QueryResponse queryResponse = await LobbyService.Instance.QueryLobbiesAsync(queryLobbiesOptions);
            OnLobbyListChanged?.Invoke(this, queryResponse.Results);
        } catch(LobbyServiceException e)
        {
            Debug.LogException(e);
        }
    }

    async void InitializeUnityAuthentication()
    {
        if (UnityServices.State == ServicesInitializationState.Initialized) return;

        InitializationOptions initializationOptions = new InitializationOptions();
        initializationOptions.SetProfile(UnityEngine.Random.Range(0, 10000).ToString());

        await UnityServices.InitializeAsync(initializationOptions);

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void CreateLobby(string lobbyName, bool isPrivate)
    {
        try
        {
            _joinedLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, MAX_PLAYERS, new CreateLobbyOptions
            {
                IsPrivate = isPrivate
            });

            NetworkManager.Singleton.StartHost();
            NetworkManager.Singleton.SceneManager.LoadScene("WaitingForPlayers", UnityEngine.SceneManagement.LoadSceneMode.Single);
        } catch(LobbyServiceException e)
        {
            Debug.LogException(e);
        }
    }

    public async void QuikJoin()
    {
        try
        {
            _joinedLobby = await LobbyService.Instance.QuickJoinLobbyAsync();
            NetworkManager.Singleton.StartClient();
        } catch(LobbyServiceException e)
        {
            Debug.LogException(e);
        }
    }

    public async void JoinWithCode(string lobbyCode)
    {
        try
        {
            _joinedLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode);
            NetworkManager.Singleton.StartClient();
        } catch(LobbyServiceException e)
        {
            Debug.LogException(e);
        }
    }
    
    public async void JoinWithId(string lobbyId)
    {
        try
        {
            _joinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId);
            NetworkManager.Singleton.StartClient();
        } catch(LobbyServiceException e)
        {
            Debug.LogException(e);
        }
    }

    public async void LeaveLobby()
    {
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(_joinedLobby.Id, AuthenticationService.Instance.PlayerId);
        } catch(LobbyServiceException e)
        {
            Debug.LogException(e);
        }
    }

    public Lobby GetJoinedLobby()
    {
        return _joinedLobby;
    }

}
