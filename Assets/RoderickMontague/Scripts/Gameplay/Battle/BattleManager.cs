using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SimpleJSON;

// TODO: do status effects.
namespace RM_BBTS
{
    // Manages the battle operations for the game. This becomes active when the game enters a battle state.
    public class BattleManager : GameState
    {
        // Becomes 'true' when the overworld is initialized.
        public bool initialized = false;

        // the manager for the game.
        public GameplayManager gameManager;

        // The door
        public Door door;

        // The textbox
        public TextBox textBox;

        // The prompt for asking the player about the treasure.
        public GameObject treasurePrompt;

        // The panel for learning a new move.
        public LearnMove learnMovePanel;

        // Auto save the game when exiting the battle scene.
        public bool autoSaveOnExit = false;

        [Header("Battle")]

        // The player.
        public Player player;

        // Values used to calculate score.
        private int turnsTaken = 0;  // The amount of turns the battle took.    

        // The Move class handles the calculations for damage taken.
        public float playerDamageTaken = 0; // The amount of damage the player took.

        // The opponent for the player.
        // TODO: should a new opponent be generated everytime? Should really just cycle through some pre-build objects.
        public BattleEntity opponent;

        // The sprite for the opponent.
        public SpriteRenderer opponentSprite;

        // Base objects that are activated for battle.
        public Enemy enemyBase; // Enemy base.
        public Treasure treasureBase; // Treasure base.
        public Boss bossBase; // Boss base.

        // [Header("Battle/Mechanics")]
        // // The move the player has selected.
        // public Move playerMove;
        // 
        // // The move the opponent has selected.
        // public Move opponentMove;

        // The chance to skip a turn if paralyzed.
        private float PARALAYSIS_SKIP_CHANCE = 0.1F;

        // The chance of learning a new move.
        private float NEW_MOVE_CHANCE = 0.80F;

        // Becomes 'true' when the battle end state has been initialized.
        private bool initBattleEnd = false;

        [Header("UI")]
        // The user interface.
        public GameObject ui;

        // The turn text. Each entry is a different page.
        public List<Page> turnText;

        // The player's move page.
        public Page playerMovePage;

        // The opponent's move page.
        public Page opponentMovePage;

        [Header("UI/Player")]
        // Charge Button
        public Button chargeButton;
        public TMP_Text chargeButtonText;

        // Run Button
        public Button runButton;
        public TMP_Text runButtonText;


        // Move 0 (index 0) button.
        [Header("UI/Player/Move 0")]
        [Tooltip("The button for using Player Move 0, which is at index [0].")]
        public Button move0Button;
        public TMP_Text move0NameText;
        public TMP_Text move0AccuracyText;

        // Move 1 (index 1) button.
        [Header("UI/Player/Move 1")]
        [Tooltip("The button for using Player Move 1, which is at index [1].")]
        public Button move1Button;
        public TMP_Text move1NameText;
        public TMP_Text move1AccuracyText;

        // Move 2 (index 2) button.
        [Header("UI/Player/Move 2")]
        [Tooltip("The button for using Player Move 2, which is at index [2].")]
        public Button move2Button;
        public TMP_Text move2NameText;
        public TMP_Text move2AccuracyText;

        // Move 3 (index 3) button.
        [Header("UI/Player/Move 3")]
        [Tooltip("The button for using Player Move 3, which is at index [3].")]
        public Button move3Button;
        public TMP_Text move3NameText;
        public TMP_Text move3AccuracyText;

        

        [Header("UI/Player/Treasure")]

        // The text for the treasure prompt.
        public TMP_Text treasurePromptText;

        // The yes button for opening the treasure.
        public Button treasureYesButton;

        // The treasure yes button text.
        public TMP_Text treasureYesButtonText;

        // THe no button for opening the treasure.
        public Button treasureNoButton;

        // The treasure no button text.
        public TMP_Text treasureNoButtonText;

        [Header("UI/Opponent")]

        // The opponent title text.
        public TMP_Text opponentNameText;

        // The health bar for the opponent.
        public ProgressBar opponentHealthBar;

        // TODO: this will not be shown in the final game.
        public TMP_Text opponentHealthText;

        // Has the text scroll for the opponent health.
        private bool opponentHealthTransitioning = false;

