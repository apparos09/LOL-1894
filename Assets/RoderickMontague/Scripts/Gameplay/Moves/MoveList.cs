using RM_BBTS;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

namespace RM_BBTS
{
    // NOTE: organize moves based on rank (all rank 1 moves > all rank 2 moves > all rank 3 moves)
    // The list of move ids.
    public enum moveId { run, charge, 
        poke, slimeShot, laserShot, fireShot, elecShot, screech, slam, chip, toss, magnify, heal, hpDrain1, healthSplit, pushBack, bam, 
        laserBurst, fireBurst, elecBurst, soundWave, magnet, torch, electrify, motivate, quickBurst, hpDrain2, statClear, cure, risk, tidalWave, wham, 
        laserBlast, fireBlast, elecBlast, sonicWave, hpDrain3, twister, waterBlast, rockBlast, airBlast, quake, chargeSun, chargeMoon, earlyBurst, allOut, kablam}

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
                    break;

                case moveId.charge: // Charge
                    move = new ChargeMove();

                    // Translated in constructor.
                    break;

                case moveId.poke: // Poke
                    move = new Move(moveId.poke, "<Poke>", 1, 15.0F, 1.0F, 0.10F);

                    move.CriticalChance = 0.4F;

                    move.defenseChangeUser = 1;
                    move.defenseChangeChanceUser = 0.05F;

                    move.description = "<An attack that has a 40% critical damage chance, and a 5% chance of raising the user's defense by 1 stage.>";
                    
                    // Sets the keys for translating the data.
                    nameKey = "mve_poke_nme";
                    descKey = "mve_poke_dsc";
                    break;

                case moveId.slimeShot: // Slime Shot
                    move = new Move(moveId.slimeShot, "<Slime Shot>", 1, 30.0F, 0.95F, 0.14F);

                    move.speedChangeTarget = -1;
                    move.speedChangeChanceTarget = 0.15F;

                    move.description = "<An attack that has a 15% chance of lowering the target's speed by 1 stage.>";

                    nameKey = "mve_slimeShot_nme";
                    descKey = "mve_slimeShot_dsc";
                    break;

                case moveId.laserShot: // Laser Shot
                    move = new Move(moveId.laserShot, "<Laser Shot>", 1, 40.0F, 0.95F, 0.12F);

                    move.description = "<A weak laser attack.>";

                    // Sets the keys for translating the data.
                    nameKey = "mve_laserShot_nme";
                    descKey = "mve_laserShot_dsc";
                    break;

                case moveId.fireShot: // Fire Shot
                    move = new Move(moveId.fireShot, "<Fire Shot>", 1, 30.0F, 0.95F, 0.14F);
                    move.BurnChance = 0.1F;

                    move.description = "<A weak fire attack with a burn chance of 10%.>";
                    
                    // Sets the keys for translating the data.
                    nameKey = "mve_fireShot_nme";
                    descKey = "mve_fireShot_dsc";
                    break;

                case moveId.elecShot: // Electric Shot
                    move = new Move(moveId.elecShot, "<Electric Shot>", 1, 30.0F, 0.95F, 0.14F);
                    move.ParalysisChance = 0.1F;

                    move.description = "<A weak electric attack with a paralysis chance of 10%.>";
                    
                    // Sets the keys for translating the data.
                    nameKey = "mve_elecShot_nme";
                    descKey = "mve_elecShot_dsc";
                    break;

                case moveId.screech: // Screech
                    move = new Move(moveId.screech, "<Screech>", 1, 25.0F, 1.0F, 0.12F);

                    move.accuracyChangeTarget = -1;
                    move.accuracyChangeChanceTarget = 0.15F;

                    move.description = "<A weak attack that has a 15% chance of lowering the target's accuracy by 1 stage.>";

                    nameKey = "mve_screech_nme";
                    descKey = "mve_screech_dsc";
                    break;

                case moveId.slam: // Slam
                    move = new Move(moveId.slam, "<Slam>", 1, 30, 0.90F, 0.15F);
                    move.CriticalChance = 0.6F;

