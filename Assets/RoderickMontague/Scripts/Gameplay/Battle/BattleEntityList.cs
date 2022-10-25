using RM_BBTS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_BBTS
{
public enum battleEntityId { unknown, treasure }

// The list of entities for the game. There only needs to be one instance of this list.
public class BattleEntityList : MonoBehaviour
{
    // The instance of the opponent list.
    private static BattleEntityList instance;

    // TODO: include list of battle entity sprites

    // The amount of opponents in the list.
    public static int OPPONENT_COUNT = 1;

    public List<Sprite> entitySprites;

    // Constructor.
    private BattleEntityList()
    {
    }

    // Awake is called when the script is loaded.
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    // Gets the instance.
    public static BattleEntityList Instance
    {
        get
        {
            // Generates the instance if it isn't set.
            if (instance == null)
            {
                // Searches for the instance if it is not set.
                instance = FindObjectOfType<BattleEntityList>(true);

                // No instance found, so make a new object.
                if(instance == null)
                {
                    GameObject go = new GameObject("Battle Entity List");
                    instance = go.AddComponent<BattleEntityList>();
                }    
                
            }

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

    // Generates BE00 - Unknown
    public BattleEntity GenerateBE00()
    {
        // TODO: change to generating an enemy.
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

}