        // Start is called before the first frame update
        void Start()
        {
            // // enemy base not set, so make a base.
            // if(enemyBase == null)
            // {
            //     GameObject go = new GameObject("Enemy Base");
            //     enemyBase = go.AddComponent<Enemy>();
            //     go.transform.parent = gameObject.transform;
            // }
            // 
            // // treasure base not set, so make a base.
            // if (treasureBase == null)
            // {
            //     GameObject go = new GameObject("Treasure Base");
            //     treasureBase = go.AddComponent<Treasure>();
            //     go.transform.parent = gameObject.transform;
            // }
            // 
            // // enemy base not set, so make a base.
            // if (bossBase == null)
            // {
            //     GameObject go = new GameObject("Boss Base");
            //     bossBase = go.AddComponent<Boss>();
            //     go.transform.parent = gameObject.transform;
            // }

            // Gets the starting turns and health of the player for score calculation.
            turnsTaken = 0;
            playerDamageTaken = 0;

            // The language definitions.
            JSONNode defs = SharedState.LanguageDefs;

            // Turns off the bases.
            enemyBase.gameObject.SetActive(false);
            treasureBase.gameObject.SetActive(false);
            bossBase.gameObject.SetActive(false);

            // Initializes the list.
            turnText = new List<Page>();

            // When the textbox disappears the turn is over, so call this function.
            textBox.OnTextBoxFinishedAddCallback(OnTurnOver);

            // Close the textbox when the player is done.
            textBox.closeOnEnd = true;

            // Hide prompt.
            treasurePrompt.gameObject.SetActive(false);

            // Turns off the learn move panel.
            learnMovePanel.windowObject.SetActive(false);

            // Charge (does it this way for translation)
            chargeButtonText.text = MoveList.Instance.ChargeMove.Name;

            // Run (does it this way for translation)
            runButtonText.text = MoveList.Instance.RunMove.Name;

            // The defs are not set.
            if(defs != null)
            {
                // Translate the treasure prompt.
                treasurePromptText.text = defs["btl_msg_treasure"];
                treasureYesButtonText.text = defs["kwd_yes"];
                treasureNoButtonText.text = defs["kwd_no"];
            }
        }

        // This function is called when the object becomes enabled and active
        private void OnEnable()
        {
            if (ui != null)
                ui.SetActive(true);
        }

        // This function is called when the behaviour becomes disabled or inactive
        private void OnDisable()
        {
            if (ui != null)
                ui.SetActive(false);
        }

        // Initializes the overworld.
        public override void Initialize()
        {
            initialized = true;

            // Sets the battle entity from the door.
            // opponent = null; // TODO: comment out.

            // Reset the stat modifiers and statuses before the battle starts.
            player.ResetStatModifiers();
            player.ResetStatuses();

            // Checks the type of entity.
            switch (door.battleEntity.id)
            {
                case battleEntityId.treasure: // treasure
                    opponent = treasureBase;
                    break;
                case battleEntityId.boss: // boss
                    opponent = bossBase;
                    break;
                default: // enemy
                    opponent = enemyBase;
                    break;

            }

            // Opponent has been set.
            if (opponent != null)
            {
                // Saves the opponent's name.
                opponentNameText.text = opponent.displayName;

                // Loads the battle data
                opponent.LoadBattleGameData(door.battleEntity);

                // SPRITE
                opponentSprite.sprite = opponent.sprite;
                opponentSprite.gameObject.SetActive(true);

                // Resets the stat modifiers and statuses.
                opponent.ResetStatModifiers();
                opponent.ResetStatuses();
            }
            else
            {
                // opponent name.
                opponentNameText.text = "-";
            }


            // Checks move activity to see if the player can use it or not.
            // Also changes the move name on the display.

            // Move 0
            // move0Button.interactable = player.Move0 != null;
            move0NameText.text = (player.Move0 != null) ? player.Move0.Name : "-";

            // Move 1
            // move1Button.interactable = player.Move1 != null;
            move1NameText.text = (player.Move1 != null) ? player.Move1.Name : "-";

            // Move 2
            // move2Button.interactable = player.Move2 != null;
            move2NameText.text = (player.Move2 != null) ? player.Move2.Name : "-";

            // Move 3
            // move3Button.interactable = player.Move3 != null;
            move3NameText.text = (player.Move3 != null) ? player.Move3.Name : "-";

            // Updates the UI so that the move accuracies show.
            UpdatePlayerMoveAccuracies();

            // Checks if the player has a full charge.
            // chargeButton.interactable = !player.HasFullCharge();

            // Changes the 'interactable' toggle for the buttons.
            // If the tutorial window is visible then don't refresh the player options.
            if (gameManager.tutorial.textBox.IsVisible()) // Disable the options if the tutorial box is open.
                DisablePlayerOptions();
            else // Refresh the options of the tutorial box is not open.
                RefreshPlayerOptions();

            // Updates the interface.
            UpdateOpponentUI();

            // If the opponent is a treasure box.
            if (opponent is Treasure)
            {
                // The player has no battle options since this isn't a fight.
                DisablePlayerOptions();

                // If the tutorial box isn't visible, turn on the treasure buttons.
                if (!gameManager.tutorial.textBox.IsVisible())
                {
                    treasureYesButton.interactable = true;
                    treasureNoButton.interactable = true;
                }
                
                // Hide health bar and health text
                opponentHealthBar.bar.gameObject.SetActive(false);
                opponentHealthText.gameObject.SetActive(false);

                // Show treasure prompt.
                treasurePrompt.gameObject.SetActive(true);
            }
            else
            {
                // Show health bar and health text
                opponentHealthBar.bar.gameObject.SetActive(true);
                
                // Set value.
                opponentHealthBar.SetValue(opponent.Health / opponent.MaxHealth, false);

                // Show game text.
                opponentHealthText.gameObject.SetActive(true);

                // Hide treasure prompt.
                treasurePrompt.gameObject.SetActive(false);
            }

            // Set initial showing of health.
            opponentHealthText.text = opponent.Health.ToString() + "/" + opponent.MaxHealth.ToString();

            // The battle has begun.
            initBattleEnd = false;

            // Do not autosave unless the player actually wins.
            autoSaveOnExit = false;
        }

