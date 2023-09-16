using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct GridCell
{
    
    int x;
    int z;

    public GridCell(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public int GetX()
    {
        return x;
    }

    public int GetZ()
    {
        return z;
    }

}