                    move.description = "<An attack that has a critical damage chance of 60%.>";

                    nameKey = "mve_slam_nme";
                    descKey = "mve_slam_dsc";
                    break;

                case moveId.chip: // Chip Off
                    move = new Move(moveId.chip, "<Chip Off>", 1, 10, 0.95F, 0.15F);
                    move.priority = 1;

                    move.description = "<A weak attack that always goes first.>";

                    nameKey = "mve_chip_nme";
                    descKey = "mve_chip_dsc";
                    break;

                case moveId.toss: // Toss
                    move = new Move(moveId.toss, "<Toss>", 1, 25, 0.95F, 0.1F);
                    
                    move.attackChangeTarget = -1;
                    move.attackChangeChanceTarget = 0.1F;

                    move.description = "<An attack that has a 10% chance of lowering the target's attack by 1 stage.>";
                    
                    nameKey = "mve_toss_nme";
                    descKey = "mve_toss_dsc";
                    break;

                case moveId.magnify: // Magnify
                    move = new StatChangeMove(moveId.magnify, "<Magnify>", 1, 0.12F);

                    move.accuracyChangeUser = 1;
                    move.accuracyChangeChanceUser = 1.0F;

                    move.description = "<The user raises their accuracy by 1 stage.>";

                    break;

                case moveId.heal: // Heal
                    move = new HealMove(moveId.heal, "<Heal>", 1, 0.40F);
                    (move as HealMove).healPercent = 0.20F;

                    move.description = "<The user heals 20% of their health.>";

                    nameKey = "mve_heal_nme";
                    descKey = "mve_heal_dsc";
                    break;

                case moveId.hpDrain1: // Drain Heal 1
                    move = new HealthDrainMove(moveId.hpDrain1, "Drain Heal 1", 1, 25, 0.95F, 0.3F);

                    (move as HealthDrainMove).damageHealPercent = 0.125F;

                    move.description = "<The user attacks the target, and restores their health by 12.5% of the damage given.>";

                    nameKey = "mve_hpDrain1_nme";
                    descKey = "mve_hpDrain1_dsc";

                    // Translation in constructor.
                    break;

                case moveId.healthSplit: // Health Split
                    move = new HealthSplitMove();
                    
                    move.description = "<The user and the target add together their proportional health, then split said health evenly between themselves.>";

                    // Translation is done in function.
                    break;

                case moveId.pushBack: // Push Back
                    move = new Move(moveId.pushBack, "<Push Back>", 1, 25.0F, 0.9F, 0.15F);

                    move.defenseChangeUser = 1;
                    move.defenseChangeChanceUser = 0.2F;

                    move.description = "<The user pushes the target back, which has a 20% chance of increasing the user's defense.>";

                    // Sets the keys for translating the data.
                    nameKey = "mve_pushBack_nme";
                    descKey = "mve_pushBack_dsc";
                    break;

                case moveId.bam: // Bam
                    move = new Move(moveId.bam, "<Bam>", 1, 25.0F, 1.0F, 0.10F);

                    move.description = "<A weak, basic attack.>";

                    // Sets the keys for translating the data.
                    nameKey = "mve_bam_nme";
                    descKey = "mve_bam_dsc";
                    break;




                    // RANK 2
                case moveId.laserBurst: // Laser Burst
                    move = new Move(moveId.laserBurst, "<Laser Burst>", 2, 70.0F, 0.90F, 0.18F);

                    move.description = "<A decent laser attack.>";

                    // Sets the keys for translating the data.
                    nameKey = "mve_laserBurst_nme";
                    descKey = "mve_laserBurst_dsc";
                    break;

                case moveId.fireBurst: // Fire Burst
                    move = new Move(moveId.fireBurst, "<Fire Burst>", 2, 60.0F, 0.85F, 0.20F);
                    move.BurnChance = 0.20F;

                    move.description = "<A decent fire attack that has a 20% chance of burning the target.>";

