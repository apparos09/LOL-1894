using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoLSDK;
using SimpleJSON;
using static UnityEngine.EventSystems.EventTrigger;

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

        // The recoil applied when using the move. This is a percentage of the damage done to the target.
        protected float recoilPercent = 0.0F;

        // STATUS EFFECTS/CHANCE EVENTS //
        // The chance of performing critical damage.
        protected float criticalChance = 0.3F;

        // Chance of burning the opponent.
        protected float burnChance = 0.0F;

        // Chance of paralyzing the opponent.
        protected float paralysisChance = 0.0F;

        // STAT CHANGES //
        // Stat increases/decreases
        // Change the attack.
        public int attackChangeUser = 0; // stages
        public int attackChangeTarget = 0; // stages
        public float attackChangeChanceUser = 0.0F; // chance
        public float attackChangeChanceTarget = 0.0F; // chance

        // Change the defense.
        public int defenseChangeUser = 0; // stages
        public int defenseChangeTarget = 0; // stages
        public float defenseChangeChanceUser = 0.0F; // chance
        public float defenseChangeChanceTarget = 0.0F; // chacne

        // Change the speed.
        public int speedChangeUser = 0; // stages
        public int speedChangeTarget = 0; // stages
        public float speedChangeChanceUser = 0.0F; // chance
        public float speedChangeChanceTarget = 0.0F; // chance

        // Change the accuracy.
        public int accuracyChangeUser = 0; // stages
        public int accuracyChangeTarget = 0; // stages
        public float accuracyChangeChanceUser = 0.0F; // chance
        public float accuracyChangeChanceTarget = 0.0F; // chance

        // The boost for critical damage.
        private float CRITICAL_BOOST = 1.125F;

        // TODO: replace name with file citation for translation.
        // Move constructor
        public Move(moveId id, string name, int rank, float power, float accuracy, float energyUsage)
        {
            this.id = id;
            this.name = name;
            this.rank = rank;
            this.power = (power >= 0.0F) ? power : 0.0F;
            this.accuracy = Mathf.Clamp01(accuracy);
            this.energyUsage = Mathf.Clamp01(energyUsage);

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
        public float EnergyUsage
        {
            get { return energyUsage; }
        }

        // Called to play the move animation.
        public void PlayAnimation()
        {
            // TODO: implement.
        }

        // Sets the percentage of recoil received from the attack, based on the damage to the target (0.0-1.0 range).
        public float RecoilPercent
        {
            get { return recoilPercent; }

            set
            {
                recoilPercent = Mathf.Clamp01(value);
            }
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

        // Checks if a move is available for the battle entity to perform.
        public bool Usable(BattleEntity user)
        {
            return (CalculateEnergyUsed(user) <= user.Energy);
        }

        // Generates a random float in the 0-1 range.
        public static float GenerateRandomFloat01()
        {
            return Random.Range(0.0F, 1.0F);
        }

        // Checks if a move accuracy returns a success.
        // This does not factor in anything else that would make the move fail.
        public bool AccuracySuccessful(BattleEntity user, bool useModified = true)
        {
            // Returns 'true' if the move would hit its target.
            // If the move always hits, 'useAccuracy' is set to true.
            bool success = false;

            if(useAccuracy) // May Not Succeed
            {
                float randFloat = GenerateRandomFloat01();

                // Checks if the modified accuracy should be used.
                success = randFloat <= ((useModified) ? user.GetModifiedAccuracy(accuracy) : accuracy);

            }
            else // Always Succeeds
            {
                success = true;
            }

            // Successful.
            return success;
        }

        // Gets the amount of used energy. This does NOT actually apply it.
        public float CalculateEnergyUsed(BattleEntity user)
        {
            return user.MaxEnergy * energyUsage;
        }

        // Reduces the energy from using the move.
        public void ReduceEnergy(BattleEntity user)
        {
            // Reduce energy amount.
            user.Energy -= CalculateEnergyUsed(user);
        }

        // Calculates the damage the user it.
        public virtual float CalculateDamage(BattleEntity user, BattleEntity target, BattleManager battle, bool useCritBoost)
        {
            // The critical boost.
            float critBoost = (useCritBoost) ? CRITICAL_BOOST : 1.0F;
            // Tells the battle someone got a critical.
            battle.gotCritical = true;

            // The damage amount.
            float damage;

            // Calculation
            damage = user.GetAttackModified() * (power * 0.15F) * critBoost - target.GetDefenseModified() * (power * 0.20F);
            damage = Mathf.Round(damage); // Round damage to whole number.
            damage = damage <= 0 ? 1.0F : damage; // The attack should do at least 1 damage.

            // Returns the damage amount.
            return damage;
        }

        // The move has the ability to change stats (not counting health and energy).
        public bool HasStatChanges()
        {
            // Checks if the stats can change.
            bool canChange = false;

            // Changes available.
            canChange = (
                (attackChangeUser != 0 && attackChangeChanceUser > 0.0F) ||
                (attackChangeTarget != 0 && attackChangeChanceTarget > 0.0F) ||

                (defenseChangeUser != 0 && defenseChangeChanceUser > 0.0F) ||
                (defenseChangeTarget != 0 && defenseChangeChanceTarget > 0.0F) ||

                (speedChangeUser != 0 && speedChangeChanceUser > 0.0F) ||
                (speedChangeTarget != 0 && speedChangeChanceTarget > 0.0F) ||

                (accuracyChangeUser != 0 && accuracyChangeChanceUser > 0.0F) ||
                (accuracyChangeTarget != 0 && accuracyChangeChanceTarget > 0.0F)
                );

            return canChange;
        }

        // Change the stats attached to the moves.
        public List<Page> ApplyStatChanges(BattleEntity user, BattleEntity target)
        {
            // THe list of pages.
            List<Page> pages = new List<Page>();

            // The list of entities.
            List<BattleEntity> entities = new List<BattleEntity>();
            entities.Add(user);
            entities.Add(target);
            
            // Applies the stat changes.
            foreach(BattleEntity entity in entities)
            {
                // A random float.
                float randFloat = 0.0F;

                // Used for getting the right chance factor.
                float chance = 0.0F;

                // ATTACK
                // Checks which chance value to use.
                chance = (entity == user) ? attackChangeChanceUser : attackChangeChanceTarget;

                // Non-0 chance of success.
                if (chance > 0.0F)
                {
                    // Float for chance generation.
                    randFloat = GenerateRandomFloat01();

                    // The change in the stat's value.
                    int statChange = (entity == user) ? attackChangeUser : attackChangeTarget;

                    // The stat should change.
                    if (randFloat <= chance)
                    {
                        int diff = entity.AttackMod;
                        entity.AttackMod += statChange;
                        diff = entity.AttackMod - diff;

                        // If a change occurred.
                        if (diff != 0)
                        {
                            pages.Add((diff > 0) ?
                                GetAttackChangePage(user, Mathf.Abs(diff), true) :
                                GetAttackChangePage(user, Mathf.Abs(diff), false)
                                );
                        }
                    }
                }


                // DEFENSE
                // Checks which chance value to use.
                chance = (entity == user) ? defenseChangeChanceUser : defenseChangeChanceTarget;

                // Non-0 chance of success.
                if (chance > 0.0F)
                {
                    // Float for chance generation.
                    randFloat = GenerateRandomFloat01();

                    // The change in the stat's value.
                    int statChange = (entity == user) ? defenseChangeUser : defenseChangeTarget;

                    // The stat should change.
                    if (randFloat <= chance)
                    {
                        int diff = entity.DefenseMod;
                        entity.DefenseMod += statChange;
                        diff = entity.DefenseMod - diff;

                        // If a change occurred.
                        if (diff != 0)
                        {
                            pages.Add((diff > 0) ?
                                GetDefenseChangePage(user, Mathf.Abs(diff), true) :
                                GetDefenseChangePage(user, Mathf.Abs(diff), false)
                                );
                        }
                    }
                }

                // SPEED
                // Checks which value to use.
                chance = (entity == user) ? speedChangeChanceUser : speedChangeChanceTarget;

                // Non-0 chance of success.
                if (chance > 0.0F)
                {
                    // Float for chance generation.
                    randFloat = GenerateRandomFloat01();

                    // The change in the stat's value.
                    int statChange = (entity == user) ? speedChangeUser : speedChangeTarget;

                    // The stat should change.
                    if (randFloat <= chance)
                    {
                        int diff = entity.SpeedMod;
                        entity.SpeedMod += statChange;
                        diff = entity.SpeedMod - diff;

                        // If a change occurred.
                        if (diff != 0)
                        {
                            pages.Add((diff > 0) ?
                                GetSpeedChangePage(user, Mathf.Abs(diff), true) :
                                GetSpeedChangePage(user, Mathf.Abs(diff), false)
                                );
                        }
                    }
                }

                // ACCURACY
                // Checks if a change should occur.
                chance = (entity == user) ? accuracyChangeChanceUser : accuracyChangeChanceTarget;

                // Non-0 chance of success.
                if (chance > 0.0F)
                {
                    // Float for chance generation.
                    randFloat = GenerateRandomFloat01();

                    // The change in the stat's value.
                    int statChange = (entity == user) ? accuracyChangeUser : accuracyChangeTarget;

                    // The stat should change.
                    if (randFloat <= chance)
                    {
                        int diff = entity.AccuracyMod;
                        entity.AccuracyMod += statChange;
                        diff = entity.AccuracyMod - diff;

                        // If a change occurred.
                        if (diff != 0)
                        {
                            pages.Add((diff > 0) ?
                                GetAccuracyChangePage(user, Mathf.Abs(diff), true) :
                                GetAccuracyChangePage(user, Mathf.Abs(diff), false)
                                );
                        }
                    }
                }
            }

            // Returns all the pages.
            return pages;
        }

        // MESSAGES //
        // Gets the move no power message.
        public Page GetMoveNoEnergyMessage(BattleEntity entity)
        {
            Page page;

            // The user is a player.
            if(entity is Player)
            {
                page = new Page(
                    BattleMessages.Instance.GetMoveNoEnergyMessage(entity.displayName),
                    BattleMessages.Instance.GetMoveNoEnergySpeakKey0()
                );
            }
            else // The user is the opponent.
            {
                page = new Page(
                    BattleMessages.Instance.GetMoveNoEnergyMessage(entity.displayName),
                    BattleMessages.Instance.GetMoveNoEnergySpeakKey1()
                    );
            }

            // Returns the page.
            return page;
        }
        // Get move hit page.
        public Page GetMoveHitPage()
        {
            Page page = new Page(
                BattleMessages.Instance.GetMoveHitMessage(),
                BattleMessages.Instance.GetMoveHitSpeakKey()
                );

            return page;
        }

        // Gets the mvoe successful page.
        public Page GetMoveSuccessfulPage()
        {
            Page page = new Page(
                BattleMessages.Instance.GetMoveSuccessfulMessage(),
                BattleMessages.Instance.GetMoveSuccessfulSpeakKey()
                );

            return page;
        }

        // Get move critical page.
        public Page GetMoveHitCriticalPage()
        {
            Page page = new Page(
                BattleMessages.Instance.GetMoveHitCriticalMessage(),
                BattleMessages.Instance.GetMoveHitCriticalSpeakKey()
                );

            return page;
        }

        // Get move recoil page.
        public Page GetMoveHitRecoilPage(BattleEntity entity)
        {
            Page page;

            if (entity is Player) // The entity is a player.
            {
                page = new Page(
                    BattleMessages.Instance.GetMoveHitRecoilMessage(entity.displayName),
                    BattleMessages.Instance.GetMoveHitRecoilSpeakKey0()
                );
            }
            else // The entity is an enemy.
            {
                page = new Page(
                    BattleMessages.Instance.GetMoveHitRecoilMessage(entity.displayName),
                    BattleMessages.Instance.GetMoveHitRecoilSpeakKey1()
                );
            }


            return page;
        }

        // Get move burned page.
        public Page GetMoveBurnedPage()
        {
            Page page = new Page(
                BattleMessages.Instance.GetMoveBurnedMessage(),
                BattleMessages.Instance.GetMoveBurnedSpeakKey()
                );

            return page;
        }

        // Get move paralyzed page.
        public Page GetMoveParalyzedPage()
        {
            Page page = new Page(
                BattleMessages.Instance.GetMoveParalyzedMessage(),
                BattleMessages.Instance.GetMoveParalyzedSpeakKey()
                );

            return page;
        }

        // Get the stat increased page.
        // 'Stages' should always be positive.
        public Page GetStatIncreasePage(BattleEntity entity, string stat, int stages)
        {
            Page page; 
            
            if(entity is Player) // The entity is a player.
            {
                page = new Page(
                    BattleMessages.Instance.GetMoveStatIncreaseMessage(entity.displayName, stat, stages),
                    BattleMessages.Instance.GetMoveStatIncreaseSpeakKey0(stages)
                );
            }
            else // The entity is an enemy.
            {
                page = new Page(
                    BattleMessages.Instance.GetMoveStatIncreaseMessage(entity.displayName, stat, stages),
                    BattleMessages.Instance.GetMoveStatIncreaseSpeakKey1(stages)
                );
            }
            
            
            return page;
        }

        // Get the stat decreased page.
        // 'Stages' should always be positive.
        public Page GetStatDecreasePage(BattleEntity entity, string stat, int stages)
        {
            Page page;

            if (entity is Player) // The entity is a player.
            {
                page = new Page(
                    BattleMessages.Instance.GetMoveStatDecreaseMessage(entity.displayName, stat, stages),
                    BattleMessages.Instance.GetMoveStatDecreaseSpeakKey0(stages)
                );
            }
            else // The entity is an enemy.
            {
                page = new Page(
                    BattleMessages.Instance.GetMoveStatDecreaseMessage(entity.displayName, stat, stages),
                    BattleMessages.Instance.GetMoveStatDecreaseSpeakKey1(stages)
                );
            }


            return page;
        }

        // Get attack change page ('increase' determines if it's an increase or a decrease).
        // 'Stages' should always be positive.
        public Page GetAttackChangePage(BattleEntity entity, int stages, bool increase)
        {
            if(increase)
                return GetStatIncreasePage(entity, "Attack", stages);
            else
                return GetStatDecreasePage(entity, "Attack", stages);
        }

        // Get defense change page ('increase' determines if it's an increase or a decrease).
        // 'Stages' should always be positive.
        public Page GetDefenseChangePage(BattleEntity entity, int stages, bool increase)
        {
            if (increase)
                return GetStatIncreasePage(entity, "Defense", stages);
            else
                return GetStatDecreasePage(entity, "Defense", stages);
        }

        // Get speed change page ('increase' determines if it's an increase or a decrease).
        // 'Stages' should always be positive.
        public Page GetSpeedChangePage(BattleEntity entity, int stages, bool increase)
        {
            if (increase)
                return GetStatIncreasePage(entity, "Speed", stages);
            else
                return GetStatDecreasePage(entity, "Speed", stages);
        }

        // Get accuracy change page ('increase' determines if it's an increase or a decrease).
        // 'Stages' should always be positive.
        public Page GetAccuracyChangePage(BattleEntity entity, int stages, bool increase)
        {
            if (increase)
                return GetStatIncreasePage(entity, "Accuracy", stages);
            else
                return GetStatDecreasePage(entity, "Accuracy", stages);
        }

        // Gets the move missed page.
        public Page GetMoveMissedPage()
        {
            Page page = new Page(
                BattleMessages.Instance.GetMoveMissedMessage(),
                BattleMessages.Instance.GetMoveMissedSpeakKey()
                );

            return page;
        }

        // Gets the move failed page.
        public Page GetMoveFailedPage()
        {
            Page page = new Page(
                BattleMessages.Instance.GetMoveFailedMessage(),
                BattleMessages.Instance.GetMoveFailedSpeakKey()
                );

            return page;
        }

        // Inserts a page after the current page.
        public void InsertPageAfterCurrentPage(BattleManager battle, Page page)
        {
            battle.textBox.pages.Insert(battle.textBox.CurrentPageIndex + 1, page);
        }

        // Inserts multiple pages after the current page.
        public void InsertPagesAfterCurrentPage(BattleManager battle, List<Page> pages)
        {
            if(pages.Count != 0)
                battle.textBox.pages.InsertRange(battle.textBox.CurrentPageIndex + 1, pages);
        }

        // TURN OTHER //
        // Tries to end the turn early if one of the entities is dead.
        public bool TryEndTurnEarly(BattleEntity user, BattleEntity target, BattleManager battle)
        {
            // The user or the target is dead.
            if (user.IsDead() || target.IsDead())
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
            // float energyUsed = user.MaxEnergy * energyUsage;
            float energyUsed = CalculateEnergyUsed(user);

            // If there isn't enough energy to use the move, nothing happens.
            if (user.Energy < energyUsed)
            {

                // The user doesn't have enough energy.
                InsertPageAfterCurrentPage(battle, GetMoveNoEnergyMessage(user));


                // // Checks object type.
                // if(user is Player) // Player
                // {
                //     battle.textBox.pages.Insert(battle.textBox.CurrentPageIndex + 1, new Page(
                //         BattleMessages.Instance.GetMoveNoEnergyMessage(user.displayName),
                //         BattleMessages.Instance.GetMoveNoEnergySpeakKey0()
                //         ));
                // }
                // else // Opponent
                // {
                //     battle.textBox.pages.Insert(battle.textBox.CurrentPageIndex + 1, new Page(
                //         BattleMessages.Instance.GetMoveNoEnergyMessage(user.displayName),
                //         BattleMessages.Instance.GetMoveNoEnergySpeakKey1()
                //         ));
                // }

                return false;
            }

            // Uses energy.
            ReduceEnergy(user);
            // user.Energy -= energyUsed; // energy


            // Updates the player's energy UI.
            if(user is Player)
                battle.gameManager.UpdatePlayerEnergyUI();

            // If the move hit successfully (or if 'useAccuracy' is set to false, meaning it always hits)
            if (AccuracySuccessful(user))
            {
                // The new pages.
                List<Page> newPages = new List<Page>();

                // NEW
                // The variable.
                bool useCritBoost = false;
                
                // If there is a critical chance, run the randomizer.
                if(criticalChance > 0.0F)
                    useCritBoost = GenerateRandomFloat01() <= criticalChance;

                // Calculates the damage.
                float damage = CalculateDamage(user, target, battle, useCritBoost);

                // OLD
                // Does damage.
                // float damage = 0.0F;
                // float critBoost = 1.0F;

                // // Randomization chance for doing a critical (extra) damage.
                // if(Random.Range(0.0F, 1.0F) <= criticalChance) // extra damage
                // {
                //     critBoost = 1.125F;
                // }
                // 
                // // Calculation
                // damage = user.GetAttackModified() * (power * 0.15F) * critBoost - target.GetDefenseModified() * (power * 0.20F);
                // damage = Mathf.Round(damage); // Round damage to whole number.
                // damage = damage <= 0 ? 1.0F : damage; // The attack should do at least 1 damage.

                // If the target is the player.
                if (target is Player)
                {
                    // If the damage is higher than the amount of health the player 
                    battle.playerDamageTaken += (target.Health < damage) ? target.Health : damage;
                }

                // Damages the target.
                target.Health -= damage;

                // Damages the user with recoil.
                // Calculates recoil damage (always does at least 1 damage).
                float recoilDamage = damage * recoilPercent;
                if (recoilDamage < 1.0F)
                    recoilDamage = 1.0F;

                // Reduces the user's health.
                // If it would kill the user, then they are left with 1 health.
                user.Health = (user.Health - recoilDamage < 0.0F) ? 1.0F : user.Health - recoilDamage;

                // Moved so that the user uses energy regardless of if the move goes off or not.
                // Uses energy.
                // user.Energy -= energyUsed; // energy

                // Adds the new page.
                if(useCritBoost) // Critical
                {
                    newPages.Add(GetMoveHitCriticalPage());

                    // newPages.Add(new Page(
                    //                         BattleMessages.Instance.GetMoveHitCriticalMessage(),
                    //                         BattleMessages.Instance.GetMoveHitCriticalSpeakKey()
                    //                         ));

                }
                else // No Critical
                {
                    newPages.Add(GetMoveHitPage());

                    // newPages.Add(new Page(
                    //                         BattleMessages.Instance.GetMoveHitMessage(),
                    //                         BattleMessages.Instance.GetMoveHitSpeakKey()
                    //                         ));
                }

                // Adds the recoil message, and triggers the recoil tutorial if applicable.
                if (recoilPercent != 0.0F)
                {
                    // Updates the user's HP for recoil damage.
                    if (user is Player) // Player
                        battle.UpdatePlayerHealthUI();
                    else // Opponent
                        battle.UpdateOpponentUI();

                    // Add the recoil page.
                    newPages.Add(GetMoveHitRecoilPage(user));
                    battle.gotRecoil = true;
                }

                // Burn Infliction
                if (!target.burned && GenerateRandomFloat01() < burnChance)
                {
                    target.burned = true;

                    newPages.Add(GetMoveBurnedPage());

                    // newPages.Add(new Page(
                    //     BattleMessages.Instance.GetMoveBurnedMessage(),
                    //     BattleMessages.Instance.GetMoveBurnedSpeakKey()
                    //     ));
                }

                // Paralysis Infliction
                if (!target.paralyzed && GenerateRandomFloat01() < paralysisChance)
                {
                    target.paralyzed = true;

                    newPages.Add(GetMoveParalyzedPage());

                    // newPages.Add(new Page(
                    //     BattleMessages.Instance.GetMoveParalyzedMessage(),
                    //     BattleMessages.Instance.GetMoveParalyzedSpeakKey()
                    //     ));
                }

                // STAT CHANGES
                if(HasStatChanges()) // There are stat changes to apply.
                {
                    // Gets the pages for applying stat changes.
                    List<Page> statPages = ApplyStatChanges(user, target);

                    // The list was made.
                    if(statPages != null)
                    {
                        // Changes were made, so add the pages to the end of the list.
                        if(statPages.Count != 0)
                        {
                            newPages.AddRange(statPages);
                        }
                    }
                }

                // Inserts a range of pages.
                InsertPagesAfterCurrentPage(battle, newPages);
                // battle.textBox.pages.InsertRange(battle.textBox.CurrentPageIndex + 1, newPages);


                // TODO: maybe move this to the battle script?
                // Checks if the user is the player or not.
                if (user is Player) // Is the player.
                {
                    // Moved.
                    // battle.gameManager.UpdatePlayerEnergyUI();
                    battle.UpdateOpponentUI(); // Updates enemy health bar.

                    // Play sound effect.
                    battle.PlayDamageTakenSfx();
                }
                else // Not the player.
                {
                    battle.gameManager.UpdatePlayerHealthUI();

                    // Play sound effect.
                    battle.PlayDamageGivenSfx();
                }

                // Tries ending the turn early.
                TryEndTurnEarly(user, target, battle);

                return true;
            }
            else
            {
                // The move missed.
                InsertPageAfterCurrentPage(battle, GetMoveMissedPage());

                // battle.textBox.pages.Insert(battle.textBox.CurrentPageIndex + 1, new Page(
                //     BattleMessages.Instance.GetMoveMissedMessage(),
                //     BattleMessages.Instance.GetMoveMissedSpeakKey()));

                return false;
            }

            
            
        }
    }
}