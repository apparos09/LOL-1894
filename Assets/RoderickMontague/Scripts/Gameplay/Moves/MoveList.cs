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
        poke, slimeShot, laserShot, fireShot, elecShot, screech, slam, chip, toss, block, heal, hpDrain1, empathy, bam, 
        laserBurst, fireBurst, elecBurst, soundWave, magnet, torch, electrify, motivate, quickBurst, hpDrain2, tripleShot, cure, energyAttackA, tidalWave, wham, 
        laserBlast, fireBlast, elecBlast, sonicWave, hpDrain3, twister, waterBlast, rockBlast, airBlast, quake, chargeSun, chargeMoon, energyAttackB, airstrike, kablam}

    // The list of moves for the game.
    public class MoveList : MonoBehaviour
    {
        // The instance of the move list.
        private static MoveList instance;

        // The move ID count.
        public const int MOVE_ID_COUNT = 18;

        // The run move that is used to play through the turn.
        private static RunMove runMove;

        // The charge move that entities use.
        // This is copied whenever someone performs a charge, and is never put into the 1-4 move slots.
        private static ChargeMove chargeMove;

        // The last rank 1 move.
        private moveId lastRank1 = moveId.bam;

        // The last rank 2 move.
        private moveId lastRank2 = moveId.wham;

        // The last rank 3 move.
        private moveId lastRank3 = moveId.kablam;

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

                case moveId.poke: // Hit
                    move = new Move(moveId.poke, "<Poke>", 1, 10.0F, 1.0F, 0.05F);

                    // Sets the keys for translating the data.
                    nameKey = "mve_poke_nme";
                    descKey = "mve_poke_dsc";
                    break;

                case moveId.slimeShot: // Slimeshot
                    move = new Move(moveId.slimeShot, "<Slimeshot>", 1, 30.0F, 0.9F, 0.05F);

                    move.speedChangeTarget = -1;
                    move.speedChangeChanceTarget = 0.15F;

                    nameKey = "mve_slimeShot_nme";
                    descKey = "mve_slimeShot_dsc";
                    break;

                case moveId.laserShot: // Lasershot (TODO: not working for osme reason)
                    move = new Move(moveId.laserShot, "<Lasershot>", 1, 40.0F, 0.9F, 0.05F);

                    // Sets the keys for translating the data.
                    nameKey = "mve_laserShot_nme";
                    descKey = "mve_laserShot_dsc";
                    break;

                case moveId.fireShot: // Fireshot
                    move = new Move(moveId.fireShot, "<Fireshot>", 1, 30.0F, 0.9F, 0.05F);
                    move.BurnChance = 0.2F;

                    // Sets the keys for translating the data.
                    nameKey = "mve_fireShot_nme";
                    descKey = "mve_fireShot_dsc";
                    break;

                case moveId.elecShot: // Electroshot
                    move = new Move(moveId.elecShot, "<Electro Shot>", 1, 30.0F, 0.9F, 0.05F);
                    move.ParalysisChance = 0.2F;

                    // Sets the keys for translating the data.
                    nameKey = "mve_elecShot_nme";
                    descKey = "mve_elecShot_dsc";
                    break;

                case moveId.screech: // Screech
                    move = new Move(moveId.screech, "<Screech>", 1, 25.0F, 1.0F, 0.05F);

                    move.accuracyChangeTarget = -1;
                    move.accuracyChangeChanceTarget = 0.15F;

                    nameKey = "mve_screech_nme";
                    descKey = "mve_screech_dsc";
                    break;

                case moveId.slam: // Slam
                    move = new Move(moveId.slam, "<Slam>", 1, 30, 1.0F, 0.05F);
                    move.CriticalChance = 0.6F;

                    nameKey = "mve_slam_nme";
                    descKey = "mve_slam_dsc";
                    break;

                case moveId.chip: // Chip Damage
                    move = new Move(moveId.chip, "<Chip Damage>", 1, 10, 0.95F, 0.1F);
                    move.priority = 1;

                    nameKey = "mve_chip_nme";
                    descKey = "mve_chip_dsc";
                    break;

                case moveId.toss: // Toss
                    move = new Move(moveId.toss, "<Toss>", 1, 25, 0.95F, 0.1F);
                    
                    move.attackChangeTarget = -1;
                    move.attackChangeChanceTarget = 0.1F;

                    nameKey = "mve_toss_nme";
                    descKey = "mve_toss_dsc";
                    break;

                case moveId.block: // Block
                    move = new Move(moveId.block, "<Block>", 1, 0.0F, 0.0F, 0.4F);
                    // TODO: implement functionality.
                    break;

                case moveId.heal: // Heal
                    move = new HealMove(moveId.heal);
                    (move as HealMove).healPercent = 0.15F;

                    nameKey = "mve_heal_nme";
                    descKey = "mve_heal_dsc";
                    break;

                case moveId.hpDrain1: // Health Drain 1
                    move = new Move(moveId.hpDrain1, "Health Drain", 1, 0.0F, 0.0F, 0.4F);
                    
                    // TODO: implement mechanics.
                    break;

                case moveId.empathy: // Empathize
                    move = new Move(moveId.empathy, "<Empathy>", 1, 0.0F, 0.9F, 0.05F);
                    
                    // TODO: implement functionality.
                    break;

                case moveId.bam: // Bam
                    move = new Move(moveId.bam, "<Bam>", 1, 10.0F, 1.0F, 0.05F);

                    // Sets the keys for translating the data.
                    nameKey = "mve_bam_nme";
                    descKey = "mve_bam_dsc";
                    break;




                    // RANK 2
                case moveId.laserBurst: // Laser Burst
                    move = new Move(moveId.laserBurst, "<Laser Burst>", 2, 70.0F, 1.0F, 0.1F);

                    // Sets the keys for translating the data.
                    nameKey = "mve_laserBurst_nme";
                    descKey = "mve_laserBurst_dsc";
                    break;

                case moveId.fireBurst: // Fire Burst
                    move = new Move(moveId.fireBurst, "<Fire Burst>", 2, 65.0F, 0.85F, 0.11F);
                    move.BurnChance = 0.35F;

                    nameKey = "mve_fireBurst_nme";
                    descKey = "mve_fireBurst_dsc";
                    break;

                case moveId.elecBurst: // Electric Burst
                    move = new Move(moveId.elecBurst, "<Electric Burst>", 2, 65.0F, 0.85F, 0.12F);
                    move.ParalysisChance = 0.35F;

                    nameKey = "mve_elecBurst_nme";
                    descKey = "mve_elecBurst_dsc";
                    break;

                case moveId.soundWave: // Sound Wave
                    move = new Move(moveId.soundWave, "<Sound Wave>", 2, 65.0F, 1.0F, 0.1F);
                    
                    move.accuracyChangeTarget = 1;
                    move.accuracyChangeChanceTarget = 0.2F;

                    nameKey = "mve_soundWave_nme";
                    descKey = "mve_soundWave_dsc";
                    break;

                case moveId.magnet: // Magnetize
                    move = new Move(moveId.magnet, "<Magnet>", 2, 0.0F, 0.9F, 0.15F);

                    move.accuracyChangeUser = 2;
                    move.accuracyChangeChanceUser = 1.0F;

                    nameKey = "mve_magnet_nme";
                    descKey = "mve_magnet_dsc";
                    break;

                case moveId.torch: // Torch
                    move = new Move(moveId.torch, "<Scorch>", 2, 30, 0.9F, 0.20F);
                    move.BurnChance = 1.0F;

                    nameKey = "mve_torch_nme";
                    descKey = "mve_torch_dsc";
                    break;

                case moveId.electrify: // Electrify
                    move = new Move(moveId.electrify, "<Electrify>", 2, 30, 0.9F, 0.2F);
                    move.ParalysisChance = 1.0F;

                    nameKey = "mve_electrify_nme";
                    descKey = "mve_electrify_dsc";
                    break;

                case moveId.motivate: // Motivate
                    move = new StatChangeMove(moveId.motivate, "<Motivate>", 2, 0.5F);
                    
                    move.useAccuracy = false;
                    move.priority = -1;

                    move.attackChangeUser = 1;
                    move.attackChangeChanceUser = 1.0F;

                    move.defenseChangeUser = 1;
                    move.defenseChangeChanceUser = 1.0F;

                    move.speedChangeUser = 1;
                    move.speedChangeChanceUser = 1.0F;

                    nameKey = "mve_motivate_nme";
                    descKey = "mve_motivate_dsc";

                    break;

                case moveId.quickBurst: // Quick Burst
                    move = new Move(moveId.quickBurst, "<Quick Burst>", 2, 70, 0.95F, 0.3F);
                    move.RecoilPercent = 0.15F;
                    move.priority = 1;

                    nameKey = "mve_quickBurst_nme";
                    descKey = "mve_quickBurst_dsc";
                    break;

                case moveId.hpDrain2: // Health Drain 2
                    move = new Move(moveId.hpDrain2, "<Health Drain 2>", 2, 60, 0.9F, 0.2F);
                    // TODO: implement mechanics.
                    break;

                case moveId.tripleShot: // Triple Shot
                    move = new Move(moveId.tripleShot, "<Triple Shot>", 2, 40, 0.6F, 0.15F);
                    // TODO: mechanics
                    break;

                case moveId.cure: // Cure
                    move = new CureMove();
                    
                    // Translation in constructor.
                    break;

                case moveId.energyAttackA: // Overboard
                    move = new Move(moveId.energyAttackA, "<Overboard>", 2, 40, 0.9F, 0.15F);
                    // TODO: mechanics.
                    break;

                case moveId.tidalWave: // Tidal Wave
                    move = new Move(moveId.tidalWave, "<Tidal Wave>", 2, 70, 1.0F, 0.20F);
                    move.useAccuracy = false;

                    nameKey = "mve_tidalWave_nme";
                    descKey = "mve_tidalWave_dsc";
                    break;

                case moveId.wham: // Wham
                    move = new Move(moveId.wham, "<Wham>", 2, 15.0F, 1.0F, 0.1F);

                    nameKey = "mve_wham_nme";
                    descKey = "mve_wham_dsc";
                    break;


                // RANK 3
                case moveId.laserBlast: // Laser Blast
                    move = new Move(moveId.laserBlast, "<Laser Blast>", 3, 100.0F, 0.8F, 0.15F);

                    nameKey = "mve_laserBlast_nme";
                    descKey = "mve_laserBlast_dsc";
                    break;

                case moveId.fireBlast: // Fire Blast
                    move = new Move(moveId.fireBlast, "<Fire Blast>", 3, 90.0F, 0.8F, 0.15F);
                    move.BurnChance = 0.5F;

                    nameKey = "mve_fireBlast_nme";
                    descKey = "mve_fireBlast_dsc";
                    break;

                case moveId.elecBlast: // Electric Blast
                    move = new Move(moveId.elecBlast, "<Electric Blast>", 3, 90.0F, 0.8F, 0.15F);
                    move.ParalysisChance = 0.5F;

                    nameKey = "mve_elecBlast_nme";
                    descKey = "mve_elecBlast_dsc";
                    break;

                case moveId.sonicWave: // Sonic Wave
                    move = new Move(moveId.sonicWave, "<Sonic Wave>", 3, 90.0F, 1.0F, 0.05F);

                    move.accuracyChangeTarget = -1;
                    move.accuracyChangeChanceTarget = 0.25F;

                    nameKey = "mve_sonicWave_nme";
                    descKey = "mve_sonicWave_dsc";
                    break;

                case moveId.hpDrain3: // HP Drain 3
                    move = new Move(moveId.hpDrain3, "<Health Drain 3>", 3, 80, 0.9F, 0.3F);
                    // TODO: need mechanics.
                    break;

                case moveId.twister: // Twister
                    move = new Move(moveId.twister, "<Twister>", 1, 70, 1.0F, 0.2F);

                    move.defenseChangeTarget = -1;
                    move.defenseChangeChanceTarget = 100.0F;

                    nameKey = "mve_twister_nme";
                    descKey = "mve_twister_dsc";
                    break;

                case moveId.waterBlast: // Water Blast
                    move = new Move(moveId.waterBlast, "<Water Blast>", 3, 90, 0.8F, 0.15F);

                    move.attackChangeTarget = -1;
                    move.attackChangeChanceTarget = 30.0F;

                    nameKey = "mve_waterBlast_nme";
                    descKey = "mve_waterBlast_dsc";
                    break;

                case moveId.rockBlast: // Rock Blast
                    move = new Move(moveId.rockBlast, "<Rock Blast>", 3, 90, 0.8F, 0.15F);

                    move.defenseChangeTarget = -1;
                    move.defenseChangeChanceTarget = 30.0F;

                    nameKey = "mve_rockBlast_nme";
                    descKey = "mve_rockBlast_dsc";
                    break;

                case moveId.airBlast: // Air Blast
                    move = new Move(moveId.airBlast, "<Air Blast>", 3, 90, 0.8F, 0.15F);

                    move.speedChangeTarget = -1;
                    move.speedChangeChanceTarget = 30.0F;

                    nameKey = "mve_airBlast_nme";
                    descKey = "mve_airBlast_dsc";

                    break;

                case moveId.quake: // Quake
                    move = new Move(moveId.quake, "<Quake>", 3, 100, 0.7F, 0.25F);

                    move.CriticalChance = 0.6F;
                    move.RecoilPercent = 0.2F;

                    nameKey = "mve_quake_nme";
                    descKey = "mve_quake_dsc";

                    break;

                case moveId.chargeSun: // Charge Sun
                    move = new Move(moveId.chargeSun, "<Charging Sun>", 3, 120, 0.85F, 0.2F);
                    // TODO: mechanics.
                    break;

                case moveId.chargeMoon: // Charge Moon
                    move = new Move(moveId.chargeMoon, "<Charging Moon>", 3, 120, 0.85F, 0.2F);
                    // TODO: mechanics.
                    break;

                case moveId.energyAttackB: // Conserve
                    move = new Move(moveId.energyAttackB, "<Conserve>", 3, 90, 0.9F, 0.15F);
                    // TODO: mechanics.

                    // Sets the keys for translating the data.
                    // nameKey = "mve_charge_nme";
                    // descKey = "mve_charge_dsc";
                    break;

                case moveId.airstrike: // Airstrike
                    move = new Move(moveId.airstrike, "<Airstrike>", 3, 100, 0.2F, 0.4F);
                    // TODO: mechanics.

                    // Sets the keys for translating the data.
                    // nameKey = "mve_charge_nme";
                    // descKey = "mve_charge_dsc";
                    break;

                case moveId.kablam: // Kablam
                    move = new Move(moveId.kablam, "<Kablam>", 3, 30.0F, 1.0F, 0.05F);

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
            moveId id;

            switch(rank)
            {
                case 1: // rank 1
                    id = (moveId)Random.Range(2, (int)lastRank1 + 1);
                    break;

                case 2: // rank 2
                    id = (moveId)Random.Range((int)lastRank1 + 1, (int)lastRank2 + 1);
                    break;

                case 3: // rank 3
                    id = (moveId)Random.Range((int)lastRank2 + 1, (int)lastRank3 + 1);
                    break;

                default: // Selects from the whole list.
                    id = (moveId)Random.Range(2, MOVE_ID_COUNT + 1);
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