using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using LoLSDK;

namespace RM_BBTS
{
    // The player stats window.
    public class PlayerStatsWindow : MonoBehaviour
    {
        // The gameplay manager.
        public GameplayManager gameManager;

        //  Player object.
        public Player player;

        // The base colour for the move buttons.
        public Color baseButtonColor = Color.white;
        
        // The selection colour for the move button.
        public Color selectButtonColor = new Color(0.372F, 1.0F, 0.498F); // Color.Green

        // The charge and run moves for showing descriptions.
        private ChargeMove chargeMove;
        private RunMove runMove;

        [Header("Text")]
        // Title Text
        public TMP_Text titleText;

        // Text windows.
        public TMP_Text levelText;
        public TMP_Text healthText;
        public TMP_Text attackText;
        public TMP_Text defenseText;
        public TMP_Text speedText;
        public TMP_Text energyText;

        // // String labels for each piece of text.
        // private string levelString = "Level";
        // private string healthString = "Health";
        // private string attackString = "Attack";
        // private string defenseString = "Defense";
        // private string speedString = "Speed";
        // private string energyString = "Energy";

        [Header("Buttons")]
        // Moves
        // M0
        public Button move0Button;
        public Image move0ButtonImage;
        public TMP_Text move0ButtonText;

        // M1
        public Button move1Button;
        public Image move1ButtonImage;
        public TMP_Text move1ButtonText;

        // M2
        public Button move2Button;
        public Image move2ButtonImage;
        public TMP_Text move2ButtonText;

        // M3
        public Button move3Button;
        public Image move3ButtonImage;
        public TMP_Text move3ButtonText;

        // Charge
        public Button chargeButton;
        public Image chargeButtonImage;
        public TMP_Text chargeButtonText;

        // Run
        public Button runButton;
        public Image runButtonImage;
        public TMP_Text runButtonText;

        // Switch Button Text
        public TMP_Text switchButtonText;

        // Back Button Text
        public TMP_Text backButtonText;

        [Header("Move Info")]
        // Move Info Object
        public GameObject moveInfoObject;

        // Move Info
        public TMP_Text moveNameText;

        public TMP_Text moveRankText;
        public TMP_Text movePowerText;
        public TMP_Text moveAccuracyText;
        public TMP_Text moveEnergyText;

        public TMP_Text moveDescriptionText;

        [Header("Move Comparison")]
        // The comparison object.
        public GameObject moveCompareObject;

        // The move comparison.
        public MoveComparison moveCompare;

        // Awake is called when the script is being loaded.
        private void Awake()
        {
            // Load charge and run moves.
            chargeMove = new ChargeMove();
            runMove = new RunMove();

            // Gives it to the move compare object. These shouldn't be loaded yet.
            moveCompare.chargeMove = chargeMove;
            moveCompare.runMove = runMove;
        }

        // Start is called before the first frame update
        void Start()
        {

            // Updates the window to start off.
            // UpdateWindow();

            // Translation
            JSONNode defs = SharedState.LanguageDefs;

            // Language definitions set.
            if(defs != null)
            {
                titleText.text = defs["kwd_stats"];
                switchButtonText.text = defs["kwd_switch"];
                backButtonText.text = defs["kwd_back"];
            }
            else
            {
                LanguageMarker marker = LanguageMarker.Instance;

                marker.MarkText(titleText);

                marker.MarkText(levelText);
                marker.MarkText(healthText);
                marker.MarkText(attackText);
                marker.MarkText(defenseText);
                marker.MarkText(speedText);
                marker.MarkText(energyText);

                marker.MarkText(move0ButtonText);
                marker.MarkText(move1ButtonText);
                marker.MarkText(move2ButtonText);
                marker.MarkText(move3ButtonText);
                marker.MarkText(chargeButtonText);
                marker.MarkText(runButtonText);

                marker.MarkText(switchButtonText);
                marker.MarkText(backButtonText);

                marker.MarkText(moveNameText);
                marker.MarkText(moveRankText);
                marker.MarkText(movePowerText);
                marker.MarkText(moveAccuracyText);
                marker.MarkText(moveEnergyText);
                marker.MarkText(moveDescriptionText);
            }


            ResetMoveButtonColors();
            UpdatePlayerInfo();
            SwitchToChargeMove();

            // Default section.
            SwitchToMoveInfo();
        }

