using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

namespace RM_BBTS
{
    // The script for managing treasure moves.
    public class MultipleMoveOffer : MonoBehaviour
    {
        // The winow object to be activated and deactivated.
        public GameObject windowObject;

        // The gameplay manager.
        public BattleManager battle;

        // The class for learning the move.
        public LearnMove learnMove;

        // The three moves that will be offered.
        private Move[] moves;

        // The index of the selected move.
        private int selectedMoveIndex = -1;

        // New move info panel.
        public MoveInfoPanel selectedMoveInfo;


        [Header("UI")]
        // The description text.
        public TMP_Text descriptionText;

        // The language key for the description text.
        public const string DESCRIPTION_TEXT_LANG_KEY = "multMoveOffer_msg_prompt";

        // The color for a button that's not selected.
        public Color unselectedColor = Color.white;

        // The color for a button that is selected.
        public Color selectedColor = Color.green;

        // Move 0
        [Header("UI/Move 0")]
        public TMP_Text move0ButtonText;
        public Image move0ButtonImage;

        // Move 1
        [Header("UI/Move 1")]
        public TMP_Text move1ButtonText;
        public Image move1ButtonImage;

        // Move 2
        [Header("UI/Move 2")]
        public TMP_Text move2ButtonText;
        public Image move2ButtonImage;

        [Header("UI/Next")]
        // THen ext button.
        public Button nextButton;
        public TMP_Text nextButtonText;

        [Header("UI/Skip")]
        // The skip button.
        public Button skipButton;
        public TMP_Text skipButtonText;

        private void Awake()
        {
            // Initialize the list of moves.
            moves = new Move[3] { null, null, null };
        }

        // Start is called before the first frame update
        void Start()
        {
            selectedMoveInfo.ClearMoveInfo();

            // The definitions for the language.
            JSONNode defs = SharedState.LanguageDefs;

            // The defs are not set.
            if (defs != null)
            {
                descriptionText.text = defs[DESCRIPTION_TEXT_LANG_KEY];
                nextButtonText.text = defs["kwd_next"];
                skipButtonText.text = defs["kwd_skip"];
            }
            else
            {
                LanguageMarker marker = LanguageMarker.Instance;

                marker.MarkText(descriptionText);

                marker.MarkText(move0ButtonText);
                marker.MarkText(move1ButtonText);
                marker.MarkText(move2ButtonText);

                marker.MarkText(nextButtonText);
                marker.MarkText(skipButtonText);
            }
        }

        // This function is called when the object becomes enabled and visible.
        private void OnEnable()
        {
            // Hides the battle textbox.
            battle.textBox.Hide();
            GenerateMoves();

            // Read out the description text if text-to-speech is enabled.
            if (GameSettings.Instance.UseTextToSpeech)
                LOLManager.Instance.textToSpeech.SpeakText(DESCRIPTION_TEXT_LANG_KEY);
        }

        // Move 0
        public Move Move0
        {
            get { return moves[0]; }

            set { moves[0] = value; }
        }

        // Move 1
        public Move Move1
        {
            get { return moves[1]; }

            set { moves[1] = value; }
        }

