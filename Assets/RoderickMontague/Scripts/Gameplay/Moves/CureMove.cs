using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_BBTS
{
    // Heals the user.
    public class CureMove : Move
    {
        // Heals the user.
        public CureMove() :
            base(moveId.cure, "<Cure>", 2, 0.0F, 100.0F, 0.1F)
        {
            // Heal moves don't have power or accuracy.

            // Name and Desc
            name = "<Cure>";
            description = "The user cures their status ailments.";

            LoadTranslation("mve_cure_nme", "mve_cure_dsc");
        }

        // Called when performing a move.
        public override bool Perform(BattleEntity user, BattleEntity target, BattleManager battle)
        {
            // If the user's move is usable, and has an ailment to heal.
            if (Usable(user) || (!user.burned && !user.paralyzed))
            {
                // Reduces the energy.
                ReduceEnergy(user);

                // Status.
                user.burned = false;
                user.paralyzed = false;

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