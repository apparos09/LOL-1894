using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace RM_BBTS
{
    // A move that is stronger the earlier it is used in a fight.
    public class TurnsLowMove : Move
    {
        // The lowest amount of power the move can have.
        public float lowestPower = 30.0F;

        // The amount of reduction for each turn taken.
        public float turnReduct = 0.1F;

        // Maximum amount of turns for power reduction.
        public uint turnsMax = 10;

        // Constructor.
        public TurnsLowMove(moveId id, string name, int rank, float power, float accuracy, float energyUsage) :
            base(id, name, rank, power, accuracy, energyUsage)
        {
            // Taken out in case the user changes the lowest power later.
            // // Swaps value if lowest power is equal to 0.
            // if (this.power < lowestPower)
            // {
            //     float temp = this.power;
            //     this.power = lowestPower;
            //     lowestPower = temp;
            // }
        }

        // Calculates the damage.
        public override float CalculateDamage(BattleEntity user, BattleEntity target, BattleManager battle, bool useCritBoost)
        {
            // The old power and the new power.
            // The energy has already been reduced, so this needs to add it back.
            float oldPower = power;
            float newPower = 0.0F;
            
            // If 10 turns have passed, this move 
            if(battle.TurnsPassed >= turnsMax) // Maximum amount of turns reached.
            {
                newPower = lowestPower;
            }
            else // Reduce the power.
            {
                newPower = lowestPower + Mathf.Ceil((power - lowestPower) * (1.0F - battle.TurnsPassed * turnReduct));
            }

            // The damage to be returned.
            float damage = 0.0F;

            // Sets the new power for damage calculation.
            power = newPower;

            // Calculate the damage.
            damage = base.CalculateDamage(user, target, battle, useCritBoost);

            // Return power to normal.
            power = oldPower;

            return damage;
        }
    }
}