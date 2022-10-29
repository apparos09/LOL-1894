using RM_BBTS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_BBTS
{
    public enum moveId { charge, hit, bam, wham, kablam }

    // The list of moves for the game.
    public class MoveList : MonoBehaviour
    {
        // The instance of the move list.
        private static MoveList instance;

        // The charge move that entities use.
        // This is copied whenever someone performs a charge, and is never put into the 1-4 move slots.
        private static ChargeMove chargeMove;

        // TODO: include list of move animations.

        // Constructor.
        private MoveList()
        {
            // Saves the charge move to the list.
            if(chargeMove == null)
                chargeMove = (ChargeMove)GenerateMove(moveId.charge);

            // Instance not set.
            if (instance == null)
                instance = this;
            else
                Destroy(this);
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

        // Gets the charge move.
        public Move ChargeMove
        {
            get { return chargeMove; }
        }

        // Generates the move.
        public Move GenerateMove(moveId id)
        {
            Move move = null;

            switch (id)
            {
                case moveId.charge: // 0. Charge
                    move = new ChargeMove();
                    break;

                case moveId.hit: // 1. Hit
                    move = new Move(moveId.hit, "Hit", 1, 1.0F, 1.0F, 1.0F);
                    break;

                case moveId.bam: // 2. Bam
                    move = new Move(moveId.hit, "Bam", 1, 1.0F, 1.0F, 1.0F);
                    break;

                case moveId.wham: // 3. Wham
                    move = new Move(moveId.hit, "Wham", 2, 1.0F, 1.0F, 1.0F);
                    break;

                case moveId.kablam: // 4. Kablam
                    move = new Move(moveId.hit, "Kablam", 3, 1.0F, 1.0F, 1.0F);
                    break;
            }

            return move;
        }
    }
}