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
        public moveId id;

        // The name of the move.
        public TMPro.TMP_Text nameText;

        // Move Attributes
        // Standard Info
        public TMPro.TMP_Text rankText;
        public TMPro.TMP_Text powerText;
        public TMPro.TMP_Text accuracyText;
        public TMPro.TMP_Text energyText;

        // Stat Change Info
        // Attack
        public TMPro.TMP_Text attackChangeUserText;
        public TMPro.TMP_Text attackChanceUserText;
        public TMPro.TMP_Text attackChangeTargetText;
        public TMPro.TMP_Text attackChanceTargetText;

        // Defense
        public TMPro.TMP_Text defenseChangeUserText;
        public TMPro.TMP_Text defenseChanceUserText;
        public TMPro.TMP_Text defenseChangeTargetText;
        public TMPro.TMP_Text defenseChanceTargetText;

        // Speed
        public TMPro.TMP_Text speedChangeUserText;
        public TMPro.TMP_Text speedChanceUserText;
        public TMPro.TMP_Text speedChangeTargetText;
        public TMPro.TMP_Text speedChanceTargetText;

        // Critical, Burn, and Paralysis
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

        // Loads the move info.
        public void LoadMoveInfo(Move move)
        {
            // Sets to see if the move info has been loaded.
            loaded = move != null;

            // Copy the id (uses 0 if no move).
            id = (move != null) ? move.Id : 0;

            // Name
            nameText.text = (move != null) ? move.Name : "-";

            // Move Attributes
            // Standard Info
            rankText.text = (move != null) ? move.Rank.ToString() : "-";
            powerText.text = (move != null) ? move.Power.ToString() : "-";
            accuracyText.text = (move != null) ? move.Accuracy.ToString() : "-";
            energyText.text = (move != null) ? move.EnergyUsage.ToString() : "-";

            // Stat Change Info
            // Attack
            attackChangeUserText.text = (move != null) ? move.attackChangeUser.ToString() : "-";
            attackChanceUserText.text = (move != null) ? move.attackChangeChanceUser.ToString() : "-";
            
            attackChangeTargetText.text = (move != null) ? move.attackChangeTarget.ToString() : "-";
            attackChanceTargetText.text = (move != null) ? move.attackChangeChanceTarget.ToString() : "-";

            // Defense
            defenseChangeUserText.text = (move != null) ? move.defenseChangeUser.ToString() : "-";
            defenseChanceUserText.text = (move != null) ? move.defenseChangeChanceUser.ToString() : "-";
            
            defenseChangeTargetText.text = (move != null) ? move.defenseChangeTarget.ToString() : "-";
            defenseChanceTargetText.text = (move != null) ? move.defenseChangeChanceTarget.ToString() : "-";

            // Speed
            speedChangeUserText.text = (move != null) ? move.speedChangeUser.ToString() : "-";
            speedChanceUserText.text = (move != null) ? move.speedChangeChanceUser.ToString() : "-";
            
            speedChangeTargetText.text = (move != null) ? move.speedChangeTarget.ToString() : "-";
            speedChanceTargetText.text = (move != null) ? move.speedChangeChanceTarget.ToString() : "-";

            // Critical, Burn, and Paralysis
            criticalChanceText.text = (move != null) ? move.CriticalChance.ToString() : "-";
            burnChanceText.text = (move != null) ? move.BurnChance.ToString() : "-";
            paralysisChanceText.text = (move != null) ? move.ParalysisChance.ToString() : "-";
        }

        // Sets the move info to the default display.
        public void UnloadMoveInfo()
        {
            LoadMoveInfo(null);
        }
    }
}