        // Called when the mouse hovers over an object.
        public override void OnMouseHovered(GameObject hoveredObject)
        {
            throw new System.NotImplementedException();
        }

        // Called when the mouse interacts with an entity.
        public override void OnMouseInteract(GameObject heldObject)
        {

        }

        // Called when the user's touch interacts with an entity.
        public override void OnTouchInteract(GameObject touchedObject, Touch touch)
        {

        }

        // Called with the object that was received with the interaction.
        protected override void OnInteractReceive(GameObject gameObject)
        {
            throw new System.NotImplementedException();
        }

        // A function to call when a tutorial starts.
        public override void OnTutorialStart()
        {
            DisablePlayerOptions();
        }

        // A function to call when a tutorial ends.
        public override void OnTutorialEnd()
        {
            RefreshPlayerOptions();
        }

        // Sets player controls to interactable or not. RefreshPlayerOptions is also called to disable buttons that do nothing. 
        public void SetPlayerOptionsAvailable(bool interactable)
        {
            move0Button.interactable = interactable;
            move1Button.interactable = interactable;
            move2Button.interactable = interactable;
            move3Button.interactable = interactable;

            chargeButton.interactable = interactable;
            runButton.interactable = interactable;

            // Changes the interaction for the treasure buttons.
            treasureYesButton.interactable = interactable;
            treasureNoButton.interactable = interactable;

            // If all were turned on, check to see if some should stay off.
            if (interactable)
                RefreshPlayerOptions();
        }

        // Enables the player options.
        public void EnablePlayerOptions()
        {
            SetPlayerOptionsAvailable(true);
        }

        // Disables the player options.
        public void DisablePlayerOptions()
        {
            SetPlayerOptionsAvailable(false);
        }

        public void RefreshPlayerOptions()
        {
            // Checks move activity to see if the player can use it or not.
            // Also changes the move name on the display.

            // Enables/disables various buttons.

            // Move 0 
            if (player.Move0 != null)
                move0Button.interactable = player.Move0.Energy <= player.Energy;
            else
                move0Button.interactable = false;

            // Move 1
            if (player.Move1 != null)
                move1Button.interactable = player.Move1.Energy <= player.Energy;
            else
                move1Button.interactable = false;

            // Move 2 
            if (player.Move2 != null)
                move2Button.interactable = player.Move2.Energy <= player.Energy;
            else
                move2Button.interactable = false;

            // Move 3
            if (player.Move3 != null)
                move3Button.interactable = player.Move3.Energy <= player.Energy;
            else
                move3Button.interactable = false;

            // Updates the move accuracy displays.
            UpdatePlayerMoveAccuracies();

            // Checks if the player has a full charge.
            chargeButton.interactable = !player.HasFullCharge();

            // Enable the run.
            runButton.interactable = true;

            // The buttons are interactable, though they are only visible in a treasure room.
            treasureYesButton.interactable = true;
            treasureNoButton.interactable = true;
        }

