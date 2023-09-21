using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{

    [SerializeField] Button _playButton;
    [SerializeField] Button _quitButton;
    [SerializeField] Transform _lobbyUI;

    private void Awake()
    {
        _playButton.onClick.AddListener(() =>
        {
            _lobbyUI.gameObject.SetActive(true);
            gameObject.SetActive(false);
        });
        _quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }

}
