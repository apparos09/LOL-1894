using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_BBTS
{
    // A panel for a move being compared.
    public class MoveComparePanel : MonoBehaviour
    {
        // Gets set to 'true' when a move is loaded.
        private bool loaded = false;

        // The id of the move being represented.
        private moveId id;

        // The name of the move.
        public TMPro.TMP_Text nameText;

        // Move Attributes
        // Standard Info
        [Header("Primary Stats")]
        public TMPro.TMP_Text rankText;
        public TMPro.TMP_Text powerText;
        public TMPro.TMP_Text accuracyText;
        public TMPro.TMP_Text energyText;

        // Stat Change Info

        // Attack
        [Header("Attack Chance Events")]
        public TMPro.TMP_Text attackChangeUserText;
        public TMPro.TMP_Text attackChanceUserText;
        public TMPro.TMP_Text attackChangeTargetText;
        public TMPro.TMP_Text attackChanceTargetText;

        // Defense
        [Header("Defense Chance Events")]
        public TMPro.TMP_Text defenseChangeUserText;
        public TMPro.TMP_Text defenseChanceUserText;
        public TMPro.TMP_Text defenseChangeTargetText;
        public TMPro.TMP_Text defenseChanceTargetText;

        // Speed
        [Header("Speed Chance Events")]
        public TMPro.TMP_Text speedChangeUserText;
        public TMPro.TMP_Text speedChanceUserText;
        public TMPro.TMP_Text speedChangeTargetText;
        public TMPro.TMP_Text speedChanceTargetText;

        // Accuracy
        [Header("Accuracy Chance Events")]
        public TMPro.TMP_Text accuracyChangeUserText;
        public TMPro.TMP_Text accuracyChanceUserText;
        public TMPro.TMP_Text accuracyChangeTargetText;
        public TMPro.TMP_Text accuracyChanceTargetText;


        // Critical, Burn, and Paralysis
        [Header("Other Chance Events")]
        public TMPro.TMP_Text criticalChanceText;
        public TMPro.TMP_Text burnChanceText;
        public TMPro.TMP_Text paralysisChanceText;

        // Checks to see if a move is loaded in.
        // Recommended you use the LoadMoveInfo() function, and don't load text in manually...
        // Otherwise this will always be false.
        public bool Loaded
        {
            get { return loaded; }
        }

        // Gets the move id.
        public moveId Id
        {
            get { return id; }
        }

        // Loads the move info.
        public void LoadMoveInfo(Move move)
        {
            // The decimal points to display. 
            string decPoints = "F" + GameplayManager.DISPLAY_DECIMAL_PLACES.ToString();

            // Sets to see if the move info has been loaded.
            loaded = move != null;

            // Copy the id (uses 0 if no move).
            id = (move != null) ? move.Id : 0;

            // Name
            nameText.text = (move != null) ? move.Name : "-";

            // Move Attributes
            // Standard Info
            rankText.text = (move != null) ? move.Rank.ToString() : "-";
            powerText.text = (move != null) ? move.GetPowerAsString() : "-";
            accuracyText.text = (move != null) ? move.GetAccuracyAsString() : "-";
            energyText.text = (move != null) ? move.GetEnergyUsageAsString(): "-";

            // Stat Change Info
            // Attack
            attackChangeUserText.text = (move != null) ? move.attackChangeUser.ToString("+#;-#;0") : "-";
            attackChanceUserText.text = (move != null) ? move.attackChangeChanceUser.ToString(decPoints) : "-";
            
            attackChangeTargetText.text = (move != null) ? move.attackChangeTarget.ToString("+#;-#;0") : "-";
            attackChanceTargetText.text = (move != null) ? move.attackChangeChanceTarget.ToString(decPoints) : "-";

            // Defense
            defenseChangeUserText.text = (move != null) ? move.defenseChangeUser.ToString("+#;-#;0") : "-";
            defenseChanceUserText.text = (move != null) ? move.defenseChangeChanceUser.ToString(decPoints) : "-";
            
            defenseChangeTargetText.text = (move != null) ? move.defenseChangeTarget.ToString("+#;-#;0") : "-";
            defenseChanceTargetText.text = (move != null) ? move.defenseChangeChanceTarget.ToString(decPoints) : "-";

            // Speed
            speedChangeUserText.text = (move != null) ? move.speedChangeUser.ToString("+#;-#;0") : "-";
            speedChanceUserText.text = (move != null) ? move.speedChangeChanceUser.ToString(decPoints) : "-";
            
            speedChangeTargetText.text = (move != null) ? move.speedChangeTarget.ToString("+#;-#;0") : "-";
            speedChanceTargetText.text = (move != null) ? move.speedChangeChanceTarget.ToString(decPoints) : "-";

            // Critical, Burn, and Paralysis
            criticalChanceText.text = (move != null) ? move.CriticalChance.ToString(decPoints) : "-";
            burnChanceText.text = (move != null) ? move.BurnChance.ToString(decPoints) : "-";
            paralysisChanceText.text = (move != null) ? move.ParalysisChance.ToString(decPoints) : "-";
        }

        // Sets the move info to the default display.
        public void UnloadMoveInfo()
        {
            LoadMoveInfo(null);
        }
    }
}