        // This function is called when the object becomes enabled and active.
        private void OnEnable()
        {
            // The menu always starts on the charge move.
            // This address a bug where the button highlight didn't match up with what was one screen...
            // To start off.
            // It could make more sense to remember the number of the last selected move...
            // But I don't feel like doing that.
            ResetMoveButtonColors();
            UpdatePlayerInfo();
            SwitchToChargeMove();


            // Checks if the move compare object is inactive.
            bool makeInactive = !moveCompareObject.activeSelf;

            // Turns the move compare object on to make sure the changes happen.
            moveCompareObject.SetActive(true);

            // Reset the scroll bar positions for the move compare object.
            moveCompare.ResetScrollBarPositions();

            // If the object should be made inactive again.
            if (makeInactive)
                moveCompareObject.SetActive(false);

            // Default section.
            SwitchToMoveInfo();
        }

        // // Called when the player stats window is disabled.
        // private void OnDisable()
        // {
        //     // Reset the scroll bar positions.
        //     moveCompareObject.gameObject.SetActive(true);
        //     moveCompare.ResetScrollBarPositions();
        //     SwitchToMoveInfo();
        // }

        // // Toggles the visibility of the player stat window.
        // public void ToggleVisibility()
        // {
        //     gameObject.SetActive(!gameObject.activeSelf);
        // 
        //     // Disables/Enables Certain Functions
        //     gameManager.mouseTouchInput.gameObject.SetActive(!gameObject.activeSelf);
        // }

        // Upates the UI for the stats window.
        public void UpdatePlayerInfo()
        {
            // TEXT
            // Level
            levelText.text = gameManager.LevelString + ": " + player.Level.ToString();

            // Stats
            healthText.text = gameManager.HealthString + ": " + Mathf.Ceil(player.Health).ToString() + "/" + Mathf.Ceil(player.MaxHealth).ToString();
            attackText.text = gameManager.AttackString + ": " + Mathf.Ceil(player.Attack).ToString();
            defenseText.text = gameManager.DefenseString + ": " + Mathf.Ceil(player.Defense).ToString();
            speedText.text = gameManager.SpeedString + ": " + Mathf.Ceil(player.Speed).ToString();

            // Energy
            // Now shows as a percentage.
            // energyText.text = gameManager.EnergyString + ": " + Mathf.Ceil(player.Energy).ToString() + "/" + Mathf.Ceil(player.MaxEnergy).ToString();
            energyText.text = gameManager.EnergyString + ": " +
                (player.Energy / player.MaxEnergy * 100.0F).ToString("F" + GameplayManager.DISPLAY_DECIMAL_PLACES.ToString()) + "%";

            // BUTTONS
            // M0
            move0Button.interactable = player.Move0 != null;
            move0ButtonText.text = (player.Move0 != null) ? player.Move0.Name : "-";

            // M1
            move1Button.interactable = player.Move1 != null;
            move1ButtonText.text = (player.Move1 != null) ? player.Move1.Name : "-";

            // M2
            move2Button.interactable = player.Move2 != null;
            move2ButtonText.text = (player.Move2 != null) ? player.Move2.Name : "-";

            // M3
            move3Button.interactable = player.Move3 != null;
            move3ButtonText.text = (player.Move3 != null) ? player.Move3.Name : "-";

            // Charge and Run Buttons
            chargeButtonText.text = MoveList.Instance.ChargeMove.Name;
            runButtonText.text = MoveList.Instance.RunMove.Name;

            // These moves can always be interacted with.
            chargeButton.interactable = true;
            runButton.interactable = true;

            // Default showing for both versions (charge move).
            UpdateMoveInfo(4);
            moveCompare.HighlightMove(4);
        }

