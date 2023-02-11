using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Timeline;
using LoLSDK;

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
        public TMP_Text nameText;

        // Move Attributes
        // Standard Info
        [Header("Primary Stats")]
        public TMP_Text rankText;
        public TMP_Text powerText;
        public TMP_Text accuracyText;
        public TMP_Text energyText;

        // Stat Change Info

        // Attack
        [Header("Attack Chance Events")]
        public TMP_Text attackChangeUserText;
        public TMP_Text attackChanceUserText;
        public TMP_Text attackChangeTargetText;
        public TMP_Text attackChanceTargetText;

        // Defense
        [Header("Defense Chance Events")]
        public TMP_Text defenseChangeUserText;
        public TMP_Text defenseChanceUserText;
        public TMP_Text defenseChangeTargetText;
        public TMP_Text defenseChanceTargetText;

        // Speed
        [Header("Speed Chance Events")]
        public TMP_Text speedChangeUserText;
        public TMP_Text speedChanceUserText;
        public TMP_Text speedChangeTargetText;
        public TMP_Text speedChanceTargetText;

        // Accuracy
        [Header("Accuracy Chance Events")]
        public TMP_Text accuracyChangeUserText;
        public TMP_Text accuracyChanceUserText;
        public TMP_Text accuracyChangeTargetText;
        public TMP_Text accuracyChanceTargetText;


        // Critical, Burn, and Paralysis
        [Header("Other Chance Events")]
        public TMP_Text criticalChanceText;
        public TMP_Text burnChanceText;
        public TMP_Text paralysisChanceText;

        // Start is just before any of the update methods are called for the first time.
        private void Start()
        {
            // Colour the text to show that it's not coming form the language file.
            if(!LOLSDK.Instance.IsInitialized)
                LanguageMarker.Instance.MarkText(nameText);
        }

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
            // The change format for the change attributes.
            string changeFormat = "+#;-#;0";
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
            accuracyText.text = (move != null) ? move.GetAccuracyAsString() : "-"; // Stat
            energyText.text = (move != null) ? move.GetEnergyUsageAsString(): "-";

            // Stat Change Info
            // Attack
            attackChangeUserText.text = (move != null) ? move.attackChangeUser.ToString(changeFormat) : "-";
            attackChanceUserText.text = (move != null) ? move.attackChangeChanceUser.ToString(decPoints) : "-";
            
            attackChangeTargetText.text = (move != null) ? move.attackChangeTarget.ToString(changeFormat) : "-";
            attackChanceTargetText.text = (move != null) ? move.attackChangeChanceTarget.ToString(decPoints) : "-";

            // Defense
            defenseChangeUserText.text = (move != null) ? move.defenseChangeUser.ToString(changeFormat) : "-";
            defenseChanceUserText.text = (move != null) ? move.defenseChangeChanceUser.ToString(decPoints) : "-";
            
            defenseChangeTargetText.text = (move != null) ? move.defenseChangeTarget.ToString(changeFormat) : "-";
            defenseChanceTargetText.text = (move != null) ? move.defenseChangeChanceTarget.ToString(decPoints) : "-";

            // Speed
            speedChangeUserText.text = (move != null) ? move.speedChangeUser.ToString(changeFormat) : "-";
            speedChanceUserText.text = (move != null) ? move.speedChangeChanceUser.ToString(decPoints) : "-";
            
            speedChangeTargetText.text = (move != null) ? move.speedChangeTarget.ToString(changeFormat) : "-";
            speedChanceTargetText.text = (move != null) ? move.speedChangeChanceTarget.ToString(decPoints) : "-";

            // Accuracy (Change)
            accuracyChangeUserText.text = (move != null) ? move.accuracyChangeUser.ToString(changeFormat) : "-";
            accuracyChanceUserText.text = (move != null) ? move.accuracyChangeChanceUser.ToString(decPoints) : "-";

            accuracyChangeTargetText.text = (move != null) ? move.accuracyChangeTarget.ToString(changeFormat) : "-";
            accuracyChanceTargetText.text = (move != null) ? move.accuracyChangeChanceTarget.ToString(decPoints) : "-";

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