        // Updates the player move accuracies for the battle state.
        public void UpdatePlayerMoveAccuracies()
        {
            // Moves won't show accuracy if they don't use the accuracy parameter.
            // Saves the accuracy (0-1 range).
            float accuracy = 0;

            // This techincally calculates accuracy regardless of if it's used, but I don't think it's a big deal.
            // Move 0
            if (player.Move0 != null)
            {
                // Calculates the accuracy to display.
                accuracy = Mathf.Ceil(Mathf.Clamp01(player.Move0.Accuracy * player.accuracyMod) * 100.0F);

                // Slots in the text.
                move0AccuracyText.text = (player.Move0.useAccuracy) ? accuracy.ToString() + "%" : "-";
            }
            else
            {
                move0AccuracyText.text = "-";
            }

            // Move 1
            if (player.Move1 != null)
            {
                // Calculates the accuracy to display.
                accuracy = Mathf.Ceil(Mathf.Clamp01(player.Move1.Accuracy * player.accuracyMod) * 100.0F);

                // Slots in the text.
                move1AccuracyText.text = (player.Move1.useAccuracy) ? accuracy.ToString() + "%" : "-";
            }
            else
            {
                move1AccuracyText.text = "-";
            }

            // Move 2
            if (player.Move2 != null)
            {
                // Calculates the accuracy to display.
                accuracy = Mathf.Ceil(Mathf.Clamp01(player.Move2.Accuracy * player.accuracyMod) * 100.0F);

                // Slots in the text.
                move2AccuracyText.text = (player.Move2.useAccuracy) ? accuracy.ToString() + "%" : "-";
            }
            else
            {
                move2AccuracyText.text = "-";
            }

            // Move 3
            if (player.Move3 != null)
            {
                // Calculates the accuracy to display.
                accuracy = Mathf.Ceil(Mathf.Clamp01(player.Move3.Accuracy * player.accuracyMod) * 100.0F);

                // Slots in the text.
                move3AccuracyText.text = (player.Move3.useAccuracy) ? accuracy.ToString() + "%" : "-";
            }
            else
            {
                move3AccuracyText.text = "-";
            }


        }

        // Updates the battle visuals.
        // If 'playerTurn' is true, then the update is coming from the player's turn.
        // If false, it's coming from the enemy's turn.
        private void AddVisualUpdateCallbacks(bool playerTurn)
        {
            // If there are pages to attach callbacks too.
            if (turnText.Count > 0)
            {
                if (playerTurn) // Player turn
                {
                    turnText[turnText.Count - 1].OnPageClosedAddCallback(UpdateOpponentUI);
                    turnText[turnText.Count - 1].OnPageClosedAddCallback(gameManager.UpdateUI);
                }
                else
                {
                    // turnText[turnText.Count - 1].OnPageClosedAddCallback(gameManager.UpdatePlayerHealthUI);
                    turnText[turnText.Count - 1].OnPageClosedAddCallback(gameManager.UpdateUI);
                }
            }
        }

        // Apply burn to the player.
        private void ApplyPlayerBurn()
        {
            if (player.burned)
                player.ApplyBurn(this);
        }

        // Apply burn to the opponent.
        private void ApplyOpponentBurn()
        {
            if (opponent.burned)
                opponent.ApplyBurn(this);
        }

        // Called to perform the player's move.
        private void PerformPlayerMove()
        {
            player.selectedMove.Perform(player, opponent, this);
        }

        // Called to perform the opponent's move.
        private void PerformOpponentMove()
        {
            opponent.selectedMove.Perform(opponent, player, this);
        }

