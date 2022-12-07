using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SimpleJSON;


namespace RM_BBTS
{
    public class LearnMove : MonoBehaviour
    {
        // The winow object to be activated and deactivated.
        public GameObject windowObject;

        // The player.
        public Player player;

        // The new move.
        public Move newMove;

        // The move being learned.
        private Move learningMove;

        // The battle object.
        public BattleManager battle;

        // The existing moves.
        private Move move0;
        private Move move1;
        private Move move2;
        private Move move3;

        // The new move message text.
        public TMP_Text switchMoveMessageText;

        // The player moves header text.
        public TMP_Text playerMovesHeaderText;

        // The move panels
        [Header("Panels")]
        // New move info panel.
        public MoveInfoPanel newMoveInfo;

        // Move info panels.
        public MoveInfoPanel move0Info;
        public MoveInfoPanel move1Info;
        public MoveInfoPanel move2Info;
        public MoveInfoPanel move3Info;

        // The buttons components for translation.
        [Header("Buttons")]
        // Switch text for the confirm button.
        public TMP_Text confirmButtonText;

        // The switch button text for the four moves.
        public TMP_Text switchMove0Text;
        public TMP_Text switchMove1Text;
        public TMP_Text switchMove2Text;
        public TMP_Text switchMove3Text;

        // Start is called before the first frame update
        void Start()
        {
            // Translation
            JSONNode defs = SharedState.LanguageDefs;

            // Language definitions set.
            if (defs != null)
            {
                // The switch move message text.
                switchMoveMessageText.text = defs["switchMoveMessage"];

                // The player moves header text.
                playerMovesHeaderText.text = defs["kwd_playerMoves"];

                // Confirm button text.
                confirmButtonText.text = defs["kwd_confirm"];

                // Switch move text.
                switchMove0Text.text = defs["kwd_switch"];
                switchMove1Text.text = defs["kwd_switch"];
                switchMove2Text.text = defs["kwd_switch"];
                switchMove3Text.text = defs["kwd_switch"];
            }
        }

        // This function is called when the object becomes enabled and visible.
        private void OnEnable()
        {
            // Hides the battle textbox.
            battle.textBox.Hide();
            LoadMoveInformation();
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

        // Loads the player moves.
        public void LoadMoveInformation()
        {
            // NEW MOVE
            newMoveInfo.LoadMoveInfo(newMove);
            learningMove = newMove;

            // MOVE 0
            move0 = player.Move0;
            move0Info.LoadMoveInfo(move0);

            // MOVE 1
            move1 = player.Move1;
            move1Info.LoadMoveInfo(move1);

            // MOVE 2
            move2 = player.Move2;
            move2Info.LoadMoveInfo(move2);

            // MOVE 3
            move3 = player.Move3;
            move3Info.LoadMoveInfo(move3);
        }

        // Switches the new move with move 0.
        public void SwitchWithMove0()
        {
            // Change out the player's move.
            move0 = player.Move0;
            player.Move0 = newMove;

            // Switch the move slots.
            Move temp = move0;
            move0 = newMove;
            newMove = temp;

            // Reloads the move info.
            newMoveInfo.LoadMoveInfo(newMove);
            move0Info.LoadMoveInfo(move0);
        }

        // Switches the new move with move 1.
        public void SwitchWithMove1()
        {
            // Change out the player's move.
            move1 = player.Move1;
            player.Move1 = newMove;

            // Change out the move slots.
            Move temp = move1;
            move1 = newMove;
            newMove = temp;

            // Reloads the move info.
            newMoveInfo.LoadMoveInfo(newMove);
            move1Info.LoadMoveInfo(move1);
        }

        // Switches the new move with move 2.
        public void SwitchWithMove2()
        {
            // Change out the player's move.
            move2 = player.Move2;
            player.Move2 = newMove;

            // Switch the move slots.
            Move temp = move2;
            move2 = newMove;
            newMove = temp;

            // Reloads the move info.
            newMoveInfo.LoadMoveInfo(newMove);
            move2Info.LoadMoveInfo(move2);
        }

        // Switches the new move with move 3.
        public void SwitchWithMove3()
        {
            // Change out the player's move.
            move3 = player.Move3;
            player.Move3 = newMove;

            // Switch the move slots.
            Move temp = move3;
            move3 = newMove;
            newMove = temp;

            // Reloads the move info.
            newMoveInfo.LoadMoveInfo(newMove);
            move3Info.LoadMoveInfo(move3);
        }

        // Accepts the move changes.
        public void ConfirmChanges()
        {
            windowObject.SetActive(false);

            // Checks to see if a new move was learned or not.
            if(newMove.Id == learningMove.Id) // new move was not learned.
            {
                battle.textBox.pages.Insert(battle.textBox.CurrentPageIndex + 1,
                    new Page(
                        BattleMessages.Instance.GetLearnMoveNoMessage(learningMove.Name),
                        BattleMessages.Instance.GetLearnMoveNoSpeakKey()
                        ));
            }
            else // new move was learned.
            {
                battle.textBox.pages.Insert(battle.textBox.CurrentPageIndex + 1,
                    new Page(
                        BattleMessages.Instance.GetLearnMoveYesMessage(learningMove.Name),
                        BattleMessages.Instance.GetLearnMoveYesSpeakKey()
                    ));
            }

            // Move onto the next pages (skip placeholder text).
            battle.textBox.NextPage();

            // Show the box again, and move onto the next page.
            // May not be needed.
            windowObject.SetActive(false);

            // This might not be needed.
            // Set these values to null.
            newMove = null;
            learningMove = null;

            // Show the textbox and go onto the move learned page.
            battle.textBox.Show();
            battle.textBox.NextPage();
        }
    }
}