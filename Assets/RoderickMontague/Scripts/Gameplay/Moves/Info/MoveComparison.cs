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

        // The charge move.
        [HideInInspector]
        public ChargeMove chargeMove;
        // The run move.
        [HideInInspector]
        public RunMove runMove;

        [Header("Moves")]
        // The 4 standard moves.
        public MoveComparePanel move0Panel;
        public MoveComparePanel move1Panel;
        public MoveComparePanel move2Panel;
        public MoveComparePanel move3Panel;

        // The charge and run move info.
        public MoveComparePanel chargeMovePanel;
        public MoveComparePanel runMovePanel;


        // Start is called before the first frame update
        void Start()
        {
            // Checks for the saved charge move.
            if (chargeMove == null)
                chargeMove = MoveList.Instance.ChargeMove;

            // Checks for the saved run move.
            if (runMove == null)
                runMove = MoveList.Instance.RunMove;


            UpdatePlayerInfo();
        }

        // This function is called when the object becomes enabled and active.
        private void OnEnable()
        {
            UpdatePlayerInfo();
        }

        // Updates the player info.
        public void UpdatePlayerInfo()
        {
            // Standard Moves
            move0Panel.LoadMoveInfo(player.Move0);
            move1Panel.LoadMoveInfo(player.Move1);
            move2Panel.LoadMoveInfo(player.Move2);
            move3Panel.LoadMoveInfo(player.Move3);

            // Charge
            chargeMovePanel.LoadMoveInfo(chargeMove);

            // Run
            runMovePanel.LoadMoveInfo(runMove);
        }
    }
}