        // Updates the move info.
        // [0 - 3] Player Moves, [4] - Charge, [5] - Run
        public void UpdateMoveInfo(int moveNumber)
        {
            // Move object
            Move move = null;

            // Checks move number
            switch (moveNumber)
            {
                case 0: // Move 0
                    move = player.Move0;
                    break;

                case 1: // Move 1
                    move = player.Move1;
                    break;

                case 2: // Move 2
                    move = player.Move2;
                    break;

                case 3: // Move 3
                    move = player.Move3;
                    break;

                case 4: // Charge
                    move = chargeMove;
                    break;

                case 5: // Run
                    move = runMove;
                    break;

                default:
                    return;

            }

            // Updates the visuals.
            // Name
            moveNameText.text = move.Name;

            // Rank
            moveRankText.text = gameManager.RankString + ": " + move.Rank.ToString();

            // Power
            movePowerText.text = gameManager.PowerString + ": " + move.GetPowerAsString();

            // Accuracy
            moveAccuracyText.text = gameManager.AccuracyString + ": " + move.GetAccuracyAsString();

            // Energy
            moveEnergyText.text = gameManager.EnergyString + ": " + move.GetEnergyUsageAsString();

            // Description
            moveDescriptionText.text = gameManager.DescriptionString + ": " + move.description.ToString();

            // If the text-to-speech is enabled, and the SDK has been initialized.. 
            if (GameSettings.Instance.UseTextToSpeech && LOLSDK.Instance.IsInitialized)
            {
                // Checks which mode the menu is in.
                // If move info is visible, then the move description is read.
                if (moveInfoObject.activeSelf)
                {
                    // Desc speak key has been set.
                    if (move.descSpeakKey != "")
                    {
                        // Voice the move description.
                        LOLManager.Instance.textToSpeech.SpeakText(move.descSpeakKey);
                    }
                }
                else // If the move info object isn't visible, just read the move name.
                {
                    // Name speak key has been set.
                    if (move.nameSpeakKey != "")
                    {
                        // Voice the move description.
                        LOLManager.Instance.textToSpeech.SpeakText(move.nameSpeakKey);
                    }
                }
            }


            // Highlights the move.
            moveCompare.HighlightMove(moveNumber);

        }

        // Resets the move button colours to the defailt.
        private void ResetMoveButtonColors()
        {
            // TODO: optimize this.
            move0ButtonImage.color = baseButtonColor;
            move1ButtonImage.color = baseButtonColor;
            move2ButtonImage.color = baseButtonColor;
            move3ButtonImage.color = baseButtonColor;
            chargeButtonImage.color = baseButtonColor;
            runButtonImage.color = baseButtonColor;
        }

        // Switch selected move to move 0.
        public void SwitchToMove0()
        {
            ResetMoveButtonColors();
            move0ButtonImage.color = selectButtonColor;
            UpdateMoveInfo(0);

        }

        // Switch selected move to move 1.
        public void SwitchToMove1()
        {
            ResetMoveButtonColors();
            move1ButtonImage.color = selectButtonColor;
            UpdateMoveInfo(1);
        }

        // Switch selected move to move 2.
        public void SwitchToMove2()
        {
            ResetMoveButtonColors();
            move2ButtonImage.color = selectButtonColor;
            UpdateMoveInfo(2);
        }

        // Switch selected move to move 3.
        public void SwitchToMove3()
        {
            ResetMoveButtonColors();
            move3ButtonImage.color = selectButtonColor;
            UpdateMoveInfo(3);
        }

        // Switch selected move to charge move.
        public void SwitchToChargeMove()
        {
            ResetMoveButtonColors();
            chargeButtonImage.color = selectButtonColor;
            UpdateMoveInfo(4);
        }

        // Switch selected move to run move.
        public void SwitchToRunMove()
        {
            ResetMoveButtonColors();
            runButtonImage.color = selectButtonColor;
            UpdateMoveInfo(5);
        }

        // VIEW SWITCHES
        // Switches the current view in the player stats.
        public void SwitchView()
        {
            bool active = moveInfoObject.activeSelf;
            moveInfoObject.SetActive(!active);
            moveCompareObject.SetActive(active);
        }

        // Switch to the move info window.
        public void SwitchToMoveInfo()
        {
            moveInfoObject.SetActive(true);
            moveCompareObject.SetActive(false);
        }

        // Switch to the move compare window.
        public void SwitchToMoveCompare()
        {
            moveInfoObject.SetActive(false);
            moveCompareObject.SetActive(true);
        }

    }
}