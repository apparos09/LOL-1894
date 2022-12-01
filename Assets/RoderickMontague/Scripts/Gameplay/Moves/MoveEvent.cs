using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// An event for a move.
namespace RM_BBTS
{
    // Used for an event with a move.
    public abstract class MoveEvent
    {
        public abstract void Perform(BattleEntity user, BattleEntity target, BattleManager battle);
    }
}