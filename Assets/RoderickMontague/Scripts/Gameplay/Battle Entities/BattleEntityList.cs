using JetBrains.Annotations;
using RM_BBTS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_BBTS
{
    // The list of IDs.
    public enum battleEntityId { unknown, treasure, combatbot, ufo1, ufo2, ufo3 }

    // The list of entities for the game. There only needs to be one instance of this list.
    public class BattleEntityList : MonoBehaviour
    {
        // The instance of the opponent list.
        private static BattleEntityList instance;

        // TODO: include list of battle entity sprites

        // The amount of opponents in the list.
        public const int BATTLE_ENTITY_ID_COUNT = 6;

        // Weights should not be negative.
        // The chance rates of the entities.
        private List<int> baseWeights;

        // The adjusted entity weights.
        private List<int> adjustedWeights;

        // Minimum and maximum adjustment values.
        private const int MIN_ADJUST = 0, MAX_ADJUST = 10;

        // The first enemy id (ignores the boss).
        private battleEntityId firstEnemyId = battleEntityId.ufo1;

        // The last enemy id (ignores the boss).
        private battleEntityId lastEnemyId = battleEntityId.ufo3;

        // The list of entities
        public List<Sprite> entitySprites;

        // Constructor - called before the Awake and Start.
        private BattleEntityList()
        {
            // ...
        }

        // Awake is called when the script is loaded.
        private void Awake()
        {
            // Instance.
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
                return;
            }

            // Unity doesn't like the constructor being used when inherting from MonoBehaviour.
            // So, this was all moved here.

            // Creates the entity weights, and lcamps them to the battle entity ID count.
            // <unknown, treasure, and boss should always be 0>
            baseWeights = new List<int> { 0, 0, 0, 30, 30, 30 };

            // If it exceeds the ID count.
            if (baseWeights.Count > BATTLE_ENTITY_ID_COUNT)
            {
                // Removes a range of values so that it's within the range.
                baseWeights.RemoveRange(BATTLE_ENTITY_ID_COUNT, baseWeights.Count - BATTLE_ENTITY_ID_COUNT);
            }

            // Generates the adjusted weights.
            // adjustedWeights = new List<int>(baseWeights);
            RandomizeEntityWeights(MIN_ADJUST, MAX_ADJUST, false);
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
        public BattleEntityGameData GenerateBattleEntityData(battleEntityId id)
        {
            // The data.
            BattleEntityGameData data = new BattleEntityGameData();

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
                    // data.sprite = entitySprites[0];

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

                case battleEntityId.combatbot: // combatbot.

                    data.id = battleEntityId.combatbot;
                    data.displayName = "<Combat Bot>";
                    data.displayNameSpeakKey = "bey_combatbot_nme";

                    data.maxHealth = 1; // 50
                    data.health = 1; // 50

                    data.attack = 20;
                    data.defense = 30;
                    data.speed = 45;

                    data.maxEnergy = 100;
                    data.energy = 100;

                    // Loads the name key.
                    nameKey = "bey_boss_nme";

                    break;

                case battleEntityId.ufo1: // UFO
                    data.id = battleEntityId.ufo1;
                    data.evoId = battleEntityId.ufo2;

                    data.displayName = "<UFO>";
                    data.displayNameSpeakKey = "bey_ufo1_nme";

                    data.maxHealth = 10;
                    data.health = 10;

                    data.attack = 1;
                    data.defense = 1;
                    data.speed = 3;

                    data.maxEnergy = 10;
                    data.energy = 10;

                    // Saves the sprite.
                    // data.sprite = entitySprites[(int)battleEntityId.ufo1];

                    // Set random moves.
                    SetRandomMovesFromList(ref data);
                    // data.move0 = moveId.lasershot;

                    // Loads the name key.
                    nameKey = "bey_ufo1_nme";
                    break;

                case battleEntityId.ufo2: // UFO MKII
                    data.id = battleEntityId.ufo2;
                    data.preEvoId = battleEntityId.ufo1;
                    data.evoId = battleEntityId.ufo3;


                    data.displayName = "<UFO MK II>";
                    data.displayNameSpeakKey = "bey_ufo2_nme";

                    data.maxHealth = 15;
                    data.health = 15;

                    data.attack = 2;
                    data.defense = 5;
                    data.speed = 10;

                    data.maxEnergy = 15;
                    data.energy = 15;

                    // Saves the sprite.
                    // data.sprite = entitySprites[(int)battleEntityId.ufo1];

                    // Set random moves.
                    SetRandomMovesFromList(ref data);

                    // Loads the name key.
                    nameKey = "bey_ufo2_nme";
                    break;

                case battleEntityId.ufo3: // UFO MKIII
                    data.id = battleEntityId.ufo3;
                    data.preEvoId = battleEntityId.ufo2;

                    data.displayName = "<UFO MK III>";
                    data.displayNameSpeakKey = "bey_ufo3_nme";

                    data.maxHealth = 30;
                    data.health = 30;

                    data.attack = 5;
                    data.defense = 10;
                    data.speed = 20;

                    data.maxEnergy = 30;
                    data.energy = 30;

                    // Saves the sprite.
                    // data.sprite = entitySprites[(int)battleEntityId.ufo1];

                    // Set random moves.
                    SetRandomMovesFromList(ref data);

                    // Loads the name key.
                    nameKey = "bey_ufo3_nme";
                    break;

            }

            // Sets the sprite if the entity's ID is a valid number for the list.
            if((int)data.id < entitySprites.Count)
                data.sprite = entitySprites[(int)data.id];

            // Loads the translation for the data.
            if (nameKey != "")
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

                case battleEntityId.combatbot:
                    moveList = new List<moveId>() { moveId.poke };
                    break;

                case battleEntityId.ufo1:
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
        private void SetRandomMovesFromList(ref BattleEntityGameData data)
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

        // Randomizes the entity weights, adding to the base weights, and saving the new weights to 'ajdustedWeights'.
        // Provided are minimum and maximum weight adjustments.
        // Weights cannot go below 0. A '0' means the entity will never be chosen.
        // The min is inclusive, and the max is exclusive since it uses the Random.Range(int, int) function.
        // If 'changeZeroes' is 'true', then entities with a 0 weight will also be affected.
        private void RandomizeEntityWeights(int minChange, int maxChange, bool changeZeroes)
        {
            // The new weights object.
            List<int> newWeights = new List<int>(baseWeights);

            // Goes through each weight.
            for (int i = 0; i < newWeights.Count; i++)
            {
                // If the weight should be changed.
                bool change = true;

                // Weights should never be less than 0.
                if (newWeights[i] < 0)
                    newWeights[i] = 0;

                // If zeroes shouldn't be changed, and the weight is equal to 0, dont' change it.
                if (!changeZeroes && newWeights[i] == 0)
                    change = false;
                    

                // If the values should be changed.
                if(change)
                    newWeights[i] += Random.Range(minChange, maxChange + 1);
            }

            // Saves the new weights.
            adjustedWeights = newWeights;
        }

        // Generates a random battle entity enemy. If 'baseEvo' is true, then the base form is provided.
        public BattleEntityGameData GenerateRandomEnemy(bool useWeights, bool randomWeights, bool baseEvo = true)
        {
            // The data.
            BattleEntityGameData data = new BattleEntityGameData();

            // Gets the random id.
            battleEntityId randomId;

            // Becomes 'true' when an id has been chosen.
            // bool idChosen = false;

            // Sets a random id.
            // This will be overwritten if weights should be used.
            // This was done so that this variable will be set to something.
            randomId = (battleEntityId)Random.Range((int)firstEnemyId, (int)lastEnemyId + 1);


            // Checks if enemy weights should be used.
            if (useWeights && baseWeights.Count != 0 && adjustedWeights.Count != 0)
            {
                // Gets the weights.
                List<int> weights = (randomWeights) ? adjustedWeights : baseWeights;

                // Saves the sum of the weights.
                int weightSum = 0;

                // Adds to the weight sum.
                foreach (int w in weights)
                    weightSum += w;

                // An entity can be found.
                if(weightSum > 0)
                {
                    // Gets a random int.
                    int randValue = Random.Range(1, weightSum + 1);

                    // The index of the id to be chosen.
                    int idNum = -1;

                    // Reusing the variable.
                    weightSum = 0;

                    // Finds the entity.
                    for(int i = 0; i < weights.Count; i++)
                    {
                        // Adds to the weight sum.
                        weightSum += weights[i];

                        // Battle Entity Found.
                        if(randValue <= weightSum)
                        {
                            idNum = i;
                            break;
                        }
                    }

                    // The id is valid, so use it. Also show that an id has been chosen.
                    if (idNum >= 0 && idNum <= (int)lastEnemyId)
                    {
                        randomId = (battleEntityId)idNum;
                        // idChosen = true;
                    }
                }
                
            }

            // // If the id hasn't been chosen already, just use a random vlue.
            // if(!idChosen)
            // {
            //     randomId = (battleEntityId)Random.Range((int)firstEnemyId, (int)lastEnemyId + 1);
            //     idChosen = true;
            // }
                

            // Gets the random data.
            data = GenerateBattleEntityData(randomId);
            
            // TODO: make this more efficient?

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
        public BattleEntityGameData GenerateBoss()
        {
            return GenerateBattleEntityData(battleEntityId.combatbot);
        }
    }

}
