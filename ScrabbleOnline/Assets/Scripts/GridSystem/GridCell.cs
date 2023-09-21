using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public struct GridCell : IEquatable<GridCell>, INetworkSerializable
{
    
    int x;
    int z;
    char letter;

    public GridCell(int x, int z, char letter)
    {
        this.x = x;
        this.z = z;
        this.letter = letter;
    }

    public int GetX()
    {
        return x;
    }

    public int GetZ()
    {
        return z;
    }

    public char GetLetter()
    {
        return letter;
    }

    public bool Equals(GridCell other)
    {
        return x == other.x && z == other.z;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref x);
        serializer.SerializeValue(ref z);
        serializer.SerializeValue(ref letter);
    }
}
