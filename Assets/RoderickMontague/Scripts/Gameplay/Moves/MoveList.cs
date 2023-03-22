using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_BBTS
{
    // NOTE: organize moves based on rank (all rank 1 moves > all rank 2 moves > all rank 3 moves)
    // The list of move ids.
    public enum moveId { run, charge, 
        poke, slimeShot, laserShot, fireShot, elecShot, screech, slam, chip, toss, magnify, heal, hpDrain1, healthSplit, pushBack, powerFirst, powerLast, shield1, breaker1, bam, 
        laserBurst, fireBurst, elecBurst, soundWave, magnet, torch, electrify, motivate, quickBurst, hpDrain2, statClear, cure, risk, tidalWave, burnBoostTarget, paraBoostTarget, shield2, breaker2, wham, 
        laserBlast, fireBlast, elecBlast, sonicWave, hpDrain3, twister, waterBlast, rockBlast, airBlast, quake, chargeSun, chargeMoon, earlyBurst, allOut, burnBoostUser, paraBoostUser, shield3, breaker3, kablam
    }

    // The list of moves for the game.
    public class MoveList : MonoBehaviour
    {
        // The instance of the move list.
        private static MoveList instance;

        // The starting move for the randomizer.
        public const int MOVE_ID_RAND_START = ((int)moveId.charge) + 1;

        // The move ID count.
        public const int MOVE_ID_COUNT = ((int)LAST_RANK_3) + 1;

        // The run move that is used to play through the turn.
        private static RunMove runMove;

        // The charge move that entities use.
        // This is copied whenever someone performs a charge, and is never put into the 1-4 move slots.
        private static ChargeMove chargeMove;

        // The last rank 1 move.
        private const moveId LAST_RANK_1 = moveId.bam;

        // The last rank 2 move.
        private const moveId LAST_RANK_2 = moveId.wham;

        // The last rank 3 move.
        private const moveId LAST_RANK_3 = moveId.kablam;

        // TODO: include list of move animations.

        // Constructor.
        private MoveList()
        {
            // Saves the run move from the list.
            if (runMove == null)
                runMove = (RunMove)GenerateMove(moveId.run);

            // Saves the charge move from the list.
            if (chargeMove == null)
                chargeMove = (ChargeMove)GenerateMove(moveId.charge);

            // Instance not set.
            if (instance == null)
                instance = this;
            else
                Destroy(this);
        }

        // Start is called just before any of the Update methods is called the first time.
        private void Start()
        {
            
        }

        // Gets the instance.
        public static MoveList Instance
        {
            get
            {
                // Generates the instance if it isn't set.
                if (instance == null)
                {
                    // Searches for the instance if it is not set.
                    instance = FindObjectOfType<MoveList>(true);

                    // No instance found, so make a new object.
                    if (instance == null)
                    {
                        GameObject go = new GameObject("Move List");
                        instance = go.AddComponent<MoveList>();
                    }

                }

                return instance;
            }

        }

        // Gets the run move.
        public RunMove RunMove
        {
            get { return runMove; }
        }


        // Gets the charge move.
        public ChargeMove ChargeMove
        {
            get { return chargeMove; }
        }


        // Converts the provided colour in (0-255) scale to a decimal number (0-1 scale)
        public static Color ConvertColor255ToDecimal(float r, float g, float b, float a)
        {
            Color color = new Color();
            color.r = r / 255.0F;
            color.g = g / 255.0F;
            color.b = b / 255.0f;
            color.a = a;

            return color;
        }

        // Generates the move.
        public Move GenerateMove(moveId id)
        {
            // The move object.
            Move move = null;

            // The keys for the move name and description key.
            string nameKey = "", descKey = "";

            switch (id)
            {
                // RANK 1 
                case moveId.run: // Run 
                    move = new RunMove();

                    // Translated in constructor. 
                    // No Battle Animation 
                    break;

                case moveId.charge: // Charge 
                    move = new ChargeMove();

                    // Translated in constructor. 
                    // No Battle Animation 
                    break;

                case moveId.poke: // Poke 
                    move = new Move(moveId.poke, "Poke", 1, 20.0F, 1.0F, 0.10F);

                    move.CriticalChance = 0.4F;

                    move.defenseChangeUser = 1;
                    move.defenseChangeChanceUser = 0.05F;

                    move.description = "An attack that has a 0.40 critical damage chance, and a 0.05 chance of raising the user's defense by 1 stage.";

                    // Animation 
                    move.animation = moveAnim.smack3;
                    move.animationColor = Color.white;

                    // Sets the keys for translating the data. 
                    nameKey = "mve_poke_nme";
                    descKey = "mve_poke_dsc";
                    break;

                case moveId.slimeShot: // Slime Shot 
                    move = new Move(moveId.slimeShot, "Slime Shot", 1, 30.0F, 0.95F, 0.14F);

                    move.speedChangeTarget = -1;
                    move.speedChangeChanceTarget = 0.15F;

                    move.description = "An attack that has a 0.15 chance of lowering the target's speed by 1 stage.";

                    // Animation 
                    move.animation = moveAnim.shot2;
                    move.animationColor = new Color(0.40F, 0.93F, 0.10F);

                    nameKey = "mve_slimeShot_nme";
                    descKey = "mve_slimeShot_dsc";
                    break;

                case moveId.laserShot: // Laser Shot 
                    move = new Move(moveId.laserShot, "Laser Shot", 1, 40.0F, 0.95F, 0.12F);

                    move.description = "A weak laser attack.";

                    // Sets the keys for translating the data. 
                    nameKey = "mve_laserShot_nme";
                    descKey = "mve_laserShot_dsc";

                    // Animation 
                    move.animation = moveAnim.shot1;
                    move.animationColor = new Color(0.942F, 0.19F, 98.7F);

                    break;

                case moveId.fireShot: // Fire Shot 
                    move = new Move(moveId.fireShot, "Fire Shot", 1, 30.0F, 0.95F, 0.14F);
                    move.BurnChance = 0.1F;

                    move.description = "A weak fire attack with a burn chance of 0.10.";

                    // Animation 
                    move.animation = moveAnim.shot1;
                    move.animationColor = new Color(0.956F, 0.305F, 0.071F);

                    // Sets the keys for translating the data. 
                    nameKey = "mve_fireShot_nme";
                    descKey = "mve_fireShot_dsc";
                    break;

                case moveId.elecShot: // Electric Shot 
                    move = new Move(moveId.elecShot, "Electric Shot", 1, 30.0F, 0.95F, 0.14F);
                    move.ParalysisChance = 0.1F;

                    move.description = "A weak electric attack with a paralysis chance of 0.10.";

                    // Animation 
                    move.animation = moveAnim.shot1;
                    move.animationColor = new Color(1.0F, 0.74F, 0.0F);

                    // Sets the keys for translating the data. 
                    nameKey = "mve_elecShot_nme";
                    descKey = "mve_elecShot_dsc";
                    break;

                case moveId.screech: // Screech 
                    move = new Move(moveId.screech, "Screech", 1, 25.0F, 0.95F, 0.12F);

                    move.accuracyChangeTarget = -1;
                    move.accuracyChangeChanceTarget = 0.15F;

                    move.description = "A weak attack that has a 0.15 chance of lowering the target's accuracy by 1 stage.";

                    // Animation 
                    move.animation = moveAnim.wave1;
                    move.animationColor = new Color(0.899F, 0.972F, 100.0F);

                    nameKey = "mve_screech_nme";
                    descKey = "mve_screech_dsc";
                    break;

                case moveId.slam: // Slam 
                    move = new Move(moveId.slam, "Slam", 1, 30, 0.90F, 0.15F);
                    move.CriticalChance = 0.6F;

                    move.description = "An attack that has a critical damage chance of 0.60.";

                    // Animation 
                    move.animation = moveAnim.slash2;
                    move.animationColor = new Color(1.0F, 0.973F, 0.973F);

                    nameKey = "mve_slam_nme";
                    descKey = "mve_slam_dsc";
                    break;

                case moveId.chip: // Chip Off 
                    move = new Move(moveId.chip, "Chip Off", 1, 20.0F, 0.95F, 0.12F);
                    move.priority = 1;

                    move.description = "A weak attack that always goes first.";

                    // Animation 
                    move.animation = moveAnim.slash1;
                    move.animationColor = new Color(0.956F, 0.603F, 0.603F);

                    nameKey = "mve_chip_nme";
                    descKey = "mve_chip_dsc";
                    break;

                case moveId.toss: // Toss 
                    move = new Move(moveId.toss, "Toss", 1, 25, 0.95F, 0.1F);

                    move.attackChangeTarget = -1;
                    move.attackChangeChanceTarget = 0.10F;

                    move.description = "An attack that has a 0.10 chance of lowering the target's attack by 1 stage.";

                    // Animation 
                    move.animation = moveAnim.slash1;
                    move.animationColor = new Color(0.978F, 0.905F, 0.866F);

                    nameKey = "mve_toss_nme";
                    descKey = "mve_toss_dsc";
                    break;

                case moveId.magnify: // Magnify 
                    move = new StatChangeMove(moveId.magnify, "Magnify", 1, 0.12F);

                    move.accuracyChangeUser = 1;
                    move.accuracyChangeChanceUser = 1.0F;

                    move.description = "The user raises their accuracy by 1 stage.";

                    // Animation 
                    move.animation = moveAnim.wave3;
                    move.animationColor = new Color(0.85F, 0.85F, 0.85F);

                    nameKey = "mve_magnify_nme";
                    descKey = "mve_magnify_dsc";
                    break;

                case moveId.heal: // Heal 
                    move = new HealMove(moveId.heal, "Heal", 1, 0.50F);
                    (move as HealMove).healPercent = 0.30F;

                    move.description = "The user heals 30% of their health.";

                    // No Animation 

                    nameKey = "mve_heal_nme";
                    descKey = "mve_heal_dsc";
                    break;

                case moveId.hpDrain1: // Drain Heal 1 
                    move = new HealthDrainMove(moveId.hpDrain1, "Drain Heal 1", 1, 25, 0.95F, 0.30F);

                    (move as HealthDrainMove).damageHealPercent = 0.30F;

                    move.description = "The user attacks the target, and restores their health by 30% of the damage given.";

                    nameKey = "mve_hpDrain1_nme";
                    descKey = "mve_hpDrain1_dsc";

                    // Animation 
                    move.animation = moveAnim.crawl1;
                    move.animationColor = new Color(0.123F, 0.555F, 0.138F);

                    // Translation in constructor. 
                    break;

                case moveId.healthSplit: // Health Split 
                    move = new HealthSplitMove();

                    // Translation is done in function. 
                    break;

                case moveId.pushBack: // Push Back 
                    move = new Move(moveId.pushBack, "Push Back", 1, 25.0F, 0.90F, 0.15F);

                    move.defenseChangeUser = 1;
                    move.defenseChangeChanceUser = 0.2F;

                    move.description = "The user pushes the target back, which has a 0.20 chance of increasing the user's defense.";

                    // Animation 
                    move.animation = moveAnim.smack1;
                    move.animationColor = new Color(0.718F, 0.313F, 0.101F);

                    // Sets the keys for translating the data. 
                    nameKey = "mve_pushBack_nme";
                    descKey = "mve_pushBack_dsc";
                    break;

                case moveId.powerFirst: // Quick Strike 
                    move = new TurnOrderMove(moveId.powerFirst, "Quick Strike", 1, 40.0F, 0.90F, 0.15F);

                    ((TurnOrderMove)move).boostFirst = true;

                    move.description = "An attack that does more damage if the user moves before the target.";

                    // Animation 
                    move.animation = moveAnim.slash1;
                    move.animationColor = new Color(0.929F, 0.216F, 0.035F);

                    // Sets the keys for translating the data. 
                    nameKey = "mve_powerFirst_nme";
                    descKey = "mve_powerFirst_dsc";
                    break;

                case moveId.powerLast: // Slow Strike 
                    move = new TurnOrderMove(moveId.powerLast, "Slow Strike", 1, 40.0F, 0.90F, 0.15F);

                    ((TurnOrderMove)move).boostFirst = false;

                    move.description = "An attack that does more damage if the user moves after the target.";

                    // Animation 
                    move.animation = moveAnim.slash1;
                    move.animationColor = new Color(0.035F, 0.722F, 0.929F);

                    // Sets the keys for translating the data. 
                    nameKey = "mve_powerLast_nme";
                    descKey = "mve_powerLast_dsc";
                    break;

                case moveId.shield1: // Shield 1 
                    move = new ShieldMove(moveId.shield1, "Shield 1", 1, 0.0F, 0.50F, 0.25F);

                    move.description = "A priority move that blocks all direct attacks on the user for a turn. It works 50% of the time.";

                    // Animation 
                    move.animation = moveAnim.shield1;
                    move.animationColor = new Color(1.0F, 0.811F, 0.760F);

                    // Sets the keys for translating the data. 
                    nameKey = "mve_shield1_nme";
                    descKey = "mve_shield1_dsc";
                    break;

                case moveId.breaker1: // Breaker 1
                    move = new ShieldBreakerMove(moveId.breaker1, "Breaker 1", 1, 20.0F, 1.00F, 0.10F);

                    move.description = "A weak attack that can break through shields. The attack does more damage if the target is shielded.";

                    // Animation
                    move.animation = moveAnim.smack1;
                    move.animationColor = new Color(0.647F, 0.961F, 0.067F);

                    // Sets the keys for translating the data.
                    nameKey = "mve_breaker1_nme";
                    descKey = "mve_breaker1_dsc";
                    break;

                case moveId.bam: // Bam 
                    move = new Move(moveId.bam, "Bam", 1, 30.0F, 1.0F, 0.10F);

                    move.description = "A weak, basic attack.";

                    // Sets the keys for translating the data. 
                    nameKey = "mve_bam_nme";
                    descKey = "mve_bam_dsc";

                    move.animation = moveAnim.smack3;
                    move.animationColor = new Color(0.95F, 0.95F, 0.95F);

                    break;




                // RANK 2 
                case moveId.laserBurst: // Laser Burst 
                    move = new Move(moveId.laserBurst, "Laser Burst", 2, 70.0F, 0.90F, 0.18F);

                    move.description = "A decent laser attack.";

                    // Animation 
                    move.animation = moveAnim.burst1;
                    move.animationColor = new Color(0.942F, 0.19F, 0.9F);

                    // Sets the keys for translating the data. 
                    nameKey = "mve_laserBurst_nme";
                    descKey = "mve_laserBurst_dsc";
                    break;

                case moveId.fireBurst: // Fire Burst 
                    move = new Move(moveId.fireBurst, "Fire Burst", 2, 60.0F, 0.85F, 0.20F);
                    move.BurnChance = 0.20F;

                    move.description = "A decent fire attack that has a 0.20 chance of burning the target.";

                    // Animation 
                    move.animation = moveAnim.burst1;
                    move.animationColor = new Color(0.978F, 0.205F, 0.059F);

                    nameKey = "mve_fireBurst_nme";
                    descKey = "mve_fireBurst_dsc";
                    break;

                case moveId.elecBurst: // Electric Burst 
                    move = new Move(moveId.elecBurst, "Electric Burst", 2, 60.0F, 0.85F, 0.20F);
                    move.ParalysisChance = 0.20F;

                    move.description = "A decent electric attack that has a 0.20 chance of paralyzing the target.";

                    // Animation 
                    move.animation = moveAnim.burst1;
                    move.animationColor = new Color(1.0F, 0.783F, 0.07F);

                    nameKey = "mve_elecBurst_nme";
                    descKey = "mve_elecBurst_dsc";
                    break;

                case moveId.soundWave: // Soundwave 
                    move = new Move(moveId.soundWave, "Soundwave", 2, 50.0F, 0.90F, 0.18F);

                    move.accuracyChangeTarget = -1;
                    move.accuracyChangeChanceTarget = 0.2F;

                    move.description = "A decent attack that has a 0.20 chance of lowering the target's accuracy by 1 stage.";

                    // Animation 
                    move.animation = moveAnim.wave2;
                    move.animationColor = new Color(0.914F, 0.982F, 0.987F);

                    nameKey = "mve_soundWave_nme";
                    descKey = "mve_soundWave_dsc";
                    break;

                case moveId.magnet: // Magnet 
                    move = new Move(moveId.magnet, "Magnet", 2, 50.0F, 0.90F, 0.15F);

                    move.accuracyChangeUser = 2;
                    move.accuracyChangeChanceUser = 1.0F;

                    move.accuracyChangeTarget = 1;
                    move.accuracyChangeChanceTarget = 1.0F;

                    move.description = "The user pulls the target towards them, which increases the user's accuracy by 2 stages, and the target's accuracy by 1 stage.";

                    // Animation 
                    move.animation = moveAnim.smack1;
                    move.animationColor = new Color(0.912F, 0.789F, 0.618F);

                    nameKey = "mve_magnet_nme";
                    descKey = "mve_magnet_dsc";
                    break;

                case moveId.torch: // Torch 
                    move = new Move(moveId.torch, "Torch", 2, 15, 0.80F, 0.35F);
                    move.BurnChance = 1.0F;

                    move.description = "A weak attack that always burns the target.";

                    // Animation 
                    move.animation = moveAnim.crawl1;
                    move.animationColor = new Color(0.947F, 0.202F, 0.089F);

                    nameKey = "mve_torch_nme";
                    descKey = "mve_torch_dsc";
                    break;

                case moveId.electrify: // Electrify 
                    move = new Move(moveId.electrify, "Electrify", 2, 15, 0.80F, 0.40F);
                    move.ParalysisChance = 1.0F;

                    move.description = "A weak attack that always paralyzes the target.";

                    // Animation 
                    move.animation = moveAnim.crawl1;
                    move.animationColor = new Color(0.947F, 0.928F, 0.089F);

                    nameKey = "mve_electrify_nme";
                    descKey = "mve_electrify_dsc";
                    break;

                case moveId.motivate: // Motivate 
                    move = new StatChangeMove(moveId.motivate, "Motivate", 2, 0.40F);

                    move.useAccuracy = false;
                    move.priority = -1;

                    move.attackChangeUser = 1;
                    move.attackChangeChanceUser = 1.0F;

                    move.defenseChangeUser = 1;
                    move.defenseChangeChanceUser = 1.0F;

                    move.speedChangeUser = 1;
                    move.speedChangeChanceUser = 1.0F;

                    move.description = "The user increases their attack, defense, and speed by 1 stage each. This move will always go last.";

                    // Animation 
                    move.animation = moveAnim.colorWave1;
                    move.animationColor = new Color(0.978F, 0.440F, 0.125F);

                    nameKey = "mve_motivate_nme";
                    descKey = "mve_motivate_dsc";

                    break;

                case moveId.quickBurst: // Quick Burst 
                    move = new Move(moveId.quickBurst, "Quick Burst", 2, 55, 0.95F, 0.3F);
                    move.RecoilPercent = 0.15F;
                    move.priority = 1;

                    move.description = "The user does a quick move that always goes first. This move will deal 0.15 of the damage dealt back to the user.";

                    // Animation 
                    move.animation = moveAnim.burst1;
                    move.animationColor = new Color(0.831F, 0.982F, 0.972F);

                    nameKey = "mve_quickBurst_nme";
                    descKey = "mve_quickBurst_dsc";
                    break;

                case moveId.hpDrain2: // Drain Heal 2 
                    move = new HealthDrainMove(moveId.hpDrain2, "Drain Heal 2", 2, 60, 0.85F, 0.40F);

                    (move as HealthDrainMove).damageHealPercent = 0.40F;

                    move.description = "The user attacks the target, gaining 40% of the damage dealt as health.";

                    // Animation 
                    move.animation = moveAnim.crawl1;
                    move.animationColor = new Color(0.164F, 0.841F, 0.18F);

                    nameKey = "mve_hpDrain2_nme";
                    descKey = "mve_hpDrain2_dsc";
                    break;

                case moveId.statClear: // Stat Clear 
                    move = new StatClearMove();

                    // Translated and animated in constructor. 
                    break;

                case moveId.cure: // Cure 
                    move = new CureMove();

                    // Translation and animated in constructor. 
                    break;

                case moveId.risk: // Risk 
                    move = new StatChangeMove(moveId.risk, "Risk", 2, 0.20F);

                    move.attackChangeUser = 2;
                    move.attackChangeChanceUser = 1.0F;

                    move.defenseChangeUser = -1;
                    move.defenseChangeChanceUser = 1.0F;

                    move.accuracyChangeUser = -1;
                    move.accuracyChangeChanceUser = 1.0F;

                    move.useAccuracy = false;

                    move.description = "The user increases their attack by 2 stages and accuracy by 1 stage, but lowers their defense by 1 stage.";

                    // Animation 
                    move.animation = moveAnim.colorWave1;
                    move.animationColor = new Color(0.965F, 0.078F, 0.0383F);

                    nameKey = "mve_risk_nme";
                    descKey = "mve_risk_dsc";
                    break;

                case moveId.tidalWave: // Tidal Wave 
                    move = new Move(moveId.tidalWave, "Tidal Wave", 2, 65, 1.0F, 0.35F);
                    move.useAccuracy = false;

                    move.description = "The user hits the target with a strong attack that never misses.";

                    // Animation 
                    move.animation = moveAnim.fill1;
                    move.animationColor = new Color(0.052F, 0.545F, 0.969F);

                    nameKey = "mve_tidalWave_nme";
                    descKey = "mve_tidalWave_dsc";
                    break;

                case moveId.burnBoostTarget: // Livefire 
                    move = new StatusAttackMove(moveId.burnBoostTarget, "Livefire", 2, 45, 0.85F, 0.30F);

                    (move as StatusAttackMove).targetBurned = true;

                    move.description = "An attack that does more damage if the target is burned.";

                    // Animation 
                    move.animation = moveAnim.burst1;
                    move.animationColor = new Color(0.967F, 0.396F, 0.106F);

                    nameKey = "mve_burnBoostTarget_nme";
                    descKey = "mve_burnBoostTarget_dsc";
                    break;

                case moveId.paraBoostTarget: // Livewire 
                    move = new StatusAttackMove(moveId.paraBoostTarget, "Livewire", 2, 50, 0.85F, 0.30F);

                    (move as StatusAttackMove).targetParalyzed = true;

                    move.description = "An attack that does more damage if the target is paralyzed.";

                    // Animation 
                    move.animation = moveAnim.burst1;
                    move.animationColor = new Color(0.741F, 0.969F, 0.106F);

                    nameKey = "mve_paraBoostTarget_nme";
                    descKey = "mve_paraBoostTarget_dsc";
                    break;

                case moveId.shield2: // Shield 2 
                    move = new ShieldMove(moveId.shield2, "Shield 2", 2, 0.0F, 0.75F, 0.40F);

                    move.description = "A priority move that blocks all direct attacks on the user for a turn. It works 75% of the time.";

                    // Animation 
                    move.animation = moveAnim.shield1;
                    move.animationColor = new Color(1.0F, 0.992F, 0.760F);

                    // Sets the keys for translating the data. 
                    nameKey = "mve_shield2_nme";
                    descKey = "mve_shield2_dsc";
                    break;

                case moveId.breaker2: // Breaker 2
                    move = new ShieldBreakerMove(moveId.breaker2, "Breaker 2", 2, 40.0F, 0.95F, 0.15F);

                    move.description = "A decent attack that can break through shields. The attack does more damage if the target is shielded.";

                    // Animation
                    move.animation = moveAnim.smack1;
                    move.animationColor = new Color(0.067F, 0.941F, 0.4F);

                    // Sets the keys for translating the data.
                    nameKey = "mve_breaker2_nme";
                    descKey = "mve_breaker2_dsc";
                    break;

                case moveId.wham: // Wham 
                    move = new Move(moveId.wham, "Wham", 2, 55.0F, 1.0F, 0.15F); 
 
                    move.description = "The user hits the target with a decent, basic attack."; 
 
                    // Animation 
                    move.animation = moveAnim.smack3; 
                    move.animationColor = new Color(0.65F, 0.65F, 0.65F); 
 
                    nameKey = "mve_wham_nme"; 
                    descKey = "mve_wham_dsc"; 
                    break; 
 
 
 
 
                // RANK 3 
                case moveId.laserBlast: // Laser Blast 
                    move = new Move(moveId.laserBlast, "Laser Blast", 3, 90.0F, 0.80F, 0.26F); 
 
                    move.description = "A strong laser blast."; 
 
                    // Animation 
                    move.animation = moveAnim.blast1; 
                    move.animationColor = new Color(0.891F, 0.078F, 0.965F); 
 
                    nameKey = "mve_laserBlast_nme"; 
                    descKey = "mve_laserBlast_dsc"; 
                    break; 
 
                case moveId.fireBlast: // Fire Blast 
                    move = new Move(moveId.fireBlast, "Fire Blast", 3, 85.0F, 0.75F, 0.28F); 
                    move.BurnChance = 0.30F; 
 
                    move.description = "A strong fire blast with a 0.30 chance of burning the target."; 
 
                    // Animation 
                    move.animation = moveAnim.blast1; 
                    move.animationColor = new Color(0.965F, 0.267F, 0.207F); 
 
                    nameKey = "mve_fireBlast_nme"; 
                    descKey = "mve_fireBlast_dsc"; 
                    break; 
 
                case moveId.elecBlast: // Electric Blast 
                    move = new Move(moveId.elecBlast, "Electric Blast", 3, 85.0F, 0.75F, 0.28F); 
                    move.ParalysisChance = 0.30F; 
 
                    move.description = "A strong electric blast with a 0.30 chance of paralyzing the target."; 
 
                    // Animation 
                    move.animation = moveAnim.blast1; 
                    move.animationColor = new Color(0.952F, 0.910F, 0.102F); 
 
                    nameKey = "mve_elecBlast_nme"; 
                    descKey = "mve_elecBlast_dsc"; 
                    break; 
 
                case moveId.sonicWave: // Sonic Wave 
                    move = new Move(moveId.sonicWave, "Sonic Wave", 3, 80.0F, 0.80F, 0.27F); 
 
                    move.accuracyChangeTarget = -1; 
                    move.accuracyChangeChanceTarget = 0.25F; 
 
                    move.description = "A strong attack that has a 0.25 chance of lowering the target's accuracy by 1 stage."; 
 
                    // Animation 
                    move.animation = moveAnim.wave2; 
                    move.animationColor = new Color(0.901F, 0.869F, 0.974F); 
 
                    nameKey = "mve_sonicWave_nme"; 
                    descKey = "mve_sonicWave_dsc"; 
                    break; 
 
                case moveId.hpDrain3: // Drain Heal 3 
                    move = new HealthDrainMove(moveId.hpDrain3, "Drain Heal 3", 3, 80, 0.75F, 0.45F); 
 
                    (move as HealthDrainMove).damageHealPercent = 0.50F; 
 
                    move.description = "The user attacks the target, gaining 50% of the damage dealt as health."; 
 
                    // Animation 
                    move.animation = moveAnim.crawl1; 
                    move.animationColor = new Color(0.421F, 0.938F, 0.094F); 
 
                    nameKey = "mve_hpDrain3_nme"; 
                    descKey = "mve_hpDrain3_dsc"; 
                    break; 
 
                case moveId.twister: // Twister 
                    move = new Move(moveId.twister, "Twister", 3, 75, 0.85F, 0.25F); 
 
                    move.defenseChangeTarget = -1; 
                    move.defenseChangeChanceTarget = 1.0F; 
 
                    move.CriticalChance = 0.0F; 
 
                    move.description = "The user attacks the target, lowering the target's defense by 1 stage. This move cannot do critical damage."; 
 
                    // Animation 
                    move.animation = moveAnim.twister1; 
                    move.animationColor = new Color(0.868F, 0.80F, 0.705F); 
 
                    nameKey = "mve_twister_nme"; 
                    descKey = "mve_twister_dsc"; 
                    break; 
 
                case moveId.waterBlast: // Water Blast 
                    move = new Move(moveId.waterBlast, "Water Blast", 3, 85, 0.75F, 0.28F); 
 
                    move.attackChangeTarget = -1; 
                    move.attackChangeChanceTarget = 0.30F; 
 
                    move.description = "The user attacks the target. This move has a 0.30 chance of lowering the target's attack by 1 stage."; 
 
                    // Animation 
                    move.animation = moveAnim.blast2; 
                    move.animationColor = new Color(0.09F, 0.788F, 0.960F); 
 
                    nameKey = "mve_waterBlast_nme"; 
                    descKey = "mve_waterBlast_dsc"; 
                    break; 
 
                case moveId.rockBlast: // Rock Blast 
                    move = new Move(moveId.rockBlast, "Rock Blast", 3, 85, 0.75F, 0.28F); 
 
                    move.defenseChangeTarget = -1; 
                    move.defenseChangeChanceTarget = 0.30F; 
 
                    move.description = "The user attacks the target. This move has a 0.30 chance of lowering the target's defense by 1 stage."; 
 
                    // Animation 
                    move.animation = moveAnim.blast2; 
                    move.animationColor = new Color(0.952F, 0.629F, 0.070F); 
 
                    nameKey = "mve_rockBlast_nme"; 
                    descKey = "mve_rockBlast_dsc"; 
                    break; 
 
                case moveId.airBlast: // Air Blast 
                    move = new Move(moveId.airBlast, "Air Blast", 3, 85, 0.75F, 0.28F); 
 
                    move.speedChangeTarget = -1; 
                    move.speedChangeChanceTarget = 0.30F; 
 
                    move.description = "The user attacks the target. This move has a 0.30 of lowering the target's speed by 1 stage."; 
 
                    // Animation 
                    move.animation = moveAnim.blast2; 
                    move.animationColor = new Color(0.661F, 0.904F, 0.921F); 
 
                    nameKey = "mve_airBlast_nme"; 
                    descKey = "mve_airBlast_dsc"; 
 
                    break; 
 
                case moveId.quake: // Quake 
                    move = new Move(moveId.quake, "Quake", 3, 75, 0.80F, 0.30F); 
 
                    move.CriticalChance = 0.30F; 
                    move.RecoilPercent = 0.40F; 
 
                    move.description = "The user hits the target with a strong move. The move has a critical chance of 0.30, and deals 40% of the damage dealt back to the user."; 
 
                    // Animation 
                    move.animation = moveAnim.slash2; 
                    move.animationColor = new Color(1.0F, 0.898F, 0.893F); 
 
                    nameKey = "mve_quake_nme"; 
                    descKey = "mve_quake_dsc"; 
 
                    break; 
 
                case moveId.chargeSun: // Charging Sun 
                    move = new EnergyAllMove(moveId.chargeSun, "Charging Sun", 3, 120, 0.80F); 
                    move.BurnChance = 0.20F; 
                    move.priority = -3; 
 
                    move.description = "A move that uses all the user's energy. The more energy used, the stronger the move. This move has a burn chance of 0.20, and always goes last."; 
 
                    // Animation 
                    move.animation = moveAnim.shootingStar1; 
                    move.animationColor = new Color(1.0F, 0.66F, 0.00F); 
 
                    nameKey = "mve_chargeSun_nme"; 
                    descKey = "mve_chargeSun_dsc"; 
 
                    break; 
 
                case moveId.chargeMoon: // Charging Moon 
                    move = new EnergyAllMove(moveId.chargeMoon, "Charging Moon", 3, 120, 0.80F); 
                    move.ParalysisChance = 0.20F; 
                    move.priority = -3; 
 
                    move.description = "A move that uses all the user's energy. The more energy used, the stronger the move. This move has a paralysis chance of 0.20, and always goes last."; 
 
                    // Animation 
                    move.animation = moveAnim.shootingStar1; 
                    move.animationColor = new Color(0.0F, 0.326F, 1.0F); 
 
                    nameKey = "mve_chargeMoon_nme"; 
                    descKey = "mve_chargeMoon_dsc"; 
                    break; 
 
                case moveId.earlyBurst: // Early Burst 
                    move = new TurnsLowMove(moveId.earlyBurst, "Early Burst", 3, 110.0F, 0.85F, 0.30F); 
 
                    move.description = "A move that gets weaker the longer the battle goes on."; 
 
                    // Animation 
                    move.animation = moveAnim.burst1; 
                    move.animationColor = new Color(0.830F, 0.974F, 0.922F); 
 
                    nameKey = "mve_earlyBurst_nme"; 
                    descKey = "mve_earlyBurst_dsc"; 
                    break; 
 
                case moveId.allOut: // All-Out Attack 
                    move = new Move(moveId.allOut, "All Out Attack", 3, 100, 0.80F, 0.45F); 
 
                    move.attackChangeUser = -1; 
                    move.attackChangeChanceUser = 1.0F; 
 
                    move.description = "A move that lowers the user's attack stat by 1 stage every time it's used."; 
 
                    // Animation 
                    move.animation = moveAnim.smack2; 
                    move.animationColor = new Color(0.974F, 0.188F, 0.144F); 
 
                    // Sets the keys for translating the data. 
                    nameKey = "mve_allOut_nme"; 
                    descKey = "mve_allOut_dsc"; 
                    break; 
 
                case moveId.burnBoostUser: // Fire Boost 
                    move = new StatusAttackMove(moveId.burnBoostUser, "Fire Boost", 3, 65, 0.85F, 0.40F); 
 
                    (move as StatusAttackMove).userBurned = true; 
 
                    move.description = "An attack that does more damage if the user is burned."; 
 
                    // Animation 
                    move.animation = moveAnim.smack2; 
                    move.animationColor = new Color(0.902F, 0.169F, 0.071F); 
 
                    nameKey = "mve_burnBoostUser_nme"; 
                    descKey = "mve_burnBoostUser_dsc"; 
                    break; 
 
                case moveId.paraBoostUser: // Electric Boost 
                    move = new StatusAttackMove(moveId.paraBoostUser, "Electric Boost", 3, 60, 0.85F, 0.40F); 
 
                    (move as StatusAttackMove).userParalyzed = true; 
 
                    move.description = "An attack that does more damage if the user is paralyzed."; 
 
                    // Animation 
                    move.animation = moveAnim.smack2; 
                    move.animationColor = new Color(0.902F, 0.886F, 0.071F); 
 
                    nameKey = "mve_paraBoostUser_nme"; 
                    descKey = "mve_paraBoostUser_dsc"; 
                    break; 
 
                case moveId.shield3: // Shield 3 
                    move = new ShieldMove(moveId.shield3, "Shield 3", 3, 0.0F, 1.00F, 0.50F); 
 
                    move.description = "A priority move that blocks direct attacks on the user for a turn. It works every time."; 
 
                    // Animation 
                    move.animation = moveAnim.shield1; 
                    move.animationColor = new Color(0.850F, 1.00F, 0.760F); 
 
                    // Sets the keys for translating the data. 
                    nameKey = "mve_shield3_nme"; 
                    descKey = "mve_shield3_dsc"; 
                    break; 

                case moveId.breaker3: // Breaker 3
                    move = new ShieldBreakerMove(moveId.breaker3, "Breaker 3", 3, 60.0F, 0.90F, 0.20F);

                    move.description = "A strong attack that can break through shields. The attack does more damage if the target is shielded.";

                    // Animation
                    move.animation = moveAnim.smack1;
                    move.animationColor = new Color(0.027F, 0.945F, 0.941F);

                    // Sets the keys for translating the data.
                    nameKey = "mve_breaker3_nme";
                    descKey = "mve_breaker3_dsc";
                    break;

                case moveId.kablam: // Kablam
                    move = new Move(moveId.kablam, "Kablam", 3, 75.0F, 1.0F, 0.20F);

                    move.description = "A strong, but basic attack.";

                    // Animation
                    move.animation = moveAnim.smack3;
                    move.animationColor = new Color(0.45F, 0.45F, 0.45F);

                    nameKey = "mve_kablam_nme";
                    descKey = "mve_kablam_dsc";
                    break;
            }

            // The move has been generated.
            if(move != null && nameKey != "" && descKey != "")
            {
                // Loads the translation.
                move.LoadTranslation(nameKey, descKey);
            }

            return move;
        }

        // Gets a random move of a random rank.
        public Move GetRandomMove()
        {
            return GetRandomMove(0);
        }

        // Gets a random move. If the rank is invalid, it picks from the whole list.
        public Move GetRandomMove(int rank)
        {
            // The id of the move being generated.
            moveId id;

            // TODO: optimize method.
            switch(rank)
            {
                case 1: // rank 1
                    id = (moveId)Random.Range(MOVE_ID_RAND_START, (int)LAST_RANK_1 + 1);
                    break;

                case 2: // rank 2
                    id = (moveId)Random.Range((int)LAST_RANK_1 + 1, (int)LAST_RANK_2 + 1);
                    break;

                case 3: // rank 3
                    id = (moveId)Random.Range((int)LAST_RANK_2 + 1, (int)LAST_RANK_3 + 1);
                    break;

                default: // Selects from the whole list.
                    // The list starts at 0, with the move count being +1 of the number value of the final move.
                    id = (moveId)Random.Range(MOVE_ID_RAND_START, MOVE_ID_COUNT);
                    break;
            }

            // Generates the move from the id.
            Move move = GenerateMove(id);
            return move;
        }

        // Gets a random rank 1 move.
        public Move GetRandomRank1Move()
        {
            return GetRandomMove(1);
        }

        // Gets a random rank 2 move.
        public Move GetRandomRank2Move()
        {
            return GetRandomMove(2);
        }

        // Gets a random rank 3 move.
        public Move GetRandomRank3Move()
        {
            return GetRandomMove(3);
        }

        // Gets multiple random moves, going by the whole list.
        public List<Move> GetRandomMoves(int count, List<moveId> ignoreList)
        {
            // Creates a move list of move selections (for the randomizer), and resultMoves (for returning moves).
            List<moveId> moveOptions = new List<moveId>();
            List<Move> chosenMoves = new List<Move>();

            // The number of the last move.
            const int LAST_MOVE_NUM = (int)LAST_RANK_3;

            // Returns an empty list if the count is invalid.
            if (count <= 0)
                return chosenMoves;

            // While there are still moves to add.
            for (int i = MOVE_ID_RAND_START; i <= LAST_MOVE_NUM; i++)
            {
                // Grabs the move id.
                moveId mid = (moveId)i;

                // If the move is not in the ignore list, add it to the list.
                if (!ignoreList.Contains(mid))
                    moveOptions.Add(mid);
            }


            // Generates the random moves.
            while (chosenMoves.Count < count && moveOptions.Count != 0)
            {
                // Generates the random index.
                int randIndex = Random.Range(0, moveOptions.Count);

                // Gets the random move ID from the list, remvoing it as well.
                moveId randMoveId = moveOptions[randIndex];
                moveOptions.RemoveAt(randIndex);

                // Generates a random move using the id provided.
                Move randMove = GenerateMove(randMoveId);

                // Adds a random move to the list of chosen moves.
                chosenMoves.Add(randMove);
            }


            // Returns the chosen moves in the list.
            return chosenMoves;
        }

        // Gets a list of random moves, ignoring any moves that the provided entity already has.
        public List<Move> GetRandomMoves(int count, BattleEntity entity)
        {
            // The list of entities to be ignored.
            List<moveId> ignoreList = new List<moveId>();

            // Goes through each move.
            foreach (Move move in entity.moves)
            {
                // If the move is not set to null, add it to the ignore list.
                if (move != null)
                    ignoreList.Add(move.Id);
            }

            // Re-uses the other function.
            return GetRandomMoves(count, ignoreList);
        }

    }
}