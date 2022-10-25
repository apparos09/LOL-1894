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

    // The rank of the move.
    protected int rank;

    // The power that a move has.
    protected float power;

    // The accuracy of a move (0.0 - 1.0)
    protected float accuracy;

    // The amount of energy a move uses.
    protected float energy;

    // TODO: add space for animation.

    // The description of a move.
    public string description = "";

    // TODO: replace name with file citation for translation.
    // Move constructor
    public Move(uint id, string name, int rank, float power, float accuracy, float energy)
    {
        this.id = id;
        this.name = name;
        this.rank = rank;
        this.power = power;
        this.accuracy = accuracy;
        this.energy = energy;

        // Default message.
        description = "No information available";
    }



    // Returns the name of the move.
    public string Name
    {
        get { return name; }
    }

    // Returns the power of the move.
    public float Power
    {
        get { return power; }
    }

    // Returns the accuracy of the move (0-1 range).
    public float Accuracy
    {
        get { return accuracy; }
    }

    // Returns the energy the move uses.
    public float Energy
    {
        get { return energy; }
    }
}