        // Move 2
        public Move Move2
        {
            get { return moves[2]; }

            set { moves[2] = value; }
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

        // Gets a move from the array.
        public Move GetMove(int index)
        {
            // Checks if a move is valid or not before putting in the index.
            if (index < 0 || index >= moves.Length)
                return null;
            else
                return moves[index];
        }

        // Sets a move in the array.
        public void SetMove(Move move, int index)
        {
            // Checks if an index is valid for saving the new move.
            if (index >= 0 && index < moves.Length)
                moves[index] = move;
        }

        // Generate the moves.
        public void GenerateMoves()
        {
            // The player.
            Player player = battle.player;

            // Clears out the moves.
            ClearMoves();

            // Generates the new moves.
            List<Move> newMoves = MoveList.Instance.GetRandomMoves(moves.Length, player);
            
            // Puts the new moves into the move array.
            for(int i = 0; i < moves.Length && i < newMoves.Count; i++)
            {
                moves[i] = newMoves[i];
            }

            // Sets the buttons text.
            move0ButtonText.text = moves[0].Name;
            move1ButtonText.text = moves[1].Name;
            move2ButtonText.text = moves[2].Name;
        }

        // Selects the provided move.
        private void SelectMove(int index)
        {
            // Is used to enable the next button.
            bool enableNext = true;

            // Deselects all the moves, and clears out the info panel.
            DeselectAllMoves();

            // Save the selected move index.
            selectedMoveIndex = index;

            // Checks which move to select.
            switch (index)
            {
                case 0: // Move 0
                    move0ButtonImage.color = selectedColor;
                    selectedMoveInfo.LoadMoveInfo(Move0);
                    break;
                case 1: // Move 1
                    move1ButtonImage.color = selectedColor;
                    selectedMoveInfo.LoadMoveInfo(Move1);
                    break;
                case 2: // Move 2
                    move2ButtonImage.color = selectedColor;
                    selectedMoveInfo.LoadMoveInfo(Move2);
                    break;

                default: // No move to select.
                    selectedMoveIndex = -1;
                    enableNext = false;
                    break;
            }

            // Is used to enable/disable the next button.
            nextButton.interactable = enableNext;
        }

        // Select the first move.
        public void SelectMove0()
        {
            SelectMove(0);
        }

        // Select the second move.
        public void SelectMove1()
        {
            SelectMove(1);
        }

        // Select the third move.
        public void SelectMove2()
        {
            SelectMove(2);
        }

        // Unselects all the moves.
        public void DeselectAllMoves()
        {
            // Change all these to the unselected colors.
            move1ButtonImage.color = unselectedColor;
            move0ButtonImage.color = unselectedColor;
            move2ButtonImage.color = unselectedColor;

            // Clear out the move info.
            selectedMoveInfo.ClearMoveInfo();

            // Disable the next button.
            nextButton.interactable = false;
        }


        // Clear out the saved moves.
        public void ClearMoves()
        {
            // Clear out the saved moves.
            for (int i = 0; i < moves.Length; i++)
                moves[i] = null;


            // Clears out the button text.
            move0ButtonText.text = "-";
            move1ButtonText.text = "-";
            move2ButtonText.text = "-";

            // Unselects all the moves.
            DeselectAllMoves();
        }

        // Used to move onto the next portion (learn move portion).
        public void Next()
        {
            // No move has been selected.
            if (selectedMoveIndex < 0 || selectedMoveIndex >= moves.Length)
                return;

            
            // Grabs the player from the battle script.
            Player player = battle.player;

            // Checks if the player has four moves or not.
            if(player.HasFourFightMoves()) // Go onto move learn screen.
            {
                // Deactivates the window object.
                Deactivate();

                // Activates the learned move.
                learnMove.SetLearningMove(moves[selectedMoveIndex], false);
                learnMove.Activate();
            }
            else // Auto learn the move.
            {
                // This case should never be reached, but it will still be checked.
                // Deactive the multi move offer.
                Deactivate();

                // The selected move.
                Move newMove = moves[selectedMoveIndex];

                // Gives the player a new move.
                for(int i = 0; i < player.moves.Length; i++)
                {
                    // Found slot for move.
                    if (player.moves[i] == null)
                    {
                        player.moves[i] = newMove;
                        break;
                    }
                        
                }

                // Say what move the player learned.
                battle.textBox.pages.Insert(battle.textBox.CurrentPageIndex + 1,
                        new Page(
                            BattleMessages.Instance.GetLearnMoveYesMessage(newMove.Name),
                            BattleMessages.Instance.GetLearnMoveYesSpeakKey()
                        ));

                // Move onto the next page (skip placeholder text).
                battle.textBox.NextPage();

                // This may not be needed, but it's done again to be sure.
                // TODO: remove this?
                windowObject.SetActive(false);

                // Show the textbox and go onto the skip page.
                battle.textBox.Show();
                battle.textBox.NextPage();
            }
        }

        // Used to skip learning the new move.
        public void Skip()
        {
            // Deactive the multi move offer.
            Deactivate();

            // Says that the player did not learn any of the new moves.
            battle.textBox.pages.Insert(battle.textBox.CurrentPageIndex + 1,
                    new Page(
                        BattleMessages.Instance.GetMultipleMoveOfferSkipMessage(),
                        BattleMessages.Instance.GetMultipleMoveOfferSkipSpeakKey()
                    ));

            // Move onto the next pages (skip placeholder text).
            battle.textBox.NextPage();

            // Show the box again, and move onto the next page.
            // May not be needed.
            windowObject.SetActive(false);

            // Show the textbox and go onto the skip page.
            battle.textBox.Show();
            battle.textBox.NextPage();
        }
    }
}