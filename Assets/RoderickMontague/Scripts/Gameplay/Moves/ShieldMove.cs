using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_BBTS
{
    // Defends the user from all damage for a turn.
    public class ShieldMove : Move
    {
        public ShieldMove(moveId id, string name, int rank, float power, float accuracy, float energyUsage) : 
            base(id, name, rank, power, accuracy, energyUsage)
        {
            // All shield have the same priority level.
            priority = 2;

            // Sets this to false so that the shield's accuracy is not effected by modifiers.
            // Also makes it so that the shield's accuracy does not display.
            useAccuracy = false;
        }

        // The success rate of the shield.
        public float SuccessRate
        {
            get { return accuracy; }

            set { accuracy = Mathf.Clamp01(value); }
        }

        // Called when performing a move.
        public override bool Perform(BattleEntity user, BattleEntity target, BattleManager battle)
        {
            // Checks if the move is usable.
            bool success = false;

            // Checks if the move is usable and successful.
            // Accuracy boosts do not effect shields
            if (Usable(user))
            {
                // Reduce the user's energy.
                ReduceEnergy(user);

                // Checks if the move will succeed.
                success = BattleManager.GenerateRandomFloat01() <= accuracy;

                // Checks if the the shield will succeed.
                if (success)
                {
                    // The user is now invulnerable for the rest of the turn.
                    user.vulnerable = false;

                    // Show the move was successful.
                    InsertPageAfterCurrentPage(battle, GetMoveSuccessfulPage());
                }
                else
                {
                    // Show the move failed.
                    InsertPageAfterCurrentPage(battle, GetMoveFailedPage());
                }

                // Checks the user.
                if (user is Player) // User is player.
                {
                    // Update the energy bar.
                    battle.UpdatePlayerEnergyUI();
                }

                if (success) // Play animations.
                    PlayAnimations(user, target, battle, moveEffect.status, moveEffect.none);

            }
            else // Not usable - not enough energy.
            {
                // The move failed - no energy.
                InsertPageAfterCurrentPage(battle, GetMoveNoEnergyMessage(user));
                success = false;
            }

            return success;
        }
    }
}