        // Performs the two moves.
        public void PerformMoves()
        {
            // Both sides have selected a move.
            if (player.selectedMove != null && opponent.selectedMove != null)
            {
                // Checks who goes first.
                bool playerFirst = false;

                // TODO: account for status effects.

                // Clears out the past text.
                turnText.Clear();


                // If one of the moves have priority.
                if (player.selectedMove.priority != opponent.selectedMove.priority)
                {
                    // Save priority result.
                    playerFirst = player.selectedMove.priority > opponent.selectedMove.priority;
                }
                else // Player is not trying to run.
                {
                    // Checks the fastest entity.
                    int fastest = BattleEntity.GetFastestEntity(player, opponent, this);

                    // Checks the variable.
                    switch (fastest)
                    {
                        case 1: // player 1st
                            playerFirst = true;
                            break;
                        case 2: // player second
                            playerFirst = false;
                            break;
                        default: // random
                            playerFirst = Random.Range(0, 2) == 1;
                            break;
                    }
                }

                // ADD TURN PAGES

                // Loads the selected moves.
                // The two pages for the player and the opponent.
                bool turnSkip = false;

                // PLAYER
                // Checks if the player is paralyzed.
                if(player.paralyzed)
                {
                    // If turn should be skipped.
                    turnSkip = Random.Range(0.0F, 1.0F) <= PARALAYSIS_SKIP_CHANCE;
                }
                else
                {
                    turnSkip = false;
                }

                // Checks if the player's turn should be skipped.
                if(turnSkip)
                {
                    // Skip
                    playerMovePage = new Page(
                        BattleMessages.Instance.GetParalyzedMessage(player.displayName),
                        BattleMessages.Instance.GetParalyzedSpeakKey0()
                        );
                }
                else
                {
                    // Adds the player's move.
                    playerMovePage = new Page(
                        BattleMessages.Instance.GetMoveUsedMessage(player.displayName, player.selectedMove.Name),
                        BattleMessages.Instance.GetMoveUsedSpeakKey0()
                        );

                    playerMovePage.OnPageOpenedAddCallback(PerformPlayerMove);
                }

                // OPPONENT
                if (opponent.paralyzed)
                {
                    // If turn should be skipped.
                    turnSkip = Random.Range(0.0F, 1.0F) <= PARALAYSIS_SKIP_CHANCE;
                }
                else
                {
                    turnSkip = false;
                }

                // Checks if the opponent's turn should be skipped.
                if(turnSkip)
                {
                    // Skip
                    opponentMovePage = new Page(
                        BattleMessages.Instance.GetParalyzedMessage(opponent.displayName),
                        BattleMessages.Instance.GetParalyzedSpeakKey1());
                }
                else // Don't skip.
                {
                    // Adds the opponent's move.
                    opponentMovePage = new Page(
                        BattleMessages.Instance.GetMoveUsedMessage(opponent.displayName, opponent.selectedMove.Name),
                        BattleMessages.Instance.GetMoveUsedSpeakKey1());
                    opponentMovePage.OnPageOpenedAddCallback(PerformOpponentMove);
                }
                

                // Places the pages in order.
                if (playerFirst)
                {
                    turnText.Add(playerMovePage);
                    turnText.Add(opponentMovePage);
                }
                else
                {
                    turnText.Add(opponentMovePage);
                    turnText.Add(playerMovePage);
                }


                // Burn Pages
                {
                    // Burn pages.
                    Page pBurnPage = null, oBurnPage = null;

                    // If the player is burned.
                    if (player.burned)
                    {
                        pBurnPage = new Page(
                            BattleMessages.Instance.GetBurnedMessage(player.displayName),
                            BattleMessages.Instance.GetBurnedSpeakKey0()
                            );
                        pBurnPage.OnPageOpenedAddCallback(ApplyPlayerBurn);
                    }

                    // If the opponent is burned.
                    if(opponent.burned)
                    {
                        oBurnPage = new Page(
                            BattleMessages.Instance.GetBurnedMessage(opponent.displayName),
                            BattleMessages.Instance.GetBurnedSpeakKey1()
                            );
                        oBurnPage.OnPageOpenedAddCallback(ApplyOpponentBurn);
                    }

                    // Burns based on who moves first.
                    if(pBurnPage != null && oBurnPage == null) // Only player is burned.
                    {
                        turnText.Add(pBurnPage);
                    }
                    else if (pBurnPage == null && oBurnPage != null) // Only opponent is burned.
                    {
                        turnText.Add(oBurnPage);
                    }
                    else if (pBurnPage != null && oBurnPage != null) // Both are burned.
                    {
                        if(playerFirst) // Player is fastest.
                        {
                            turnText.Add(pBurnPage);
                            turnText.Add(oBurnPage);
                        }
                        else // Opponent is fastest.
                        {
                            turnText.Add(oBurnPage);
                            turnText.Add(pBurnPage);
                        }
                        
                    }
                }


                // Show the textbox.
                // TODO: hide player move controls.
                textBox.ReplacePages(turnText);
                textBox.Open();

                // Disable the player options since the textbox is open.
                DisablePlayerOptions();

                // Adds to the amount of turns the battle has taken.
                turnsTaken++;

                // Add to the total turns counter.
                gameManager.turnsPassed++;
            }
            else
            {
                // Gets the moves from the player and the opponent.
                player.OnBattleTurn(); // does nothing right now.

                // opponent.
                opponent.OnBattleTurn(); // calculates next move (evenutally)
            }
        }


        // Called when the player attempts to run away. TODO: have the enemy's move still go off if the run fails.
        public void RunAway()
        {
            // Becomes 'true' if the run attempt was successful.
            bool success = false;

            // Overrides the selected move.
            player.selectedMove = MoveList.Instance.RunMove;

            // If there's no opponent then the player can always run away.
            if (opponent == null)
            {
                success = true;
            }
            // If there is an opponent there the player may be unable to leave.
            else
            {
                // There's a 1/2 chance of running away.
                success = (Random.Range(0, 2) == 1);
            }

            // Returns to the overworld if the run was successful.
            if (success)
            {
                ToOverworld();
            }
            else
            {
                // Debug.Log("Run failed.");
            }

            // Returns the success of the operation.
            // return success;
                
        }

        // TODO: this doesn't work. You should fix this.
        // // Ends the turn early.
        // public void EndTurnEarly()
        // {
        //     // Removes the rest of the pages so that the turn ends early.
        //     List<Page> savedPages = textBox.pages;
        //     savedPages.RemoveRange(textBox.CurrentPageIndex + 1, savedPages.Count - textBox.CurrentPageIndex - 1);
        //     textBox.pages = savedPages;
        // }

        // Called when the turn is over.
        private void OnTurnOver()
        {
            player.selectedMove = null;
            opponent.selectedMove = null;

            playerMovePage = null;
            opponentMovePage = null;

            EnablePlayerOptions();
        }

