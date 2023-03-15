using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoLSDK;
using SimpleJSON;
using JetBrains.Annotations;

namespace RM_BBTS
{

    // The game data for a battle entity.
    public struct BattleEntityGameData
    {
        // The battle entity id.
        public battleEntityId id;

        // The pre-evolution id.
        public battleEntityId preEvoId;

        // The evolution id.
        public battleEntityId evoId;

        // The entity name.
        public string displayName;

        // The speak key for the display name.
        public string displayNameSpeakKey;

        // The level of the entity, and the rate that it levels up at.
        public uint level;
        public float levelRate;

        // The stats
        public float maxHealth;
        public float health;

        public float attack;
        public float defense;
        public float speed;

        public float maxEnergy;
        public float energy;

        // The stat specialty
        public BattleEntity.specialty statSpecial;

        // Stat modifiers
        public int attackMod;
        public int defenseMod;
        public int speedMod;

        // The moves
        public moveId move0, move1, move2, move3;

        // The sprite image of the entity.
        public Sprite sprite;

    }

    // Save data for a battle entity, which only holds the entity id, level, stats, and moves.
    // This is the minimum information needed to construct the entity.
    [System.Serializable]
    public struct BattleEntitySaveData
    {
        // The battle entity id.
        public battleEntityId id;

        // The level of the entity.
        public uint level;

        // The stats
        public float maxHealth;
        public float health;

        public float attack;
        public float defense;
        public float speed;

        public float maxEnergy;
        public float energy;

        // The moves
        public moveId move0, move1, move2, move3;
    }

    // A class inherited by entities that do battle.
    public class BattleEntity : MonoBehaviour
    {
        // The speciality of the enemy.
        public enum specialty { none, health, attack, defense, speed }


        // The display name for the battle entity.
        public string displayName = "";

        // The speak key for the display name.
        public string displayNameSpeakKey = "";

        // The sprite that the battle entity uses.
        public Sprite sprite;

        // the id number of the entity.
        public battleEntityId id = 0;

        // the id number of the pre-evolution. If this is 0, or if it is set to the same as 'id', then the entity has no pre-evolution.
        public battleEntityId preEvoId = 0;

        // the id number of the evolution. If this is 0, or if it is set to the same as 'id', then the entity has no evolution.
        public battleEntityId evoId = 0;

        // Level
        // Entity rate.
        protected uint level = 1;

        // The rate that the entity levels up at. The player levels up at a faster rate than the enemies.
        public float levelRate = 1.0F;

        [Header("Stats")]

        // The entity's stat speciality.
        public specialty statSpecial = specialty.none;

        // BASE STATS
        // The stats of the battle entity.
        protected float maxHealth = 1;
        protected float health = 1;

        protected float attack = 1;
        protected float defense = 1;
        protected float speed = 1;

        protected float maxEnergy = 100;
        protected float energy = 100;

        // Checks if the entity is vulernable to attacking moves.
        public bool vulnerable = true;

        // STAT MOFIDIERS (TEMP INC/DEC)

        // Modifier for attack, defense, speed, and accuracy.
        protected int attackMod = 0;
        protected int defenseMod = 0;
        protected int speedMod = 0;
        protected int accuracyMod = 0;

        // The minimum for the stat modifiers.
        public const int STAT_MOD_MIN = -4;

        // The maximum for the stat modifiers.
        public const int STAT_MOD_MAX = 4;

        // LEVEL UP

        // level ups increasess (minimum and maximum).
        public const int STAT_LEVEL_INC_MIN = 1;
        public const int STAT_LEVEL_INC_MAX = 3;
        public const int STAT_LEVEL_BONUS_INC = 3;
        public const int STAT_LEVEL_SPECIALITY_INC = 5;

        // float chargeRate = 1.0F; // the rate for charging - may not be used.

        // Moves
        [Header("Moves")]
        // The moves that the battle entity has.
        public Move[] moves = new Move[4] { null, null, null, null };

        // The total amount of moves.
        public const int MOVE_COUNT = 4;

        // The selected move to be used.
        public Move selectedMove = null;

        // Status effects appled to the entity.
        [Header("Stauses")]

        // Has burn status, which causes damage every turn.
        public bool burned = false;

