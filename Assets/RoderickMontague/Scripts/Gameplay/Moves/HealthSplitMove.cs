using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_BBTS
{
    // Combines the current health (percentage wise) and splits between the two.
    public class HealthSplitMove : Move
    {
        // The stat change move.
        public HealthSplitMove()
            : base(moveId.healthSplit, "Health Split", 1, 0, 1.0F, 0.2F)
        {
            description = "The user and the target add together their proportional health, then split said health evenly between themselves.";

            // Animation
            animation = moveAnim.colorWave1;
            animationColor = new Color(0.98F, 0.978F, 0.687F);

            // Loads the translation for the health.
            LoadTranslation("mve_healthSplit_nme", "mve_healthSplit_dsc");
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
                // Checks if the target can be hit.
                else if(!TargetIsVulnerable(target))
                {
                    InsertPageAfterCurrentPage(battle, GetMoveFailedPage());
                    return false;
                }

                // Calculates the percentages.
                float userHealthPercent = user.Health / user.MaxHealth;
                float targetHealthPercent = target.Health / target.MaxHealth;

                // Checks if the percentages are equal. If so, then nothing will change, so the move fails.
                if (userHealthPercent == targetHealthPercent) // Fail.
                {
                    InsertPageAfterCurrentPage(battle, GetMoveFailedPage());
                    success = false;
                }
                else // Success
                {
                    // The percentage of health for each entity.
                    float split = (userHealthPercent + targetHealthPercent) / 2.0F;

                    // Set the health values.
                    user.Health = user.MaxHealth * split;
                    target.Health = target.MaxHealth * split;

                    // Update the health for both the player and the opponent.
                    battle.UpdatePlayerHealthUI();
                    battle.UpdateOpponentUI();

                    InsertPageAfterCurrentPage(battle, GetMoveSuccessfulPage());
                    success = true;
                }

                // Updates the player's energy UI if the user is a player.
                if (user is Player)
                    battle.gameManager.UpdatePlayerEnergyUI();


                // Play status animations for both.
                if (success) 
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