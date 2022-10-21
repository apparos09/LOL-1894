using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A class for a move.
public class Move
{
    // The number of the move.
    private uint id;

    // The name of the move.
    protected string name;

    // The power that a move has.
    protected float power;

    // The accuracy of a move.
    protected float accuracy;

    // TODO: create variables for battle effects.
    public string Name
    {
        get { return name; }
    }
}
