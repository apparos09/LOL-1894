using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RM_BBTS
{
    // The script for the move comparisons.
    public class MoveComparison : MonoBehaviour
    {
        // The gameplay manager.
        public GameplayManager gameManager;

        //  Player object.
        public Player player;

        // The charge move.
        [HideInInspector]
        public ChargeMove chargeMove;
        // The run move.
        [HideInInspector]
        public RunMove runMove;

        // The legend text.
        [Header("Legend")]
        // Legend text.
        public TMP_Text legendTitleText;

        // Rank, power, and accuracy.
        public TMP_Text rankLabel;
        public TMP_Text powerLabel;
        public TMP_Text accuracyLabel;
        public TMP_Text energyLabel;

        // Attack, defense, and speed.
        public TMP_Text attackChangeLabel;
        public TMP_Text defenseChangeLabel;
        public TMP_Text speedChangeLabel;
        public TMP_Text accuracyChangeLabel;

        // Effect on Self, and Effect on Target
        public TMP_Text effectSelfLabel;
        public TMP_Text effectTargetLabel;

        // Critical, burn, and paralysis effects.
        public TMP_Text criticalLabel;
        public TMP_Text burnLabel;
        public TMP_Text paralysisLabel;


        [Header("Moves")]
        // The marker for the highlighted move.
        public GameObject moveHighlight;

        // The offset for highlighting a move, which is relative to a move panel.
        public Vector3 moveHighlightOffset = new Vector3(0.0F, 0.0F, 0.0F);

        // The 4 standard moves.
        public MoveComparePanel move0Panel;
        public MoveComparePanel move1Panel;
        public MoveComparePanel move2Panel;
        public MoveComparePanel move3Panel;

        // The charge and run move info.
        public MoveComparePanel chargeMovePanel;
        public MoveComparePanel runMovePanel;

        [Header("Scroll Bars")]
        
        // If 'true', the scroll values are set automatically.
        [Tooltip("Sets the reset values based on the values of the sliders at compile time.")]
        public bool autoSetDefaultScrollValues = true;

        // Resets the scroll bar positions when the move comparison is enabled.
        [Tooltip("Resets the scroll bar positions when the script is enabled.")]
        public bool resetScrollBarsOnEnable = false;

        [Header("Scrollbars/Legend")]
        // The horizontal and vertical scrollbars.
        public Scrollbar legendHorizontal;
        public float legendHoriResetValue = 0.0F;
        public Scrollbar legendVertical;
        public float legendVertResetValue = 1.0F;

        [Header("Scrollbars/Moves")]
        // The horizontal and vertical scrollbars.
        public Scrollbar moveHorizontal;
        public float moveHoriResetValue = 0.0F;
        public Scrollbar moveVertical;
        public float moveVertResetValue = 1.0F;


        // Awake is called when the script instance is loaded.
        private void Awake()
        {
            // Auto set the scroll values.
            if (autoSetDefaultScrollValues)
            {
                legendHoriResetValue = legendHorizontal.value;
                legendVertResetValue = legendVertical.value;

                moveHoriResetValue = moveHorizontal.value;
                moveVertResetValue = moveVertical.value;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            // Checks for the saved charge move.
            if (chargeMove == null)
                chargeMove = MoveList.Instance.ChargeMove;

            // Checks for the saved run move.
            if (runMove == null)
                runMove = MoveList.Instance.RunMove;

            // Translation
            JSONNode defs = SharedState.LanguageDefs;

            // Language definitions set.
            if (defs != null)
            {
                legendTitleText.text = defs["kwd_legend"];

                // Standard Symbols
                rankLabel.text = gameManager.RankString;
                powerLabel.text = gameManager.PowerString;
                accuracyLabel.text = gameManager.AccuracyString;
                energyLabel.text = gameManager.EnergyString;

                // Stat Changes
                attackChangeLabel.text = defs["kwd_attackChange"];
                defenseChangeLabel.text = defs["kwd_defenseChange"];
                speedChangeLabel.text = defs["kwd_speedChange"];
                accuracyChangeLabel.text = defs["kwd_accuracyChange"];

                // Effects
                effectSelfLabel.text = defs["kwd_effectSelf"];
                effectTargetLabel.text = defs["kwd_effectTarget"];
                criticalLabel.text = defs["kwd_criticalChance"];
                burnLabel.text = defs["kwd_burnChance"];
                paralysisLabel.text = defs["kwd_paralysisChance"];
            }
            else // Mark text to show that it's not loaded from the langauge file.
            {
                LanguageMarker marker = LanguageMarker.Instance;

                marker.MarkText(legendTitleText);

                // Standard Symbols
                marker.MarkText(rankLabel);
                marker.MarkText(powerLabel);
                marker.MarkText(accuracyLabel);
                marker.MarkText(energyLabel);

                // Stat Changes
                marker.MarkText(attackChangeLabel);
                marker.MarkText(defenseChangeLabel);
                marker.MarkText(speedChangeLabel);
                marker.MarkText(accuracyChangeLabel);

                // Effects
                marker.MarkText(effectSelfLabel);
                marker.MarkText(effectTargetLabel);
                marker.MarkText(criticalLabel);
                marker.MarkText(burnLabel);
                marker.MarkText(paralysisLabel);
            }


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
            // HighlightChargeMove();

            // Reset the scroll bar positions.
            if (resetScrollBarsOnEnable)
                ResetScrollBarPositions();
        }

        // Resets the scroll bar positions.
        public void ResetScrollBarPositions()
        {
            // Resets the legend horizontal.
            if (legendHorizontal != null)
                legendHorizontal.value = legendHoriResetValue;

            // Resets the legend vertical.
            if (legendVertical != null)
                legendVertical.value = legendVertResetValue;


            // Resets the move horizontal.
            if (moveHorizontal != null)
                moveHorizontal.value = moveHoriResetValue;

            // Resets the move vertical.
            if (moveVertical != null)
                moveVertical.value = moveVertResetValue;
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
            moveHighlight.transform.Translate(moveHighlightOffset);
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