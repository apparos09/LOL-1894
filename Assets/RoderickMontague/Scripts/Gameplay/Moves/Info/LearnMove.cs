using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SimpleJSON;
using UnityEngine.UI;
using System.Linq;

namespace RM_BBTS
{
    public class LearnMove : MonoBehaviour
    {
        // The winow object to be activated and deactivated.
        public GameObject windowObject;

        // The player.
        public Player player;

        // The battle object.
        public BattleManager battle;

        // The title text.
        public TMP_Text titleText;

        // The instructional text.
        public TMP_Text instructText;
        // The language key for the instruct text.
        public const string INSTRUCT_TEXT_LANG_KEY = "msg_learnMove";

        // The move panels
        [Header("Moves")]

        // The instructional text.
        public TMP_Text moveOfferHeaderText;

        // The move being offered to be switched.
        // This changes when moves are switched.
        private Move moveOffer;

        // The new move being offered. 
        // This is used to see if the new move was added or not.
        private Move newMove = null;

        // New move info panel.
        public MoveInfoPanel moveOfferInfo = null;

        // The move objects.
        private Move moveSelect;

        // The colours for the move buttons when deselected, and selected.
        public Color moveDeselectColor = Color.white;
        public Color moveSelectColor = Color.green;

        // Player moves header text.
        public TMP_Text currentMovesHeaderText;

        [Header("Moves/Move 0")]
        // Move 0
        public Button move0Button;
        public TMP_Text move0ButtonText;

        [Header("Moves/Move 1")]
        // Move 1
        public Button move1Button;
        public TMP_Text move1ButtonText;

        [Header("Moves/Move 2")]
        // Move 2
        public Button move2Button;
        public TMP_Text move2ButtonText;

        [Header("Moves/Move 3")]
        // Move 3
        public Button move3Button;
        public TMP_Text move3ButtonText;

        [Header("Moves/Switch")]

        // The info panel for the selected move.
        public MoveInfoPanel moveSelectInfo;

        // The text for the switch moves button
        public Button switchButton;
        
        // The switch button text.
        public TMP_Text switchButtonText;

        // The finish button text.
        public TMP_Text finishButtonText;

        // Start is called before the first frame update
        void Start()
        {
            // NOTE: enabled is called before start, so be careful.

            // The switch button cannot be used until a move is selected.
            switchButton.interactable = false;

            // Translation
            JSONNode defs = SharedState.LanguageDefs;

            // Language definitions set.
            if (defs != null)
            {
                // Title text.
                titleText.text = defs["kwd_learnMoveTitle"];

                // The instructional text.
                instructText.text = defs[INSTRUCT_TEXT_LANG_KEY];

                // The player moves header text.
                moveOfferHeaderText.text = defs["kwd_moveOffer"];

                // The player moves header text.
                currentMovesHeaderText.text = defs["kwd_currentMoves"];

                // The switch move message text.
                switchButtonText.text = defs["kwd_switchMoves"];

                // The switch move message text.
                finishButtonText.text = defs["kwd_finish"];
            }
            else
            {
                // The language marker.
                LanguageMarker marker = LanguageMarker.Instance;

                // The title text.
                marker.MarkText(titleText);

                // The instructional text.
                marker.MarkText(instructText);

                // The move offer header text.
                marker.MarkText(moveOfferHeaderText);

                // The player moves header text.
                marker.MarkText(currentMovesHeaderText);

                // The switch moves text.
                marker.MarkText(switchButtonText);

                // The finish text.
                marker.MarkText(finishButtonText);
            }
        }

        // This function is called when the object becomes enabled and visible.
        private void OnEnable()
        {
            // Hides the battle textbox.
            battle.textBox.Hide();
            LoadMoveInformation();

            // Read out the instructional text if text-to-speech is enabled.
            if (GameSettings.Instance.UseTextToSpeech)
                LOLManager.Instance.textToSpeech.SpeakText(INSTRUCT_TEXT_LANG_KEY);
        }
        
        // Activates the panel.
        public void Activate()
        {
            windowObject.SetActive(true);
        }

        // Deactivates the panel.
        public void Deactivate()
        {
            windowObject.SetActive(false);
        }

        // Sets the move being learned.
        // If all the move information should be refreshed now, refresh it.
        public void SetLearningMove(Move move, bool refreshInfo)
        {
            // These two variables are used to check if a new move was learned.
            moveOffer = move;
            newMove = move;

            // If the information should be reloaded.
            if (refreshInfo && move != null)
                LoadMoveInformation();
        }

