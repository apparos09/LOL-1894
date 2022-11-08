using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RM_BBTS
{
    // The player stats window.
    public class PlayerStatsWindow : MonoBehaviour
    {
        // The gameplay manager.
        public GameplayManager gameManager;

        //  Player object.
        public Player player;

        // The charge and run moves for showing descriptions.
        private ChargeMove chargeMove;
        private RunMove runMove;

        [Header("Text")]
        // Text windows.
        public TMP_Text levelText;
        public TMP_Text healthText;
        public TMP_Text attackText;
        public TMP_Text defenseText;
        public TMP_Text speedText;
        public TMP_Text energyText;

        [Header("Buttons")]
        // Moves
        public Button move0Button;
        public TMP_Text move0Text;

        public Button move1Button;
        public TMP_Text move1Text;

        public Button move2Button;
        public TMP_Text move2Text;

        public Button move3Button;
        public TMP_Text move3Text;

        // Charge and Run
        public Button chargeButton;
        public Button runButton;

        [Header("Move Info")]
        // Move Info
        public TMP_Text moveNameText;

        public TMP_Text moveRankText;
        public TMP_Text movePowerText;
        public TMP_Text moveAccuracyText;
        public TMP_Text moveEnergyText;

        public TMP_Text moveDescriptionText;

        // Awake is called when the script is being loaded.
        private void Awake()
        {
            // Load moves.
            chargeMove = new ChargeMove();
            runMove = new RunMove();
        }

        // Start is called before the first frame update
        void Start()
        {

            // Updates the window to start off.
            // UpdateWindow();
        }

        // This function is called when the object becomes enabled and active.
        private void OnEnable()
        {
            UpdatePlayerInfo();
        }

        // Toggles the visibility of the player stat window.
        public void ToggleVisibility()
        {
            gameObject.SetActive(!gameObject.activeSelf);

            // Disables/Enables Certain Functions
            gameManager.mouseTouchInput.gameObject.SetActive(!gameObject.activeSelf);
        }

        // Upates the UI for the stats window.
        public void UpdatePlayerInfo()
        {
            // TEXT
            // Level
            levelText.text = "Level: " + player.Level.ToString();

            // Stats
            healthText.text = "Health: " + Mathf.Ceil(player.Health).ToString() + "/" + Mathf.Ceil(player.MaxHealth).ToString();
            attackText.text = "Attack: " + Mathf.Ceil(player.Attack).ToString();
            defenseText.text = "Defense: " + Mathf.Ceil(player.Defense).ToString();
            speedText.text = "Speed: " + Mathf.Ceil(player.Speed).ToString();

            // Energy
            energyText.text = "Energy: " + Mathf.Ceil(player.Energy).ToString() + "/" + Mathf.Ceil(player.MaxEnergy).ToString();

            // BUTTONS
            // M0
            move0Button.interactable = player.Move0 != null;
            move0Text.text = (player.Move0 != null) ? player.Move0.Name : "-";

            // M1
            move1Button.interactable = player.Move1 != null;
            move1Text.text = (player.Move1 != null) ? player.Move1.Name : "-";

            // M2
            move2Button.interactable = player.Move2 != null;
            move2Text.text = (player.Move2 != null) ? player.Move2.Name : "-";

            // M3
            move3Button.interactable = player.Move3 != null;
            move3Text.text = (player.Move3 != null) ? player.Move3.Name : "-";


            // These moves can always be interacted with.
            chargeButton.interactable = true;
            runButton.interactable = true;

            // Default showing.
            UpdateMoveInfo(4);
        }

        // Updates the move info.
        // [0 - 3] Player Moves, [4] - Charge, [5] - Run
        public void UpdateMoveInfo(int moveNumber)
        {
            // Move object
            Move move = null;

            // Checks move number
            switch(moveNumber)
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
            moveNameText.text = move.Name;

            moveRankText.text = "Rank: " + move.Rank.ToString();
            movePowerText.text = "Power: " + move.Power.ToString();
            moveAccuracyText.text = "Accuracy: " + Mathf.Round(move.Accuracy * 100.0F).ToString() + "%";
            moveEnergyText.text = "Energy: " + move.Energy.ToString();

            moveDescriptionText.text = "Description: " + move.description.ToString();

        }

        
        // Switch selected move to move 0.
        public void SwitchToMove0()
        {
            UpdateMoveInfo(0);

        }

        // Switch selected move to move 1.
        public void SwitchToMove1()
        {
            UpdateMoveInfo(1);
        }

        // Switch selected move to move 2.
        public void SwitchToMove2()
        {
            UpdateMoveInfo(2);
        }

        // Switch selected move to move 3.
        public void SwitchToMove3()
        {
            UpdateMoveInfo(3);
        }

        // Switch selected move to charge move.
        public void SwitchToChargeMove()
        {
            UpdateMoveInfo(4);
        }

        // Switch selected move to run move.
        public void SwitchToRunMove()
        {
            UpdateMoveInfo(5);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}