        // Calculates the score for winning the battle.
        public int CalculateBattleScore()
        {
            // Result variable.
            int result = 0;

            // The par for turns taken.
            int turnsTakenPar = 5;

            // The par for damage taken. This is 75% of the player's current max health.
            int damageTakenPar = Mathf.RoundToInt(player.MaxHealth * 0.75F);

            // Room Complete Plus
            // Checks the type of the opponent for providing the base amount.
            if(opponent is Boss) // Opponent was a boss.
            {
                result = 300; // High
            }
            else if(opponent is Treasure) // Opponent was treasure.
            {
                result = 50; // Low
            }
            else // The opponent was a regular enemy.
            {
                result = 100; // Mid
            }

            // Turns Taken Bonus
            if(turnsTaken <= turnsTakenPar) // Took the expected amount of turns or less.
            {
                result += 100 * turnsTakenPar - turnsTaken;
            }

            // Damage Taken Bonus
            if(playerDamageTaken <= damageTakenPar) // Took the expected amount of damage or less.
            {
                result += Mathf.RoundToInt(100 * (damageTakenPar - playerDamageTaken));
            }

            return result;
        }

        // Called when the player has won the battle.
        public void OnPlayerBattleWon()
        {
            textBox.OnTextBoxFinishedRemoveCallback(OnPlayerBattleWon);
            
            // Moved so that an update message can be posted on screen.
            // player.LevelUp();

            // Completed a Battle
            gameManager.roomsCompleted++;

            // Calculates the score for completing the round, and adds it to the game score.
            gameManager.score += CalculateBattleScore();

            // Submit the player's current progress now that the roomsCompleted and score have been updated.
            gameManager.SubmitProgress();

            // The door is now locked since the room is cleared.
            door.Locked = true;

            // Auto save before going to the overworld.
            autoSaveOnExit = true;

            // TODO: check and see if you need to move this.
            // Auto save the game.
            if(autoSaveOnExit)
            {
                // Save and continue the game.
                gameManager.SaveAndContinueGame();
            }

            // Go to the overworld.
            ToOverworld();
        }

        // Called when the player has lost the battle.
        public void OnPlayerBattleLost()
        {
            textBox.OnTextBoxFinishedRemoveCallback(OnPlayerBattleLost);
            gameManager.OnGameOver();
            ToOverworld();
        }

        // Call this function to open the treasure.
        public void OpenTreasure()
        {
            // Hide prompt.
            treasurePrompt.gameObject.SetActive(false);

            // The "battle" is over.
            opponent.Health = 0;
        }

        // Call this function to leave the treasure.
        public void LeaveTreasure()
        {
            // Hide prompt.
            treasurePrompt.gameObject.SetActive(false);

            // Return to the overworld.
            ToOverworld();
        }

        // Called when potentially learning a new move.
        public void OnLearningNewMove()
        {
            /// <summary>
            /// TODO: there are times where the player gets prompted to learn a move twice.
            /// The first time is the accidental one, and appears over the 'player won' message.
            /// I don't know why this is happening.
            /// My assumption is that it's doing it either based on the last page closing...
            /// Or the current page opening, but I don't know which.
            /// Maybe try removing the OnLearningNewMove callback from both pages, but I don't know...
            /// Why they would have these callbacks in the first place. As such, I don't know if that...
            /// Would fix it. I don't think I've even tried to learn a new move twice...
            /// Nor do I know if it's exactly the same message.
            /// I'm going to leave it for now, but this is an error that NEEDS TO BE FIXED.
            /// ---
            /// From what I can gather, it seems to only happen on the second battle won.
            /// I don't know if clearing a treasure room before it (thus making it not the second room)...
            /// Fixes this.
            /// I also think it might only happen in the tutorial, but I'm not sure, and I don't...
            /// Know why that would be.
            /// </summary>

            // NEW
            // It appears to happen before and after the player levels up.
            // It also generates a new move for the player to learn.

            // Hide the box gameobject.
            textBox.Close();

            // TODO: implement new move learning.
            // The phase.
            int phase = gameManager.GetGamePhase();

            // The new move.
            Move newMove;

            // Checks the phase.
            switch (phase)
            {
                case 1: // beginning - 1
                    newMove = MoveList.Instance.GetRandomRank1Move();
                    break;
                case 2: // middle - 2
                    newMove = MoveList.Instance.GetRandomRank2Move();
                    break;
                case 3: // end - 3
                default:
                    newMove = MoveList.Instance.GetRandomRank3Move();
                    break;
            }

            // If the player already has this move.
            if (player.HasMove(newMove))
            {
                // Becomes 'true' when the new move is set.
                bool newMoveFound = false;

                // Attempts at generating a new move.
                int attempts = 5;

                // Tries to generate a new move that the player doesn't have 5 times.
                for (int n = 0; n < attempts; n++)
                {
                    // Grabs a new move.
                    newMove = MoveList.Instance.GetRandomMove();
                    newMoveFound = player.HasMove(newMove);

                    // If a new move was found already, break early.
                    if (newMoveFound)
                        break;
                }

                // If the new move was still the same, well then the player will have the chance to get multiples of the same move.
                // This shouldn't happen, but it likely won't happen.

            }

            // If the player has less than 4 moves, automatically learn the move.
            if (player.GetMoveCount() < 4)
            {
                for (int i = 0; i < player.moves.Length; i++)
                {
                    if (player.moves[i] == null)
                    {
                        player.moves[i] = newMove;
                        break;
                    }
                }

                // Remove this callback (DOESN'T WORK)
                // textBox.CurrentPage.OnPageClosedRemoveCallback(OnLearningNewMove);

                // Removes the placeholder page.
                textBox.pages.RemoveAt(textBox.CurrentPageIndex + 1);

                // Inserts a new page.
                textBox.pages.Insert(textBox.CurrentPageIndex + 1, new Page(
                    BattleMessages.Instance.GetLearnMoveYesMessage(newMove.Name),
                    BattleMessages.Instance.GetLearnMoveYesSpeakKey()));

                // Go onto the next page.
                textBox.Open();
                textBox.NextPage();
            }
            else
            {

                // Update the information.
                learnMovePanel.newMove = newMove;
                learnMovePanel.LoadMoveInformation(); // Happens on enable.

                // Turn on the move panel, which also updates the move list.
                learnMovePanel.windowObject.SetActive(true);
            }
        }

