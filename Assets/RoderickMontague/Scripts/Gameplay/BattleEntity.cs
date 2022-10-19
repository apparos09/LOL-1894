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
        private float maxHealth = 1;
        private float health = 1;

        private float attack = 1;
        private float defense = 1;
        private float speed = 1;
        private float energy = 1;

        // Moves
        // The moves that the battle entity has.
        public Move[] moves = new Move[4] {null, null, null, null};

        // The total amount of moves.
        public const int MOVE_COUNT = 4;

        // Start is called before the first frame update
        protected virtual void Start()
        {

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

        // Update is called once per frame
        protected virtual void Update()
        {

        }
    }
}