        // Has paralysis status, which lows the entity down and maybe makes them miss a turn.
        public bool paralyzed = false;

        // Awake is called when the script instance is being loaded.
        protected virtual void Awake()
        {
            // ...
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            // Saves the display name as the object name.
            if (displayName == "")
                displayName = name;

            // The displayNameSpeakKey isn't set since it's unknown what the key would be.

            // NOTE: this caused an error when loading in game data before.
            // This overrides any existing data when loading in a game save.
            // As such, the game save load was moved to a PostStart() function.
            
            // NOTE: this still causes issues for enemies when loading in from a saved game (overrides save data health and energy).
            // I don't want to move or comment this out, so I wrote a workaround in BattleManager.cs.
            health = maxHealth;
            energy = maxEnergy;
        }

        // Loads the translated information for the move.
        // Provided are the name key and the description key.
        public void LoadTranslation(string nameKey)
        {
            // Grabs the language definitions.
            JSONNode defs = SharedState.LanguageDefs;

            // If the SDK has been initialized.
            if (defs != null)
            {
                // Loads in the name and the speak key.
                displayName = defs[nameKey];
                displayNameSpeakKey = nameKey;
            }

        }

        // Loads translation information, changing the provided battle data.
        public static void LoadTranslationForData(ref BattleEntityGameData data, string nameKey)
        {
            // Grabs the language definitions.
            JSONNode defs = SharedState.LanguageDefs;

            // If the SDK has been initialized.
            if (defs != null)
            {
                // Loads in the name and description.
                data.displayName = defs[nameKey];
                data.displayNameSpeakKey = nameKey;
            }
        }

        // Generates the battle entity data for this entity.
        public BattleEntityGameData GenerateBattleEntityGameData()
        {
            // Creates the data object.
            BattleEntityGameData data = new BattleEntityGameData();

            // Sets the evolutions. 
            data.id = id;
            data.preEvoId = preEvoId;
            data.evoId = evoId;

            // Sets the names.
            data.displayName = displayName;
            data.displayNameSpeakKey = displayNameSpeakKey;

            // Level
            data.level = level;
            data.levelRate = levelRate;

            // Health
            data.maxHealth = maxHealth;
            data.health = health;

            // Stats
            data.attack = attack;
            data.defense = defense;
            data.speed = speed;

            // Energy
            data.maxEnergy = maxEnergy;
            data.energy = energy;

            // Modifiers
            data.attackMod = attackMod;
            data.defenseMod = defenseMod;
            data.speedMod = speedMod;

            // Stat speciality.
            data.statSpecial = statSpecial;

            // Move 0 Set.
            if(Move0 != null)
                data.move0 = Move0.Id;

            // Move 1 Set
            if (Move1 != null)
                data.move1 = Move1.Id;

            // Move 2 Set
            if (Move2 != null)
                data.move2 = Move2.Id;

            // Move 3 Set
            if (Move3 != null)
                data.move3 = Move3.Id;


            // Sprite
            data.sprite = sprite;

            return data;
        }

        // Generate the battle entity data with base stats.
        public virtual BattleEntityGameData GenerateBattleEntityGameDataWithBaseStats()
        {
            BattleEntityGameData data = BattleEntityList.Instance.GenerateBattleEntityData(id);
            return data;
        }

        // Generates save data for battle entity.
        public BattleEntitySaveData GenerateBattleEntitySaveData()
        {
            return ConvertBattleEntityGameDataToSaveData(GenerateBattleEntityGameData());
        }

        // Converts save data to game data for the battle entity.
        public static BattleEntitySaveData ConvertBattleEntityGameDataToSaveData(BattleEntityGameData gameData)
        {
            // Save data object.
            BattleEntitySaveData saveData = new BattleEntitySaveData();

            // Copy ID and Level
            saveData.id = gameData.id;
            saveData.level = gameData.level;

            // Copy Health
            saveData.maxHealth = gameData.maxHealth;
            saveData.health = gameData.health;

            // Copy Other Stats
            saveData.attack = gameData.attack;
            saveData.defense = gameData.defense;
            saveData.speed = gameData.speed;

            // Copy Energy
            saveData.maxEnergy = gameData.maxEnergy;
            saveData.energy = gameData.energy;

            // Copy Moves
            saveData.move0 = gameData.move0;
            saveData.move1 = gameData.move1;
            saveData.move2 = gameData.move2;
            saveData.move3 = gameData.move3;

            // returns the save data.
            return saveData;
        }

