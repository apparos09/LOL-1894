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
            : base(moveId.statClear, "<Stat Clear>", 2, 0.0F, 100.0F, 0.35F)
        {
            useAccuracy = false;

            description = "<The user resets all stat changes for themselves and their opponent.>";

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

                // There are stat changes to be reset.
                if(user.HasStatModifiers() || target.HasStatModifiers())
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
                if (user is Player)
                    battle.gameManager.UpdatePlayerEnergyUI();
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