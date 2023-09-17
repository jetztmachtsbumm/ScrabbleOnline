using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public struct GridCell : IEquatable<GridCell>, INetworkSerializable
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

    public bool Equals(GridCell other)
    {
        return x == other.x && z == other.z;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref x);
        serializer.SerializeValue(ref z);
    }
}
