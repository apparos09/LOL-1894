using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoLSDK;
using SimpleJSON;

namespace RM_BBTS
{

    // The data for a battle entity.
    public struct BattleEntityData
    {
        // The battle entity id.
        public battleEntityId id;

        // The pre-evolution id.
        public battleEntityId preEvoId;

        // The evolution id.
        public battleEntityId evoId;

        // The entity name.
        public string displayName;

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

        // The sprite image of the entity.
        public Sprite sprite;

    }

    // A class inherited by entities that do battle.
    public class BattleEntity : MonoBehaviour
    {
        // The display name for the battle entity.
        public string displayName = "";

        // The sprite that the battle entity uses.
        public Sprite sprite;

        // the id number of the entity.
        public battleEntityId id = 0;

        // the id number of the pre-evolution. If this is 0, or if it is set to the same as 'id', then the entity has no pre-evolution.
        public battleEntityId preEvoId = 0;

        // the id number of the evolution. If this is 0, or if it is set to the same as 'id', then the entity has no evolution.
        public battleEntityId evoId = 0;

        [Header("Stats")]

        // Level
        protected uint level = 1;

        // Stats
        // The stats of the battle entity.
        protected float maxHealth = 1;
        protected float health = 1;

        protected float attack = 1;
        protected float defense = 1;
        protected float speed = 1;

        protected float maxEnergy = 10;
        protected float energy = 10;

        // level ups incs (minimum and maximum).
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
        public static void LoadTranslationForData(ref BattleEntityData data, string nameKey)
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
        public BattleEntityData GenerateBattleEntityData()
        {
            // Creates the data object.
            BattleEntityData data = new BattleEntityData();

            // Sets the values.
            data.id = id;
            data.displayName = displayName;
            data.level = level;

            data.maxHealth = maxHealth;
            data.health = health;

            data.attack = attack;
            data.defense = defense;
            data.speed = speed;

            data.maxEnergy = maxEnergy;
            data.energy = energy;

            data.move0 = Move0.Id;
            data.move1 = Move1.Id;
            data.move2 = Move2.Id;
            data.move3 = Move3.Id;

            data.sprite = sprite;

            return data;
        }

        // Loads the battle data into this object.
        public void LoadBattleData(BattleEntityData data)
        {
            id = data.id;
            displayName = data.displayName;
            level = data.level;

            maxHealth = data.maxHealth;
            health = data.health;

            attack = data.attack;
            defense = data.defense;
            speed = data.speed;

            maxEnergy = data.maxEnergy;
            energy = data.energy;

            // Generates the four moves and adds them in as objects.
            Move0 = MoveList.Instance.GenerateMove(data.move0);
            Move1 = MoveList.Instance.GenerateMove(data.move1);
            Move2 = MoveList.Instance.GenerateMove(data.move2);
            Move3 = MoveList.Instance.GenerateMove(data.move3);

            sprite = data.sprite;
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

        // Levels up the entity. To get the entity's base stats the BattleEntityList should be consulted.
        // (times) refers to how many times the entity is leveled up.
        public void LevelUp(uint times = 1)
        {
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
            BattleEntityData data = GenerateBattleEntityData();
            data = LevelUpData(data, times);

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
        public static BattleEntityData LevelUpData(BattleEntityData data, uint times = 1)
        {
            BattleEntityData newData = data;

            int rand;
            float hpPercent = data.health / data.maxHealth;
            float engPercent = data.energy / data.maxEnergy;

            newData.level += times;

            newData.maxHealth += Random.Range(STAT_LEVEL_INC_MIN, STAT_LEVEL_INC_MAX) * times;
            newData.attack += Random.Range(STAT_LEVEL_INC_MIN, STAT_LEVEL_INC_MAX) * times;
            newData.defense += Random.Range(STAT_LEVEL_INC_MIN, STAT_LEVEL_INC_MAX) * times;
            newData.speed += Random.Range(STAT_LEVEL_INC_MIN, STAT_LEVEL_INC_MAX) * times;
            newData.maxEnergy += Random.Range(STAT_LEVEL_INC_MIN, STAT_LEVEL_INC_MAX) * times;

            // Adds a differet bonus per level.
            for(int lvl = 0; lvl < times; lvl++)
            {
                rand = Random.Range(0, 5);

                // Random +3 factor
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

            // Proportional changes.
            newData.health = hpPercent * newData.maxHealth;
            newData.energy = engPercent * newData.maxEnergy;

            // Restores health and energy
            newData.health += newData.maxHealth * LEVEL_UP_RESTORE_PERCENT;
            newData.energy += newData.maxEnergy * LEVEL_UP_RESTORE_PERCENT;

            // Clamps the health and energy levels.
            newData.health = Mathf.Clamp(newData.health, 0, newData.maxHealth);
            newData.energy = Mathf.Clamp(newData.energy, 0, newData.maxEnergy);

            // Returns the new data.
            return newData;
        }

        // Evolves the battle entity.
        public static BattleEntityData Evolve(BattleEntityData oldData)
        {
            // Can't evolve.
            if (oldData.evoId == oldData.id)
                return oldData;

            // Gets the base data.
            BattleEntityData baseData = BattleEntityList.Instance.GenerateBattleEntityData(oldData.id);

            // Gets the evolved data.
            BattleEntityData evolved = BattleEntityList.Instance.GenerateBattleEntityData(oldData.evoId);

            // Same Level
            evolved.level = oldData.level;

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
            float speed1 = be1.speed * (be1.paralyzed ? 0.75F : 1.0F);
            float speed2 = be1.speed * (be2.paralyzed ? 0.75F : 1.0F);

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
                battle.UpdateUI();
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