        // Goes to the overworld.
        public void ToOverworld()
        {
            // Clear out the textbox.
            if (textBox.IsVisible())
            {
                textBox.Close();
                textBox.pages.Clear();
            }

            // Player options should be enabled before leaving so that they're ready for the next battle?
            EnablePlayerOptions();

            // Remove stat changes and status effects
            // This already happens in the initialization phase, but it happens here just to be sure. 
            // TODO: maybe take this out?
            player.ResetStatModifiers();
            player.ResetStatuses();

            // Save battle entity data.
            door.battleEntity = opponent.GenerateBattleEntityGameData();

            // Hide opponent sprite.
            opponentSprite.gameObject.SetActive(false);

            // Go to the overworld.
            gameManager.UpdateUI();
            gameManager.EnterOverworld();
        }

        // Updates all UI elements.
        public void UpdateUI()
        {
            RefreshPlayerOptions();
            UpdateOpponentUI();
        }

        // Updates the opponent AI.
        public void UpdateOpponentUI()
        {
            opponentHealthBar.SetValue(opponent.Health / opponent.MaxHealth);

            // If false, the text is changed instantly. If true, the text is not updated here.
            // This prevents the final number from flashing for a frame.
            if (!gameManager.syncTextToBars)
                opponentHealthText.text = opponent.Health.ToString() + "/" + opponent.MaxHealth.ToString();
        }

