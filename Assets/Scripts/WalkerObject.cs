using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerObject
{
    public Vector2 Position;
    public Vector2 Direction;
    public float ChangeProbability;

    public WalkerObject(Vector2 p, Vector2 d, float changeProbability)
    {
        Position = p;
        Direction = d;
        ChangeProbability = changeProbability;
    }
}
