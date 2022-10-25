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
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    // Gets the instance.
    public static MoveList Instance
    {
        get
        {
            // Generates the instance if it isn't set.
            if (instance == null)
            {
                // Searches for the instance if it is not set.
                instance = FindObjectOfType<MoveList>(true);

                // No instance found, so make a new object.
                if (instance == null)
                {
                    GameObject go = new GameObject("Move List");
                    instance = go.AddComponent<MoveList>();
                }

            }

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

    // MV00 - Debug
    public Move GenerateMV00()
    {
        return new Move(0, "ATK", 1, 1.0F, 1.0F, 1.0F);
    }

    // MV01 - Bam
    public Move GenerateMV01()
    {
        return new Move(1, "Bam", 1, 1.0F, 1.0F, 1.0F);
    }

    // MV02 - Wham
    public Move GenerateMV02()
    {
        return new Move(2, "Wham", 2, 1.0F, 1.0F, 1.0F);
    }

    // MV03 - Kablam
    public Move GenerateMV03()
    {
        return new Move(3, "Kablam", 3, 1.0F, 1.0F, 1.0F);
    }
}
