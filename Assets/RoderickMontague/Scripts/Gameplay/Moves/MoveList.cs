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
        poke, slimeshot, lasershot, fireshot, elecshot, screech, slam, chip, toss, block, heal, hpdrain1, empathy, bam, 
        laserburst, fireburst, elecburst, soundwave, magnet, scorch, electrify, motivate, quickburst, hpdrain2, tripleshot, cure, energyattacka, wave, wham, 
        laserblast, fireblast, elecblast, sonicwave, hpdrain3, twister, waterblast, rockblast, airblast, quake, chargesun, chargemoon, energyattackb, airstrike, kablam}

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

                    // Sets the keys for translating the data.
                    // nameKey = "mve_run_nme";
                    // descKey = "mve_run_dsc";
                    break;

                case moveId.charge: // Charge
                    move = new ChargeMove();

                    // Sets the keys for translating the data.
                    // nameKey = "mve_charge_nme";
                    // descKey = "mve_charge_dsc";
                    break;

                case moveId.poke: // Hit
                    move = new Move(moveId.poke, "<Poke>", 1, 10.0F, 1.0F, 0.05F);

                    // Sets the keys for translating the data.
                    // nameKey = "mve_charge_nme";
                    // descKey = "mve_charge_dsc";
                    break;

                case moveId.slimeshot: // Slimeshot
                    move = new Move(moveId.slimeshot, "<Slimeshot>", 1, 30.0F, 0.9F, 0.05F);

                    // Sets the keys for translating the data.
                    // nameKey = "mve_charge_nme";
                    // descKey = "mve_charge_dsc";
                    break;

                case moveId.lasershot: // Lasershot (TODO: not working for osme reason)
                    move = new Move(moveId.lasershot, "<Lasershot>", 1, 40.0F, 0.9F, 0.05F);

                    // Sets the keys for translating the data.
                    // nameKey = "mve_charge_nme";
                    // descKey = "mve_charge_dsc";
                    break;

                case moveId.fireshot: // Fireshot
                    move = new Move(moveId.fireshot, "<Fireshot>", 1, 30.0F, 0.9F, 0.05F);
                    move.BurnChance = 0.2F;

                    // Sets the keys for translating the data.
                    // nameKey = "mve_charge_nme";
                    // descKey = "mve_charge_dsc";
                    break;

                case moveId.elecshot: // Electroshot
                    move = new Move(moveId.elecshot, "<Electro Shot>", 1, 30.0F, 0.9F, 0.05F);
                    move.ParalysisChance = 0.2F;

                    // Sets the keys for translating the data.
                    // nameKey = "mve_charge_nme";
                    // descKey = "mve_charge_dsc";
                    break;

                case moveId.screech: // Screech
                    move = new Move(moveId.screech, "<Screech>", 1, 25.0F, 1.0F, 0.05F);

                    // Sets the keys for translating the data.
                    // nameKey = "mve_charge_nme";
                    // descKey = "mve_charge_dsc";
                    break;

                case moveId.slam: // Slam
                    move = new Move(moveId.slam, "<Slam>", 1, 30, 1.0F, 0.05F);
                    move.CriticalChance = 0.6F;
                    break;

                case moveId.chip: // Chip Damage
                    move = new Move(moveId.chip, "<Chip Damage>", 1, 10, 0.95F, 0.1F);
                    move.priority = 1;
                    break;

                case moveId.toss: // Toss
                    move = new Move(moveId.toss, "<Toss>", 1, 25, 0.95F, 0.1F);
                    // TODO: implement stat changes. 
                    break;

                case moveId.block: // Block
                    move = new Move(moveId.block, "<Block>", 1, 0.0F, 0.0F, 0.4F);
                    // TODO: implement functionality.
                    break;

                case moveId.heal: // Heal
                    move = new Move(moveId.heal, "<Heal>", 1, 0.0F, 0.0F, 0.4F);
                    // TODO: implement functionality.
                    break;

                case moveId.hpdrain1: // Health Drain 1
                    move = new Move(moveId.hpdrain1, "Health Drain", 1, 0.0F, 0.0F, 0.4F);
                    // TODO: implement mechanics.
                    break;

                case moveId.empathy: // Empathize
                    move = new Move(moveId.empathy, "<Empathy>", 1, 0.0F, 0.9F, 0.05F);
                    // TODO: implement functionality.
                    break;

                case moveId.bam: // Bam
                    move = new Move(moveId.bam, "<Bam>", 1, 10.0F, 1.0F, 0.05F);

                    // Sets the keys for translating the data.
                    // nameKey = "mve_charge_nme";
                    // descKey = "mve_charge_dsc";
                    break;




                    // RANK 2
                case moveId.laserburst: // Laser Burst
                    move = new Move(moveId.laserburst, "<Laser Burst>", 2, 70.0F, 1.0F, 0.05F);

                    // Sets the keys for translating the data.
                    // nameKey = "mve_charge_nme";
                    // descKey = "mve_charge_dsc";
                    break;

                case moveId.fireburst: // Fire Burst
                    move = new Move(moveId.fireburst, "<Fire Burst>", 2, 65.0F, 1.0F, 0.05F);
                    move.BurnChance = 0.3F;

                    // Sets the keys for translating the data.
                    // nameKey = "mve_charge_nme";
                    // descKey = "mve_charge_dsc";
                    break;

                case moveId.elecburst: // Electric Burst
                    move = new Move(moveId.elecburst, "<Electric Burst>", 2, 65.0F, 1.0F, 0.05F);
                    move.ParalysisChance = 0.3F;

                    // Sets the keys for translating the data.
                    // nameKey = "mve_charge_nme";
                    // descKey = "mve_charge_dsc";
                    break;

                case moveId.soundwave: // Sound Wave
                    move = new Move(moveId.soundwave, "<Sound Wave>", 2, 65.0F, 1.0F, 0.05F);

                    // Sets the keys for translating the data.
                    // nameKey = "mve_charge_nme";
                    // descKey = "mve_charge_dsc";
                    break;

                case moveId.wham: // Wham
                    move = new Move(moveId.wham, "<Wham>", 2, 15.0F, 1.0F, 0.05F);

                    // Sets the keys for translating the data.
                    // nameKey = "mve_charge_nme";
                    // descKey = "mve_charge_dsc";
                    break;

                case moveId.magnet: // Magnetize
                    move = new Move(moveId.magnet, "<Magnet>", 2, 0.0F, 0.05F, 0.15F);
                    // TODO: implement the mechanics.
                    break;

                case moveId.scorch: // Scorch
                    move = new Move(moveId.scorch, "<Scorch>", 2, 30, 0.85F, 0.20F);
                    move.BurnChance = 1.0F;
                    break;

                case moveId.electrify: // Electrify
                    move = new Move(moveId.electrify, "<Electrify>", 2, 30, 0.9F, 0.16F);
                    move.ParalysisChance = 1.0F;
                    break;

                case moveId.motivate: // Motivate
                    move = new Move(moveId.motivate, "<Motivate>", 2, 0.0F, 0.0F, 0.5F);
                    // TODO: set up mechanics.
                    break;

                case moveId.quickburst: // Quick Burst
                    move = new Move(moveId.quickburst, "<Quick Burst>", 2, 80, 0.95F, 0.5F);
                    move.RecoilPercent = 0.1F;
                    break;

                case moveId.hpdrain2: // Health Drain 2
                    move = new Move(moveId.hpdrain2, "<Health Drain 2>", 2, 60, 0.9F, 0.2F);
                    // TODO: implement mechanics.
                    break;

                case moveId.tripleshot: // Triple Shot
                    move = new Move(moveId.tripleshot, "<Triple Shot>", 2, 40, 0.6F, 0.15F);
                    // TODO: mechanics
                    break;

                case moveId.cure: // Cure
                    move = new Move(moveId.cure, "<Cure>", 2, 0.0F, 0.0F, 0.35F);
                    // TODO: mechanics.
                    break;

                case moveId.energyattacka: // Overboard
                    move = new Move(moveId.energyattacka, "<Overboard>", 2, 40, 0.9F, 0.15F);
                    // TODO: mechanics.
                    break;

                case moveId.wave: // Tidal Wave
                    move = new Move(moveId.wave, "<Tidal Wave>", 2, 70, 1.0F, 0.15F);
                    move.useAccuracy = false;
                    break;


                // RANK 3
                case moveId.laserblast: // Laser Blast
                    move = new Move(moveId.laserblast, "<Laser Blast>", 3, 100.0F, 1.0F, 0.05F);

                    // Sets the keys for translating the data.
                    // nameKey = "mve_charge_nme";
                    // descKey = "mve_charge_dsc";
                    break;

                case moveId.fireblast: // Fire Blast
                    move = new Move(moveId.fireblast, "<Fire Blast>", 3, 90.0F, 1.0F, 0.05F);
                    move.BurnChance = 0.4F;

                    // Sets the keys for translating the data.
                    // nameKey = "mve_charge_nme";
                    // descKey = "mve_charge_dsc";
                    break;

                case moveId.elecblast: // Electric Blast
                    move = new Move(moveId.elecblast, "<Electric Blast>", 3, 90.0F, 1.0F, 0.05F);
                    move.ParalysisChance = 0.4F;

                    // Sets the keys for translating the data.
                    // nameKey = "mve_charge_nme";
                    // descKey = "mve_charge_dsc";
                    break;

                case moveId.sonicwave: // Sonic Wave
                    move = new Move(moveId.sonicwave, "<Sonic Wave>", 3, 90.0F, 1.0F, 0.05F);

                    // Sets the keys for translating the data.
                    // nameKey = "mve_charge_nme";
                    // descKey = "mve_charge_dsc";
                    break;


                case moveId.hpdrain3: // HP Drain 3
                    move = new Move(moveId.hpdrain3, "<Health Drain 3>", 3, 80, 0.9F, 0.3F);
                    // TODO: need mechanics.
                    break;

                case moveId.twister: // Twister
                    move = new Move(moveId.twister, "<Twister>", 1, 70, 1.0F, 0.2F);
                    // TODO: need mechanics.
                    break;

                case moveId.waterblast: // Water Blast
                    move = new Move(moveId.waterblast, "<Water Blast>", 3, 90, 0.8F, 0.15F);
                    // TODO: need mechanics.
                    break;

                case moveId.rockblast: // Rock Blast
                    move = new Move(moveId.rockblast, "<Rock Blast>", 3, 90, 0.8F, 0.15F);
                    // TODO: implement mechanics.
                    break;

                case moveId.airblast: // Air Blast
                    move = new Move(moveId.airblast, "<Air Blast>", 3, 90, 0.8F, 0.15F);
                    // TODO: implement mechanics.
                    break;

                case moveId.quake: // Quake
                    move = new Move(moveId.quake, "<Quake>", 3, 100, 0.7F, 0.25F);
                    // TODO: implement mechanics.
                    break;

                case moveId.chargesun: // Charge Sun
                    move = new Move(moveId.chargesun, "<Charging Sun>", 3, 120, 0.85F, 0.2F);
                    // TODO: mechanics.
                    break;

                case moveId.chargemoon: // Charge Moon
                    move = new Move(moveId.chargemoon, "<Charging Moon>", 3, 120, 0.85F, 0.2F);
                    // TODO: mechanics.
                    break;

                case moveId.energyattackb: // Conserve
                    move = new Move(moveId.energyattackb, "<Conserve>", 3, 90, 0.9F, 0.15F);
                    // TODO: mechanics.
                    break;

                case moveId.airstrike: // Airstrike
                    move = new Move(moveId.airstrike, "<Airstrike>", 3, 100, 0.2F, 0.4F);
                    // TODO: mechanics.
                    break;

                case moveId.kablam: // Kablam
                    move = new Move(moveId.kablam, "<Kablam>", 3, 30.0F, 1.0F, 0.05F);

                    // Sets the keys for translating the data.
                    // nameKey = "mve_charge_nme";
                    // descKey = "mve_charge_dsc";
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