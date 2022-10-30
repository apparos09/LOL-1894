using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_BBTS
{
    // A class for a move.
    public class Move
    {
        // The number of the move.
        protected moveId id = 0;

        // The name of the move.
        protected string name;

        // The rank of the move.
        protected int rank;

        // The power that a move has.
        protected float power;

        // The accuracy of a move (0.0 - 1.0)
        protected float accuracy;

        // The amount of energy a move uses.
        protected float energy;

        // TODO: add space for animation.

        // The description of a move.
        public string description = "";

        // TODO: replace name with file citation for translation.
        // Move constructor
        public Move(moveId id, string name, int rank, float power, float accuracy, float energy)
        {
            this.id = id;
            this.name = name;
            this.rank = rank;
            this.power = power;
            this.accuracy = accuracy;
            this.energy = energy;

            // Default message.
            description = "No information available";
        }



        // Returns the name of the move.
        public string Name
        {
            get { return name; }
        }

        // Returns the power of the move.
        public float Power
        {
            get { return power; }
        }

        // Returns the accuracy of the move (0-1 range).
        public float Accuracy
        {
            get { return accuracy; }
        }

        // Returns the energy the move uses.
        public float Energy
        {
            get { return energy; }
        }

        // Called when the move is being performed.
        public virtual bool Perform(BattleEntity user, BattleEntity target)
        {
            // If there isn't enough energy to use the move, nothing happens.
            if (user.Energy < energy)
                return false;

            // Does damage.
            target.Health -= 1.0F; // power * user.Attack;

            // Uses energy.
            user.Energy -= energy;

            return true;
        }
    }
}