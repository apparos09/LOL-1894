using RM_BBTS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_BBTS
{
    // The list of IDs. CombatBot, Comet, and BlackHole are all bosses.
    public enum battleEntityId { 
        unknown, treasure, combatBot, ufo1, ufo2, ufo3, insect1, insect2, spaceGhost1, spaceGhost2, comet,
        sunRock1, sunRock2, moonRock1, moonRock2, fireBot1, fireBot2, waterBot1, waterBot2, earthBot1, 
        earthBot2, airBot1, airBot2, sharp1, sharp2, virusRed1, virusRed2, virusBlue1, virusBlue2, 
        virusYellow1, virusYellow2, blackHole, planet1, planet2
    }

    // The list of entities for the game. There only needs to be one instance of this list.
    public class BattleEntityList : MonoBehaviour
    {
        // The instance of the opponent list.
        private static BattleEntityList instance;

        // The amount of opponents in the list.
        public const int BATTLE_ENTITY_ID_COUNT = 34;

        // Weights should not be negative.
        // The chance rates of the entities.
        private List<int> baseWeights;

        // The adjusted entity weights.
        private List<int> adjustedWeights;

        // Minimum and maximum adjustment values.
        private const int MIN_ADJUST = 0, MAX_ADJUST = 10;

        // The first enemy id (combat bot is currently a boss.).
        private const battleEntityId FIRST_ENEMY_ID = battleEntityId.combatBot;

        // The last enemy id (ignores the boss).
        private const battleEntityId LAST_ENEMY_ID = battleEntityId.planet2;

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
            // <unknown, treasure, and bosses should always be 0>
            baseWeights = new List<int> {
                0, 0, 0, 40, 0, 0, 30, 0, 15, 0, 0, 20,
                0, 20, 0, 10, 0, 10, 0, 10, 0, 10, 0,
                20, 0, 30, 0, 30, 0, 30, 0, 0, 5, 0
            };

            // If it exceeds the ID count.
            if (baseWeights.Count > BATTLE_ENTITY_ID_COUNT)
            {
                // Removes a range of values so that it's within the range.
                baseWeights.RemoveRange(BATTLE_ENTITY_ID_COUNT, baseWeights.Count - BATTLE_ENTITY_ID_COUNT);
            }

            // TODO: maybe take this out? Maybe it's not needed.
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

            // If set to 'true', the entity is given random moves.
            bool randomMoves = true;

            // All entities start at level 1, and by default the evo values are set to 'unknown' (i.e. not set).
            // The player is the only entity that doesn't have a 1.0F level rate.
            data.level = 1;
            data.levelRate = 1.0F;

            data.preEvoId = battleEntityId.unknown;
            data.evoId = battleEntityId.unknown;

            // This will be modified in the specific case statement for the entity.
            data.statSpecial = BattleEntity.specialty.none;

            switch (id)
            {
                // An unknown battle entity.
                case battleEntityId.unknown: // Unknown
                default:
                    data.id = battleEntityId.unknown;
                    data.displayName = "Unknown";
                    data.displayNameSpeakKey = "bey_unknown_nme";

                    data.maxHealth = 10;
                    data.health = 10;

                    data.attack = 3;
                    data.defense = 2;
                    data.speed = 1;

                    data.maxEnergy = 100;
                    data.energy = 100;

                    data.statSpecial = BattleEntity.specialty.none;

                    // Sets the default moves.
                    data.move0 = moveId.poke;
                    data.move1 = moveId.bam;
                    data.move2 = moveId.wham;
                    data.move3 = moveId.kablam;
                    randomMoves = false;

                    break;

                case battleEntityId.treasure: // 1. Treasure Chest
                    data.id = battleEntityId.treasure;
                    data.displayName = "Treasure";
                    data.displayNameSpeakKey = "bey_treasure_nme";

                    data.maxHealth = 1;
                    data.health = 1;

                    data.attack = 1;
                    data.defense = 1;
                    data.speed = 1;

                    data.maxEnergy = 1;
                    data.energy = 1;

                    data.statSpecial = BattleEntity.specialty.none;

                    data.move0 = moveId.bam;
                    randomMoves = false;

                    break;

                case battleEntityId.combatBot: // Combat Bot

                    data.id = battleEntityId.combatBot;
                    data.displayName = "Combat Bot";
                    data.displayNameSpeakKey = "bey_combatBot_nme";

                    data.maxHealth = 150;
                    data.health = data.maxHealth;

                    data.attack = 145;
                    data.defense = 95;
                    data.speed = 110;

                    data.maxEnergy = 100;
                    data.energy = data.maxEnergy;

                    data.statSpecial = BattleEntity.specialty.attack;

                    break;

                case battleEntityId.ufo1: // UFO
                    data.id = battleEntityId.ufo1;
                    data.preEvoId = battleEntityId.unknown;
                    data.evoId = battleEntityId.ufo2;

                    data.displayName = "UFO";
                    data.displayNameSpeakKey = "bey_ufo1_nme";

                    data.maxHealth = 45;
                    data.health = data.maxHealth;

                    data.attack = 20;
                    data.defense = 20;
                    data.speed = 50;

                    data.statSpecial = BattleEntity.specialty.speed;

                    data.maxEnergy = 100;
                    data.energy = data.maxEnergy;

                    // Saves the sprite.
                    // data.sprite = entitySprites[(int)battleEntityId.ufo1];

                    // data.move0 = moveId.lasershot;
                    break;

                case battleEntityId.ufo2: // UFO MK 2
                    data.id = battleEntityId.ufo2;
                    data.preEvoId = battleEntityId.ufo1;
                    data.evoId = battleEntityId.ufo3;


                    data.displayName = "UFO MK 2";
                    data.displayNameSpeakKey = "bey_ufo2_nme";

                    // Stats
                    data.maxHealth = 70;
                    data.health = data.maxHealth;

                    data.attack = 65;
                    data.defense = 95;
                    data.speed = 20;

                    data.maxEnergy = 100;
                    data.energy = data.maxEnergy;

                    data.statSpecial = BattleEntity.specialty.defense;
                    break;

                case battleEntityId.ufo3: // UFO MK 3
                    data.id = battleEntityId.ufo3;
                    data.preEvoId = battleEntityId.ufo2;
                    data.evoId = battleEntityId.unknown;

                    data.displayName = "UFO MK 3";
                    data.displayNameSpeakKey = "bey_ufo3_nme";

                    // Stats
                    data.maxHealth = 100;
                    data.health = data.maxHealth;

                    data.attack = 105;
                    data.defense = 90;
                    data.speed = 110;

                    data.maxEnergy = 100;
                    data.energy = data.maxEnergy;

                    data.statSpecial = BattleEntity.specialty.none;
                    break;

                case battleEntityId.insect1: // Starfly
                    data.id = battleEntityId.insect1;
                    data.preEvoId = battleEntityId.unknown;
                    data.evoId = battleEntityId.insect2;


                    data.displayName = "Starfly";
                    data.displayNameSpeakKey = "bey_insect1_nme";

                    // Stats
                    data.maxHealth = 55;
                    data.health = data.maxHealth;

                    data.attack = 25;
                    data.defense = 25;
                    data.speed = 35;

                    data.maxEnergy = 100;
                    data.energy = data.maxEnergy;

                    data.statSpecial = BattleEntity.specialty.speed;
                    data.levelRate = 1.10F;

                    break;

                case battleEntityId.insect2: // Spacectoid
                    data.id = battleEntityId.insect2;
                    data.preEvoId = battleEntityId.insect1;
                    data.evoId = battleEntityId.unknown;

                    // Stats
                    data.displayName = "Spacectoid";
                    data.displayNameSpeakKey = "bey_insect2_nme";

                    data.maxHealth = 80;
                    data.health = data.maxHealth;

                    data.attack = 50;
                    data.defense = 30;
                    data.speed = 90;

                    data.maxEnergy = 100;
                    data.energy = data.maxEnergy;

                    data.statSpecial = BattleEntity.specialty.speed;
                    data.levelRate = 1.15F;

                    break;

                case battleEntityId.spaceGhost1: // Techno Ghost
                    data.id = battleEntityId.spaceGhost1;
                    data.preEvoId = battleEntityId.unknown;
                    data.evoId = battleEntityId.spaceGhost2;

                    // Stats
                    data.displayName = "Techno Ghost";
                    data.displayNameSpeakKey = "bey_ghost1_nme";

                    data.maxHealth = 30;
                    data.health = data.maxHealth;

                    data.attack = 25;
                    data.defense = 35;
                    data.speed = 20;

                    data.maxEnergy = 100;
                    data.energy = data.maxEnergy;

                    data.statSpecial = BattleEntity.specialty.defense;
                    break;

                case battleEntityId.spaceGhost2: // Techno Phantom
                    data.id = battleEntityId.spaceGhost2;
                    data.preEvoId = battleEntityId.spaceGhost1;
                    data.evoId = battleEntityId.unknown;

                    // Stats
                    data.displayName = "Techno Phantom";
                    data.displayNameSpeakKey = "bey_ghost2_nme";

                    data.maxHealth = 60;
                    data.health = data.maxHealth;

                    data.attack = 45;
                    data.defense = 75;
                    data.speed = 30;

                    data.maxEnergy = 100;
                    data.energy = data.maxEnergy;

                    data.statSpecial = BattleEntity.specialty.defense;
                    break;

                case battleEntityId.comet: // Comet (BOSS)
                    data.id = battleEntityId.comet;
                    data.preEvoId = battleEntityId.unknown;
                    data.evoId = battleEntityId.unknown;

                    // Stats
                    data.displayName = "Comet";
                    data.displayNameSpeakKey = "bey_comet_nme";

                    data.maxHealth = 150;
                    data.health = data.maxHealth;

                    data.attack = 110;
                    data.defense = 90;
                    data.speed = 150;

                    data.maxEnergy = 100;
                    data.energy = data.maxEnergy;

                    data.statSpecial = BattleEntity.specialty.speed;
                    break;

                case battleEntityId.sunRock1: // Stellosis
                    data.id = battleEntityId.sunRock1;
                    data.preEvoId = battleEntityId.unknown;
                    data.evoId = battleEntityId.sunRock2;

                    // Stats
                    data.displayName = "Stellosis";
                    data.displayNameSpeakKey = "bey_sunRock1_nme";

                    data.maxHealth = 42;
                    data.health = data.maxHealth;

                    data.attack = 36;
                    data.defense = 32;
                    data.speed = 10;

                    data.maxEnergy = 100;
                    data.energy = data.maxEnergy;

                    data.statSpecial = BattleEntity.specialty.attack;
                    break;

                case battleEntityId.sunRock2: // Sunoliss
                    data.id = battleEntityId.sunRock2;
                    data.preEvoId = battleEntityId.sunRock1;
                    data.evoId = battleEntityId.unknown;

                    // Stats
                    data.displayName = "Sunoliss";
                    data.displayNameSpeakKey = "bey_sunRock2_nme";

                    data.maxHealth = 62;
                    data.health = data.maxHealth;

                    data.attack = 86;
                    data.defense = 41;
                    data.speed = 71;

                    data.maxEnergy = 100;
                    data.energy = data.maxEnergy;

                    data.statSpecial = BattleEntity.specialty.attack;
                    break;

                case battleEntityId.moonRock1: // Stelluna
                    data.id = battleEntityId.moonRock1;
                    data.preEvoId = battleEntityId.unknown;
                    data.evoId = battleEntityId.moonRock2;

                    // Stats
                    data.displayName = "Stelluna";
                    data.displayNameSpeakKey = "bey_moonRock1_nme";

                    data.maxHealth = 42;
                    data.health = data.maxHealth;

                    data.attack = 32;
                    data.defense = 36;
                    data.speed = 10;

                    data.maxEnergy = 100;
                    data.energy = data.maxEnergy;

                    data.statSpecial = BattleEntity.specialty.defense;
                    break;

                case battleEntityId.moonRock2: // Lunooma
                    data.id = battleEntityId.moonRock2;
                    data.preEvoId = battleEntityId.moonRock1;
                    data.evoId = battleEntityId.unknown;

                    // Stats
                    data.displayName = "Lunooma";
                    data.displayNameSpeakKey = "bey_moonRock2_nme";

                    data.maxHealth = 62;
                    data.health = data.maxHealth;

                    data.attack = 41;
                    data.defense = 86;
                    data.speed = 71;

                    data.maxEnergy = 100;
                    data.energy = data.maxEnergy;

                    data.statSpecial = BattleEntity.specialty.defense;

                    break;

                case battleEntityId.fireBot1: // Pyrobot
                    data.id = battleEntityId.fireBot1;
                    data.preEvoId = battleEntityId.unknown;
                    data.evoId = battleEntityId.fireBot2;

                    // Stats
                    data.displayName = "Pyrobot";
                    data.displayNameSpeakKey = "bey_fireBot1_nme";

                    data.maxHealth = 40;
                    data.health = data.maxHealth;

                    data.attack = 60;
                    data.defense = 25;
                    data.speed = 25;

                    data.maxEnergy = 100;
                    data.energy = data.maxEnergy;

                    data.statSpecial = BattleEntity.specialty.attack;
                    data.levelRate = 1.00F;

                    break;

                case battleEntityId.fireBot2: // Pyrobot MK 2
                    data.id = battleEntityId.fireBot2;
                    data.preEvoId = battleEntityId.fireBot1;
                    data.evoId = battleEntityId.unknown;

                    // Stats
                    data.displayName = "Pyrobot MK 2";
                    data.displayNameSpeakKey = "bey_fireBot2_nme";

                    data.maxHealth = 95;
                    data.health = data.maxHealth;

                    data.attack = 100;
                    data.defense = 50;
                    data.speed = 55;

                    data.maxEnergy = 100;
                    data.energy = data.maxEnergy;

                    data.statSpecial = BattleEntity.specialty.attack;
                    data.levelRate = 1.00F;

                    break;

                case battleEntityId.waterBot1: // Aquadroid
                    data.id = battleEntityId.waterBot1;
                    data.preEvoId = battleEntityId.unknown;
                    data.evoId = battleEntityId.waterBot2;

                    // Stats
                    data.displayName = "Aquadroid";
                    data.displayNameSpeakKey = "bey_waterBot1_nme";

                    data.maxHealth = 90;
                    data.health = data.maxHealth;

                    data.attack = 20;
                    data.defense = 20;
                    data.speed = 20;

                    data.maxEnergy = 100;
                    data.energy = data.maxEnergy;

                    data.statSpecial = BattleEntity.specialty.health;
                    data.levelRate = 1.00F;

                    break;

                case battleEntityId.waterBot2: // Aquadroid MK 2
                    data.id = battleEntityId.waterBot2;
                    data.preEvoId = battleEntityId.waterBot1;
                    data.evoId = battleEntityId.unknown;

                    // Stats
                    data.displayName = "Aquadroid MK 2";
                    data.displayNameSpeakKey = "bey_waterBot2_nme";

                    data.maxHealth = 110;
                    data.health = data.maxHealth;

                    data.attack = 70;
                    data.defense = 40;
                    data.speed = 80;

                    data.maxEnergy = 100;
                    data.energy = data.maxEnergy;

                    data.statSpecial = BattleEntity.specialty.health;
                    data.levelRate = 1.00F;

                    break;

                case battleEntityId.earthBot1: // Terrachine
                    data.id = battleEntityId.earthBot1;
                    data.preEvoId = battleEntityId.unknown;
                    data.evoId = battleEntityId.earthBot2;

                    // Stats
                    data.displayName = "Terrachine";
                    data.displayNameSpeakKey = "bey_earthBot1_nme";

                    data.maxHealth = 30;
                    data.health = data.maxHealth;

                    data.attack = 30;
                    data.defense = 60;
                    data.speed = 30;

                    data.maxEnergy = 100;
                    data.energy = data.maxEnergy;

                    data.statSpecial = BattleEntity.specialty.defense;
                    data.levelRate = 1.00F;

                    break;

                case battleEntityId.earthBot2: // Terrachine MK 2
                    data.id = battleEntityId.earthBot2;
                    data.preEvoId = battleEntityId.earthBot1;
                    data.evoId = battleEntityId.unknown;

                    // Stats
                    data.displayName = "Terrachine MK 2";
                    data.displayNameSpeakKey = "bey_earthBot2_nme";

                    data.maxHealth = 70;
                    data.health = data.maxHealth;

                    data.attack = 70;
                    data.defense = 90;
                    data.speed = 70;

                    data.maxEnergy = 100;
                    data.energy = data.maxEnergy;

                    data.statSpecial = BattleEntity.specialty.defense;
                    data.levelRate = 1.00F;

                    break;

                case battleEntityId.airBot1: // Airtomaton
                    data.id = battleEntityId.airBot1;
                    data.preEvoId = battleEntityId.unknown;
                    data.evoId = battleEntityId.airBot2;

                    // Stats
                    data.displayName = "Airtomaton";
                    data.displayNameSpeakKey = "bey_airBot1_nme";

                    data.maxHealth = 40;
                    data.health = data.maxHealth;

                    data.attack = 25;
                    data.defense = 20;
                    data.speed = 65;

                    data.maxEnergy = 100;
                    data.energy = data.maxEnergy;

                    data.statSpecial = BattleEntity.specialty.speed;
                    data.levelRate = 1.00F;

                    break;

                case battleEntityId.airBot2: // Airtomaton MK 2
                    data.id = battleEntityId.airBot2;
                    data.preEvoId = battleEntityId.airBot1;
                    data.evoId = battleEntityId.unknown;

                    // Stats
                    data.displayName = "Airtomaton MK 2";
                    data.displayNameSpeakKey = "bey_airBot2_nme";

                    data.maxHealth = 90;
                    data.health = data.maxHealth;

                    data.attack = 65;
                    data.defense = 45;
                    data.speed = 100;

                    data.maxEnergy = 100;
                    data.energy = data.maxEnergy;

                    data.statSpecial = BattleEntity.specialty.speed;
                    data.levelRate = 1.00F;

                    break;

                case battleEntityId.sharp1: // Inkarp
                    data.id = battleEntityId.sharp1;
                    data.preEvoId = battleEntityId.unknown;
                    data.evoId = battleEntityId.sharp2;

                    // Stats
                    data.displayName = "Inkarp";
                    data.displayNameSpeakKey = "bey_sharp1_nme";

                    data.maxHealth = 40;
                    data.health = data.maxHealth;

                    data.attack = 50;
                    data.defense = 20;
                    data.speed = 60;

                    data.maxEnergy = 100;
                    data.energy = data.maxEnergy;

                    data.statSpecial = BattleEntity.specialty.attack;
                    data.levelRate = 1.05F;

                    break;

                case battleEntityId.sharp2: // Poily
                    data.id = battleEntityId.sharp2;
                    data.preEvoId = battleEntityId.sharp1;
                    data.evoId = battleEntityId.unknown;

                    // Stats
                    data.displayName = "Poily";
                    data.displayNameSpeakKey = "bey_sharp2_nme";

                    data.maxHealth = 75;
                    data.health = data.maxHealth;

                    data.attack = 105;
                    data.defense = 35;
                    data.speed = 90;

                    data.maxEnergy = 100;
                    data.energy = data.maxEnergy;

                    data.statSpecial = BattleEntity.specialty.attack;
                    data.levelRate = 1.10F;

                    break;

                case battleEntityId.virusRed1: // Red
                    data.id = battleEntityId.virusRed1;
                    data.preEvoId = battleEntityId.unknown;
                    data.evoId = battleEntityId.virusRed2;

                    // Stats
                    data.displayName = "Red";
                    data.displayNameSpeakKey = "bey_cBugRed1_nme";

                    data.maxHealth = 35;
                    data.health = data.maxHealth;

                    data.attack = 45;
                    data.defense = 25;
                    data.speed = 30;

                    data.maxEnergy = 100;
                    data.energy = data.maxEnergy;

                    data.statSpecial = BattleEntity.specialty.attack;
                    data.levelRate = 1.05F;

                    break;

                case battleEntityId.virusRed2: // Red X
                    data.id = battleEntityId.virusRed2;
                    data.preEvoId = battleEntityId.virusRed1;
                    data.evoId = battleEntityId.unknown;

                    // Stats
                    data.displayName = "Red X";
                    data.displayNameSpeakKey = "bey_cBugRed2_nme";

                    data.maxHealth = 70;
                    data.health = data.maxHealth;

                    data.attack = 90;
                    data.defense = 45;
                    data.speed = 65;

                    data.maxEnergy = 100;
                    data.energy = data.maxEnergy;

                    data.statSpecial = BattleEntity.specialty.attack;
                    data.levelRate = 1.10F;

                    break;

                case battleEntityId.virusBlue1: // Blue
                    data.id = battleEntityId.virusBlue1;
                    data.preEvoId = battleEntityId.unknown;
                    data.evoId = battleEntityId.virusBlue2;

                    // Stats
                    data.displayName = "Blue";
                    data.displayNameSpeakKey = "bey_cBugBlue1_nme";

                    data.maxHealth = 35;
                    data.health = data.maxHealth;

                    data.attack = 30;
                    data.defense = 25;
                    data.speed = 45;

                    data.maxEnergy = 100;
                    data.energy = data.maxEnergy;

                    data.statSpecial = BattleEntity.specialty.speed;
                    data.levelRate = 1.05F;

                    break;

                case battleEntityId.virusBlue2: // Blue X
                    data.id = battleEntityId.virusBlue2;
                    data.preEvoId = battleEntityId.virusBlue1;
                    data.evoId = battleEntityId.unknown;

                    // Stats
                    data.displayName = "Blue X";
                    data.displayNameSpeakKey = "bey_cBugBlue2_nme";

                    data.maxHealth = 70;
                    data.health = data.maxHealth;

                    data.attack = 65;
                    data.defense = 45;
                    data.speed = 90;

                    data.maxEnergy = 100;
                    data.energy = data.maxEnergy;

                    data.statSpecial = BattleEntity.specialty.speed;
                    data.levelRate = 1.10F;

                    break;

                case battleEntityId.virusYellow1: // Yellow
                    data.id = battleEntityId.virusYellow1;
                    data.preEvoId = battleEntityId.unknown;
                    data.evoId = battleEntityId.virusYellow2;

                    // Stats
                    data.displayName = "Yellow";
                    data.displayNameSpeakKey = "bey_cBugYellow1_nme";

                    data.maxHealth = 35;
                    data.health = data.maxHealth;

                    data.attack = 25;
                    data.defense = 45;
                    data.speed = 30;

                    data.maxEnergy = 100;
                    data.energy = data.maxEnergy;

                    data.statSpecial = BattleEntity.specialty.defense;
                    data.levelRate = 1.05F;

                    break;

                case battleEntityId.virusYellow2: // Yellow X
                    data.id = battleEntityId.virusYellow2;
                    data.preEvoId = battleEntityId.virusYellow1;
                    data.evoId = battleEntityId.unknown;

                    // Stats
                    data.displayName = "Yellow X";
                    data.displayNameSpeakKey = "bey_cBugYellow2_nme";

                    data.maxHealth = 70;
                    data.health = data.maxHealth;

                    data.attack = 45;
                    data.defense = 90;
                    data.speed = 65;

                    data.maxEnergy = 100;
                    data.energy = data.maxEnergy;

                    data.statSpecial = BattleEntity.specialty.defense;
                    data.levelRate = 1.10F;

                    break;

                case battleEntityId.blackHole: // Vortex (BOSS)
                    data.id = battleEntityId.blackHole;
                    data.preEvoId = battleEntityId.unknown;
                    data.evoId = battleEntityId.unknown;

                    // Stats
                    data.displayName = "Vortex";
                    data.displayNameSpeakKey = "bey_blackHole_nme";

                    data.maxHealth = 120;
                    data.health = data.maxHealth;

                    data.attack = 100;
                    data.defense = 150;
                    data.speed = 130;

                    data.maxEnergy = 100;
                    data.energy = data.maxEnergy;

                    data.statSpecial = BattleEntity.specialty.defense;

                    break;

                case battleEntityId.planet1: // Strange Island
                    data.id = battleEntityId.planet1;
                    data.preEvoId = battleEntityId.unknown;
                    data.evoId = battleEntityId.planet2;

                    // Stats
                    data.displayName = "Strange Island";
                    data.displayNameSpeakKey = "bey_planet1_nme";

                    data.maxHealth = 100;
                    data.health = data.maxHealth;

                    data.attack = 25;
                    data.defense = 19;
                    data.speed = 16;

                    data.maxEnergy = 100;
                    data.energy = data.maxEnergy;

                    data.statSpecial = BattleEntity.specialty.health;

                    break;

                case battleEntityId.planet2: // Strange Planet
                    data.id = battleEntityId.planet2;
                    data.preEvoId = battleEntityId.planet1;
                    data.evoId = battleEntityId.unknown;

                    // Stats
                    data.displayName = "Strange Planet";
                    data.displayNameSpeakKey = "bey_planet2_nme";

                    data.maxHealth = 200;
                    data.health = data.maxHealth;

                    data.attack = 41;
                    data.defense = 24;
                    data.speed = 25;

                    data.maxEnergy = 100;
                    data.energy = data.maxEnergy;

                    data.statSpecial = BattleEntity.specialty.health;
                    break;


            }

            // Randomize the moves.
            if(randomMoves)
                SetRandomMovesFromList(ref data);

            // Sets the sprite if the entity's ID is a valid number for the list.
            if ((int)data.id < entitySprites.Count)
                data.sprite = entitySprites[(int)data.id];

            // Sets the name key to the speak key text (this should be the same as the name key).
            if(nameKey == "")
                nameKey = data.displayNameSpeakKey;

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
                    moveList = new List<moveId>() { moveId.poke, moveId.bam, moveId.wham, moveId.kablam };
                    break;

                case battleEntityId.treasure:
                    moveList = new List<moveId>() { moveId.bam };
                    break;

                case battleEntityId.combatBot: // BOSS 1
                    moveList = new List<moveId>() 
                    {
                        moveId.slimeShot, moveId.laserBurst, moveId.fireBurst, moveId.elecBurst,
                        moveId.soundWave, moveId.quickBurst, moveId.statClear, moveId.laserBlast,
                        moveId.fireBlast, moveId.elecBlast, moveId.sonicWave, moveId.twister,
                        moveId.waterBlast, moveId.rockBlast, moveId.airBlast
                    };
                    break;

                case battleEntityId.ufo1:
                    moveList = new List<moveId>() {
                        moveId.slimeShot, moveId.laserShot, moveId.fireShot, moveId.elecShot,
                        moveId.slam, moveId.magnify, moveId.pushBack, moveId.shield1, moveId.bam
                    };
                    break;

                case battleEntityId.ufo2:
                    moveList = new List<moveId>() 
                    {
                        moveId.slimeShot, moveId.laserShot, moveId.fireShot, moveId.elecShot,
                        moveId.slam, moveId.magnify, moveId.bam, moveId.laserBurst,
                        moveId.fireBurst, moveId.elecBurst, moveId.magnet, moveId.electrify,
                        moveId.wham
                    };
                    break;

                case battleEntityId.ufo3:
                    moveList = new List<moveId>() 
                    {
                        moveId.slimeShot, moveId.magnify, moveId.laserBurst, moveId.fireBurst, 
                        moveId.elecBurst, moveId.quickBurst, moveId.magnet, moveId.electrify, 
                        moveId.wham, moveId.laserBlast, moveId.fireBlast, moveId.elecBlast, 
                        moveId.earlyBurst, moveId.kablam

                    };
                    break;

                case battleEntityId.insect1:
                    moveList = new List<moveId>() 
                    {
                        moveId.poke, moveId.slimeShot, moveId.screech, moveId.chip,
                        moveId.hpDrain1, moveId.healthSplit, moveId.bam, moveId.soundWave,
                        moveId.wham
                    };
                    break;

                case battleEntityId.insect2:
                    moveList = new List<moveId>() 
                    {
                        moveId.poke, moveId.slimeShot, moveId.screech, moveId.chip,
                        moveId.hpDrain1, moveId.healthSplit, moveId.bam, moveId.soundWave,
                        moveId.quickBurst, moveId.torch, moveId.hpDrain2, moveId.wham
                    };
                    break;

                case battleEntityId.spaceGhost1:
                    moveList = new List<moveId>() 
                    {
                        moveId.slimeShot, moveId.laserShot, moveId.elecShot, moveId.powerLast,
                        moveId.elecBurst, moveId.laserBurst, moveId.electrify, moveId.risk
                    };
                    break;

                case battleEntityId.spaceGhost2:
                    moveList = new List<moveId>() 
                    {
                        moveId.slimeShot, moveId.laserShot, moveId.elecShot, moveId.powerLast,
                        moveId.laserBurst, moveId.elecBurst, moveId.electrify, moveId.risk,
                        moveId.elecBlast, moveId.laserBlast
                    };
                    break;

                case battleEntityId.comet: // BOSS 2
                    moveList = new List<moveId>() 
                    {
                        moveId.slam, moveId.chip, moveId.quickBurst, moveId.powerFirst,
                        moveId.bam, moveId.tidalWave, moveId.shield2, moveId.wham,
                        moveId.quake, moveId.waterBlast, moveId.airBlast, moveId.chargeSun,
                        moveId.chargeMoon, moveId.earlyBurst, moveId.allOut, moveId.kablam
                    };
                    break;

                case battleEntityId.sunRock1:
                    moveList = new List<moveId>() 
                    {
                        moveId.poke, moveId.slam, moveId.chip, moveId.powerLast,
                        moveId.bam, moveId.magnet, moveId.wham
                    };
                    break;

                case battleEntityId.sunRock2:
                    moveList = new List<moveId>() 
                    {
                        moveId.slam, moveId.chip, moveId.powerLast, moveId.bam,
                        moveId.magnet, moveId.wham, moveId.rockBlast, moveId.airBlast,
                        moveId.chargeSun, moveId.quake, moveId.kablam
                    };
                    break;

                case battleEntityId.moonRock1:
                    moveList = new List<moveId>() 
                    {
                        moveId.poke, moveId.slam, moveId.chip, moveId.powerLast, 
                        moveId.bam, moveId.magnet, moveId.wham
                    };
                    break;

                case battleEntityId.moonRock2:
                    moveList = new List<moveId>() 
                    {
                        moveId.slam, moveId.chip, moveId.powerLast, moveId.bam,
                        moveId.magnet, moveId.wham, moveId.quake, moveId.rockBlast,
                        moveId.airBlast, moveId.chargeMoon, moveId.kablam
                    };
                    break;

                case battleEntityId.fireBot1:
                    moveList = new List<moveId>() 
                    {
                        moveId.laserShot, moveId.fireShot, moveId.magnify, moveId.pushBack,
                        moveId.powerLast, moveId.bam, moveId.fireBurst, moveId.torch, 
                        moveId.burnBoostTarget, moveId.wham
                    };
                    break;

                case battleEntityId.fireBot2:
                    moveList = new List<moveId>()
                    {
                        moveId.laserShot, moveId.fireShot, moveId.magnify, moveId.laserBurst,
                        moveId.fireBurst, moveId.torch, moveId.burnBoostTarget, moveId.laserBlast,
                        moveId.fireBlast, moveId.kablam
                    };
                    break;

                case battleEntityId.waterBot1:
                    moveList = new List<moveId>() 
                    {
                        moveId.slimeShot, moveId.laserShot, moveId.chip, moveId.toss,
                        moveId.magnify, moveId.hpDrain1, moveId.pushBack, moveId.powerLast, 
                        moveId.bam, moveId.laserBurst, moveId.tidalWave, moveId.wham
                    };
                    break;

                case battleEntityId.waterBot2:
                    moveList = new List<moveId>()
                    {
                        moveId.slimeShot, moveId.laserShot, moveId.chip, moveId.toss,
                        moveId.magnify, moveId.hpDrain1, moveId.healthSplit, moveId.laserBurst,
                        moveId.tidalWave, moveId.powerLast, moveId.laserBlast, moveId.waterBlast,
                        moveId.kablam
                    };
                    break;

                case battleEntityId.earthBot1:
                    moveList = new List<moveId>() 
                    {
                        moveId.laserShot, moveId.elecShot, moveId.chip, moveId.slam,
                        moveId.toss, moveId.bam, moveId.magnify, moveId.magnet,
                        moveId.statClear, moveId.paraBoostTarget, moveId.wham
                    };
                    break;

                case battleEntityId.earthBot2:
                    moveList = new List<moveId>()
                    {
                        moveId.chip, moveId.slam, moveId.toss, moveId.powerLast, 
                        moveId.bam, moveId.laserBurst, moveId.elecBurst, moveId.magnify, 
                        moveId.magnet, moveId.statClear, moveId.wham, moveId.rockBlast, 
                        moveId.quake, moveId.kablam
                    };
                    break;

                case battleEntityId.airBot1:
                    moveList = new List<moveId>() 
                    { 
                        moveId.laserShot, moveId.elecShot, moveId.screech, moveId.bam,
                        moveId.laserBurst, moveId.elecBurst, moveId.soundWave, moveId.wham
                    };
                    break;

                case battleEntityId.airBot2:
                    moveList = new List<moveId>()
                    {
                        moveId.laserShot, moveId.elecShot, moveId.powerFirst, moveId.laserBurst, 
                        moveId.elecBurst, moveId.soundWave, moveId.laserBlast, moveId.elecBlast, 
                        moveId.sonicWave, moveId.twister, moveId.airBlast, moveId.kablam
                    };
                    break;


                case battleEntityId.sharp1:
                    moveList = new List<moveId>() 
                    { 
                        moveId.poke, moveId.slam, moveId.chip, moveId.breaker1, 
                        moveId.bam, moveId.motivate, moveId.risk, moveId.breaker2, moveId.wham
                    };
                    break;

                case battleEntityId.sharp2:
                    moveList = new List<moveId>() 
                    {
                        moveId.poke, moveId.slam, moveId.chip, moveId.toss, 
                        moveId.powerFirst, moveId.motivate, moveId.risk, moveId.breaker2, 
                        moveId.wham, moveId.allOut, moveId.breaker3, moveId.kablam
                    };
                    break;

                case battleEntityId.virusRed1:
                    moveList = new List<moveId>() 
                    { 
                        moveId.laserShot, moveId.elecShot, moveId.shield1,
                        moveId.laserBurst, moveId.elecBurst, moveId.screech, 
                        moveId.sonicWave
                    };
                    break;

                case battleEntityId.virusRed2:
                    moveList = new List<moveId>() 
                    { 
                        moveId.laserShot, moveId.screech,moveId.elecShot, moveId.soundWave, 
                        moveId.shield1, moveId.laserBurst, moveId.elecBurst, 
                        moveId.laserBlast, moveId.elecBlast, moveId.sonicWave
                    };
                    break;

                case battleEntityId.virusBlue1:
                    moveList = new List<moveId>() 
                    {
                        moveId.laserShot, moveId.elecShot, moveId.shield1,
                        moveId.laserBurst, moveId.elecBurst, moveId.screech,
                        moveId.sonicWave
                    };
                    break;

                case battleEntityId.virusBlue2:
                    moveList = new List<moveId>() 
                    {
                        moveId.laserShot, moveId.screech, moveId.elecShot, moveId.soundWave,
                        moveId.shield1, moveId.laserBurst, moveId.elecBurst,
                        moveId.laserBlast, moveId.elecBlast, moveId.sonicWave
                    };
                    break;

                case battleEntityId.virusYellow1:
                    moveList = new List<moveId>() 
                    {
                        moveId.laserShot, moveId.elecShot, moveId.shield1,
                        moveId.laserBurst, moveId.elecBurst, moveId.screech,
                        moveId.sonicWave
                    };
                    break;

                case battleEntityId.virusYellow2:
                    moveList = new List<moveId>() 
                    {
                        moveId.laserShot, moveId.screech, moveId.elecShot, moveId.soundWave,
                        moveId.shield1, moveId.laserBurst, moveId.elecBurst,
                        moveId.laserBlast, moveId.elecBlast, moveId.sonicWave
                    };
                    break;

                case battleEntityId.blackHole: // BOSS 3
                    moveList = new List<moveId>() 
                    {
                        moveId.powerLast, moveId.hpDrain2, moveId.hpDrain3, moveId.twister,
                        moveId.quickBurst, moveId.waterBlast, moveId.rockBlast, moveId.quake, 
                        moveId.earlyBurst, moveId.burnBoostUser, moveId.paraBoostUser, moveId.kablam
                    };
                    break;

                case battleEntityId.planet1:
                    moveList = new List<moveId>() 
                    { 
                        moveId.magnify, moveId.hpDrain1, moveId.healthSplit, moveId.powerLast, 
                        moveId.bam, moveId.magnet, moveId.statClear, moveId.cure, 
                        moveId.wham, moveId.quake, moveId.burnBoostUser, moveId.paraBoostUser
                    };
                    break;

                case battleEntityId.planet2:
                    moveList = new List<moveId>() 
                    { 
                        moveId.magnify, moveId.hpDrain1, moveId.powerLast, moveId.magnet, 
                        moveId.statClear, moveId.cure, moveId.tidalWave, moveId.twister, 
                        moveId.waterBlast, moveId.airBlast, moveId.quake, moveId.chargeSun, 
                        moveId.chargeMoon, moveId.burnBoostUser, moveId.paraBoostUser, moveId.kablam
                    };
                    break;
            }

            return moveList;
        }

        // Sets random moves for the UFO.
        private void SetRandomMovesFromList(ref BattleEntityGameData data, int totalMoves = 4)
        {
            // Grabs the move list.
            List<moveId> moveList = GenerateBattleEntityMoveList(data.id);

            // List of moves added.
            int totalCount = Mathf.Clamp(totalMoves, 1, 4);
            int count = 0;

            // Grab the moves.
            while(count < totalCount && moveList.Count != 0)
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
            randomId = (battleEntityId)Random.Range((int)FIRST_ENEMY_ID, (int)LAST_ENEMY_ID + 1);


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
                    // This ignroes ids 0 and 1, which are unknown and treasure respectively.
                    if (idNum >= (int)FIRST_ENEMY_ID && idNum <= (int)LAST_ENEMY_ID)
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
            
            // The evolved forms all have a spawn rate of 0, so the opitmization shouldn't be a problem.

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
        public BattleEntityGameData GenerateBoss(int bossNum)
        {
            BattleEntityGameData data;

            switch(bossNum)
            {
                default:
                case 1:
                    data = GenerateBattleEntityData(battleEntityId.combatBot);
                    break;
                case 2:
                    data = GenerateBattleEntityData(battleEntityId.comet);
                    break;
                case 3:
                    data = GenerateBattleEntityData(battleEntityId.blackHole);
                    break;
            }

            return data;
        }

        // Generates a tutorial enemy.
        public BattleEntityGameData GenerateTutorialEnemy()
        {
            // List of tutorial ids.
            List<battleEntityId> ids = new List<battleEntityId>();
            
            // Add the entities to the list.
            ids.Add(battleEntityId.ufo1);
            ids.Add(battleEntityId.insect1);

            // Generates a random index for the ids list, and gets the id.
            int randomIndex = Random.Range(0, ids.Count);
            battleEntityId randId = ids[randomIndex];

            // Generate the data, and return it.
            BattleEntityGameData data = GenerateBattleEntityData(randId);
            return data;
        }

        // Checks to see if it's a tutorial enemy.
        public static bool IsTutorialEnemy(battleEntityId id)
        {
            List<battleEntityId> ids = new List<battleEntityId>();
            bool usable = false;

            // These should be enemies that can reasonably be beaten in a tutorial fight.
            // Said enemies also shouldn't die too quickly.
            ids.Add(battleEntityId.ufo1);
            ids.Add(battleEntityId.insect1);
            // ids.Add(battleEntityId.sharp1); // Too annoying.
            // ids.Add(battleEntityId.cBugRed1); // Too weak.
            // ids.Add(battleEntityId.moonRock1); // Too tanky
            // ids.Add(battleEntityId.virusYellow1); // Too annoying
            // ids.Add(battleEntityId.cBugBlue1); // Too weak.

            // Checks if this is a tutorial enemy or not.
            usable = ids.Contains(id);

            // Returns result.
            return usable;
        }
    }

}
