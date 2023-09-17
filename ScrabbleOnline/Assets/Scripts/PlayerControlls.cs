using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            if (MultiplayerManager.Instance.IsClientInTurn())
            {
                BrickGhost.Instance.CreateBrickVisualServerRpc();
            }
        }    
    }

}
