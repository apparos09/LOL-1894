using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        // float chargeRate = 1.0F; // the rate for charging - may not be used.

        // Moves
        [Header("Moves")]
        // The moves that the battle entity has.
        public Move[] moves = new Move[4] {null, null, null, null};

        // The total amount of moves.
        public const int MOVE_COUNT = 4;

        // The selected move to be used.
        public Move selectedMove = null;

        // Awake is called when the script instacne is being loaded.
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

        // Loads the battle data into this object.
        public void LoadBattleData(BattleEntityData data)
        {
            id = data.id;
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

        // STATS //
        // The max health getter/setter.
        public float MaxHealth
        {
            get { return maxHealth; }

            // set { maxHealth = (value < 0) ? 1 : value; }
        }

        // The health getter/setter.
        public float Health
        {
            get { return health; }

            set { health = (value < 0) ? 0 : (value > maxHealth) ? maxHealth : value; }
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

            // set { maxEnergy = (value < 0) ? 1 : value; }
        }

        // The energy getter/setter.
        public float Energy
        {
            get { return energy; }

            set { energy = (value < 0) ? 0 : (value > maxEnergy) ? maxEnergy : value; }
        }

        // Returns 'true' if the entity has the maximum amount of energy.
        public bool HasFullCharge()
        {
            return energy == maxEnergy;
        }

        // Levels up the entity. To get the entity's base stats the BattleEntityList should be consulted.
        public void LevelUp()
        {

        }

        // MOVES //
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