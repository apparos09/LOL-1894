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
        }

        // Called when performing a move.
        public override bool Perform(BattleEntity user, BattleEntity target, BattleManager battle)
        {
            // Charging text.
            battle.textBox.pages.Insert(battle.textBox.CurrentPageIndex + 1, new Page(user.displayName + " charged their energy!"));

            float chargePlus = user.MaxEnergy * 0.4F;
            user.Energy += chargePlus;

            // Updates the player's energy level.
            battle.gameManager.UpdatePlayerEnergyUI();

            return true;
        }
    }
}