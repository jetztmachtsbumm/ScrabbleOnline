using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public struct LetterData : INetworkSerializable
{

    char letter;
    int totalAmount;
    int score;

    public LetterData(char letter, int totalAmount, int score)
    {
        this.letter = letter;
        this.totalAmount = totalAmount;
        this.score = score;
    }

    public char GetLetter()
    {
        return letter;
    }

    public int GetTotalAmount()
    {
        return totalAmount;
    }

    public int GetScore()
    {
        return score;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref letter);
        serializer.SerializeValue(ref totalAmount);
        serializer.SerializeValue(ref score);
    }
}
