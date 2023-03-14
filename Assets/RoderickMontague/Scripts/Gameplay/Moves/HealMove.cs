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

                // TODO: the health restore animation is broken (it goes instantly instead of gradually now).
                // I don't know what happened, but fix it.
                user.Health += user.MaxHealth * healPercent; // Heal

                // Update the energy display for the player if they're the one that used the move.
                // The health is updated by calling 'PlayAnimations'
                if (user is Player) // User is player.
                {
                    battle.UpdatePlayerEnergyUI();
                }                    

                // TODO: overlaps with the button SFX.
                // Play the move effect sfx.
                // battle.PlayMoveEffectSfx();

                // Prints a different message based on if the move actually did anything.
                if (success) // The move success message - health was restored.
                {
                    InsertPageAfterCurrentPage(battle, GetMoveSuccessfulPage());

                    // Play the heal animation.
                    if (success)
                        PlayAnimations(user, target, battle, moveEffect.heal, moveEffect.none);
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