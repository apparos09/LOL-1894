using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_BBTS
{
    // A move that does more damage based on the turn order.
    public class TurnOrderMove : Move
    {
        // The attack boost.
        public const float POWER_BOOST = 1.7F;

        // If 'true', the power is boosted for the user moving first.
        // If 'false', the power is boosted for the user moving last.
        public bool boostFirst = false;

        // Constructor.
        public TurnOrderMove(moveId id, string name, int rank, float power, float accuracy, float energyUsage) : 
            base(id, name, rank, power, accuracy, energyUsage)
        {
        }

        // Calculates the damage the move does.
        public override float CalculateDamage(BattleEntity user, BattleEntity target, BattleManager battle, bool useCritBoost)
        {
            // Checks to see if a boost will be used.
            bool useBoost = false;

            // Checks if the boost should be used.
            // The variable 'order' shows the order people have been going in.
            useBoost = (boostFirst && battle.order == 1) || (!boostFirst && battle.order == 2);


            // Calculates the new power for the move.
            float oldPower = power;
            float newPower = (useBoost) ? power * POWER_BOOST : power;

            // Changes the power for teh damage calculation, then puts it back.
            power = newPower;
            float result = base.CalculateDamage(user, target, battle, useCritBoost);
            power = oldPower;

            // Returns the resulting damage.
            return result;
        }

    }
}
