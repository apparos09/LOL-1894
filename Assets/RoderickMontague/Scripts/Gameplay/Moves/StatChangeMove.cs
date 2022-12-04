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
            if(Usable(user) && AccuracySuccessful(user))
            {
                // Reduce the user's energy.
                ReduceEnergy(user);

                // Applies the stat changes.
                List<Page> statPages = ApplyStatChanges(user, target);

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