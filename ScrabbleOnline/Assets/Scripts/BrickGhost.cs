using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickGhost : MonoBehaviour
{

    public static BrickGhost Instance { get; private set; }

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
        transform.position = MouseWorld.Instance.GetMouseWorldPosition() + new Vector3(0, 3, 0);
    }

}
