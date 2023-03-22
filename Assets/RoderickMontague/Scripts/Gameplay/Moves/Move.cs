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
        // An enum used to determine what animaton to play for the user, and the target.
        public enum moveEffect { none, hurt, status, heal};

        // The number of the move.
        protected moveId id = 0;

        // STATS //
        // The name of the move.
        protected string name;

        // The speak key for the move name.
        public string nameSpeakKey = "";

        // The rank of the move.
        protected int rank;

        // The power that a move has.
        protected float power;

        // The accuracy of a move (0.0 - 1.0)
        protected float accuracy;

        // The amount of energy a move uses.
        protected float energyUsage;

        // The description of a move.
        public string description = "";

        // The speak key for the description.
        public string descSpeakKey = "";

        // A move of priority '0' has no priority. Two moves with the same priority are based on speed.
        // A move with a higher priority number goes first.
        public int priority = 0;

        // If set to 'false', the move will always hit if there's enough energy.
        public bool useAccuracy = true;

        // CHANGES //
        // The recoil applied when using the move. This is a percentage of the damage done to the target.
        protected float recoilPercent = 0.0F;

        // STATUS EFFECTS/CHANCE EVENTS //
        // The chance of performing critical damage.
        protected float criticalChance = 0.20F; // Originally 0.30

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
        public const float CRITICAL_BOOST = 1.75F; // 1.20 originally.
        
        // The move animation, and its color.
        public moveAnim animation = moveAnim.none;
        public Color animationColor = Color.white;

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

                // Saves the name speak key and description speak key.
                nameSpeakKey = nameKey;
                descSpeakKey = descKey;
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

        // Gets the power of the move as a string.
        public string GetPowerAsString()
        {
            // The result to be returned.
            string result = "";

            // Checks if power is greater than 0.
            result = (Power > 0) ? Power.ToString() : "-";

            return result;
        }

        // Gets the accuracy of the move as a string.
        public string GetAccuracyAsString()
        {
            // The result to be returned.
            string result = "";

            // Checks if accuracy should be used.
            if(useAccuracy)
            {
                // Checks if accuracy is above 0.
                result = (Accuracy > 0) ? Accuracy.ToString("F" + GameplayManager.DISPLAY_DECIMAL_PLACES.ToString()) : "-";
            }
            else
            {
                result = "-";
            }

            return result;
        }

        // Gets the energy usage as a string.
        public string GetEnergyUsageAsString()
        {
            // The result to be returned.
            string result = "";

            // Checks if energy usage is above 0.
            if(EnergyUsage > 0)
            {
                result = Mathf.Round(EnergyUsage * 100.0F).ToString("F" + GameplayManager.DISPLAY_DECIMAL_PLACES.ToString());
                result += "%";
            }
            else // Energy not used.
            {
                result = "-";
            }

            return result;
        }

        // Checks if a move is available for the battle entity to perform.
        public bool Usable(BattleEntity user)
        {
            // Grabs the amount of decimal place moves, and calculates the multiplier.
            int movePlaces = GameplayManager.DISPLAY_DECIMAL_PLACES;

            // The result of the comparison.
            bool result;

            // If the 'decimals' value is greater than 0, then the decimal point will be moved.
            // If the value of 'decimals' was 0, it would make the mult 1.0.
            // If the value of 'decimals' is less than 0, it would cause a negative exponent.
            // As such, any value that would result in a <= 0 exponent just does a direct comparison.
            if(movePlaces > 0)
            {
                // This moves the decimal point over to the right by (decimal) amount of times.
                // If 'decimals' is set to 0, then it will become 1.
                float mult = Mathf.Pow(10.0F, movePlaces);

                // Moves the decimal places and converts the values to an int.
                // This gets rid of the rest of the decimal content.
                // The values are floored for this comparison.
                int moveEnergyAdjust = Mathf.FloorToInt(CalculateEnergyUsed(user) * mult);
                int userEnergyAdjust = Mathf.FloorToInt(user.Energy * mult);

                // Compares the floored values.
                result = (moveEnergyAdjust <= userEnergyAdjust);
            }
            else
            {
                // Standard, direct calculation.
                result = (CalculateEnergyUsed(user) <= user.Energy);
            }

            return result;
        }

        // Checks to see if the target is vulnerable.
        public bool TargetIsVulnerable(BattleEntity target)
        {
            return target.vulnerable;
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
                float randFloat = BattleManager.GenerateRandomFloat01();

                // Checks if the modified accuracy should be used.
                // The accuracy isn't clamped to make sure the calculations work as intended.
                success = randFloat <= ((useModified) ? user.GetModifiedAccuracy(accuracy, false) : accuracy);

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

            // Tells the battle someone got a critical, which is needed to trigger the tutorial.
            // This was put here so that it applies to all the moves.
            // If gotCritical is 'true' already, don't change it so that the tutorial is guaranteed to trigger.
            if(!battle.gotCritical)
                battle.gotCritical = useCritBoost;

            // The damage amount.
            float damage;

            // Calculation
            // Original
            // damage = user.GetAttackModified() * (power * 0.15F) * critBoost - target.GetDefenseModified() * (power * 0.20F);

            // New
            // power * 0.75 * ((attack * 1.15)/(defense * 3.05)) * critical
            damage = power * 0.75F * ((user.GetAttackModified() * 1.15F) / (target.GetDefenseModified() * 3.05F)) * critBoost;

            damage = Mathf.Ceil(damage); // Round Up to nearest whole number.
            damage = damage <= 0 ? 1.0F : damage; // The attack should do at least 1 damage.

            // A critical hit will always do at least 1 extra point of damage.
            if (useCritBoost)
                damage += 1.0F;

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
        public List<Page> ApplyStatChanges(BattleEntity user, BattleEntity target, BattleManager battle)
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
                    randFloat = BattleManager.GenerateRandomFloat01();

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
                                GetAttackChangePage(entity, Mathf.Abs(diff), true, battle) :
                                GetAttackChangePage(entity, Mathf.Abs(diff), false, battle)
                                );
                        }
                        else // Limit was reached, so no change happened.
                        {

                            // Checks if it was a positive stat change or a negatie one.
                            if(statChange > 0) // Upper limit reached.
                            {
                                pages.Add(GetAttackLimitReachedPage(entity, true, battle));
                            }
                            else if(statChange < 0) // Lower limit reached.
                            {
                                pages.Add(GetAttackLimitReachedPage(entity, false, battle));
                            }
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
                    randFloat = BattleManager.GenerateRandomFloat01();

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
                                GetDefenseChangePage(entity, Mathf.Abs(diff), true, battle) :
                                GetDefenseChangePage(entity, Mathf.Abs(diff), false, battle)
                                );
                        }
                        else // Limit was reached, so no change happened.
                        {

                            // Checks if it was a positive stat change or a negative one.
                            if (statChange > 0) // Upper limit reached.
                            {
                                pages.Add(GetDefenseLimitReachedPage(entity, true, battle));
                            }
                            else if (statChange < 0) // Lower limit reached.
                            {
                                pages.Add(GetDefenseLimitReachedPage(entity, false, battle));
                            }
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
                    randFloat = BattleManager.GenerateRandomFloat01();

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
                                GetSpeedChangePage(entity, Mathf.Abs(diff), true, battle) :
                                GetSpeedChangePage(entity, Mathf.Abs(diff), false, battle)
                                );
                        }
                        else // Limit was reached, so no change happened.
                        {

                            // Checks if it was a positive stat change or a negatie one.
                            if (statChange > 0) // Upper limit reached.
                            {
                                pages.Add(GetSpeedLimitReachedPage(entity, true, battle));
                            }
                            else if (statChange < 0) // Lower limit reached.
                            {
                                pages.Add(GetSpeedLimitReachedPage(entity, false, battle));
                            }
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
                    randFloat = BattleManager.GenerateRandomFloat01();

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
                                GetAccuracyChangePage(entity, Mathf.Abs(diff), true, battle) :
                                GetAccuracyChangePage(entity, Mathf.Abs(diff), false, battle)
                                );
                        }
                        else // Limit was reached, so no change happened.
                        {

                            // Checks if it was a positive stat change or a negatie one.
                            if (statChange > 0) // Upper limit reached.
                            {
                                pages.Add(GetAccuracyLimitReachedPage(entity, true, battle));
                            }
                            else if (statChange < 0) // Lower limit reached.
                            {
                                pages.Add(GetAccuracyLimitReachedPage(entity, false, battle));
                            }
                        }
                    }
                }
            }

            // Returns all the pages.
            return pages;
        }

        // MESSAGES //
        // Gets the move no power message.
        public static Page GetMoveNoEnergyMessage(BattleEntity entity)
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
        public static Page GetMoveHitPage()
        {
            Page page = new Page(
                BattleMessages.Instance.GetMoveHitMessage(),
                BattleMessages.Instance.GetMoveHitSpeakKey()
                );

            return page;
        }

        // Gets the mvoe successful page.
        public static Page GetMoveSuccessfulPage()
        {
            Page page = new Page(
                BattleMessages.Instance.GetMoveSuccessfulMessage(),
                BattleMessages.Instance.GetMoveSuccessfulSpeakKey()
                );

            return page;
        }

        // Get move critical page.
        public static Page GetMoveHitCriticalPage()
        {
            Page page = new Page(
                BattleMessages.Instance.GetMoveHitCriticalMessage(),
                BattleMessages.Instance.GetMoveHitCriticalSpeakKey()
                );

            return page;
        }

        // Get move recoil page.
        public static Page GetMoveHitRecoilPage(BattleEntity entity)
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
        public static Page GetMoveBurnedPage(BattleEntity entity)
        {
            Page page;

            if(entity is Player) // The entity is a player.
            {
                page = new Page(
                    BattleMessages.Instance.GetMoveBurnedMessage(entity.displayName),
                    BattleMessages.Instance.GetMoveBurnedSpeakKey0()
                    );
            }
            else // The entity is an opponent.
            {
                page = new Page(
                    BattleMessages.Instance.GetMoveBurnedMessage(entity.displayName),
                    BattleMessages.Instance.GetMoveBurnedSpeakKey1()
                    );
            }

            return page;
        }

        // Get move paralyzed page.
        public static Page GetMoveParalyzedPage(BattleEntity entity)
        {
            Page page;

            if (entity is Player) // The entity is a player.
            {
                page = new Page(
                    BattleMessages.Instance.GetMoveParalyzedMessage(entity.displayName),
                    BattleMessages.Instance.GetMoveParalyzedSpeakKey0()
                );
            }
            else // The entity is an opponent.
            {
                page = new Page(
                    BattleMessages.Instance.GetMoveParalyzedMessage(entity.displayName),
                    BattleMessages.Instance.GetMoveParalyzedSpeakKey1()
                );
            }

            return page;
        }

        // Get the stat increased page.
        // 'Stages' should always be positive.
        public static Page GetStatIncreasePage(BattleEntity entity, string stat, int stages)
        {
            Page page; 
            
            if(entity is Player) // The entity is a player.
            {
                page = new Page(
                    BattleMessages.Instance.GetMoveStatIncreaseMessage(entity.displayName, stat, stages),
                    BattleMessages.Instance.GetMoveStatIncreaseSpeakKey0()
                );
            }
            else // The entity is an enemy.
            {
                page = new Page(
                    BattleMessages.Instance.GetMoveStatIncreaseMessage(entity.displayName, stat, stages),
                    BattleMessages.Instance.GetMoveStatIncreaseSpeakKey1()
                );
            }
            
            
            return page;
        }

        // Get the stat decreased page.
        // 'Stages' should always be positive.
        public static Page GetStatDecreasePage(BattleEntity entity, string stat, int stages)
        {
            Page page;

            if (entity is Player) // The entity is a player.
            {
                page = new Page(
                    BattleMessages.Instance.GetMoveStatDecreaseMessage(entity.displayName, stat, stages),
                    BattleMessages.Instance.GetMoveStatDecreaseSpeakKey0()
                );
            }
            else // The entity is an enemy.
            {
                page = new Page(
                    BattleMessages.Instance.GetMoveStatDecreaseMessage(entity.displayName, stat, stages),
                    BattleMessages.Instance.GetMoveStatDecreaseSpeakKey1()
                );
            }


            return page;
        }

        // Get attack change page ('increase' determines if it's an increase or a decrease).
        // 'Stages' should always be positive.
        public static Page GetAttackChangePage(BattleEntity entity, int stages, bool increase, BattleManager battle)
        {
            if(increase)
                return GetStatIncreasePage(entity, battle.gameManager.AttackString, stages);
            else
                return GetStatDecreasePage(entity, battle.gameManager.AttackString, stages);
        }

        // Get defense change page ('increase' determines if it's an increase or a decrease).
        // 'Stages' should always be positive.
        public static Page GetDefenseChangePage(BattleEntity entity, int stages, bool increase, BattleManager battle)
        {
            if (increase)
                return GetStatIncreasePage(entity, battle.gameManager.DefenseString, stages);
            else
                return GetStatDecreasePage(entity, battle.gameManager.DefenseString, stages);
        }

        // Get speed change page ('increase' determines if it's an increase or a decrease).
        // 'Stages' should always be positive.
        public static Page GetSpeedChangePage(BattleEntity entity, int stages, bool increase, BattleManager battle)
        {
            if (increase)
                return GetStatIncreasePage(entity, battle.gameManager.SpeedString, stages);
            else
                return GetStatDecreasePage(entity, battle.gameManager.SpeedString, stages);
        }

        // Get accuracy change page ('increase' determines if it's an increase or a decrease).
        // 'Stages' should always be positive.
        public static Page GetAccuracyChangePage(BattleEntity entity, int stages, bool increase, BattleManager battle)
        {
            if (increase)
                return GetStatIncreasePage(entity, battle.gameManager.AccuracyString, stages);
            else
                return GetStatDecreasePage(entity, battle.gameManager.AccuracyString, stages);
        }

        // Get stat limit reached message.
        public static Page GetMoveStatLimitReachedPage(BattleEntity entity, string stat, bool upperLimit)
        {
            Page page;

            if (entity is Player) // The entity is a player.
            {
                page = new Page(
                        BattleMessages.Instance.GetMoveStatLimitReachedMessage(entity.displayName, stat, upperLimit),
                        BattleMessages.Instance.GetMoveStatLimitReachedSpeakKey0(upperLimit)
                        );
            }
            else // The entity is an enemy.
            {
                page = new Page(
                        BattleMessages.Instance.GetMoveStatLimitReachedMessage(entity.displayName, stat, upperLimit),
                        BattleMessages.Instance.GetMoveStatLimitReachedSpeakKey1(upperLimit)
                        );
            }


            return page;
        }

        // Get attack limit reached.
        public static Page GetAttackLimitReachedPage(BattleEntity entity, bool upperLimit, BattleManager battle)
        {
            return GetMoveStatLimitReachedPage(entity, battle.gameManager.AttackString, upperLimit);
        }

        // Get defense limit reached.
        public static Page GetDefenseLimitReachedPage(BattleEntity entity, bool upperLimit, BattleManager battle)
        {
            return GetMoveStatLimitReachedPage(entity, battle.gameManager.DefenseString, upperLimit);
        }

        // Get speed limit reached.
        public static Page GetSpeedLimitReachedPage(BattleEntity entity, bool upperLimit, BattleManager battle)
        {
            return GetMoveStatLimitReachedPage(entity, battle.gameManager.SpeedString, upperLimit);
        }

        // Get accuracy limit reached.
        public static Page GetAccuracyLimitReachedPage(BattleEntity entity, bool upperLimit, BattleManager battle)
        {
            return GetMoveStatLimitReachedPage(entity, battle.gameManager.AccuracyString, upperLimit);
        }

        // MOVE MISSED/FAILED
        // Gets the move missed page.
        public static Page GetMoveMissedPage()
        {
            Page page = new Page(
                BattleMessages.Instance.GetMoveMissedMessage(),
                BattleMessages.Instance.GetMoveMissedSpeakKey()
                );

            return page;
        }

        // Gets the move failed page.
        public static Page GetMoveFailedPage()
        {
            Page page = new Page(
                BattleMessages.Instance.GetMoveFailedMessage(),
                BattleMessages.Instance.GetMoveFailedSpeakKey()
                );

            return page;
        }

        // Inserts a page after the current page.
        public static void InsertPageAfterCurrentPage(BattleManager battle, Page page)
        {
            battle.textBox.pages.Insert(battle.textBox.CurrentPageIndex + 1, page);
        }

        // Inserts multiple pages after the current page.
        public static void InsertPagesAfterCurrentPage(BattleManager battle, List<Page> pages)
        {
            if(pages.Count != 0)
                battle.textBox.pages.InsertRange(battle.textBox.CurrentPageIndex + 1, pages);
        }


        // Called when the move is being performed.
        // order: the order number for the move. 1 = first move of the turn, 2 = second move of the turn.
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


            // Checks if the target can actually be hit.
            if(TargetIsVulnerable(target))
            {
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
                        useCritBoost = BattleManager.GenerateRandomFloat01() <= criticalChance;

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

                    // Saves the old target health.
                    float oldTargetHealth = target.Health;

                    // Damages the target.
                    target.Health -= damage;

                    // Damages the user with recoil.
                    // Calculates recoil damage (always does at least 1 damage).

                    // TODO: maybe change the recoil is based off of the amount of health lost, not the damage done.
                    float recoilDamage = damage * recoilPercent; // Old (damage done)
                    // float recoilDamage = (oldTargetHealth - target.Health) * recoilPercent; // New (health lost)
                    
                    if (recoilDamage < 1.0F && recoilPercent != 0.0F) // Always does at least 1 damage.
                        recoilDamage = 1.0F;

                    // Reduces the user's health.
                    // OLD
                    // // If it would kill the user, then they are left with 1 health.
                    // user.Health = (user.Health - recoilDamage < 0.0F) ? 1.0F : user.Health - recoilDamage;
                
                    // NEW
                    // Recoil can kill the user.
                    // If both the player and the opponent die, the player wins.
                    user.Health -= recoilDamage;

                    // Moved so that the user uses energy regardless of if the move goes off or not.
                    // Uses energy.
                    // user.Energy -= energyUsed; // energy

                    // Adds the new page.
                    if (useCritBoost) // Critical
                    {
                        newPages.Add(GetMoveHitCriticalPage());
                    }
                    else // No Critical
                    {
                        newPages.Add(GetMoveHitPage());
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

                        // Mark that recoil has been received.
                        battle.gotRecoil = true;
                    }

                    // Burn Infliction
                    if (!target.burned && BattleManager.GenerateRandomFloat01() < burnChance)
                    {
                        target.burned = true;

                        newPages.Add(GetMoveBurnedPage(target));
                    }

                    // Paralysis Infliction
                    if (!target.paralyzed && BattleManager.GenerateRandomFloat01() < paralysisChance)
                    {
                        target.paralyzed = true;

                        newPages.Add(GetMoveParalyzedPage(target));
                    }

                    // STAT CHANGES
                    if(HasStatChanges()) // There are stat changes to apply.
                    {
                        // Gets the pages for applying stat changes.
                        List<Page> statPages = ApplyStatChanges(user, target, battle);

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
                    // if (user is Player) // Is the player.
                    // {
                    //     // Moved.
                    //     // battle.gameManager.UpdatePlayerEnergyUI();
                    //     battle.UpdateOpponentUI(); // Updates enemy health bar.
                    // 
                    //     // Play opponent's damage animation.
                    //     battle.PlayOpponentHurtAnimation();
                    // }
                    // else // Not the player.
                    // {
                    //     battle.gameManager.UpdatePlayerHealthUI();
                    // 
                    //     // Play palyer damage animation.
                    //     battle.PlayPlayerHurtAnimation();
                    // }

                    // Play the animations. 
                    if (this is HealthDrainMove) // Checks if it's a health drain move. 
                    {
                        // Play heal and hurt animations.
                        PlayAnimations(user, target, battle, moveEffect.heal, moveEffect.hurt);
                    }
                    else // Regular move. 
                    {
                        // Play hurt animation onyl.
                        PlayAnimations(user, target, battle, moveEffect.none, moveEffect.hurt);
                    }

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
            else
            {
                // The target isn't vulnerable, so the move failed.
                InsertPageAfterCurrentPage(battle, GetMoveFailedPage());

                // Move failed.
                return false;
            }
            
        }

        // Plays animations for the move, taking the user, target, battle, the user's effect, and the target's effect.
        public void PlayAnimations(BattleEntity user, BattleEntity target, BattleManager battle,
            moveEffect userEffect, moveEffect targetEffect)
        {
            // If animations should play, and the move has a proper animation.
            if (BattleManager.PLAY_IDLE_AND_MOVE_ANIMATIONS && animation != moveAnim.none)
            {
                // Sets the information and plays the animation.
                // The animation is flipped if the opponent is using the move.
                battle.moveAnimation.SetMove(this, user, target, battle, userEffect, targetEffect, !(user is Player));
                battle.moveAnimation.PlayAnimation(animation);
            }
            else
            {
                // Shows the move performance results right away.
                ShowPerformanceResults(user, target, battle, userEffect, targetEffect);
            }
        }

        // Shows the performance results of the move, which calls functions to play animations and SFX.
        public virtual void ShowPerformanceResults(BattleEntity user, BattleEntity target, BattleManager battle,
            moveEffect userEffect, moveEffect targetEffect)
        {
            // The list of effects.
            List<BattleEntity> entities = new List<BattleEntity>() { user, target };
            List<moveEffect> effects = new List<moveEffect>() { userEffect, targetEffect };


            // Goes through each animation.
            for (int i = 0; i < entities.Count && i < effects.Count; i++)
            {
                // Checks if the entity is the player, or the enemy.
                bool isPlayer = entities[i] is Player;

                // NOTE: the energy isn't updated here because this doesn't get called if the move fails.
                // But if the move fails, it still uses energy.

                // Checks the effect.
                switch (effects[i])
                {
                    case moveEffect.hurt:
                        // Checks who the animation applies to.
                        if (isPlayer)
                        {
                            battle.UpdatePlayerHealthUI();
                            battle.PlayPlayerHurtAnimation();
                        }
                        else
                        {
                            battle.UpdateOpponentUI();
                            battle.PlayOpponentHurtAnimation();
                        }

                        break;

                    case moveEffect.status:
                        // Checks who the animation applies to.
                        if (isPlayer)
                        {
                            battle.PlayPlayerStatusAnimation();
                        }
                        else
                        {
                            battle.PlayOpponentStatusAnimation();
                        }

                        break;

                    case moveEffect.heal:
                        // Checks who the animation applies to.
                        if (isPlayer)
                        {
                            battle.UpdatePlayerHealthUI();
                            battle.PlayPlayerHealAnimation();
                        }
                        else
                        {
                            battle.UpdateOpponentUI();
                            battle.PlayOpponentHealAnimation();
                        }

                        break;
                }
            }

            //// Checks if the user is the player or not.
            //if (user is Player) // Is the player.
            //{
            //    // Checks which action to perform for the user's animation.
            //    switch(userEffect)
            //    {
            //        case moveEffect.hurt:
            //            battle.UpdatePlayerHealthUI();
            //            battle.PlayPlayerHurtAnimation();
            //            break;

            //        case moveEffect.status:
            //            battle.PlayPlayerStatusAnimation();
            //            break;

            //        case moveEffect.heal:
            //            battle.PlayPlayerHealAnimation();
            //            break;
            //    }

            //    // Checks which action to perform for the target's animation.
            //    switch (targetEffect)
            //    {
            //        case moveEffect.hurt:
            //            battle.UpdateOpponentUI();
            //            battle.PlayOpponentHurtAnimation();
            //            break;

            //        case moveEffect.status:
            //            battle.PlayOpponentStatusAnimation();
            //            break;

            //        case moveEffect.heal:
            //            battle.PlayOpponentHealAnimation();
            //            break;
            //    }
            //}
            //else // Not the player, which means its the opponent.
            //{
            //    // Checks which action to perform for the user's animation.
            //    switch (userEffect)
            //    {
            //        case moveEffect.hurt:
            //            battle.UpdateOpponentUI();
            //            battle.PlayOpponentHurtAnimation();
            //            break;

            //        case moveEffect.status:
            //            battle.PlayOpponentStatusAnimation();
            //            break;

            //        case moveEffect.heal:
            //            battle.PlayOpponentHealAnimation();
            //            break;
            //    }

            //    // Checks which action to perform for the target's animation.
            //    switch (targetEffect)
            //    {
            //        case moveEffect.hurt:
            //            battle.UpdatePlayerHealthUI();
            //            battle.PlayPlayerHurtAnimation();
            //            break;

            //        case moveEffect.status:
            //            battle.PlayPlayerStatusAnimation();
            //            break;

            //        case moveEffect.heal:
            //            battle.PlayPlayerHealAnimation();
            //            break;
            //    }
            //}
        }
    }
}