using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_BBTS
{
    // Return all stats to zero.
    public class StatClearMove : Move
    {
        // The stat clear move.
        public StatClearMove()
            : base(moveId.statClear, "Stat Clear", 2, 0.0F, 100.0F, 0.30F)
        {
            useAccuracy = false;

            description = "The user resets all stat changes for themselves and their opponent.";

            // Animation
            animation = moveAnim.colorWave1;
            animationColor = new Color(0.971F, 0.897F, 0.982F);

            LoadTranslation("mve_statClear_nme", "mve_statClear_dsc");
        }

        // Called when performing a move.
        public override bool Perform(BattleEntity user, BattleEntity target, BattleManager battle)
        {
            // Checks if the move is usable.
            bool success = false;

            // Checks if the move is usable (enough energy).
            if (Usable(user))
            {
                // Reduce the user's energy.
                ReduceEnergy(user);

                // Checks if the move successfully hit its target.
                if (!AccuracySuccessful(user)) // Move missed.
                {
                    InsertPageAfterCurrentPage(battle, GetMoveMissedPage());
                    return false;

                }
                else if(!TargetIsVulnerable(target))
                {
                    // Note that the move won't effect the user or the opponent if one can't be hit.
                    InsertPageAfterCurrentPage(battle, GetMoveFailedPage());
                    return false;
                }

                // There are stat changes to be reset.
                if (user.HasStatModifiers() || target.HasStatModifiers())
                {
                    // Reset the modifiers.
                    user.ResetStatModifiers();
                    target.ResetStatModifiers();

                    InsertPageAfterCurrentPage(battle, GetMoveSuccessfulPage());
                    success = true;
                }
                else // No changes to be made.
                {
                    InsertPageAfterCurrentPage(battle, GetMoveFailedPage());
                    success = false;
                }

                // Updates the player's energy UI if the user is a player.
                if (user is Player) // Player
                {
                    battle.gameManager.UpdatePlayerEnergyUI();
                }

                if (success) // Play the status animations for both.
                    PlayAnimations(user, target, battle, moveEffect.status, moveEffect.status);

            }
            else // Not usable.
            {
                // The move failed.
                InsertPageAfterCurrentPage(battle, GetMoveNoEnergyMessage(user));
                success = false;
            }

            return success;
        }
    }
}