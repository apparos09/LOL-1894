using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_BBTS
{
    // The move for an entity charging their energy.
    public class ChargeMove : Move
    {
        // Constructor for the charge move.
        public ChargeMove() : 
            base(moveId.charge, "<Charge>", 1, 0, 1.0F, 0)
        {

            description = "<The user charges their energy by 40%.>";
            useAccuracy = false;

            // Loads in the translation for the run name and description.
            LoadTranslation("mve_charge_nme", "mve_charge_dsc");
        }

        // Called when performing a move.
        public override bool Perform(BattleEntity user, BattleEntity target, BattleManager battle)
        {
            // Checks if the user has a full charge.
            if(user.HasFullCharge()) // User already has a full charge.
            {
                // Move failed page.
                battle.textBox.pages.Insert(battle.textBox.CurrentPageIndex + 1, new Page(
                                    BattleMessages.Instance.GetMoveFailedMessage(),
                                    BattleMessages.Instance.GetMoveFailedSpeakKey()));

                return false;
            }
            else // The user does not have a full amount of energy.
            {
                // This really shouldn't be needed since it would make no sense, but it's here anyway.
                // Checks if the move successfully hit its target.
                if (!AccuracySuccessful(user)) // Move missed.
                {
                    InsertPageAfterCurrentPage(battle, GetMoveMissedPage());
                    return false;

                }

                // Charging text.
                // Checks who is charging the energy to pass the correct speak key.
                if (user is Player) // Player
                {
                    battle.textBox.pages.Insert(battle.textBox.CurrentPageIndex + 1, new Page(
                                    BattleMessages.Instance.GetMoveChargeUsedMessage(user.displayName),
                                    BattleMessages.Instance.GetMoveChargeUsedSpeakKey0()));
                }
                else // Opponent
                {
                    battle.textBox.pages.Insert(battle.textBox.CurrentPageIndex + 1, new Page(
                                    BattleMessages.Instance.GetMoveChargeUsedMessage(user.displayName),
                                    BattleMessages.Instance.GetMoveChargeUsedSpeakKey1()));
                }


                float chargePlus = user.MaxEnergy * 0.4F;
                user.Energy += chargePlus;

                // Updates the player's energy level.
                battle.gameManager.UpdatePlayerEnergyUI();

                return true;
            } 
        }
    }
}
