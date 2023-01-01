using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_BBTS
{
    // The script for the move comparisons.
    public class MoveComparison : MonoBehaviour
    {
        //  Player object.
        public Player player;

        [Header("Moves")]
        // The 4 standard moves.
        public MoveComparePanel move0;
        public MoveComparePanel move1;
        public MoveComparePanel move2;
        public MoveComparePanel move3;

        // The charge and run move info.
        public MoveComparePanel chargeMove;
        public MoveComparePanel runMove;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}