        // Converts save data to game data.
        public static BattleEntityGameData ConvertBattleEntitySaveDataToGameData(BattleEntitySaveData saveData)
        {
            // Generates the base data.
            BattleEntityGameData gameData = BattleEntityList.Instance.GenerateBattleEntityData(saveData.id);

            // Set ID and Level
            gameData.id = saveData.id; // This should be the same anyway.
            gameData.level = saveData.level;

            // Set Health Values
            gameData.maxHealth = saveData.maxHealth;
            gameData.health = saveData.health;

            // Set Other Stats
            gameData.attack = saveData.attack;
            gameData.defense = saveData.defense;
            gameData.speed = saveData.speed;

            // Copy Energy
            gameData.maxEnergy = saveData.maxEnergy;
            gameData.energy = saveData.energy;

            // Copy Moves
            gameData.move0 = saveData.move0;
            gameData.move1 = saveData.move1;
            gameData.move2 = saveData.move2;
            gameData.move3 = saveData.move3;

            // Returns the game data.
            return gameData;
        }

        // Loads the battle data into this object.
        public virtual void LoadBattleGameData(BattleEntityGameData data)
        {
            // Evos 
            id = data.id;
            preEvoId = data.preEvoId;
            evoId = data.evoId;

            // Names 
            displayName = data.displayName;
            displayNameSpeakKey = data.displayNameSpeakKey;

            // Levels
            level = data.level;
            levelRate = data.levelRate;

            // Main Stats
            maxHealth = data.maxHealth;
            health = data.health;

            attack = data.attack;
            defense = data.defense;
            speed = data.speed;

            maxEnergy = data.maxEnergy;
            energy = data.energy;


            // Stat speciality.
            statSpecial = data.statSpecial;


            // Stat modifiers.
            attackMod = data.attackMod;
            defenseMod = data.defenseMod;
            speedMod = data.speedMod;

            // Generates the four moves and adds them in as objects.
            // Moves are set to 'null' if the move provied is Run or Charge (they don't get put in standard move slots).
            // Move 0
            if (data.move0 == moveId.run || data.move0 == moveId.charge)
                Move0 = null;
            else
                Move0 = MoveList.Instance.GenerateMove(data.move0);

            // Move 1
            if (data.move1 == moveId.run || data.move1 == moveId.charge)
                Move1 = null;
            else
                Move1 = MoveList.Instance.GenerateMove(data.move1);

            // Move 2
            if (data.move2 == moveId.run || data.move2 == moveId.charge)
                Move2 = null;
            else
                Move2 = MoveList.Instance.GenerateMove(data.move2);

            // Move 3
            if (data.move3 == moveId.run || data.move3 == moveId.charge)
                Move3 = null;
            else
                Move3 = MoveList.Instance.GenerateMove(data.move3);

            // If the entity has no moves, give it the move "bam".
            if (!HasMovesSet())
                Move0 = MoveList.Instance.GenerateMove(moveId.bam);

            // Original
            // Move0 = MoveList.Instance.GenerateMove(data.move0);
            // Move1 = MoveList.Instance.GenerateMove(data.move1);
            // Move2 = MoveList.Instance.GenerateMove(data.move2);
            // Move3 = MoveList.Instance.GenerateMove(data.move3);

            // Set sprite data.
            sprite = data.sprite;
        }

        // Loads the battle save data into the object.
        public virtual void LoadBattleSaveData(BattleEntitySaveData saveData)
        {
            // Converts the data.
            BattleEntityGameData gameData = ConvertBattleEntitySaveDataToGameData(saveData);

            // Loads the game data.
            LoadBattleGameData(gameData);
        }

        // Returns the level of the battle enttiy.
        public uint Level
        {
            get { return level; }
        }

        // STATS //
        // The max health getter/setter.
        public float MaxHealth
        {
            get { return maxHealth; }

            set
            {
                maxHealth = (value < 0) ? 1 : value;
                health = Mathf.Clamp(value, 0, maxHealth);
            }
        }

