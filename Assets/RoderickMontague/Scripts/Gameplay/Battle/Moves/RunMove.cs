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
            // ...
            return true;
        }
    }
}