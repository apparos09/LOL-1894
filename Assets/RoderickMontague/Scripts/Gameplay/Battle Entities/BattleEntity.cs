using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoLSDK;
using SimpleJSON;

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
        // TODO: implement the level rate.
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

        protected float maxEnergy = 10;
        protected float energy = 10;

        // STAT MOFIDIERS (TEMP INC/DEC)

        // Modifier for attack.
        public int attackMod = 0;

        // Modifier for defense.
        public int defenseMod = 0;

        // Modifier for speed.
        public int speedMod = 0;

        // The base accuracy for the battle entity. This can get adjusted by moves in the game.
        public float accuracyMod = 1.0F;

        // The minimum for the stat modifiers.
        public const int STAT_MOD_MIN = -3;

        // The maximum for the stat modifiers.
        public const int STAT_MOD_MAX = -3;

        // LEVEL UP

        // level ups increasess (minimum and maximum).
        public const int STAT_LEVEL_INC_MIN = 1;
        public const int STAT_LEVEL_INC_MAX = 3;
        public const int STAT_LEVEL_BONUS_INC = 3;
        public const float LEVEL_UP_RESTORE_PERCENT = 0.2F;

        // float chargeRate = 1.0F; // the rate for charging - may not be used.

        // Moves
        [Header("Moves")]
        // The moves that the battle entity has.
        public Move[] moves = new Move[4] {null, null, null, null};

        // The total amount of moves.
        public const int MOVE_COUNT = 4;

        // The selected move to be used.
        public Move selectedMove = null;

        // Status effects appled to the entity.
        [Header("Stauses")]

        // Has burn status, which causes damage every turn.
        public bool burned;

        // Has paralysis status, which lows the entity down and maybe makes them miss a turn.
        public bool paralyzed;

        // Awake is called when the script instance is being loaded.
        protected virtual void Awake()
        {
            
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            // Saves the display name as the object name.
            if (displayName == "")
                displayName = name;

            // TODO: this causes an error when loading game data.
            // This overrides any existing data when loading in a game save.
            // As such, the game save load was moved to a PostStart() function.
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
                // Loads in the name and description.
                displayName = defs[nameKey];
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
            }
        }

        // Generates the battle entity data for this entity.
        public BattleEntityGameData GenerateBattleEntityGameData()
        {
            // Creates the data object.
            BattleEntityGameData data = new BattleEntityGameData();

            // Sets the values.
            data.id = id;
            data.displayName = displayName;
            data.level = level;
            data.levelRate = levelRate;

            data.maxHealth = maxHealth;
            data.health = health;

            data.attack = attack;
            data.defense = defense;
            data.speed = speed;

            data.maxEnergy = maxEnergy;
            data.energy = energy;

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

            data.sprite = sprite;

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
            id = data.id;
            displayName = data.displayName;
            displayNameSpeakKey = data.displayNameSpeakKey;

            level = data.level;
            levelRate = data.levelRate;

            maxHealth = data.maxHealth;
            health = data.health;

            attack = data.attack;
            defense = data.defense;
            speed = data.speed;

            statSpecial = data.statSpecial;

            maxEnergy = data.maxEnergy;
            energy = data.energy;

            // Generates the four moves and adds them in as objects.
            Move0 = MoveList.Instance.GenerateMove(data.move0);
            Move1 = MoveList.Instance.GenerateMove(data.move1);
            Move2 = MoveList.Instance.GenerateMove(data.move2);
            Move3 = MoveList.Instance.GenerateMove(data.move3);

            // Save sprite data.
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

        // Returns 'true' if the entity has the maximum amount of energy.
        public bool HasFullCharge()
        {
            return energy == maxEnergy;
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

            // Returns the value.
            return attack + attack * attackMod * 0.05F;
        }

        // Gets the modified defense of the entity.
        public float GetDefenseModified()
        {
            // Clamp modifier.
            defenseMod = Mathf.Clamp(defenseMod, STAT_MOD_MIN, STAT_MOD_MAX);

            // Returns the value.
            return defense + defense * defenseMod * 0.05F;
        }

        // Gets the modified speed of the entity.
        public float GetSpeedModified()
        {
            // Clamp modifier.
            speedMod = Mathf.Clamp(speedMod, STAT_MOD_MIN, STAT_MOD_MAX);

            // Returns the value. This is affected by paralysis.
            return speed + speed * speedMod * 0.05F * (paralyzed ? 0.75F : 1.0F);
        }

        // Resets the stat modifiers.
        public void ResetStatModifiers()
        {
            attackMod = 0;
            defenseMod = 0;
            speedMod = 0;
        }

        // Resets the status effects.
        public void ResetStatuses()
        {
            burned = false;
            paralyzed = false;
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
            // TODO: implement level up and level rate.

            // // Relative hp and energy.
            // float hpPercent = health / maxHealth;
            // float engPercent = energy / maxEnergy;
            // 
            // // Bonus increase.
            // float bonus = STAT_LEVEL_BONUS_INC;
            // int rand = Random.Range(0, 5);
            // 
            // // The restoration percentage.
            // float restorePercent = LEVEL_UP_RESTORE_PERCENT;
            // 
            // // Increase the level.
            // level++;
            // 
            // // Increases the 5 stats.
            // maxHealth += Random.Range(STAT_LEVEL_INC_MIN, STAT_LEVEL_INC_MAX);
            // attack += Random.Range(STAT_LEVEL_INC_MIN, STAT_LEVEL_INC_MAX);
            // defense += Random.Range(STAT_LEVEL_INC_MIN, STAT_LEVEL_INC_MAX);
            // speed += Random.Range(STAT_LEVEL_INC_MIN, STAT_LEVEL_INC_MAX);
            // maxEnergy += Random.Range(STAT_LEVEL_INC_MIN, STAT_LEVEL_INC_MAX);
            // 
            // 
            // // Random +3 factor
            // switch(rand)
            // {
            //     case 0: // HP
            //         maxHealth += bonus;
            //         break;
            //     case 1: // ATTACK
            //         attack += bonus;
            //         break;
            //     case 2: // DEFENSE
            //         defense += bonus;
            //         break;
            //     case 3: // SPEED
            //         speed += bonus;
            //         break;
            //     case 4: // ENERGY
            //         maxEnergy += bonus;
            //         break;
            // }
            // 
            // // Sets new health and energy proportional to new maxes.
            // health = hpPercent * maxHealth;
            // energy = engPercent * maxEnergy;
            // 
            // // Restores health and energy
            // Health += maxHealth * restorePercent;
            // Energy += maxEnergy * restorePercent;

            // Generate and level up the data.
            BattleEntityGameData data = GenerateBattleEntityGameData();
            data = LevelUpData(data, special, levelRate, times);

            // Save level
            level = data.level;
            
            // Doesn't change this since it would overwrite the player's level rate.
            // levelRate = data.levelRate;

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
        public static BattleEntityGameData LevelUpData(BattleEntityGameData data, specialty special, float levelRate, uint times = 1)
        {
            // No level up.
            if (times == 0)
                return data;

            BattleEntityGameData newData = data;

            int rand;
            float hpPercent = data.health / data.maxHealth;
            float engPercent = data.energy / data.maxEnergy;

            newData.level += times;

            newData.maxHealth += Random.Range(STAT_LEVEL_INC_MIN, STAT_LEVEL_INC_MAX) * levelRate * times;
            newData.attack += Random.Range(STAT_LEVEL_INC_MIN, STAT_LEVEL_INC_MAX) * levelRate * times;
            newData.defense += Random.Range(STAT_LEVEL_INC_MIN, STAT_LEVEL_INC_MAX) * levelRate * times;
            newData.speed += Random.Range(STAT_LEVEL_INC_MIN, STAT_LEVEL_INC_MAX) * levelRate * times;
            newData.maxEnergy += Random.Range(STAT_LEVEL_INC_MIN, STAT_LEVEL_INC_MAX) * levelRate * times;

            // Adds a differet bonus per level.
            for(int lvl = 0; lvl < times; lvl++)
            {
                rand = Random.Range(0, 5);

                // Random +3 factor
                switch (rand)
                {
                    case 0: // HP
                        newData.maxHealth += STAT_LEVEL_BONUS_INC * levelRate;
                        break;
                    case 1: // ATTACK
                        newData.attack += STAT_LEVEL_BONUS_INC * levelRate;
                        break;
                    case 2: // DEFENSE
                        newData.defense += STAT_LEVEL_BONUS_INC * levelRate;
                        break;
                    case 3: // SPEED
                        newData.speed += STAT_LEVEL_BONUS_INC * levelRate;
                        break;
                    case 4: // ENERGY
                        newData.maxEnergy += STAT_LEVEL_BONUS_INC * levelRate;
                        break;
                }
            }

            // TODO: do speciality bonus.

            // Proportional changes.
            newData.health = hpPercent * newData.maxHealth;
            newData.energy = engPercent * newData.maxEnergy;

            // // TODO: move this to the player class.
            // // Restores health and energy
            // newData.health += newData.maxHealth * LEVEL_UP_RESTORE_PERCENT;
            // newData.energy += newData.maxEnergy * LEVEL_UP_RESTORE_PERCENT;

            // Clamps the health and energy levels.
            newData.health = Mathf.Clamp(newData.health, 0, newData.maxHealth);
            newData.energy = Mathf.Clamp(newData.energy, 0, newData.maxEnergy);

            // Returns the new data.
            return newData;
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
            // Can't evolve.
            if (oldData.evoId == oldData.id || oldData.evoId == battleEntityId.unknown)
                return oldData;

            // Gets the base data.
            BattleEntityGameData baseData = BattleEntityList.Instance.GenerateBattleEntityData(oldData.id);

            // Gets the evolved data.
            BattleEntityGameData evolved = BattleEntityList.Instance.GenerateBattleEntityData(oldData.evoId);

            // Same Level and Rate
            evolved.level = oldData.level;
            evolved.levelRate = oldData.levelRate;

            // Give stats to evolved form.
            evolved.maxHealth += oldData.maxHealth - baseData.maxHealth;
            evolved.health = evolved.maxHealth;

            evolved.attack += oldData.attack - baseData.attack;
            evolved.defense += oldData.defense - baseData.defense;
            evolved.speed += oldData.speed - baseData.speed;

            evolved.maxEnergy += oldData.maxEnergy - baseData.maxEnergy;
            evolved.energy = evolved.maxEnergy;

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

        // Checks if the battle entity has a certain move.
        public bool HasMove(Move move)
        {
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
            Health -= MaxHealth * (1.0F / 16.0F);

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
            // ...
        }

        // Update is called once per frame
        protected virtual void Update()
        {

        }
    }
}