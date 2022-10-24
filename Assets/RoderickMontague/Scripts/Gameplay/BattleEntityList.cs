using RM_BBTS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The list of entities for the game. There only needs to be one instance of this list.
public class BattleEntityList : MonoBehaviour
{
    // The instance of the opponent list.
    private static BattleEntityList instance;

    // TODO: include list of battle entity sprites

    // The amount of opponents in the list.
    public static int OPPONENT_COUNT = 1;

    // Constructor.
    private BattleEntityList()
    {
    }

    // Gets the instance.
    public static BattleEntityList Instance
    {
        get
        {
            if (instance == null)
                instance = new BattleEntityList();

            return instance;
        }
        
    }

    // Generates and returns a battle entity.
    public BattleEntity GenerateBattleEntity(int id)
    {
        switch(id)
        {
            case 0:
                return null;
            case 1: // treasure chest
                return null;
        }

        return null;
    }

    // Generates BE00 - Player/Copy
    public BattleEntity GenerateBE00()
    {
        GameObject go = new GameObject("Battle Entity");
        BattleEntity be = go.AddComponent<BattleEntity>();
        return be;
    }

    // Generate BE00's move list.
    public List<int> GetBE00MoveList()
    {
        return null;
    }

    // BE01 - Treasure Chest
    public BattleEntity GenerateBE01()
    {
        return null;
    }


    // Generate BE01's move list.
    public List<int> GetBE01MoveList()
    {
        return null;
    }
}