        // The health getter/setter.
        public float Health
        {
            get { return health; }

            set { health = Mathf.Clamp(value, 0, maxHealth); }
        }

        // The attack getter/setter.
        public float Attack
        {
            get { return attack; }

            set { attack = (value < 0) ? 1 : value; }
        }

        // The defense getter/setter.
        public float Defense
        {
            get { return defense; }

            set { defense = (value < 0) ? 1 : value; }
        }

        // The speed getter/setter.
        public float Speed
        {
            get { return speed; }

            set { speed = (value < 0) ? 1 : value; }
        }

        // The max energy getter/setter.
        public float MaxEnergy
        {
            get { return maxEnergy; }

            set
            {
                maxEnergy = (value < 0) ? 1 : value;
                energy = Mathf.Clamp(value, 0, MaxEnergy);
            }
        }

        // The energy getter/setter.
        public float Energy
        {
            get { return energy; }

            set { energy = Mathf.Clamp(value, 0, MaxEnergy); }
        }


        // Checks if the entity is at full health.
        public bool HasFullHealth()
        {
            return health == maxHealth;
        }

        // Sets the energy to its maximum value.
        public void SetHealthToMax()
        {
            health = maxHealth;
        }

        // Sets the health relative to the entity's new max health.
        public void SetHealthRelativeToMaxHealth(float newMaxHealth)
        {
            // Gets the percentage of the health from the old maxHealth.
            float percent = (Health / MaxHealth);

            // Sets the max health.
            MaxHealth = newMaxHealth;

            // Sets the regular health based on the new max health.
            Health = percent * MaxHealth;

        }

        // Restores the entity's health by the provided percentage of their max health.
        public void RestoreHealth(float percent)
        {
            Health += Mathf.Ceil(MaxHealth * percent);
        }

        // Returns 'true' if the entity has no energy.
        public bool HasNoEnergy()
        {
            return energy <= 0.0F;
        }

        // Returns 'true' if the entity has the maximum amount of energy.
        public bool HasFullCharge()
        {
            return energy == maxEnergy;
        }

        // Sets the energy to its maximum value.
        public void SetEnergyToMax()
        {
            energy = maxEnergy;
        }

        // Restores energy by a certain percentage, not rounding the amount added.
        // The percent is locked in a [0, 1] range, with 1.0 meaning 100%.
        // This function makes it so that no rounding is done.
        public void RestoreEnergy(float percent)
        {
            RestoreEnergy(percent, -1);
        }

        // Restores energy to the player by a certain percent. The percent is locked in a [0, 1] range, with 1.0 meaning 100%.
        // Variable 'decimalPlaces' determines how many decimal places are rounded for the addition.
        // If 'decimalPlaces' is 0, then it rounds to a whole number.
        // If 'decimalPlaces' is negative, then no rounding is done.
        public void RestoreEnergy(float percent, int decimalPlaces)
        {
            // Gets the raw calculation.
            float chargePlus = MaxEnergy * percent;

            // Checks if rounding should be done or not.
            if (decimalPlaces >= 0) // Round
            {
                // Find the amount of decimal places.
                float mult = Mathf.Pow(10.0F, decimalPlaces);

                // Multiply to keep (X) amount of decimal places, then round to get rid of the remaining decimal points.
                // After that, divide by the mult so that there are decimal points again.
                float chargePlusRounded = (Mathf.Round(chargePlus * mult)) / mult;

                // Add the rounded amount to the player's energy.
                Energy += chargePlusRounded;

            }
            else // Don't Round
            {
                Energy += chargePlus;
            }
        }


        // GET MODIFIED STATS
        // Attack  Mod
        public int AttackMod
        {
            get { return attackMod; }

            set
            {
                attackMod = Mathf.Clamp(value, STAT_MOD_MIN, STAT_MOD_MAX);
            }
        }

        // Defense Mod
        public int DefenseMod
        {
            get { return defenseMod; }

            set
            {
                defenseMod = Mathf.Clamp(value, STAT_MOD_MIN, STAT_MOD_MAX);
            }
        }

