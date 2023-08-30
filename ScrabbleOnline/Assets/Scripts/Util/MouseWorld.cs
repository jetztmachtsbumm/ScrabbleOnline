using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{

    public static MouseWorld Instance { get; private set; }

    [SerializeField] LayerMask _mouseHitLayerMask;

    public void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("There is more than one MouseWorld object active in the scene!");
            Destroy(gameObject);
        }
        Instance = this;
    }

    public Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, _mouseHitLayerMask);
        return hit.point;
    }

}
