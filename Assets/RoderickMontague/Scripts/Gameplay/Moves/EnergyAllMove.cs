using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RM_BBTS
{
    // A move that uses all the user's energy.
    public class EnergyAllMove : Move
    {
        // The lowest amount of power the move can have.
        // The energy-percentage based power is based off of the provided power minus the lowest power.
        // power = lowestPower + (power * energy/maxEnergy).
        public float lowestPower = 30.0F;

        public EnergyAllMove(moveId id, string name, int rank, float power, float accuracy) : 
            base(id, name, rank, power, accuracy, 0.0F)
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
            float newPower = lowestPower + 
                Mathf.Ceil((power - lowestPower) * ((user.Energy + energyUsage * user.MaxEnergy) / user.MaxEnergy));
            
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

        // Called when performing a move.
        public override bool Perform(BattleEntity user, BattleEntity target, BattleManager battle)
        {
            bool success = base.Perform(user, target, battle);

            // The user's energy is now at 0.
            user.Energy = 0.0F;

            // Update the energy UI.
            if(user is Player)
                battle.UpdatePlayerEnergyUI();

            return success;
        }
    }
}