        // Speed Mod
        public int SpeedMod
        {
            get { return speedMod; }

            set
            {
                speedMod = Mathf.Clamp(value, STAT_MOD_MIN, STAT_MOD_MAX);
            }
        }

        // Accuracy Mod
        public int AccuracyMod
        {
            get { return accuracyMod; }

            set
            {
                accuracyMod = Mathf.Clamp(value, STAT_MOD_MIN, STAT_MOD_MAX);
            }
        }

        // Gets the stat total (health, attack, defense, speed).
        // Energy isn't included since the actual amount isn't relevant.
        public float GetStatTotal()
        {
            return maxHealth + attack + defense + speed;
        }

        // Gets the modified attack of the entity.
        public float GetAttackModified()
        {
            // Clamp modifier.
            attackMod = Mathf.Clamp(attackMod, STAT_MOD_MIN, STAT_MOD_MAX);

            // Calculates the attack.
            // A modifier will always change the attack by at least 1 point per stage.
            float result = attack + attack * attackMod * 0.25F + (1.0F * attackMod); // default: 0.05F

            // If the attack stat would be 0 or negative, set it to 1.
            if (result < 1.0F)
                result = 1.0F;

            // Return the result.
            return result;
        }

        // Gets the modified defense of the entity.
        public float GetDefenseModified()
        {
            // Clamp modifier.
            defenseMod = Mathf.Clamp(defenseMod, STAT_MOD_MIN, STAT_MOD_MAX);

            // A modifier will always change the defense by at least 1 point per stage.
            float result = defense + defense * defenseMod * 0.25F + (1.0F * defenseMod); // default: 0.05F

            // If the defense is less than or equal to 0, set it to 1.
            if (result < 1.0F)
                result = 1.0F;

            // Return the result.
            return result;
        }

        // Gets the modified speed of the entity.
        public float GetSpeedModified()
        {
            // Clamp modifier.
            speedMod = Mathf.Clamp(speedMod, STAT_MOD_MIN, STAT_MOD_MAX);

            // Calculates the speed - this is affected by paralysis.
            // A modifier will always change the speed by at least 1 point per stage.
            float result = (speed + speed * speedMod * 0.25F) * (paralyzed ? 0.80F : 1.0F) + (1.0F * speedMod); // default: 0.05F

            // If the speed would be 0 or negative, set it to 1.
            if (result < 1.0F)
                result = 1.0F;

            return result;
        }

        // Gets the accuracy modified. If 'clamp' is true, the accuracy is clamped at [0,1] range.
        public float GetModifiedAccuracy(float baseAccuracy, bool clamp = true)
        {
            // Clamp modifier.
            accuracyMod = Mathf.Clamp(accuracyMod, STAT_MOD_MIN, STAT_MOD_MAX);

            // Calculates the accuracy. Each stage is 0.05F in inc/dec.
            float result = baseAccuracy + (accuracyMod * 0.05F);

            // If the accuracy should be clamped at [0, 1] (0%-100%).
            if (clamp)
                result = Mathf.Clamp01(result);

            return result;
        }

        // Returns 'true' has the stat modifiers.
        public bool HasStatModifiers()
        {
            return (attackMod != 0 || defenseMod != 0 || speedMod != 0 || accuracyMod != 0);
        }

        // Resets the stat modifiers.
        public void ResetStatModifiers()
        {
            attackMod = 0;
            defenseMod = 0;
            speedMod = 0;
            accuracyMod = 0;
        }

        // Resets the status effects.
        public void ResetStatuses()
        {
            burned = false;
            paralyzed = false;
        }
 
        // Checks if the battle entity is dead.
        public bool IsDead()
        {
            // If the health is less than 0, set it to 0.
            if (health < 0)
                health = 0;

            return health <= 0;
        }

        // Basic level up.
        public virtual void LevelUp()
        {
            LevelUp(specialty.none, 1);
        }

