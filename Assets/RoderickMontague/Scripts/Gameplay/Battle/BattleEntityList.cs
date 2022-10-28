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
                    if (instance == null)
                    {
                        GameObject go = new GameObject("Battle Entity List");
                        instance = go.AddComponent<BattleEntityList>();
                    }

                }

                return instance;
            }

        }

        // Generates and returns a battle entity with its base stats (stats it has at level 1).
        public BattleEntityData GenerateBattleEntityData(battleEntityId id)
        {
            BattleEntityData data = new BattleEntityData();
            switch (id)
            {
                // An unknown battle entity.
                case battleEntityId.unknown:
                    data.id = battleEntityId.unknown;
                    data.level = 1;

                    data.maxHealth = 1;
                    data.health = 1;

                    data.attack = 1;
                    data.defense = 1;
                    data.speed = 1;

                    data.maxEnergy = 1;
                    data.energy = 1;

                    break;

                case battleEntityId.treasure: // treasure chest
                    data.id = battleEntityId.treasure;
                    data.level = 1;

                    data.maxHealth = 1;
                    data.health = 1;

                    data.attack = 1;
                    data.defense = 1;
                    data.speed = 1;

                    data.maxEnergy = 1;
                    data.energy = 1;
                    break;
            }

            return data;
        }

        // Generates a battle entity move list.
        public List<moveId> GenerateBattleEntityMoveList(battleEntityId id)
        {
            // The move list.
            List<moveId> moveList = new List<moveId>();

            // Checks the ID of the battleEntity to get its move list.
            switch(id)
            {
                case battleEntityId.unknown:
                    moveList = new List<moveId>() { moveId.hit };
                    break;

                case battleEntityId.treasure:
                    moveList = new List<moveId>() { moveId.hit };
                    break;

                default:
                    moveList = new List<moveId>();
                    break;
            }

            return moveList;
        }
    }

}
