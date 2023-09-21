using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class BrickGhost : NetworkBehaviour
{

    public static BrickGhost Instance { get; private set; }

    [SerializeField] Transform _brickVisualPrefab;
    [SerializeField] TextMeshProUGUI _letterText;
    [SerializeField] TextMeshProUGUI _scoreText;

    LetterData _currentLetterData;

    public void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("There is more than one BrickGhost object active in the scene!");
            Destroy(gameObject);
        }
        Instance = this;
    }

    void Update()
    {
        if (GameManager.Instance.IsClientInTurn() && !EventSystem.current.IsPointerOverGameObject())
        {
            ChangePositionServerRpc(GridSystem.Instance.GetCellAtPosition(MouseWorld.Instance.GetMouseWorldPosition()));
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void ChangePositionServerRpc(GridCell gridCell)
    {
        transform.position = GridSystem.Instance.GetWorldPosition(gridCell) + new Vector3(0, 3, 0);
    }

    [ServerRpc(RequireOwnership = false)] 
    public void CreateBrickVisualServerRpc(GridCell gridCell)
    {
        CreateBrickVisualClientRpc(gridCell);
        GameManager.Instance.SetNextPlayerInTurnServerRpc();
    }

    [ClientRpc]
    void CreateBrickVisualClientRpc(GridCell gridCell)
    {
        Transform brickVisual = Instantiate(_brickVisualPrefab, GridSystem.Instance.GetWorldPosition(gridCell) + new Vector3(0, 0.25f, 0), Quaternion.identity);
        brickVisual.Find("Canvas").Find("Letter").GetComponent<TextMeshProUGUI>().text = _currentLetterData.GetLetter();
        brickVisual.Find("Canvas").Find("Score").GetComponent<TextMeshProUGUI>().text = _currentLetterData.GetScore().ToString();
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetCurrentLetterDataServerRpc(LetterData letterData)
    {
        SetCurrentLetterDataClientRpc(letterData);
    }

    [ClientRpc]
    void SetCurrentLetterDataClientRpc(LetterData letterData)
    {
        _currentLetterData = letterData;
        _letterText.text = letterData.GetLetter();
        _scoreText.text = letterData.GetScore().ToString();
    }

}
