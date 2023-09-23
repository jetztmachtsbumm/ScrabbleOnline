using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControlls : MonoBehaviour
{

    public static PlayerControlls Instance { get; private set; }

    [SerializeField] Transform cameraTransform;
    [SerializeField] float cameraSpeed = 1f;
    [SerializeField] float cameraZoomSpeed = 1f;
    [SerializeField] float cameraMaxHeight, cameraMinHeight;

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

        float cameraZoom = Input.mouseScrollDelta.y * cameraZoomSpeed * 1000 * Time.deltaTime;
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * cameraSpeed * Time.deltaTime;
        cameraTransform.position = new Vector3(cameraTransform.position.x + movement.x, Mathf.Clamp(cameraTransform.position.y - cameraZoom, 5, 50), cameraTransform.position.z + movement.z);
    }

}
