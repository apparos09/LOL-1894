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
            // Running away text.
            battle.turnText.Add(new Page(user.displayName + " is charging their energy!"));

            float chargePlus = user.MaxEnergy * 0.4F;
            user.Energy += chargePlus;
            return true;
        }
    }
}
