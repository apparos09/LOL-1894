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
            if (Usable(user) && !user.HasFullHealth())
            {
                user.Health += user.MaxHealth * healPercent; // Heal
                ReduceEnergy(user); // Reduce energy

                // Update the health and the energy.
                if (user is Player) // User is player.
                    battle.gameManager.UpdateUI();
                else // User is opponent.
                    battle.UpdateOpponentUI();

                // The move success message.
                InsertPageAfterCurrentPage(battle, GetMoveSuccessfulPage());

                return true;

            }
            else // Move failed.
            {
                // The move failed message.
                InsertPageAfterCurrentPage(battle, GetMoveFailedPage());

                return false;
            }
        }
    }
}