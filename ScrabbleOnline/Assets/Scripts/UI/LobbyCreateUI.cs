using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyCreateUI : MonoBehaviour
{

    [SerializeField] Button _createPublicButton;
    [SerializeField] Button _createPrivateButton;
    [SerializeField] Button _closeButton;
    [SerializeField] TMP_InputField _lobbyNameInputField;

    private void Awake()
    {
        _createPublicButton.onClick.AddListener(() =>
        {
            LobbyManager.Instance.CreateLobby(_lobbyNameInputField.text, false);
        });
        _createPrivateButton.onClick.AddListener(() =>
        {
            LobbyManager.Instance.CreateLobby(_lobbyNameInputField.text, true);
        });
        _closeButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });
    }

}
