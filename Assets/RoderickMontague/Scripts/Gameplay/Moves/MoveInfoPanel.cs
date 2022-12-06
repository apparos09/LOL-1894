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
            id = move.Id;

            nameText.text = move.Name;

            rankText.text = move.Rank.ToString();
            powerText.text = (move.Power == 0.0F) ? "-" : move.Power.ToString();
            accuracyText.text = (move.Accuracy * 100.0F).ToString("F" + GameplayManager.DISPLAY_DECIMAL_PLACES.ToString()) + "%";
            energyText.text = (move.EnergyUsage * 100.0F).ToString("F" + GameplayManager.DISPLAY_DECIMAL_PLACES.ToString()) + "%";

            description.text = move.description;

        }
    }
}