                    nameKey = "mve_fireBurst_nme";
                    descKey = "mve_fireBurst_dsc";
                    break;

                case moveId.elecBurst: // Electric Burst
                    move = new Move(moveId.elecBurst, "<Electric Burst>", 2, 60.0F, 0.85F, 0.20F);
                    move.ParalysisChance = 0.20F;

                    move.description = "<A decent electric attack that has a 20% chance of paralyzing the target.>";

                    nameKey = "mve_elecBurst_nme";
                    descKey = "mve_elecBurst_dsc";
                    break;

                case moveId.soundWave: // Soundwave
                    move = new Move(moveId.soundWave, "<Soundwave>", 2, 50.0F, 1.0F, 0.18F);
                    
                    move.accuracyChangeTarget = 1;
                    move.accuracyChangeChanceTarget = 0.2F;

                    move.description = "<A decent attack that has a 20% chance of lowering the target's accuracy by 1 stage.>";

                    nameKey = "mve_soundWave_nme";
                    descKey = "mve_soundWave_dsc";
                    break;

                case moveId.magnet: // Magnet
                    move = new Move(moveId.magnet, "<Magnet>", 2, 0.0F, 0.9F, 0.15F);

                    move.accuracyChangeUser = 2;
                    move.accuracyChangeChanceUser = 1.0F;

                    move.accuracyChangeTarget = 1;
                    move.accuracyChangeChanceTarget = 1.0F;

                    move.description = "<The user increases their accuracy by 2 stages, and the target's accuracy by 1 stage.>";

                    nameKey = "mve_magnet_nme";
                    descKey = "mve_magnet_dsc";
                    break;

                case moveId.torch: // Torch
                    move = new Move(moveId.torch, "<Scorch>", 2, 10, 0.9F, 0.40F);
                    move.BurnChance = 1.0F;

                    move.description = "<A weak attack that always burns the target.>";

                    nameKey = "mve_torch_nme";
                    descKey = "mve_torch_dsc";
                    break;

                case moveId.electrify: // Electrify
                    move = new Move(moveId.electrify, "<Electrify>", 2, 10, 0.9F, 0.40F);
                    move.ParalysisChance = 1.0F;

                    move.description = "<A weak attack that always paralyzes the target.>";

                    nameKey = "mve_electrify_nme";
                    descKey = "mve_electrify_dsc";
                    break;

                case moveId.motivate: // Motivate
                    move = new StatChangeMove(moveId.motivate, "<Motivate>", 2, 0.4F);
                    
                    move.useAccuracy = false;
                    move.priority = -1;

                    move.attackChangeUser = 1;
                    move.attackChangeChanceUser = 1.0F;

                    move.defenseChangeUser = 1;
                    move.defenseChangeChanceUser = 1.0F;

                    move.speedChangeUser = 1;
                    move.speedChangeChanceUser = 1.0F;

                    move.description = "<The user increases their attack, defense, and speed by 1 stage each. This move will always go last.>";

                    nameKey = "mve_motivate_nme";
                    descKey = "mve_motivate_dsc";

                    break;

                case moveId.quickBurst: // Quick Burst
                    move = new Move(moveId.quickBurst, "<Quick Burst>", 2, 55, 0.95F, 0.3F);
                    move.RecoilPercent = 0.15F;
                    move.priority = 1;

                    move.description = "<The user does a quick move that always goes first. This move will deal 15% of the damage dealt back to the user.>";

                    nameKey = "mve_quickBurst_nme";
                    descKey = "mve_quickBurst_dsc";
                    break;

                case moveId.hpDrain2: // Drain Heal 2
                    move = new HealthDrainMove(moveId.hpDrain2, "<Drain Heal 2>", 2, 60, 0.85F, 0.35F);

                    (move as HealthDrainMove).damageHealPercent = 0.25F;

                    move.description = "<The user attacks the target, gaining 25% of the damage dealt as health.>";

                    nameKey = "mve_hpDrain2_nme";
                    descKey = "mve_hpDrain2_dsc";
                    break;

