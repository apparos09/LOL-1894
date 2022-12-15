using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace RM_BBTS
{
    // The run move, which is selected if the player attempts to run.
    // This move has no functionality. This just allows the turn to go through if the player's run fails.
    public class RunMove : Move
    {
        // The success chance of running away.
        const float SUCCESS_CHANCE = 0.5F;

        // Constructor for the charge move.
        public RunMove() :
            base(moveId.run, "<Run>", 1, 0, SUCCESS_CHANCE, 0)
        {
            description = "<The user attempts to run away. There is a 50% chance of success.>";

            // Don't use the accuracy parameter.
            useAccuracy = false;

            // This is arbitrary. It's just there for making the run failed message appear first.
            priority = 10;

            // Loads in the translation for the run name and description.
            LoadTranslation("mve_run_nme", "mve_run_dsc");
        }

        // Called when performing a move.
        public override bool Perform(BattleEntity user, BattleEntity target, BattleManager battle)
        {
            // Running away text.
            // battle.turnText.Add(new Page(user.displayName + " is trying to run away!"));

            // TODO: try to move the run call here?
            // Maybe only play a sound since the screen switches over before the textbox plays.

            // If the user isn't the player nothing will happen.
            // This should never be called for an entity.
            if (user is Player) // Player
            {
                // Calls the run function.
                // bool success = AccuracySuccessful(user, false);
                bool success = GenerateRandomFloat01() <= SUCCESS_CHANCE;

                // Checks if the player was able to run away successfully.
                if (success)
                {
                    // Ends the turn early.
                    battle.EndTurnEarly();

                    // Grabs the 'end turn early' page?
                    // This is set to run when the next page is opened. I don't think this is the end turn early page though.
                    // Maybe just add a (...) page after the current page and set it to the run page for safety?
                    Page page = battle.textBox.pages[battle.textBox.CurrentPageIndex + 1];

                    // Call run away when the page closes.
                    page.OnPageOpenedAddCallback(battle.RunAway);

                }
                else // Failed to run away.
                {
                    battle.textBox.pages.Insert(battle.textBox.CurrentPageIndex + 1, new Page(
                    BattleMessages.Instance.GetMoveRunFailedMessage(user.displayName),
                    BattleMessages.Instance.GetMoveRunFailedSpeakKey0()
                    ));
                }

                return success;
         

            }
            else // Not player.
            {
                battle.textBox.pages.Insert(battle.textBox.CurrentPageIndex + 1, new Page(
                    BattleMessages.Instance.GetMoveNothingMessage(),
                    BattleMessages.Instance.GetMoveNothingSpeakKey()
                    ));
            }

            return false;
        }
    }
}