        // Levels up the entity. To get the entity's base stats the BattleEntityList should be consulted.
        // (times) refers to how many times the entity is leveled up.
        public virtual void LevelUp(specialty special, uint times = 1)
        {
            // Generate data for current enemy stats, and level up the data.
            BattleEntityGameData data = GenerateBattleEntityGameData();

            // Levels up the data.
            data = LevelUpData(data, levelRate, special, times);

            // Save level
            level = data.level;

            // Save health
            maxHealth = data.maxHealth;
            health = data.health;

            // Save stats
            attack = data.attack;
            defense = data.defense;
            speed = data.speed;

            // Save energy
            maxEnergy = data.maxEnergy;
            energy = data.energy;

        }

        // Levels up the provided data and returns a copy.
        // (times) refers to how many times the entity should be leveled up.
        public static BattleEntityGameData LevelUpData(BattleEntityGameData data, float levelRate, specialty special, uint times = 1)
        {
            // No level up.
            if (times == 0)
                return data;

            // Copies the data so that a new one is made.
            BattleEntityGameData newData = data;

            // HP and Energy Percentage.
            float hpPercent = data.health / data.maxHealth;
            float engPercent = data.energy / data.maxEnergy;

            // If the level rate is negative or 0, set it to 1.0F (default).
            if (levelRate <= 0.0F)
                levelRate = 1.0F;

            // Increase the level.
            newData.level += times;

            // Run the randomizer for each stat.
            for(int n = 0; n < 5; n++)
            {
                // Calculates the additional value.
                float value = Mathf.Ceil(Random.Range(STAT_LEVEL_INC_MIN, STAT_LEVEL_INC_MAX + 1) * levelRate * times);
                
                // Choose what stat to add the value too.
                switch (n)
                {
                    case 0: // Max HP
                        newData.maxHealth += value;
                        break;
                    case 1: // Attack
                        newData.attack += value;
                        break;
                    case 2: // Defense
                        newData.defense += value;
                        break;
                    case 3: // Speed
                        newData.speed += value;
                        break;
                    case 4: // Max Energy (No Longer Used)
                        newData.maxEnergy += value;
                        break;
                }

            }


            // newData.maxHealth += Mathf.Ceil(Random.Range(STAT_LEVEL_INC_MIN, STAT_LEVEL_INC_MAX + 1) * levelRate * times);
            // newData.attack += Mathf.Ceil(Random.Range(STAT_LEVEL_INC_MIN, STAT_LEVEL_INC_MAX + 1) * levelRate * times);
            // newData.defense += Mathf.Ceil(Random.Range(STAT_LEVEL_INC_MIN, STAT_LEVEL_INC_MAX + 1) * levelRate * times);
            // newData.speed += Mathf.Ceil(Random.Range(STAT_LEVEL_INC_MIN, STAT_LEVEL_INC_MAX + 1) * levelRate * times);
            // newData.maxEnergy += Mathf.Ceil(Random.Range(STAT_LEVEL_INC_MIN, STAT_LEVEL_INC_MAX + 1) * levelRate * times);

            // Adds a differet bonus per level.
            for (int lvl = 0; lvl < times; lvl++)
            {
                // Energy isn't used anymore, but it's being kept in since it just acts as a 'no bonus' level-up.
                int rand = Random.Range(0, 5);

                // Random +5 factor
                // Not applying a level rate factor for this.
                switch (rand)
                {
                    case 0: // HP
                        newData.maxHealth += STAT_LEVEL_BONUS_INC;
                        break;
                    case 1: // ATTACK
                        newData.attack += STAT_LEVEL_BONUS_INC;
                        break;
                    case 2: // DEFENSE
                        newData.defense += STAT_LEVEL_BONUS_INC;
                        break;
                    case 3: // SPEED
                        newData.speed += STAT_LEVEL_BONUS_INC;
                        break;
                    case 4: // ENERGY
                        newData.maxEnergy += STAT_LEVEL_BONUS_INC;
                        break;
                }
            }

            // Checks for the special bonus.
            {
                float value = STAT_LEVEL_SPECIALITY_INC * levelRate;

                switch (special)
                {
                    case specialty.none:
                        // Divides the value by 4 and adds it to each stat.
                        newData.maxHealth += Mathf.Ceil(value / 4.0F);
                        newData.attack += Mathf.Ceil(value / 4.0F);
                        newData.defense += Mathf.Ceil(value / 4.0F);
                        newData.speed += Mathf.Ceil(value / 4.0F);
                        break;
                    case specialty.health:
                        newData.maxHealth += value;
                        break;
                    case specialty.attack:
                        newData.attack += value;
                        break;
                    case specialty.defense:
                        newData.defense += value;
                        break;
                    case specialty.speed:
                        newData.speed += value;
                        break;

                }
            }

            // Make sure all values are whole numbers.
            newData.maxHealth = Mathf.Ceil(newData.maxHealth);
            newData.maxEnergy = Mathf.Ceil(newData.maxEnergy);
            newData.attack = Mathf.Ceil(newData.attack);
            newData.defense = Mathf.Ceil(newData.defense);
            newData.speed = Mathf.Ceil(newData.speed);

            
            // Proportional changes to health and energy.
            newData.health = Mathf.Ceil(hpPercent * newData.maxHealth);
            newData.energy = Mathf.Ceil(engPercent * newData.maxEnergy);

            // Clamps the health and energy levels.
            newData.health = Mathf.Clamp(newData.health, 0, newData.maxHealth);
            newData.energy = Mathf.Clamp(newData.energy, 0, newData.maxEnergy);

            // Returns the new data.
            return newData;
        }