        // Loads the player moves.
        public void LoadMoveInformation()
        {
            // NEW MOVE
            moveOfferInfo.LoadMoveInfo(moveOffer);

            // Array of move buttons and texts.
            Button[] moveButtons = new Button[4] { move0Button, move1Button, move2Button, move3Button };
            TMP_Text[] moveTexts = new TMP_Text[4] { move0ButtonText, move1ButtonText, move2ButtonText, move3ButtonText };

            // Loads the move text.
            for(int i = 0; i < moveTexts.Length && i < player.moves.Length; i++)
            {
                // Filling the move information.
                moveButtons[i].image.color = moveDeselectColor;
                moveButtons[i].interactable = player.moves[i] != null;
                moveTexts[i].text = player.moves[i] != null ? player.moves[i].Name : "-";
            }

            // Reset the colors.
            ResetMoveButtonColors();

            // Clear the selection info.
            moveSelect = null;
            moveSelectInfo.ClearMoveInfo();
            switchButton.interactable = false;
        }

        // Resets the button colours.
        public void ResetMoveButtonColors()
        {
            // Reset the colours of the move buttons.
            move0Button.image.color = moveDeselectColor;
            move1Button.image.color = moveDeselectColor;
            move2Button.image.color = moveDeselectColor;
            move3Button.image.color = moveDeselectColor;
        }

        // Selects a move.
        public void SelectMove(int select)
        {
            // Reset button colours.
            ResetMoveButtonColors();

            // Checks which move to select.
            switch (select)
            {
                case 0: // Move 0
                    moveSelect = player.moves[0];
                    move0Button.image.color = moveSelectColor;
                    break;

                case 1: // Move 1
                    moveSelect = player.moves[1];
                    move1Button.image.color = moveSelectColor;
                    break;

                case 2: // Move 2
                    moveSelect = player.moves[2];
                    move2Button.image.color = moveSelectColor;
                    break;

                case 3: // Move 3
                    moveSelect = player.moves[3];
                    move3Button.image.color = moveSelectColor;
                    break;

                default:
                    moveSelect = null;
                    break;
            }


            // Checks if a move was selected.
            if(moveSelect != null) // Load info and set button to be interactable.
            {
                moveSelectInfo.LoadMoveInfo(moveSelect);
                switchButton.interactable = true;
            }
            else // Clear info and make the button non-interactable.
            {
                moveSelectInfo.ClearMoveInfo();
                switchButton.interactable = false;
            }
        }

        // Select move 0.
        public void SelectMove0()
        {
            SelectMove(0);
        }

        // Select move 1.
        public void SelectMove1()
        {
            SelectMove(1);
        }

        // Select move 2.
        public void SelectMove2()
        {
            SelectMove(2);
        }

        // Select move 3.
        public void SelectMove3()
        {
            SelectMove(3);
        }

        public void SwitchMoves()
        {
            // If the move offer is not set, leave the function.
            if (moveOffer == null)
                return;

            // The index of the selected move in the player's array.
            int moveSelectIndex = 0;

            // Gets the index of the move selected.
            for(int i = 0; i < player.moves.Length; i++)
            {
                // If the selected move has been found, save the index.
                if (player.moves[i] == moveSelect)
                {
                    moveSelectIndex = i;
                    break;
                }
            }

            // Switch the two moves.
            {
                Move temp = moveOffer;
                moveOffer = moveSelect;

                moveSelect = temp;
                player.moves[moveSelectIndex] = temp;
            }

            // Reload the new move information now that changes have been made.
            LoadMoveInformation();
        }

        // Accepts the move changes.
        public void ConfirmChanges()
        {
            // Turn off the window object.
            windowObject.SetActive(false);

            // Checks to see if the new move was learned or not.
            if(moveOffer.Id == newMove.Id) // new move was not learned.
            {
                battle.textBox.pages.Insert(battle.textBox.CurrentPageIndex + 1,
                    new Page(
                        BattleMessages.Instance.GetLearnMoveNoMessage(newMove.Name),
                        BattleMessages.Instance.GetLearnMoveNoSpeakKey()
                        ));
            }
            else // new move was learned.
            {
                battle.textBox.pages.Insert(battle.textBox.CurrentPageIndex + 1,
                    new Page(
                        BattleMessages.Instance.GetLearnMoveYesMessage(newMove.Name),
                        BattleMessages.Instance.GetLearnMoveYesSpeakKey()
                    ));
            }

            // Move onto the next pages (skip placeholder text).
            battle.textBox.NextPage();

            // Show the box again, and move onto the next page.
            // May not be needed.
            windowObject.SetActive(false);

            // Reset these values.
            moveOffer = null;
            newMove = null;

            // Show the textbox and go onto the move learned page.
            battle.textBox.Show();
            battle.textBox.NextPage();
        }
    }
}