using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// A panel for the moves.
namespace RM_BBTS
{
    public class MoveInfoPanel : MonoBehaviour
    {
        // The id of the move being represented.
        public moveId id;

        // Move Title
        public TMPro.TMP_Text nameText;

        // Move Attributes
        public TMPro.TMP_Text rankText;
        public TMPro.TMP_Text powerText;
        public TMPro.TMP_Text accuracyText;
        public TMPro.TMP_Text energyText;

        // Move Description
        public TMPro.TMP_Text description;

        // Loads the move into the move info pnael.
        public void LoadMoveInfo(Move move)
        {
            // Id
            id = move.Id;

            // Name
            nameText.text = move.Name;

            // Rank
            rankText.text = move.Rank.ToString();

            // Power
            powerText.text = (move.Power == 0.0F) ? "-" : move.Power.ToString();

            // Accuracy
            if (move.useAccuracy)
            {
                // Percentage
                // accuracyText.text = (move.Accuracy * 100.0F).ToString("F" + GameplayManager.DISPLAY_DECIMAL_PLACES.ToString()) + "%";

                // Decimal
                accuracyText.text = move.Accuracy.ToString("F" + GameplayManager.DISPLAY_DECIMAL_PLACES.ToString());
            }
            else
            {
                accuracyText.text = "-";
            }
                

            // Energy
            energyText.text = (move.EnergyUsage * 100.0F).ToString("F" + GameplayManager.DISPLAY_DECIMAL_PLACES.ToString()) + "%";

            // Description
            description.text = move.description;

        }
    }
}