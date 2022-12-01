using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_BBTS
{
    // Heals the user.
    public class HealMove : Move
    {
        // The heal percentage.
        public float healPercent = 0.0F;

        // Heals the user.
        public HealMove(moveId id) : 
            base(id, "<Heal>", 1, 0.0F, 100.0F, 0.1F)
        {
            // Heal moves don't have power or accuracy.

            // Checks the ID of the move.
            switch(id)
            {
                case moveId.heal: // Heal Move
                    rank = 1;

                    // Name and Desc
                    name = "<Heal>";
                    description = "The user heals themself by 12.5% of their max health.";

                    // Stats
                    power = 0.0F;
                    accuracy = 100.0F;
                    useAccuracy = false; // Move always succeeds.
                    energyUsage = 0.85F;

                    // Heal Amount
                    healPercent = 0.125F;

                    LoadTranslation("mve_heal_nme", "mve_heal_dsc");
                    break;                
            }
        }

        // Called when performing a move.
        public override bool Perform(BattleEntity user, BattleEntity target, BattleManager battle)
        {
            // If the user's move was enabled.
            if (Usable(user) && !user.HasFullHealth())
            {
                user.Health += user.MaxHealth * healPercent; // Heal
                ReduceEnergy(user); // Reduce energy

                // Update the health and the energy.
                battle.gameManager.UpdateUI();

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