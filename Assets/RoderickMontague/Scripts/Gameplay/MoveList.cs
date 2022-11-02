using RM_BBTS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_BBTS
{
    // NOTE: organize moves based on rank (all rank 1 moves > all rank 2 moves > all rank 3 moves)
    // The list of move ids.
    public enum moveId { run, charge, poke, slimeshot, lasershot, fireshot, screech, bam, wham, kablam }

    // The list of moves for the game.
    public class MoveList : MonoBehaviour
    {
        // The instance of the move list.
        private static MoveList instance;

        // The move ID count.
        public const int MOVE_ID_COUNT = 10;

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
            Move move = null;

            switch (id)
            {
                    // RANK 1
                case moveId.run: // Run
                    move = new RunMove();
                    break;

                case moveId.charge: // Charge
                    move = new ChargeMove();
                    break;

                case moveId.poke: // Hit
                    move = new Move(moveId.poke, "Poke", 1, 10.0F, 1.0F, 1.0F);
                    break;

                case moveId.slimeshot: // Slimeshot
                    move = new Move(moveId.slimeshot, "Slimeshot", 1, 30.0F, 0.9F, 4.0F);
                    break;

                case moveId.lasershot: // Lasershot
                    move = new Move(moveId.lasershot, "Lasershot", 1, 40.0F, 0.9F, 4.0F);
                    break;

                case moveId.fireshot: // Fireshot
                    move = new Move(moveId.fireshot, "Fireshot", 1, 30.0F, 0.9F, 5.0F);
                    break;

                case moveId.screech: // Screech
                    move = new Move(moveId.screech, "Screech", 1, 25.0F, 1.0F, 3.0F);
                    break;

                case moveId.bam: // Bam
                    move = new Move(moveId.poke, "Bam", 1, 10.0F, 1.0F, 1.0F);
                    break;


                    // RANK 2
                case moveId.wham: // Wham
                    move = new Move(moveId.poke, "Wham", 2, 15.0F, 1.0F, 1.5F);
                    break;

                case moveId.kablam: // Kablam
                    move = new Move(moveId.poke, "Kablam", 3, 20.0F, 1.0F, 2.0F);
                    break;
            }

            return move;
        }

        // Gets a random move.
        public Move GetRandomMove()
        {
            moveId id = (moveId)Random.Range(2, MOVE_ID_COUNT);
            Move move = GenerateMove(id);
            return move;
        }

        // Gets a random rank 1 move.
        public Move GetRandomRank1Move()
        {
            // Grabs a random move. The first two are skipped since 'run' and 'charge' should not be used.
            moveId id = (moveId)Random.Range(2, (int)lastRank1 + 1);
            Move move = GenerateMove(id);
            return move;
        }

        // Gets a random rank 2 move.
        public Move GetRandomRank2Move()
        {
            moveId id = (moveId)Random.Range((int)lastRank1 + 1, (int)lastRank2 + 1);
            Move move = GenerateMove(id);
            return move;
        }

        // Gets a random rank 3 move.
        public Move GetRandomRank3Move()
        {
            // Gets a random rank 3 move.
            moveId id = (moveId)Random.Range((int)lastRank2 + 1, (int)lastRank3 + 1);
            Move move = GenerateMove(id);
            return move;
        }
    }
}