        // Update is called once per frame
        void Update()
        {
            // If the text box is not visible.
            if (!textBox.IsVisible())
            {
                // If both entities are alive do battle calculations.
                if (player.Health > 0 && opponent.Health > 0)
                {
                    // If the opponent isn't a treasure chest try to perform moves.
                    if (!(opponent is Treasure))
                        PerformMoves();
                }
                else
                {
                    // Checks if the battle end has been initialized.
                    if (!initBattleEnd)
                    {
                        // Returns to the overworld. TODO: account for game over.
                        // The player got a game over.
                        if (player.Health <= 0) // game over
                        {
                            // Restores the opponent's health to max (stops both from dying on the same round.
                            opponent.Health = opponent.MaxHealth;

                            textBox.pages.Clear();
                            textBox.pages.Add(new Page(
                                BattleMessages.Instance.GetBattleLostMessage(),
                                BattleMessages.Instance.GetBattleLostSpeakKey()
                                ));
                            textBox.SetPage(0);
                            textBox.OnTextBoxFinishedAddCallback(OnPlayerBattleLost);

                            DisablePlayerOptions();
                            textBox.Open();
                        }
                        else // The player won the fight.
                        {
                            textBox.pages.Clear();

                            // Checks the opponent type.
                            if (opponent is Treasure) // Is Treasure
                            {
                                textBox.pages.Add(new Page(
                                    BattleMessages.Instance.GetOpenTreasureMessage(),
                                    BattleMessages.Instance.GetOpenTreasureSpeakKey()
                                    ));
                            }
                            else if(opponent is Boss) // Final boss beaten.
                            {
                                // TODO: test this.
                                // Boss page and callback.
                                Page bossPage = new Page(
                                    BattleMessages.Instance.GetBattleWonBossMessage(),
                                    BattleMessages.Instance.GetBattleWonBossSpeakKey()
                                    );
                                bossPage.OnPageClosedAddCallback(gameManager.ToResultsScene);

                                // Adds the boss page. 
                                textBox.pages.Add(bossPage);
                                textBox.pages.Add(new Page("..."));
                            }
                            else // Not Treasure
                            {
                                textBox.pages.Add(new Page(
                                    BattleMessages.Instance.GetBattleWonMessage(),
                                    BattleMessages.Instance.GetBattleWonSpeakKey()));
                            }

                            // Levels up the player.
                            {
                                // The temporary page object.
                                Page tempPage;

                                // Level up message.
                                tempPage = new Page(
                                    BattleMessages.Instance.GetLevelUpMessage(),
                                    BattleMessages.Instance.GetLevelUpSpeakKey()
                                    );

                                // Add the page to the list.
                                textBox.pages.Add(tempPage);

                                // Update the UI when this page has been displayed.
                                tempPage.OnPageOpenedAddCallback(gameManager.UpdateUI);

                                // Saves the old stats.
                                float oldMaxHp = player.MaxHealth;
                                float oldAtk = player.Attack;
                                float oldDef = player.Defense;
                                float oldSpd = player.Speed;
                                float oldMaxEng = player.MaxEnergy;

                                // Levels up te player.
                                player.LevelUp();

                                // Adds page with the increases in stats.
                                textBox.pages.Add(new Page(
                                    gameManager.HealthString + " +" + Mathf.RoundToInt(player.MaxHealth - oldMaxHp).ToString() + "   |   " +
                                    gameManager.AttackString + " +" + Mathf.RoundToInt(player.Attack - oldAtk).ToString() + "   |   " +
                                    gameManager.DefenseString + " +" + Mathf.RoundToInt(player.Defense - oldDef).ToString() + "   |   \n" +
                                    gameManager.SpeedString + " +" + Mathf.RoundToInt(player.Speed - oldSpd).ToString() + "   |   " +
                                    gameManager.EnergyString + " +" + Mathf.RoundToInt(player.MaxEnergy - oldMaxEng).ToString()
                                    ));

                                // Adds page with new stats.
                                textBox.pages.Add(new Page(
                                    gameManager.LevelString + " = " + player.Level.ToString() + " | " +
                                    gameManager.HealthString + " = " + Mathf.RoundToInt(player.MaxHealth).ToString() + "   |   " +
                                    gameManager.AttackString + " = " + Mathf.RoundToInt(player.Attack).ToString() + "   |   \n" +
                                    gameManager.DefenseString + " = " + Mathf.RoundToInt(player.Defense).ToString() + "   |   " +
                                    gameManager.SpeedString + " = " + Mathf.RoundToInt(player.Speed).ToString() + "   |   " +
                                    gameManager.EnergyString + " = " + Mathf.RoundToInt(player.MaxEnergy).ToString()
                                    ));

                                
                            }

                            // Checks to see if a new move should be learned.
                            bool learnMove = (Random.Range(0.0F, 1.0F) <= NEW_MOVE_CHANCE || opponent is Treasure);

                            // If this is the tutorial, and it's the first room cleared, always give the player a new move.
                            if(learnMove == false)
                            {
                                // Learn a new move.
                                if (gameManager.useTutorial && gameManager.roomsCompleted == 0)
                                    learnMove = true;
                            }
                            

                            // Checks to see if the player will be learning a new move.
                            // If the opponet was a treasure box the player will always get the chance to learn a new move.
                            if (learnMove)
                            {
                                Page movePage = new Page(
                                    BattleMessages.Instance.GetLearnMoveMessage(),
                                    BattleMessages.Instance.GetLearnMoveSpeakKey()
                                    );

                                movePage.OnPageClosedAddCallback(OnLearningNewMove);
                                textBox.pages.Add(movePage);

                                // Placeholder page.
                                textBox.pages.Add(new Page("..."));
                            }

                            // Set up the textbox.
                            textBox.SetPage(0);
                            textBox.OnTextBoxFinishedAddCallback(OnPlayerBattleWon);

                            DisablePlayerOptions();
                            textBox.Open();
                        }

                        // The battle end has been initialized.
                        initBattleEnd = true;
                    }

                }
            }

            // If the text should match the bars.
            if(gameManager.syncTextToBars)
            {
                // Opponent Health Scrolling Text
                // Checks if the opponent health bar is transitioning.
                if (opponentHealthBar.IsTransitioning())
                {
                    opponentHealthText.text = Mathf.Round(opponentHealthBar.GetSliderValueAsPercentage() * opponent.MaxHealth).ToString() + "/" +
                        opponent.MaxHealth.ToString();

                    // The health is transitioning.
                    opponentHealthTransitioning = true;
                }
                else if (opponentHealthTransitioning) // Transition done.
                {
                    // Set to exact value.
                    opponentHealthText.text = opponent.Health.ToString() + "/" + opponent.MaxHealth.ToString();

                    opponentHealthTransitioning = false;
                }
            }
            

        }


    }
}