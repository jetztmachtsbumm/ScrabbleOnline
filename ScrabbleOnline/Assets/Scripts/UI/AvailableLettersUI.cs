using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class AvailableLettersUI : MonoBehaviour
{

    [SerializeField] Transform _letterTemplate;

    void Start()
    {
        GameManager.Instance.OnAvailableLettersChanged += GameManager_OnAvailableLettersChanged;
        GameManager.Instance.DrawInitialLettersServerRpc(NetworkManager.Singleton.LocalClientId);
    }

    void GameManager_OnAvailableLettersChanged(object sender, System.EventArgs e)
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        foreach(LetterData letter in GameManager.Instance.GetAvailableLetters())
        {
            Transform UILetter = Instantiate(_letterTemplate, transform);

            UILetter.Find("Letter").GetComponent<TextMeshProUGUI>().text = letter.GetLetter();
            UILetter.Find("Score").GetComponent<TextMeshProUGUI>().text = letter.GetScore().ToString();

            UILetter.GetComponent<Button>().onClick.AddListener(() =>
            {
                BrickGhost.Instance.SetCurrentLetterDataServerRpc(letter);
            });
        }    
    }

}