                case moveId.statClear: // Stat Clear
                    move = new StatClearMove();

                    // Translated in constructor.
                    break;

                case moveId.cure: // Cure
                    move = new CureMove();
                    
                    // Translation in constructor.
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

                    move.description = "<The user increases their attack by 2 stages and accuracy by 1 stage, but lowers their defense by 1 stage.>";   

                    nameKey = "mve_risk_nme";
                    descKey = "mve_risk_dsc";
                    break;

                case moveId.tidalWave: // Tidal Wave
                    move = new Move(moveId.tidalWave, "<Tidal Wave>", 2, 70, 1.0F, 0.20F);
                    move.useAccuracy = false;

                    move.description = "<The user hits the target with a strong attack that never misses.>";

                    nameKey = "mve_tidalWave_nme";
                    descKey = "mve_tidalWave_dsc";
                    break;

                case moveId.wham: // Wham
                    move = new Move(moveId.wham, "<Wham>", 2, 50.0F, 1.0F, 0.15F);

                    move.description = "<The user hits the target with a decent, basic attack.>";

                    nameKey = "mve_wham_nme";
                    descKey = "mve_wham_dsc";
                    break;


                // RANK 3
                case moveId.laserBlast: // Laser Blast
                    move = new Move(moveId.laserBlast, "<Laser Blast>", 3, 90.0F, 0.85F, 0.27F);

                    move.description = "<A strong laser blast.>";

                    nameKey = "mve_laserBlast_nme";
                    descKey = "mve_laserBlast_dsc";
                    break;

                case moveId.fireBlast: // Fire Blast
                    move = new Move(moveId.fireBlast, "<Fire Blast>", 3, 85.0F, 0.8F, 0.30F);
                    move.BurnChance = 0.30F;

                    move.description = "<A strong fire blast with a 30% chance of burning the target.>";

                    nameKey = "mve_fireBlast_nme";
                    descKey = "mve_fireBlast_dsc";
                    break;

                case moveId.elecBlast: // Electric Blast
                    move = new Move(moveId.elecBlast, "<Electric Blast>", 3, 85.0F, 0.8F, 0.30F);
                    move.ParalysisChance = 0.30F;

                    move.description = "<A strong electric blast with a 30% chance of paralyzing the target.>";

                    nameKey = "mve_elecBlast_nme";
                    descKey = "mve_elecBlast_dsc";
                    break;

                case moveId.sonicWave: // Sonic Wave
                    move = new Move(moveId.sonicWave, "<Sonic Wave>", 3, 80.0F, 0.95F, 0.27F);

                    move.accuracyChangeTarget = -1;
                    move.accuracyChangeChanceTarget = 0.25F;

                    move.description = "<A strong attack that has a 25% chance of lowering the target's accuracy by 1 stage.>";

                    nameKey = "mve_sonicWave_nme";
                    descKey = "mve_sonicWave_dsc";
                    break;

                case moveId.hpDrain3: // Drain Heal 3
                    move = new HealthDrainMove(moveId.hpDrain3, "<Drain Heal 3>", 3, 80, 0.75F, 0.4F);

                    (move as HealthDrainMove).damageHealPercent = 0.50F;

                    move.description = "<The user attacks the target, gaining 40% of the damage dealt as health.>";

                    nameKey = "mve_hpDrain3_nme";
                    descKey = "mve_hpDrain3_dsc";
                    break;

                case moveId.twister: // Twister
                    move = new Move(moveId.twister, "<Twister>", 1, 70, 0.95F, 0.30F);

                    move.defenseChangeTarget = -1;
                    move.defenseChangeChanceTarget = 100.0F;

                    move.CriticalChance = 0.0F;

                    move.description = "<The user attacks the target, lowering the target's defense by 1 stage. This move cannot do critical damage.>";

                    nameKey = "mve_twister_nme";
                    descKey = "mve_twister_dsc";
                    break;

