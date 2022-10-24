using RM_BBTS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The list of moves for the game.
public class MoveList : MonoBehaviour
{
    // The instance of the move list.
    private static MoveList instance;

    // TODO: include list of move animations.

    // Constructor.
    private MoveList()
    {
    }

    // Gets the instance.
    public static MoveList Instance
    {
        get
        {
            if (instance == null)
                instance = new MoveList();

            return instance;
        }
        
    }

    // Generates and returns a battle entity.
    public Move GenerateMove(int id)
    {
        switch (id)
        {
            case 0:
                return null;
            case 1: // ...
                return null;
        }

        return null;
    }

    public Move GenerateMV00()
    {
        return null;
    }

    // MV01 - ...
    public Move GenerateMV01()
    {
        return null;
    }

    // MV02 - ...
    public Move GenerateMV02()
    {
        return null;
    }
}
