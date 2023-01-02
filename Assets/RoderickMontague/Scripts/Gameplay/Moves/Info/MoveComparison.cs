using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        // The marker for the highlighted move.
        public GameObject moveHighlight;

        // The offset for highlighting a move, which is relative to a move panel.
        public Vector3 highlightOffset = new Vector3(0.0F, 0.0F, 0.0F);

        [Header("Moves")]
        // The 4 standard moves.
        public MoveComparePanel move0Panel;
        public MoveComparePanel move1Panel;
        public MoveComparePanel move2Panel;
        public MoveComparePanel move3Panel;

        // The charge and run move info.
        public MoveComparePanel chargeMovePanel;
        public MoveComparePanel runMovePanel;

        [Header("Scroll Bars")]
        
        // Resets the scroll bar positions when the move comparison is enabled.
        public bool resetScrollBarsOnEnable = true;

        // The horizontal and vertical scrollbars.
        public Scrollbar horizontal;
        public Scrollbar vertical;


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
            HighlightChargeMove();

            // Reset the scroll bar positions.
            if (resetScrollBarsOnEnable)
                ResetScrollBarPositions();
        }

        // This function is called when the object becomes enabled and active.
        private void OnEnable()
        {
            UpdatePlayerInfo();
            HighlightChargeMove();

            // Reset the scroll bar positions.
            if (resetScrollBarsOnEnable)
                ResetScrollBarPositions();
        }

        // Resets the scroll bar positions.
        public void ResetScrollBarPositions()
        {
            // Resets the horizontal.
            if (horizontal != null)
                horizontal.value = 0;

            // Resets the vertical.
            if (vertical != null)
                vertical.value = 0;
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
        
        // Highlights the move.
        public void HighlightMove(int moveNumber)
        {
            // The panel to be highlighted.
            MoveComparePanel panel = null;

            // Checks the move number.
            switch(moveNumber)
            {
                case 0: // Move 0
                    panel = move0Panel;
                    break;
                case 1: // Move 1
                    panel = move1Panel;
                    break;
                case 2: // Move 2
                    panel = move2Panel;
                    break;
                case 3: // Move 3
                    panel = move3Panel;
                    break;
                case 4: // Charge
                    panel = chargeMovePanel;
                    break;
                case 5: // Run
                    panel = runMovePanel;
                    break;
                default:
                    return;
            }

            // Moves the highlight object.
            Vector3 newPos = moveHighlight.transform.position;
            newPos.y = panel.transform.position.y;

            moveHighlight.transform.position = newPos;
            moveHighlight.transform.Translate(highlightOffset);
        }

        // Highlights move 0.
        public void HighlightMove0()
        {
            HighlightMove(0);
        }

        // Highlights move 1.
        public void HighlightMove1()
        {
            HighlightMove(1);
        }

        // Highlights move 2.
        public void HighlightMove2()
        {
            HighlightMove(2);
        }

        // Highlights move 3.
        public void HighlightMove3()
        {
            HighlightMove(3);
        }

        // Highlights charge move.
        public void HighlightChargeMove()
        {
            HighlightMove(4);
        }

        // Highlights run move.
        public void HighlightRunMove()
        {
            HighlightMove(5);
        }
    }
}