        // Levels up the data, slotting in its level rate and keeping the provided speciality.
        public static BattleEntityGameData LevelUpData(BattleEntityGameData data, specialty special, uint times = 1)
        {
            return LevelUpData(data, data.levelRate, special, times);
        }

        // Levels up the data, slotting in its speciality.
        public static BattleEntityGameData LevelUpData(BattleEntityGameData data, float levelRate, uint times = 1)
        {
            return LevelUpData(data, levelRate, data.statSpecial, times);
        }

        // Levels up the data, slotting in its level rate and specialty.
        public static BattleEntityGameData LevelUpData(BattleEntityGameData data, uint times = 1)
        {
            return LevelUpData(data, data.levelRate, data.statSpecial, times);
        }


        // Returns 'true' if the entity can evolve.
        public bool CanEvolve()
        {
            // If the evolution ID is the same, or if it is set to "unknown", the entity does not evolve.
            bool evolves = !(evoId == id || evoId == battleEntityId.unknown);

            // Return result.
            return evolves;
        }

        // Returns 'true' if the entity can evolve.
        public static bool CanEvolve(BattleEntityGameData data)
        {
            // If the evolution ID is the same, or if it is set to "unknown", the entity does not evolve.
            bool evolves = !(data.evoId == data.id || data.evoId == battleEntityId.unknown);

            // Return result.
            return evolves;
        }


        // Evolves the entity. It fails if the entity does not have an evolution.
        public bool Evolve()
        {
            // If there is no evolution, it returns false.
            if (evoId == battleEntityId.unknown)
            {
                return false;
            }
            else // Evolve.
            {
                BattleEntityGameData data = GenerateBattleEntityGameData();
                data = EvolveData(data);
                LoadBattleGameData(data);
                return true;
            }
                
        }

        // Evolves the battle entity.
        public static BattleEntityGameData EvolveData(BattleEntityGameData oldData)
        {
            // Can't evolve if the evolution is the same entity, or if it's set to unknown.
            if (!CanEvolve(oldData))
                return oldData;

            // Gets the base data.
            BattleEntityGameData baseData = BattleEntityList.Instance.GenerateBattleEntityData(oldData.id);

            // Gets the evolved data.
            BattleEntityGameData evolved = BattleEntityList.Instance.GenerateBattleEntityData(oldData.evoId);

            // Same level as old data (don't keep the same level rate).
            evolved.level = oldData.level;

            // Give stats to evolved form.
            // Health
            evolved.maxHealth += oldData.maxHealth - baseData.maxHealth;
            evolved.health = evolved.maxHealth;

            // Attack, Defense, and Speed
            evolved.attack += oldData.attack - baseData.attack;
            evolved.defense += oldData.defense - baseData.defense;
            evolved.speed += oldData.speed - baseData.speed;

            // Energy levels (return to full energy level as well).
            evolved.maxEnergy += oldData.maxEnergy - baseData.maxEnergy;
            evolved.energy = evolved.maxEnergy;

            // Keep modifiers, though this shouldn't do anything.
            evolved.attackMod = oldData.attackMod;
            evolved.defenseMod = oldData.defenseMod;
            evolved.speedMod = oldData.speedMod;

            // Return the evolved form.
            return evolved;

        }


