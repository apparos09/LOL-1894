using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_BBTS
{
    // Heals the user.
    public class HealMove : Move
    {
        // The heal percentage.
        public float healPercent = 1.0F;

        // Heals the user.
        public HealMove(moveId id, string name, int rank, float energyUsage) : 
            base(id, name, rank, 0.0F, 100.0F, energyUsage)
        {
            // Heal moves don't have power or accuracy.
            useAccuracy = false;
        }

        // Called when performing a move.
        public override bool Perform(BattleEntity user, BattleEntity target, BattleManager battle)
        {
            // If the user's move is usable.
            if (Usable(user))
            {
                // If the user already has full health, the move will fail to do anything.
                bool success = !user.HasFullHealth();

                ReduceEnergy(user); // Reduce energy

                // This really shouldn't be needed since it would make no sense, but it's here anyway.
                // Checks if the move successfully hit its target.
                if (!AccuracySuccessful(user)) // Move missed.
                {
                    InsertPageAfterCurrentPage(battle, GetMoveMissedPage());
                    return false;

                }

                user.Health += user.MaxHealth * healPercent; // Heal

                // Update the health and the energy.
                if (user is Player) // User is player.
                    battle.gameManager.UpdateUI();
                else // User is opponent.
                    battle.UpdateOpponentUI();

                // TODO: overlaps with the button SFX.
                // Play the move effect sfx.
                // battle.PlayMoveEffectSfx();

                // Prints a different message based on if the move actually did anything.
                if (success) // The move success message - health was restored.
                {
                    InsertPageAfterCurrentPage(battle, GetMoveSuccessfulPage());
                }
                else // The move fail message - entity was at full health.
                {
                    InsertPageAfterCurrentPage(battle, GetMoveFailedPage());
                }

                return success;

            }
            else // Not enough energy.
            {
                // Not enough energy to use the move.
                InsertPageAfterCurrentPage(battle, GetMoveNoEnergyMessage(user));

                return false;
            }
        }
    }
}