using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoLSDK;
using SimpleJSON;

namespace RM_BBTS
{
    // A class for a move.
    // This inherits from monobehaviour so that related functions can be called.
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
        protected float energyUsage;

        // TODO: add space for animation.

        // The description of a move.
        public string description = "";

        // A move of priority '0' has no priority. Two moves with the same priority are based on speed.
        // A move with a higher priority number goes first.
        public int priority = 0;

        // If set to 'false', the move will always hit if there's enough energy.
        public bool useAccuracy = true;

        // STATUS EFFECTS/CHANCE EVENTS //
        // The chance of performing critical damage.
        protected float criticalChance = 0.3F;

        // Chance of burning the opponent.
        protected float burnChance = 0.0F;

        // Chance of paralyzing the opponent.
        protected float paralysisChance = 0.0F;

        // TODO: replace name with file citation for translation.
        // Move constructor
        public Move(moveId id, string name, int rank, float power, float accuracy, float energyUsage)
        {
            this.id = id;
            this.name = name;
            this.rank = rank;
            this.power = power;
            this.accuracy = accuracy;
            this.energyUsage = energyUsage;

            // Default message.
            description = "No information available";
        }

        // Loads the translated information for the move.
        // Provided are the name key and the description key.
        public void LoadTranslation(string nameKey, string descKey)
        {
            // Grabs the language definitions.
            JSONNode defs = SharedState.LanguageDefs;

            // If the SDK has been initialized.
            if (defs != null)
            {
                // Loads in the name and description.
                name = defs[nameKey];
                description = defs[descKey];
            }

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
            get { return energyUsage; }
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
                criticalChance = Mathf.Clamp01(value);
            }
        }

        // Burn
        public float BurnChance
        {
            get { return burnChance; }

            set
            {
                burnChance = Mathf.Clamp01(value);
            }
        }

        // Paralysis
        public float ParalysisChance
        {
            get { return paralysisChance; }

            set
            {
                paralysisChance = Mathf.Clamp01(value);
            }
        }

        // Tries to end the turn early if one of the entities is dead.
        public bool TryEndTurnEarly(BattleEntity user, BattleEntity target, BattleManager battle)
        {
            // The user or the target is dead.
            if (user.Health <= 0 || target.Health <= 0)
            {
                // Ends the turn early.
                battle.EndTurnEarly();
                return true;
            }
            else // Don't end the battle early.
            {
                return false;
            }
        }


        // Called when the move is being performed.
        public virtual bool Perform(BattleEntity user, BattleEntity target, BattleManager battle)
        {
            // The move inserts a message after the current page in the text box.

            // Takes a percentage of the user's max energy.
            float energyUsed = user.MaxEnergy * energyUsage;

            // If there isn't enough energy to use the move, nothing happens.
            if (user.Energy < energyUsed)
            {
                // Checks object type.
                if(user is Player) // Player
                {
                    battle.textBox.pages.Insert(battle.textBox.CurrentPageIndex + 1, new Page(
                        BattleMessages.Instance.GetMoveNoPowerMessage(user.displayName),
                        BattleMessages.Instance.GetMoveNoPowerSpeakKey0()
                        ));
                }
                else // Opponent
                {
                    battle.textBox.pages.Insert(battle.textBox.CurrentPageIndex + 1, new Page(
                        BattleMessages.Instance.GetMoveNoPowerMessage(user.displayName),
                        BattleMessages.Instance.GetMoveNoPowerSpeakKey1()
                        ));
                }
                
                return false;
            }
                
            // If the move hit successfully (or if 'useAccuracy' is set to false, meaning it always hits)
            if(Random.Range(0.0F, 1.0F) <= accuracy * user.accuracyMod || !useAccuracy)
            {
                // The new pages.
                List<Page> newPages = new List<Page>();

                // Does damage.
                float damage = 0.0F;
                float critBoost = 1.0F;

                // Randomization chance for doing a critical (extra) damage.
                if(Random.Range(0.0F, 1.0F) <= criticalChance) // extra damage
                {
                    critBoost = 1.125F;
                }

                // Calculation
                damage = user.GetAttackModified() * (power * 0.15F) * critBoost - target.GetDefenseModified() * (power * 0.20F);
                damage = Mathf.Round(damage); // Round damage to whole number.
                damage = damage <= 0 ? 1.0F : damage; // The attack should do at least 1 damage.

                // If the target is the player.
                if(target is Player)
                {
                    // If the damage is higher than the amount of health the player 
                    battle.playerDamageTaken += (target.Health < damage) ? target.Health : damage;
                }

                // Damages the target.
                target.Health -= damage;

                // Uses energy.
                user.Energy -= energyUsed; // energy

                // Adds the new page.
                if(critBoost == 1.0F) // No critical
                    newPages.Add(new Page(
                        BattleMessages.Instance.GetMoveHitMessage(),
                        BattleMessages.Instance.GetMoveHitSpeakKey()
                        ));

                else // Critical
                    newPages.Add(new Page(
                        BattleMessages.Instance.GetMoveHitCriticalMessage(),
                        BattleMessages.Instance.GetMoveHitCriticalSpeakKey()
                        ));

                // Burn Infliction
                if(!target.burned && Random.Range(0.0F, 1.0F) < burnChance)
                {
                    target.burned = true;
                    newPages.Add(new Page(
                        BattleMessages.Instance.GetMoveBurnedMessage(),
                        BattleMessages.Instance.GetMoveBurnedSpeakKey()
                        ));
                }

                // Paralysis Infliction
                if (!target.paralyzed && Random.Range(0.0F, 1.0F) < paralysisChance)
                {
                    target.paralyzed = true;
                    newPages.Add(new Page(
                        BattleMessages.Instance.GetMoveParalyzedMessage(),
                        BattleMessages.Instance.GetMoveParalyzedSpeakKey()
                        ));
                }

                // Inserts a range of pages.
                battle.textBox.pages.InsertRange(battle.textBox.CurrentPageIndex + 1, newPages);

                // TODO: maybe move this to the battle script?
                // Checks if the user is the player or not.
                if (user is Player) // Is the player.
                {
                    battle.gameManager.UpdatePlayerEnergyUI();
                    battle.UpdateOpponentUI(); // Updates enemy health bar.
                }
                else // Not the player.
                {
                    battle.gameManager.UpdatePlayerHealthUI();
                }

                // Tries ending the turn early.
                TryEndTurnEarly(user, target, battle);

                return true;
            }
            else
            {
                // The move missed.
                battle.textBox.pages.Insert(battle.textBox.CurrentPageIndex + 1, new Page(
                    BattleMessages.Instance.GetMoveMissedMessage(),
                    BattleMessages.Instance.GetMoveMissedSpeakKey()));

                return false;
            }

            
            
        }
    }
}