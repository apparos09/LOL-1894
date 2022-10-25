using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_BBTS
{

    // The data for a battle entity.
    public struct BattleEntityData
    {
        // The battle entity id.
        public uint id;

        // The level
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
        public Move move0, move1, move2, move3;

        // The sprite image of the entity.
        public Sprite sprite;

    }

    // A class inherited by entities that do battle.
    public class BattleEntity : MonoBehaviour
    {
        // The sprite that the battle entity uses.
        public Sprite sprite;

        // the id number of the entity.
        public uint id;

        [Header("Stats")]

        // Level
        public uint level = 1;

        // Stats
        // The stats of the battle entity.
        public float maxHealth = 1;
        public float health = 1;

        public float attack = 1;
        public float defense = 1;
        public float speed = 1;

        public float maxEnergy = 10;
        public float energy = 10;

        // Moves
        [Header("Moves")]
        // The moves that the battle entity has.
        public Move[] moves = new Move[4] {null, null, null, null};

        // The total amount of moves.
        public const int MOVE_COUNT = 4;

        // The selected move to be used.
        public Move selectedMove = null;

        // Start is called before the first frame update
        protected virtual void Start()
        {
            health = maxHealth;
            energy = maxEnergy;
        }

        // STATS //
        // The health getter/setter.
        public float Health
        {
            get { return health; }

            set { health = (value < 0) ? 1 : value; }
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

        // The energy getter/setter.
        public float Energy
        {
            get { return energy; }

            set { energy = (value < 0) ? 1 : value; }
        }

        // Returns 'true' if the entity has the maximum amount of energy.
        public bool HasFullCharge()
        {
            return energy == maxEnergy;
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
            if (index < 0 || index >= moves.Length)
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
            // ...
        }

        // Update is called once per frame
        protected virtual void Update()
        {

        }
    }
}