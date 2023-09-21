using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BrickGhost : NetworkBehaviour
{

    public static BrickGhost Instance { get; private set; }

    [SerializeField] Transform _brickVisualPrefab;

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
        if (GameManager.Instance.IsClientInTurn())
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
    public void CreateBrickVisualServerRpc()
    {
        CreateBrickVisualClientRpc();
        GameManager.Instance.SetNextPlayerInTurnServerRpc();
    }

    [ClientRpc]
    void CreateBrickVisualClientRpc()
    {
        Instantiate(_brickVisualPrefab, transform.position + new Vector3(0, -3, 0), Quaternion.identity);
    }

}
