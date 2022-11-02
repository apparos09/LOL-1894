using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_BBTS
{
    // A class for a move.
    public class Move
    {
        // The number of the move.
        protected moveId id = 0;

        // The name of the move.
        protected string name;

        // The rank of the move.
        protected int rank;

        // The power that a move has.
        protected float power;

        // The accuracy of a move (0.0 - 1.0)
        protected float accuracy;

        // The amount of energy a move uses.
        protected float energy;

        // TODO: add space for animation.

        // The description of a move.
        public string description = "";

        // STATUS EFFECTS/CHANCE EVENTS //
        // The chance of performing critical damage.
        protected float criticalChance = 0.3F;

        // Chance of burning the opponent.
        protected float burnChance = 0.0F;

        // Chance of paralyzing the opponent.
        protected float paralysisChance = 0.0F;

        // TODO: replace name with file citation for translation.
        // Move constructor
        public Move(moveId id, string name, int rank, float power, float accuracy, float energy)
        {
            this.id = id;
            this.name = name;
            this.rank = rank;
            this.power = power;
            this.accuracy = accuracy;
            this.energy = energy;

            // Default message.
            description = "No information available";
        }

        // Returns the ID of the move.
        public moveId Id
        {
            get { return id; }
        }

        // Returns the name of the move.
        public string Name
        {
            get { return name; }
        }

        // Gets the rank of the move.
        public int Rank
        {
            get { return rank; }
        }

        // Returns the power of the move.
        public float Power
        {
            get { return power; }
        }

        // Returns the accuracy of the move (0-1 range).
        public float Accuracy
        {
            get { return accuracy; }
        }

        // Returns the energy the move uses.
        public float Energy
        {
            get { return energy; }
        }

        // Called to play the move animation.
        public void PlayAnimation()
        {
            // TODO: implement.
        }

        // Critical
        public float CriticalChance
        {
            get { return criticalChance; }

            set
            {
                criticalChance = Mathf.Clamp01(criticalChance);
            }
        }

        // Burn
        public float BurnChance
        {
            get { return burnChance; }

            set
            {
                burnChance = Mathf.Clamp01(burnChance);
            }
        }

        // Paralysis
        public float ParalysisChance
        {
            get { return paralysisChance; }

            set
            {
                paralysisChance = Mathf.Clamp01(paralysisChance);
            }
        }


        // Called when the move is being performed.
        public virtual bool Perform(BattleEntity user, BattleEntity target, BattleManager battle)
        {
            // The move inserts a message after the current page in the text box.

            // If there isn't enough energy to use the move, nothing happens.
            if (user.Energy < energy)
            {
                battle.textBox.pages.Insert(battle.textBox.CurrentPageIndex + 1, new Page(user.name + " does not have enough power!"));
                return false;
            }
                
            // If the move hit successfully.
            if(Random.Range(0.0F, 1.0F) <= accuracy)
            {
                // Does damage.
                float damage = 0.0F;
                float critBoost = 1.0F;

                // Randomization chance for doing a critical (extra) damage.
                if(Random.Range(0.0F, 1.0F) <= criticalChance) // extra damage
                {
                    critBoost = 1.125F;
                }

                // Calculation
                damage = user.Attack * (power * 0.25F) * critBoost - target.Defense * (power * 0.25F);

                damage = damage < 0 ? 1.0F : damage; // The attack should do at least 1 damage.
                target.Health -= damage; // power * user.Attack;

                // Uses energy.
                user.Energy -= energy;

                // Adds the new page.
                if(critBoost == 1.0F) // No critical
                    battle.textBox.pages.Insert(battle.textBox.CurrentPageIndex + 1, new Page("The move hit!"));
                else // Critical
                    battle.textBox.pages.Insert(battle.textBox.CurrentPageIndex + 1, new Page("The move hit, and it did critical damage!"));

                // TODO: maybe move this to the battle script?
                // Checks if the user is the player or not.
                if (user is Player) // Is the player.
                {
                    battle.gameManager.UpdatePlayerEnergyUI();
                    battle.UpdateUI(); // Updates enemy health bar.
                }
                else // Not the player.
                {
                    battle.gameManager.UpdatePlayerHealthUI();
                }

                return true;
            }
            else
            {
                // Failure.
                battle.textBox.pages.Insert(battle.textBox.CurrentPageIndex + 1, new Page("The move failed."));

                return false;
            }

            
            
        }
    }
}