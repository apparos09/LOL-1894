using RM_BBTS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_BBTS
{
    // The list of IDs.
    public enum battleEntityId { unknown, treasure, boss, ufo, ufo2, ufo3 }

    // The list of entities for the game. There only needs to be one instance of this list.
    public class BattleEntityList : MonoBehaviour
    {
        // The instance of the opponent list.
        private static BattleEntityList instance;

        // TODO: include list of battle entity sprites

        // The amount of opponents in the list.
        public const int BATTLE_ENTITY_ID_COUNT = 6;

        // The first enemy id (ignores the boss).
        private battleEntityId firstEnemyId = battleEntityId.ufo;

        // The last enemy id (ignores the boss).
        private battleEntityId lastEnemyId = battleEntityId.ufo3;

        // The list of entities
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
            // The data.
            BattleEntityData data = new BattleEntityData();

            // The namekey.
            string nameKey = "";

            // All entities start at level 1, and by default the evo values are set to 'unknown' (i.e. not set).
            data.level = 1;
            data.preEvoId = battleEntityId.unknown;
            data.evoId = battleEntityId.unknown;

            switch (id)
            {
                // An unknown battle entity.
                case battleEntityId.unknown:
                default:
                    data.id = battleEntityId.unknown;
                    data.displayName = "<Unknown>";
                    data.displayNameSpeakKey = "bey_unknown_nme";

                    data.maxHealth = 10;
                    data.health = 10;

                    data.attack = 3;
                    data.defense = 2;
                    data.speed = 1;

                    data.maxEnergy = 10;
                    data.energy = 10;

                    data.move0 = moveId.poke;

                    // Saves the sprite.
                    data.sprite = entitySprites[0];

                    // Loads the name key.
                    nameKey = "bey_unknown_nme";

                    break;

                case battleEntityId.treasure: // treasure chest
                    data.id = battleEntityId.treasure;
                    data.displayName = "<Treasure>";
                    data.displayNameSpeakKey = "bey_treasure_nme";

                    data.maxHealth = 1;
                    data.health = 1;

                    data.attack = 1;
                    data.defense = 1;
                    data.speed = 1;

                    data.maxEnergy = 1;
                    data.energy = 1;

                    // Loads the name key.
                    nameKey = "bey_treasure_nme";
                    break;

                case battleEntityId.boss: // boss

                    data.id = battleEntityId.boss;
                    data.displayName = "<Boss>";
                    data.displayNameSpeakKey = "bey_boss_nme";

                    data.maxHealth = 50;
                    data.health = 50;

                    data.attack = 20;
                    data.defense = 30;
                    data.speed = 45;

                    data.maxEnergy = 100;
                    data.energy = 100;

                    // Loads the name key.
                    nameKey = "bey_boss_nme";

                    break;

                case battleEntityId.ufo: // UFO
                    data.id = battleEntityId.ufo;
                    data.evoId = battleEntityId.ufo2;

                    data.displayName = "<UFO>";
                    data.displayNameSpeakKey = "bey_ufo_nme";

                    data.maxHealth = 10;
                    data.health = 10;

                    data.attack = 1;
                    data.defense = 1;
                    data.speed = 3;

                    data.maxEnergy = 10;
                    data.energy = 10;

                    // Saves the sprite.
                    data.sprite = entitySprites[(int)battleEntityId.ufo];

                    // Set random moves.
                    SetRandomMovesFromList(ref data);
                    // data.move0 = moveId.lasershot;

                    // Loads the name key.
                    nameKey = "bey_ufo_nme";
                    break;

                case battleEntityId.ufo2: // UFO MKII
                    data.id = battleEntityId.ufo2;
                    data.preEvoId = battleEntityId.ufo;
                    data.evoId = battleEntityId.ufo3;


                    data.displayName = "<UFO MKII>";
                    data.displayNameSpeakKey = "bey_ufo2_nme_alt";

                    data.maxHealth = 15;
                    data.health = 15;

                    data.attack = 2;
                    data.defense = 5;
                    data.speed = 10;

                    data.maxEnergy = 15;
                    data.energy = 15;

                    // Saves the sprite.
                    data.sprite = entitySprites[(int)battleEntityId.ufo];

                    // Set random moves.
                    SetRandomMovesFromList(ref data);

                    // Loads the name key.
                    nameKey = "bey_ufo2_nme";
                    break;

                case battleEntityId.ufo3: // UFO MKIII
                    data.id = battleEntityId.ufo3;
                    data.preEvoId = battleEntityId.ufo2;

                    data.displayName = "<UFO MKIII>";
                    data.displayNameSpeakKey = "bey_ufo3_nme_alt";

                    data.maxHealth = 30;
                    data.health = 30;

                    data.attack = 5;
                    data.defense = 10;
                    data.speed = 20;

                    data.maxEnergy = 30;
                    data.energy = 30;

                    // Saves the sprite.
                    data.sprite = entitySprites[(int)battleEntityId.ufo];

                    // Set random moves.
                    SetRandomMovesFromList(ref data);

                    // Loads the name key.
                    nameKey = "bey_ufo3_nme";
                    break;

            }

            // Loads the translation for the data.
            if(nameKey != "")
                BattleEntity.LoadTranslationForData(ref data, nameKey);

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
                default:
                    moveList = new List<moveId>() { moveId.poke };
                    break;

                case battleEntityId.treasure:
                    moveList = new List<moveId>() { moveId.poke };
                    break;

                case battleEntityId.boss:
                    moveList = new List<moveId>() { moveId.poke };
                    break;

                case battleEntityId.ufo:
                    moveList = new List<moveId>() { moveId.slimeshot, moveId.lasershot, moveId.fireshot, moveId.elecshot };
                    break;

                case battleEntityId.ufo2:
                    moveList = new List<moveId>() { moveId.slimeshot, moveId.lasershot, moveId.fireshot, moveId.elecshot };
                    break;

                case battleEntityId.ufo3:
                    moveList = new List<moveId>() { moveId.slimeshot, moveId.lasershot, moveId.fireshot, moveId.elecshot };
                    break;
            }

            return moveList;
        }

        // Sets random moves for the UFO.
        private void SetRandomMovesFromList(ref BattleEntityData data)
        {
            // Grabs the move list.
            List<moveId> moveList = GenerateBattleEntityMoveList(data.id);

            // List of moves added.
            int count = 0;

            // Grab the moves.
            while(count < 4 && moveList.Count != 0)
            {
                // Grabs the random index.
                int index = Random.Range(0, moveList.Count);

                // Check current count to know what move slot to fill.
                switch (count)
                {
                    case 0:
                        data.move0 = moveList[index];
                        break;
                    case 1:
                        data.move1 = moveList[index];
                        break;
                    case 2:
                        data.move2 = moveList[index];
                        break;
                    case 3:
                        data.move3 = moveList[index];
                        break;
                }

                // Removes at the current index.
                moveList.RemoveAt(index);

                // Increase count.
                count++;
            }
        }

        // Generates a random battle entity enemy. If 'baseEvo' is true, then the base form is provided.
        public BattleEntityData GenerateRandomEnemy(bool baseEvo = true)
        {
            // The data.
            BattleEntityData data = new BattleEntityData();
            
            // Gets the random id.
            battleEntityId randomId = (battleEntityId)Random.Range((int)firstEnemyId, (int)lastEnemyId + 1);

            // Gets the random data.
            data = GenerateBattleEntityData(randomId);
            
            // TODO: make this mroe efficient?

            // If the base evo should be returned.
            if(baseEvo)
            {
                // Counts the amount of changes.
                int changes = 0;

                // While the pre-evo is not set to unknown (base form).
                // No enemy should have more than 3 forms, so only two changes should be needed.
                while(data.preEvoId != battleEntityId.unknown && changes < 2)
                {
                    // Goes to the base form.
                    data = GenerateBattleEntityData(data.preEvoId);

                    // A change has been made.
                    changes++;
                }
            }

            return data;
        }

        // Generates the boss.
        public BattleEntityData GenerateBoss()
        {
            return GenerateBattleEntityData(battleEntityId.boss);
        }
    }

}
