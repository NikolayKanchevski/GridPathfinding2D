using System;
using System.Collections.Generic;
using UnityEngine;
using Ultimate.Algorithms.AStar;

public class Node
{
    public int X { get; set; }
    public int Y { get; set; }

    public int Cost { get; set; }
    public int Distance { get; set; }
    public int CostDistance => Cost + Distance;

    public bool Walkable = false;
    public float penalty;

    public int gCost;
    public int hCost;
    public Node parent;

    public Node(float _price, int _gridX, int _gridY)
    {
        penalty = _price;
        X = _gridX;
        Y= _gridY;

        Walkable = penalty == 1f;
    }

    public void SetPenalty(float price)
    {
        penalty = price;

        Walkable = penalty == 1f;
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
}