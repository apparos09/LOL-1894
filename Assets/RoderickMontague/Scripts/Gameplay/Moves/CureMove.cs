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
            base(moveId.cure, "Cure", 2, 0.0F, 100.0F, 0.30F)
        {
            // Heal moves don't have power or accuracy.

            // Name and Desc
            name = "Cure";

            description = "The user removes all their status ailments.";

            // Animation
            animation = moveAnim.colorWave1;
            animationColor = new Color(0.897F, 0.982F, 0.974F);

            LoadTranslation("mve_cure_nme", "mve_cure_dsc");
        }

        // Called when performing a move.
        public override bool Perform(BattleEntity user, BattleEntity target, BattleManager battle)
        {
            // Checks if the move is usable.
            if (Usable(user))
            {
                // Checks if the user had a status to begin with.
                // If they didn't, then the move will use energy, but it will fail.
                bool hadStatus = user.burned || user.paralyzed;

                // Reduces the energy.
                ReduceEnergy(user);

                // This really shouldn't be needed since it would make no sense, but it's here anyway.
                // Checks if the move successfully hit its target.
                if (!AccuracySuccessful(user)) // Move missed.
                {
                    InsertPageAfterCurrentPage(battle, GetMoveMissedPage());
                    return false;

                }

                // Status.
                user.burned = false;
                user.paralyzed = false;

                // TODO: overlaps with the button SFX.
                // Play the move effect sfx.
                // battle.PlayMoveEffectSfx();

                // Prints a different message based on if the move actually did anything.
                if (hadStatus) // A status was cured.
                {
                    // Success page.
                    InsertPageAfterCurrentPage(battle, GetMoveSuccessfulPage());

                    // // Checks what status animation to play.
                    // if (user is Player) // Player
                    // {
                    //     battle.PlayPlayerStatusAnimation();
                    // }
                    // else // Opponent
                    // {
                    //     battle.PlayOpponentStatusAnimation();
                    // }
                    
                    // Play the status animation for the user.
                    PlayAnimations(user, target, battle, moveEffect.status, moveEffect.none);

                    return true;
                } 
                else // The entity never had a status, so the move failed.
                {
                    InsertPageAfterCurrentPage(battle, GetMoveFailedPage());
                    return false;
                }     
            }
            else // Not enough power.
            {
                // The battler does not have enough energy to perform the move.
                InsertPageAfterCurrentPage(battle, GetMoveNoEnergyMessage(user));

                return false;
            }
        }
    }
}