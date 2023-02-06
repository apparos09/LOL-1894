using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_BBTS
{
    // Apply the state changes.
    public class StatChangeMove : Move
    {
        // The stat change move.
        public StatChangeMove(moveId id, string name, int rank, float energyUsage)
            : base(id, name, rank, 0.0F, 100.0F, energyUsage)
        {
            // LoadTranslation(nameKey, descKey);
        }

        // Called when performing a move.
        public override bool Perform(BattleEntity user, BattleEntity target, BattleManager battle)
        {
            // Checks if the move is usable.
            bool success = false;

            // Checks if the move is usable (enough energy).
            if(Usable(user))
            {
                // Reduce the user's energy.
                ReduceEnergy(user);

                // Checks if the move successfully hit its target.
                if (!AccuracySuccessful(user)) // Move missed.
                {
                    InsertPageAfterCurrentPage(battle, GetMoveMissedPage());
                    return false;

                }
                // Checks to see if this move will have effect on the target, and if the target can be hit.
                else if(
                    (attackChangeTarget != 0 || defenseChangeTarget != 0 ||
                    speedChangeTarget != 0 || accuracyChangeTarget != 0) && !TargetIsVulnerable(target))
                {
                    InsertPageAfterCurrentPage(battle, GetMoveFailedPage());
                    return false;
                }

                // Applies the stat changes.
                List<Page> statPages = ApplyStatChanges(user, target, battle);

                // If there are no pages, then the stats could not be changed, meaning that the move failed.
                if(statPages.Count == 0)
                {
                    InsertPageAfterCurrentPage(battle, GetMoveFailedPage());
                    success = false;
                }
                else // Implement stage pages.
                {
                    InsertPagesAfterCurrentPage(battle, statPages);
                    success = true;
                }

                // Updates the player's energy UI if the user is a player.
                if (user is Player) // Player
                {
                    battle.gameManager.UpdatePlayerEnergyUI();
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