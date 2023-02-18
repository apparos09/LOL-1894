using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_BBTS
{
    // A move that boosts the move's power if a status effect is involved.
    public class StatusAttackMove : Move
    {
        // The attack boost.
        public const float POWER_BOOST = 1.8F;

        // The kind of condition needed to get a boost.
        public bool userBurned = false;
        public bool userParalyzed = false;
        public bool targetBurned = false;
        public bool targetParalyzed = false;

        // Constructor.
        public StatusAttackMove(moveId id, string name, int rank, float power, float accuracy, float energyUsage) : 
            base(id, name, rank, power, accuracy, energyUsage)
        {
        }

        // Calculates the damage the move does.
        public override float CalculateDamage(BattleEntity user, BattleEntity target, BattleManager battle, bool useCritBoost)
        {
            // Checks to see if a boost will be used.
            bool useBoost = false;

            // Checks if the boost should be used.
            useBoost = (userBurned && user.burned) || (userParalyzed && user.paralyzed) ||
                (targetBurned && target.burned) || (targetParalyzed && target.paralyzed);


            // Calculates the new power for the move.
            float oldPower = power;
            float newPower = (useBoost) ? power * POWER_BOOST : power;

            // Changes the power for the damage calculation, then puts it back to its default.
            power = newPower;
            float result = base.CalculateDamage(user, target, battle, useCritBoost);
            power = oldPower;

            // Returns the resulting damage.
            return result;
        }
    }
}