        // MOVES //
        // The amount of moves the battle entity has.
        public int GetMoveCount()
        {
            // The move count.
            int count = 0;

            // Increases count for saved move instances.
            if (moves[0] != null) // m0
                count++;

            if (moves[1] != null) // m1
                count++;

            if (moves[2] != null) // m2
                count++;

            if (moves[3] != null) // m3
                count++;

            return count;

        }

        // Checks to see if the battle entity has 4 moves.
        public bool HasFourFightMoves()
        {
            // The result variable.
            bool result = true;

            // Checks that he has all four move slots filled.
            result = (moves[0] != null && moves[1] != null && moves[2] != null && moves[3] != null);

            // Return result.
            return result;
        }

        // Checks if the battle entity has a certain move.
        public bool HasMove(Move move)
        {
            // Checks if the move is set to null.
            // If so, return a false value.
            // If true, go through with the move check.
            if (move == null)
                return false;
            else
                return HasMove(move.Id);
        }

        // Checks if the battle entity has a certain move.
        public bool HasMove(moveId compId)
        {
            // Checks each index
            for(int i = 0; i < moves.Length; i++)
            {
                // No move in this slot.
                if (moves[i] == null)
                    continue;

                // If the ids are the same, then the entity has the move.
                if (moves[i].Id == compId)
                {
                    return true;
                }
            }

            return false;
        }

        // Move 0
        public Move Move0
        {
            get { return moves[0]; }

            set { moves[0] = value; }
        }

        // Move 1
        public Move Move1
        {
            get { return moves[1]; }

            set { moves[1] = value; }
        }

        // Move 2
        public Move Move2
        {
            get { return moves[2]; }

            set { moves[2] = value; }
        }

        // Move 3
        public Move Move3
        {
            get { return moves[3]; }

            set { moves[3] = value; }
        }

        // Checks to see if the entity has moves set to it.
        public bool HasMovesSet()
        {
            // Checks each move.
            foreach(Move move in moves)
            {
                // Found a move.
                if (move != null)
                    return true;
            }

            // No moves set.
            return false;
        }

        // Selects the move from the provided index.
        public void SelectMove(int index)
        {
            if (index >= 0 && index < moves.Length)
                selectedMove = moves[index];
            else
                selectedMove = null;
        }

        // Selects hte move at index 0.
        public void SelectMove0()
        {
            SelectMove(0);
        }

        // Selects hte move at index 1.
        public void SelectMove1()
        {
            SelectMove(1);
        }

        // Selects move at index 2.
        public void SelectMove2()
        {
            SelectMove(2);
        }

        // Selects the move at index 3.
        public void SelectMove3()
        {
            SelectMove(3);
        }

        // Selects the charge move.
        public void SelectCharge()
        {
            selectedMove = MoveList.Instance.ChargeMove;
        }

        // Returns '1' if BE1 is the fastest entity, '2' if BE2 is the fastest entity, and '0' if both are the same speed.
        public static int GetFastestEntity(BattleEntity be1, BattleEntity be2, BattleManager battle)
        {
            // Gets the two speeds.
            float speed1 = be1.GetSpeedModified();
            float speed2 = be2.GetSpeedModified();

            // Gets the result.
            if (speed1 > speed2)
                return 1;
            else if (speed1 < speed2)
                return 2;
            else
                return 0;
        }

        // Applies burn damage.
        public void ApplyBurn(BattleManager battle)
        {
            // Does 1/16th damage.
            Health -= MaxHealth * BattleManager.BURN_DAMAGE;

            // Checks the battle entity's type.
            if (this is Player) // Is the player, so update health UI for them.
            {
                battle.gameManager.UpdatePlayerHealthUI();
            }
            else // Not the player, so update the battle UI.
            {
                battle.UpdateOpponentUI();
            }
        }

        
        // Called when a turn happens during a battle.
        public virtual void OnBattleTurn()
        {
            // The entity is vulernable by default.
            vulnerable = true;
        }


        // Update is called once per frame
        protected virtual void Update()
        {

        }
    }
}