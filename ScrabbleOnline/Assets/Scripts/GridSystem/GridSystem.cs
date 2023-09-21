using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{

    public static GridSystem Instance { get; private set; }

    [SerializeField] int _width;
    [SerializeField] int _height;

    GridCell[,] _cells;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("There is more than one GridSystem object active in the scene!");
            Destroy(gameObject);
        }

        CreateCells();
        Instance = this;
        Debug.Log("Grid created");
    }

    void CreateCells()
    {
        _cells = new GridCell[_width, _height];

        for(int x = 0; x < _width; x++)
        {
            for(int z = 0; z < _height; z++)
            {
                _cells[x,z] = new GridCell(x, z, new char());
            }
        }
    }

    public Vector3 GetWorldPosition(GridCell gridCell)
    {
        return new Vector3(gridCell.GetX(), 0, gridCell.GetZ());
    }

    public GridCell GetCellAtPosition(Vector3 position)
    {
        int x = Mathf.RoundToInt(position.x);
        int z = Mathf.RoundToInt(position.z);

        if (_cells == null)
        {
            return new GridCell();
        }

        return _cells[x, z];
    }

    public Vector3 SnapToGrid(Vector3 worldPos)
    {
        GridCell gridCell = GetCellAtPosition(worldPos);
        return GetWorldPosition(gridCell);
    }

}
