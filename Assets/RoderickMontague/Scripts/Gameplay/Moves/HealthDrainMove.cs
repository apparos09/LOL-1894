using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_BBTS
{
    // Apply the state changes.
    public class HealthDrainMove : Move
    {
        // The percentage of the damage done that's used to heal the user.
        public float damageHealPercent = 1.0F;

        // The stat change move.
        public HealthDrainMove(moveId id, string name, int rank, float power, float accuracy, float energyUsage)
            : base(id, name, rank, power, accuracy, energyUsage)
        {
            // ...
        }

        // Calculates the amount of damage that will be given.
        public override float CalculateDamage(BattleEntity user, BattleEntity target, BattleManager battle, bool useCritBoost)
        {
            // Calculate the damage.
            float damage = base.CalculateDamage(user, target, battle, useCritBoost);

            // Restore health based on the damage done.
            user.Health += Mathf.Ceil(damage * damageHealPercent);

            return damage;
        }

        // Called when performing a move.
        public override bool Perform(BattleEntity user, BattleEntity target, BattleManager battle)
        {
            // Performs the move. 
            bool success = base.Perform(user, target, battle);

            // NOTE: because of the way this function is set up, this doesn't re-call the PlayAnimations() function. 

            // The animations are now handled in the base Perform function. 

            // // Attack was success. 
            // if(success) 
            // { 
            //     // The health of the target has already updated, so the other one needs to be called. 
            //     // Checks if the user is a Player or an Enemy. 
            //     if (user is Player) 
            //     { 
            //         // Updates player's health for healed content. 
            //         battle.UpdatePlayerHealthUI(); 
            //         battle.PlayPlayerHealAnimation(); 
            //     } 
            //     else 
            //     { 
            //         // Updates opponent's health. 
            //         battle.UpdateOpponentUI(); 
            //         battle.PlayOpponentHealAnimation(); 
            //     } 
            //  
            //      
            // } 

            return success;
        }
    }
}