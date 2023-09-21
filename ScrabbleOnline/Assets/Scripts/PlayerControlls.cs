using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControlls : MonoBehaviour
{

    public static PlayerControlls Instance { get; private set; }

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("There is more than one PlayerControlls object active in the scene!");
            Destroy(gameObject);
        }
        Instance = this;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (GameManager.Instance.IsClientInTurn() && !EventSystem.current.IsPointerOverGameObject())
            {
                BrickGhost.Instance.CreateBrickVisualServerRpc(GridSystem.Instance.GetCellAtPosition(MouseWorld.Instance.GetMouseWorldPosition()));
            }
        }    
    }

}
