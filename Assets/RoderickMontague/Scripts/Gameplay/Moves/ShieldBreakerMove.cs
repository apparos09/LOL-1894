using RM_BBTS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A move used to break the target's shield.
public class ShieldBreakerMove : Move
{
    // Used to check if the target's shield was broken.
    private bool brokeShield = false;

    // The power boost for a shield breaker.
    public const float POWER_BOOST = 2.0F;

    // A shield breaker move.
    public ShieldBreakerMove(moveId id, string name, int rank, float power, float accuracy, float energyUsage) :
        base(id, name, rank, power, accuracy, energyUsage)
    {

    }

    // Calculates the amount of damage that will be given.
    public override float CalculateDamage(BattleEntity user, BattleEntity target, BattleManager battle, bool useCritBoost)
    {
        // Checks to see if damage should be multiplied.
        bool useBoost = brokeShield;

        // The power multipliers are applied.
        float oldPower = power;
        float newPower = (useBoost) ? power * POWER_BOOST : power;

        // Changes the power for the damage calculation, then puts it back to its default.
        power = newPower;
        float result = base.CalculateDamage(user, target, battle, useCritBoost);
        power = oldPower;

        // Returns the resulting power.
        return result;
    }

    // Called when performing a move.
    public override bool Perform(BattleEntity user, BattleEntity target, BattleManager battle)
    {
        // Used to check and see if the target's shiled as broken.
        brokeShield = false;

        // If a shield was broken, the move should always hit.
        bool oldUseAccuracy = useAccuracy;

        // If the target's move is a shield move.
        // Only the shield move can make the target invulnerable, but this move checks anyway so that the logic makes sense.
        // If the shield failed, the move won't do extra damage.
        if(target.selectedMove is ShieldMove && !target.vulnerable)
        {
            // Makes the target vulnerable.
            target.vulnerable = true;

            // The target's shield has been broken.
            brokeShield = true;
        }

        // If the shield was broken, make sure the move always hits.
        if(brokeShield)
        {
            // Change the variable.
            oldUseAccuracy = useAccuracy;
            useAccuracy = false;
        }

        // Performs the move.
        bool success = base.Perform(user, target, battle);

        // Set 'useAccuracy' back to its old value.
        if (brokeShield)
            useAccuracy = oldUseAccuracy;

        // Set back to default.
        brokeShield = false;

        // Result.
        return success;
    }


}