                case moveId.waterBlast: // Water Blast
                    move = new Move(moveId.waterBlast, "<Water Blast>", 3, 85, 0.80F, 0.30F);

                    move.attackChangeTarget = -1;
                    move.attackChangeChanceTarget = 30.0F;

                    move.description = "<The user attacks the target. This move has a 30% chance of lowering the target's attack by 1 stage.>";

                    nameKey = "mve_waterBlast_nme";
                    descKey = "mve_waterBlast_dsc";
                    break;

                case moveId.rockBlast: // Rock Blast
                    move = new Move(moveId.rockBlast, "<Rock Blast>", 3, 85, 0.80F, 0.30F);

                    move.defenseChangeTarget = -1;
                    move.defenseChangeChanceTarget = 30.0F;

                    move.description = "<The user attacks the target. This move has a 30% chance of lowering the target's defense by 1 stage.>";

                    nameKey = "mve_rockBlast_nme";
                    descKey = "mve_rockBlast_dsc";
                    break;

                case moveId.airBlast: // Air Blast
                    move = new Move(moveId.airBlast, "<Air Blast>", 3, 85, 0.80F, 0.30F);

                    move.speedChangeTarget = -1;
                    move.speedChangeChanceTarget = 30.0F;

                    move.description = "<The user attacks the target. This move has a 30% of lowering the target's speed by 1 stage.>";

                    nameKey = "mve_airBlast_nme";
                    descKey = "mve_airBlast_dsc";

                    break;

                case moveId.quake: // Quake
                    move = new Move(moveId.quake, "<Quake>", 3, 75, 0.80F, 0.30F);

                    move.CriticalChance = 0.3F;
                    move.RecoilPercent = 0.4F;

                    move.description = "<The user hits the target with a strong move. The move has a critical chance of 30%, and deals 40% of the damage dealt back to the user.>";

                    nameKey = "mve_quake_nme";
                    descKey = "mve_quake_dsc";

                    break;

                case moveId.chargeSun: // Charging Sun
                    move = new EnergyAllMove(moveId.chargeSun, "<Charging Sun>", 3, 100, 0.85F);
                    move.BurnChance = 0.2F;
                    move.priority = -3;

                    move.description = "<A move that uses all the user's energy. The more energy used, the stronger the move. This move has a burn chance of 20%, and always goes last.>";


                    nameKey = "mve_chargeSun_nme";
                    descKey = "mve_chargeSun_dsc";

                    break;

                case moveId.chargeMoon: // Charging Moon
                    move = new EnergyAllMove(moveId.chargeMoon, "<Charging Moon>", 3, 100, 0.85F);
                    move.ParalysisChance = 0.2F;
                    move.priority = -3;

                    move.description = "<A move that uses all the user's energy. The more energy used, the stronger the move. This move has a paralysis chance of 20%, and always goes last.>";

                    nameKey = "mve_chargeMoon_nme";
                    descKey = "mve_chargeMoon_dsc";
                    break;

                case moveId.earlyBurst: // Early Burst
                    move = new TurnsLowMove(moveId.earlyBurst, "<Early Burst>", 3, 90, 0.85F, 0.30F);

                    move.description = "<A move that gets weaker the longer the battle goes on.>";

                    nameKey = "mve_earlyBurst_nme";
                    descKey = "mve_earlyBurst_dsc";
                    break;

                case moveId.allOut: // All-Out Attack
                    move = new Move(moveId.allOut, "<All Out Attack>", 3, 90, 0.90F, 0.45F);

                    move.attackChangeUser = -1;
                    move.attackChangeChanceUser = 1.0F;

                    move.description = "<A move that lowers the user's attack stat by 1 stage every time it's used.>";

                    // Sets the keys for translating the data.
                    nameKey = "mve_allOut_nme";
                    descKey = "mve_allOut_dsc";
                    break;

                case moveId.kablam: // Kablam
                    move = new Move(moveId.kablam, "<Kablam>", 3, 70.0F, 1.0F, 0.20F);

                    move.description = "<A strong, but basic attack.>";

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
    }
}