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
            base(moveId.charge, "Charge", 1, 0, 1.0F, 0)
        {
            // Loads in the translation for the run name and description.
            LoadTranslation("mve_charge_nme", "mve_charge_dsc");
        }

        // Called when performing a move.
        public override bool Perform(BattleEntity user, BattleEntity target, BattleManager battle)
        {
            // Charging text.
            battle.textBox.pages.Insert(battle.textBox.CurrentPageIndex + 1, new Page(BattleMessages.Instance.GetMoveChargeUsedMessage(user.displayName)));

            float chargePlus = user.MaxEnergy * 0.4F;
            user.Energy += chargePlus;

            // Updates the player's energy level.
            battle.gameManager.UpdatePlayerEnergyUI();

            return true;
        }
    }
}