using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_BBTS
{
    // The run move, which is selected if the player attempts to run.
    // This move has no functionality. This just allows the turn to go through if the player's run fails.
    public class RunMove : Move
    {
        // Constructor for the charge move.
        public RunMove() :
            base(moveId.run, "Run", 1, 0, 1.0F, 0)
        {
        }

        // Called when performing a move.
        public override bool Perform(BattleEntity user, BattleEntity target, BattleManager battle)
        {
            // Running away text.
            // battle.turnText.Add(new Page(user.displayName + " is trying to run away!"));

            // Maybe only play a sound since the screen switches over before the textbox plays.

            // If the user isn't the player nothing will happen.
            // This should never be called for an entity.
            if(user is Player) // Player
            {
                // If this message is shown then that means the player failed to run.
                battle.textBox.pages.Insert(battle.textBox.CurrentPageIndex + 1, new Page("The player failed to run."));
            }
            else // Not player.
            {
                battle.textBox.pages.Insert(battle.textBox.CurrentPageIndex + 1, new Page("Nothing happened."));
            }

            return true;
        }
    }
}