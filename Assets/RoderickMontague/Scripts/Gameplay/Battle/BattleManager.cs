using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SimpleJSON;
using UnityEngine.SceneManagement;
using System.Data.Common;
using LoLSDK;

namespace RM_BBTS
{
    // Manages the battle operations for the game. This becomes active when the game enters a battle state.
    public class BattleManager : GameState
    {
        // Becomes 'true' when the overworld is initialized.
        private bool initialized = false;

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

        // The burn damage amount (1/16 damage).
        public const float BURN_DAMAGE = 0.0625F;

        // The chance to skip a turn if paralyzed.
        public const float PARALAYSIS_SKIP_CHANCE = 0.4F;

        // The chance of learning a new move.
        private float NEW_MOVE_CHANCE = 0.80F;

        // The chance of the randomly learned move being a random rank.
        private float RANDOM_RANK_MOVE_CHANCE = 0.05F;

        // Becomes 'true' when the battle end state has been initialized.
        private bool initBattleEnd = false;

        // Gets set to 'true' when a move gets a critical hit during a battle.
        // This is used to trigger the critical tutorial.
        [HideInInspector]
        public bool gotCritical = false;

        // Gets set to 'true' when a move gets recoil during a battle.
        // This is used to trigger the recoil tutorial.
        [HideInInspector]
        public bool gotRecoil = false;

        // The order of the move, which refers to what place the current move is in.
        [HideInInspector]
        public int order = 0;

        // Gets set to 'true' when the player has learned a move at the end of the battle.
        [HideInInspector]
        private bool learnedMove = false;

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

        // The treasure prompt text key for text-to-speech.
        private string treasurePromptTextKey = "btl_msg_treasure";

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

        [Header("Audio")]
        // Battle bgm.
        public AudioClip battleBgm;

        // The boss BGM.
        public AudioClip bossBgm;

        // The jingle for winning a battle.
        public AudioClip battleWonJng;

        // The jingle for losing a battle.
        public AudioClip battleLostJng;

        // The sound effect for the player taking damage.
        public AudioClip playerHurtSfx;

        // The sound effect for the opponent taking damage.
        public AudioClip opponentHurtSfx;

        // THe sound efect for a non-damaging move.
        public AudioClip moveEffectSfx;

        // The burn sound effect.
        public AudioClip burnSfx;

        // The paralysis sound effect.
        public AudioClip paralysisSfx;

        [Header("Animations")]
        // The player's animator.
        public Animator playerAnimator;

        // The image for the player animation (is recoloured as needed).
        public Image playerAnimationImage;

        // The opponent's animator.
        public Animator opponentAnimator;

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
                // treasurePromptTextKey = "btl_msg_treasure"; // Set by default so that the text-to-speech can use it.
                treasurePromptText.text = defs[treasurePromptTextKey];
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
            // TODO: this variable isn't being reset like it should, and I don't know why.
            // It works fine without this check though, so I'm taking it out.

            // The battle has already been initialized.
            // The tutorial box causes this function to be called over and over again. This should stop things from resetting.
            if(initialized)
            {
                // Debug.LogAssertion("The battle has already been initialized.");
                return;
            }

            // In the battle state.
            gameManager.SetStateToBattle();

            // Remove all pages just to be safe.
            textBox.ClearPages();

            // Sets the battle entity from the door.
            // opponent = null; // TODO: comment out.

            // Reset the stat modifiers and statuses before the battle starts.
            player.selectedMove = null;
            player.ResetStatModifiers();
            player.ResetStatuses();

            // Checks to see what type of entity is being faced.
            if(door.isBossDoor) // Boss
            {
                opponent = bossBase;
            }
            else if(door.isTreasureDoor) // Treasure
            {
                opponent = treasureBase;
            }
            else // Enemy
            {
                opponent = enemyBase;
            }

            // Opponent has been set.
            if (opponent != null)
            {
                opponent.selectedMove = null;

                // Loads the battle data
                opponent.LoadBattleGameData(door.battleEntity);

                // Saves the opponent's name.
                opponentNameText.text = opponent.displayName;

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
                    // Enable the buttons for answering the question.
                    treasureYesButton.interactable = true;
                    treasureNoButton.interactable = true;

                    // Reads out the treasure prompt if the treasure tutorial isn't being shown.
                    if(LOLSDK.Instance.IsInitialized && GameSettings.Instance.UseTextToSpeech && treasurePromptTextKey != "")
                    {
                        // Speaks the text for the treasure prompt.
                        LOLManager.Instance.textToSpeech.SpeakText(treasurePromptTextKey);
                    }

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

            // Plays the BGM based on the opponent.
            if(opponent is Boss)
            {
                // Boss BGM.
                PlayBossBgm();
            }
            else if(opponent is Treasure)
            {
                // Treasure BGM
                PlayTreasureBgm();
            }   
            else
            {
                // Battle BGM.
                PlayBattleBgm();
            }

            // The battle has begun.
            initBattleEnd = false;

            // Do not autosave unless the player actually wins.
            autoSaveOnExit = false;

            // No moves have been performed.
            order = 0;

            // No move has neen learned for this battle.
            learnedMove = false;

            // The battle has been initialized.
            initialized = true;
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

        // Retunrs 'true' if the overworld is initialized.
        public bool Initialized
        {
            get { return initialized; }
        }

        // Gets the amount of turns the battle has taken.
        public float TurnsTaken
        {
            get { return turnsTaken; }
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
            if (player.Move0 != null && !(opponent is Treasure))
                move0Button.interactable = player.Move0.Usable(player);
            else
                move0Button.interactable = false;

            // Move 1
            if (player.Move1 != null && !(opponent is Treasure))
                move1Button.interactable = player.Move1.Usable(player);
            else
                move1Button.interactable = false;

            // Move 2 
            if (player.Move2 != null && !(opponent is Treasure))
                move2Button.interactable = player.Move2.Usable(player);
            else
                move2Button.interactable = false;

            // Move 3
            if (player.Move3 != null && !(opponent is Treasure))
                move3Button.interactable = player.Move3.Usable(player);
            else
                move3Button.interactable = false;

            // Updates the move accuracy displays.
            UpdatePlayerMoveAccuracies();

            // Checks if the player has a full charge.
            chargeButton.interactable = !player.HasFullCharge() && !(opponent is Treasure);

            // Enable the run option.
            // If this is the tutorial battle for the game, the run option is disabled.
            if((gameManager.useTutorial && gameManager.roomsCompleted == 0) || opponent is Treasure)
                runButton.interactable = false;
            else
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
            // The accuracy can be outside of the [0, 1] bounds, but the display won't do that.
            float accuracy = 0;

            // This techincally calculates accuracy regardless of if it's used, but I don't think it's a big deal.
            // Move 0
            if (player.Move0 != null)
            {
                // Calculates the accuracy to display.
                accuracy = Mathf.Clamp01(player.GetModifiedAccuracy(player.Move0.Accuracy)) * 100.0F;

                // Slots in the text.
                move0AccuracyText.text = (player.Move0.useAccuracy) ? 
                    accuracy.ToString("F" + GameplayManager.DISPLAY_DECIMAL_PLACES.ToString()) + "%" : "-";
            }
            else
            {
                move0AccuracyText.text = "-";
            }

            // Move 1
            if (player.Move1 != null)
            {
                // Calculates the accuracy to display.
                accuracy = Mathf.Clamp01(player.GetModifiedAccuracy(player.Move1.Accuracy)) * 100.0F;

                // Slots in the text.
                move1AccuracyText.text = (player.Move1.useAccuracy) ? 
                    accuracy.ToString("F" + GameplayManager.DISPLAY_DECIMAL_PLACES.ToString()) + "%" : "-";
            }
            else
            {
                move1AccuracyText.text = "-";
            }

            // Move 2
            if (player.Move2 != null)
            {
                // Calculates the accuracy to display.
                accuracy = Mathf.Clamp01(player.GetModifiedAccuracy(player.Move2.Accuracy)) * 100.0F;

                // Slots in the text.
                move2AccuracyText.text = (player.Move2.useAccuracy) ? 
                    accuracy.ToString("F" + GameplayManager.DISPLAY_DECIMAL_PLACES.ToString()) + "%" : "-";
            }
            else
            {
                move2AccuracyText.text = "-";
            }

            // Move 3
            if (player.Move3 != null)
            {
                // Calculates the accuracy to display.
                accuracy = Mathf.Clamp01(player.GetModifiedAccuracy(player.Move3.Accuracy)) * 100.0F;

                // Slots in the text.
                move3AccuracyText.text = (player.Move3.useAccuracy) ? 
                    accuracy.ToString("F" + GameplayManager.DISPLAY_DECIMAL_PLACES.ToString()) + "%" : "-";
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

        // Called to perform the player's move.
        private void PerformPlayerMove()
        {
            order++;

            // Checks if the player is dead.
            // This addresses a problem where a move would go off even though the entity was already dead.
            if(player.IsDead()) // Dead, so don't do anything.
            {
                textBox.InsertAfterCurrentPage(Move.GetMoveFailedPage());
            }
            else // Perform move.
            {
                player.selectedMove.Perform(player, opponent, this);
            }
        }

        // Called to perform the opponent's move.
        private void PerformOpponentMove()
        {
            order++;

            // Prevents the opponent from using its move if it's already dead.
            // This is being used to address a glitch where an attack goes off when it shouldn't.
            if(opponent.IsDead()) // Dead, so don't do anything.
            {
                textBox.InsertAfterCurrentPage(Move.GetMoveFailedPage());
            }
            else // Perform move.
            {
                opponent.selectedMove.Perform(opponent, player, this);
            }   
        }

        // Performs the two moves.
        public void PerformMoves()
        {
            // Both sides have selected a move, and the tutorial isn't active.
            if (player.selectedMove != null && opponent.selectedMove != null &&
                !gameManager.tutorial.TextBoxIsVisible())
            {
                // Going to perform moves now.
                order = 0;

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
                // The player will never be paralyzed if charging their move or attempting to run away (enemies will though).
                if(player.paralyzed && player.selectedMove.Id != moveId.run && player.selectedMove.Id != moveId.charge)
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

                    // Play the paralyzed animation.
                    playerMovePage.OnPageOpenedAddCallback(PlayPlayerParalyzedAnimation);
                }
                else
                {
                    // Adds the player's move.
                    playerMovePage = new Page(
                        BattleMessages.Instance.GetMoveUsedMessage(player.displayName, player.selectedMove.Name),
                        BattleMessages.Instance.GetMoveUsedSpeakKey0()
                        );

                    playerMovePage.OnPageOpenedAddCallback(PerformPlayerMove);
                    playerMovePage.OnPageOpenedAddCallback(PostPerformMove);
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

                    // Play the paralyzed animation.
                    opponentMovePage.OnPageOpenedAddCallback(PlayOpponentParalyzedAnimation);
                }
                else // Don't skip.
                {
                    // Adds the opponent's move.
                    opponentMovePage = new Page(
                        BattleMessages.Instance.GetMoveUsedMessage(opponent.displayName, opponent.selectedMove.Name),
                        BattleMessages.Instance.GetMoveUsedSpeakKey1());
                    
                    opponentMovePage.OnPageOpenedAddCallback(PerformOpponentMove);
                    opponentMovePage.OnPageOpenedAddCallback(PostPerformMove);
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
                // No moves have been performed yet, so the order is 0.
                order = 0;

                // Gets the moves from the player and the opponent.
                player.OnBattleTurn(); // does nothing right now.

                // opponent.
                opponent.OnBattleTurn(); // calculates next move (evenutally)
            }
        }

        // Any checks that should be run after a move ahs been performed.
        public void PostPerformMove()
        {
            TryEndTurnEarly();
        }

        // Apply burn to the player.
        private void ApplyPlayerBurn()
        {
            // Player was burned.
            if (player.burned)
            {
                player.ApplyBurn(this);
                PlayBurnSfx(); // Plays the sound effect.
                PlayPlayerHurtAnimation(false); // Play animation.
            }
                
        }

        // Apply burn to the opponent.
        private void ApplyOpponentBurn()
        {
            // Checks if the opponent is burned.
            if (opponent.burned)
            {
                opponent.ApplyBurn(this);

                PlayBurnSfx(); // Plays the sound effect.
                PlayOpponentHurtAnimation(false); // Plays the animation.
            }

        }


        // Called when the player attempts to run away. TODO: have the enemy's move still go off if the run fails.
        public void RunAway()
        {
            // Becomes 'true' if the run attempt was successful.
            bool success = false;

            // Overrides the selected move.
            player.selectedMove = MoveList.Instance.RunMove;

            // Run has now been fixed so that it's tied to the actual move.
            // If this function is called it will always be a success.

            // // If there's no opponent then the player can always run away.
            // if (opponent == null)
            // {
            //     success = true;
            // }
            // // If there is an opponent there the player may be unable to leave.
            // else
            // {
            //     // There's a 1/2 chance of running away.
            //     // success = (Random.Range(0, 2) == 1);
            //     success = true;
            // }

            success = true;

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

        // Attempt to end the turn early.
        // Tries to end the turn early if one of the entities is dead.
        public bool TryEndTurnEarly()
        {
            // The turn won't end early if the battle would normally end at this point anyway.
            if (order <= 0 || order >= 2)
                return false;

            // The user or the target is dead.
            if (player.IsDead() || opponent.IsDead())
            {
                // Ends the turn early.
                EndTurnEarly();
                return true;
            }
            else // Don't end the battle early.
            {
                return false;
            }
        }

        // Ends the turn early.
        public void EndTurnEarly()
        {
            // NOTE: this doesn't skip messages about paralysis and burn from the killing move, but I think that's fine.

            // Adds an end page so that the battle can end early.
            Page endPage = new Page(" ");
            endPage.OnPageOpenedAddCallback(textBox.Close);

            // Original - ended things early and inserted the page right after the current one.
            // textBox.InsertAfterCurrentPage(endPage);

            // New - inserts page at second to last index.
            // This puts it before the other battler's move.
            textBox.InsertPage(endPage, textBox.GetPageCount() - 1);
        }

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
        public void TakeTreasure()
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
            // A move has already been learned, so don't go through this function again.
            // This happens because the game needs to force the textbox to move on when a move is added...
            // If the player has less than 4 moves at the time.
            // As such, it ends up calling this function multiple times.
            if (learnedMove)
                return;

            // NEW
            // It appears to happen before and after the player levels up.
            // It also generates a new move for the player to learn.

            // Hide the box gameobject.
            // textBox.Close();
            textBox.Hide();

            // TODO: implement new move learning.
            // The phase.
            int phase = gameManager.GetGamePhase();

            // Runs a randomizer to see if a move of a random rank will be chosen.
            // The random rank being chosen.
            int randRank = (Random.Range(0.0F, 1.0F) <= RANDOM_RANK_MOVE_CHANCE) ? -1 : phase;

            // The new move.
            Move newMove;

            // Becomes 'true' if the move was found.
            bool moveFound = false;

            // The attempts to get a new move.
            int attempts = 0;

            do
            {
                // Checks the phase.
                switch (randRank)
                {
                    case 1: // beginning - 1
                        newMove = MoveList.Instance.GetRandomRank1Move();
                        break;
                    case 2: // middle - 2
                        newMove = MoveList.Instance.GetRandomRank2Move();
                        break;
                    case 3: // end - 3
                        newMove = MoveList.Instance.GetRandomRank3Move();
                        break;
                    default: // random
                        newMove = MoveList.Instance.GetRandomMove();
                        break;
                }

                // Checks if the player has the move already.
                if(player.HasMove(newMove)) // Move is not valid.
                {
                    // Pick from all moves.
                    randRank = 0;

                    // Move has not been found.
                    moveFound = false;
                }
                else // Move is valid.
                {
                    moveFound = true;
                }

                // Increases the amount of attempts made.
                attempts++;

                // Max amount of attempts were made, so just stick with whatever move the game gave.
                if (attempts >= 5)
                    moveFound = true;

            } while (!moveFound);

           
            // If the player has less than 4 moves, automatically learn the move.
            if (player.GetMoveCount() < 4)
            {
                // Adds the new move to the list.
                for (int i = 0; i < player.moves.Length; i++)
                {
                    if (player.moves[i] == null)
                    {
                        player.moves[i] = newMove;
                        break;
                    }
                }

                // New move has been added.
                learnedMove = true;

                // Remove this callback (DOESN'T WORK)
                // textBox.CurrentPage.OnPageClosedRemoveCallback(OnLearningNewMove);

                // Removes the placeholder page.
                textBox.pages.RemoveAt(textBox.CurrentPageIndex + 1);

                // Inserts a new page.
                textBox.InsertAfterCurrentPage(new Page(
                    BattleMessages.Instance.GetLearnMoveYesMessage(newMove.Name),
                    BattleMessages.Instance.GetLearnMoveYesSpeakKey()));

                // NOT NEEDED.
                // Go onto the next page.
                // textBox.Open();
                textBox.Show();
                textBox.NextPage();
            }
            else
            {
                // New move panel will be opened.
                learnedMove = true;

                // Hide the textbox so that the learn move panel is shown.
                textBox.Hide(); // This already gets called in the learn move panel OnEnable(). (TODO: remove?)

                // Update the information.
                learnMovePanel.newMove = newMove;
                learnMovePanel.LoadMoveInformation(); // Happens on enable (TODO: remove?)

                // Turn on the move panel, which also updates the move list.
                learnMovePanel.Activate(); // Turns on the object.

                
            }            
        }

        // Goes to the overworld.
        public void ToOverworld()
        {
            // Clear out the textbox.
            if (textBox.IsVisible())
            {
                textBox.Close();
                // textBox.pages.Clear();
                textBox.ClearPages(); // Use the dedicated function.
            }

            // Player options should be enabled before leaving so that they're ready for the next battle?
            EnablePlayerOptions();

            // Remove selected move.
            player.selectedMove = null;

            // Remove stat changes and status effects
            // This already happens in the initialization phase, but it happens here just to be sure. 
            // TODO: maybe take this out?
            player.ResetStatModifiers();
            player.ResetStatuses();

            // Save battle entity data.
            door.battleEntity = opponent.GenerateBattleEntityGameData();

            // Remove selected move.
            opponent.selectedMove = null;

            // Hide opponent sprite and reset the animation.
            opponentSprite.gameObject.SetActive(false);
            PlayDefaultOpponentAnimation();

            // Prepare for next battle.
            gotCritical = false;
            gotRecoil = false;
            order = 0;
            learnedMove = false;

            // The battle gets initialized everytime one starts.
            initialized = false;

            // Go to the overworld.
            gameManager.UpdateUI();

            // Checks if transitions are being used to know the right function to call.
            if (gameManager.useTransitions)
                gameManager.EnterOverworldWithTransition();
            else
                gameManager.EnterOverworld();
        }

        // Updates all UI elements.
        public void UpdateUI()
        {
            RefreshPlayerOptions();
            UpdateOpponentUI();

            // Calls functions in the GameplayManager.
            UpdatePlayerHealthUI();
            UpdatePlayerEnergyUI();
        }

        // Updates the player health UI.
        public void UpdatePlayerHealthUI()
        {
            gameManager.UpdatePlayerHealthUI();
        }

        // Updates the player energy UI.
        public void UpdatePlayerEnergyUI()
        {
            gameManager.UpdatePlayerEnergyUI();
        }

        // Updates the opponent AI.
        public void UpdateOpponentUI()
        {
            opponentHealthBar.SetValue(opponent.Health / opponent.MaxHealth);

            // If false, the text is changed instantly. If true, the text is not updated here.
            // This prevents the final number from flashing for a frame.
            if (!gameManager.syncTextToBars)
                opponentHealthText.text = Mathf.Ceil(opponent.Health).ToString() + "/" + Mathf.Ceil(opponent.MaxHealth).ToString();
        }

        // AUDIO //
        // Plays the battle bgm.
        public void PlayBattleBgm()
        {
            // Gets the phase of the game.
            int phase = gameManager.GetGamePhase();

            // Gets the audio manager.
            AudioManager audioManager = gameManager.audioManager;

            // Checks the phase for playing the BGM.
            switch (phase)
            {
                default:
                case 1: // Normal Speed
                    audioManager.PlayBgm(battleBgm, 1.0F);
                    break;

                case 2: // Faster
                    audioManager.PlayBgm(battleBgm, 1.2F);
                    break;

                case 3: // Faster
                    audioManager.PlayBgm(battleBgm, 1.4F);
                    break;
            }
        }

        // Plays the treasure bgm.
        public void PlayTreasureBgm()
        {
            // Slower version of the battle theme.
            gameManager.audioManager.PlayBgm(battleBgm, 0.8F);
        }

        // Plays the battle - boss bgm.
        public void PlayBossBgm()
        {
            gameManager.audioManager.PlayBackgroundMusic(bossBgm);
        }

        // Plays the battle results BGM.
        public void PlayBattleResultsBgm()
        {
            // Reuses the overworld BGM at plays it at a lower pitch.
            gameManager.audioManager.PlayBgm(
                gameManager.overworld.overworldBgm,
                0.8F);
        }

        // SFX //
        // Play sound effect for the opponent did damage to the player.
        public void PlayPlayerHurtSfx()
        {
            gameManager.audioManager.PlaySoundEffect(playerHurtSfx);
        }

        // Play sound effect for the player did damage to the opponent.
        public void PlayOpponentHurtSfx()
        {
            gameManager.audioManager.PlaySoundEffect(opponentHurtSfx);
        }

        // Play sound effect for non-damaging moves.
        public void PlayMoveEffectSfx()
        {
            gameManager.audioManager.PlaySoundEffect(moveEffectSfx);
        }

        // Play the burn sound effect.
        public void PlayBurnSfx()
        {
            gameManager.audioManager.PlaySoundEffect(burnSfx);
        }

        // Play the paralysis sound effect.
        public void PlayParalysisSfx()
        {
            gameManager.audioManager.PlaySoundEffect(paralysisSfx);
        }

        // JNG //
        // Plays the battle won jingle.
        public void PlayBattleWonJng()
        {
            // This will play when the jingle is done.
            PlayBattleResultsBgm();

            gameManager.audioManager.PlayJingle(battleWonJng, false);
        }

        // Plays the battle lost jingle.
        public void PlayBattleLostJng()
        {
            // This will play when the jingle is done.
            PlayBattleResultsBgm();

            gameManager.audioManager.PlayJingle(battleLostJng, false);
        }


        // ANIMATIONS //
        // Plays the player animation.
        public void PlayPlayerAnimation(int color)
        {
            // Enables the game object so that the animation automatically plays.
            switch(color)
            {
                case 1: // Damage
                    playerAnimationImage.color = Color.red;
                    break;
                case 2: // Status
                    playerAnimationImage.color = Color.blue;
                    break;
                case 3: // Heal/Health Restore (Move)
                    playerAnimationImage.color = Color.green;
                    break;
                case 4: // Paralysis
                    playerAnimationImage.color = Color.yellow;
                    break;
                default: // Default
                    playerAnimationImage.color = Color.white;
                    break;

            }
            playerAnimator.gameObject.SetActive(true);

            // Get the length of the animation.
            float animTime = playerAnimator.GetCurrentAnimatorStateInfo(0).length / playerAnimator.speed;

            // Turn off the animation.
            StartCoroutine(AnimatorDisableDelayed(playerAnimator, animTime, false));
        }

        // Stops the player paralysis animation.
        public void StopPlayerAnimation()
        {
            playerAnimator.gameObject.SetActive(false);
        }

        // Plays the player damage animation.
        public void PlayPlayerHurtAnimation(bool playSound = true)
        {
            // Play sound effect.
            if(playSound)
                PlayPlayerHurtSfx();

            PlayPlayerAnimation(1);
        }

        // Plays the player status effected animation.
        public void PlayPlayerStatusAnimation()
        {
            PlayMoveEffectSfx();

            PlayPlayerAnimation(2);
        }

        // Plays the player heal animation.
        // This is only used when the player uses a healing move.
        // This isn't used when the player is healed after completing a battle.
        public void PlayPlayerHealAnimation()
        {
            PlayMoveEffectSfx();

            PlayPlayerAnimation(3);
        }

        // Plays when the player suffers paralysis.
        public void PlayPlayerParalyzedAnimation()
        {
            PlayParalysisSfx();

            PlayPlayerAnimation(4);
        }


        // Plays the opponent damage animation.
        public void PlayOpponentAnimation(string parameter, int value)
        {
            // Play animation by changing the value, then change it again so that it only plays once.
            opponentAnimator.SetInteger(parameter, value);

            // Get the length of the animation.
            // Added extra time to be safe - may be unneeded.
            float animTime = opponentAnimator.GetCurrentAnimatorStateInfo(0).length / opponentAnimator.speed;

            // Turn off the animation.
            StartCoroutine(AnimationSetIntegerDelayed(opponentAnimator, parameter, animTime, 0));

        }

        // Called to stop an opponent animation.
        public void PlayOpponentDefaultAnimation()
        {
            // Play animation by changing the value.
            opponentAnimator.SetInteger("anim", 0);
        }

        // Plays the opponent damage animation.
        public void PlayOpponentHurtAnimation(bool playSound = true)
        {
            // Play sound effect.
            if(playSound)
                PlayOpponentHurtSfx();

            // Play the animation.
            PlayOpponentAnimation("anim", 1);

        }

        // Plays the opponent status inflicted animation.
        public void PlayOpponentStatusAnimation()
        {
            PlayMoveEffectSfx();

            // Play the animation.
            PlayOpponentAnimation("anim", 2);

        }

        // Plays the opponent heal animation.
        public void PlayOpponentHealAnimation()
        {
            PlayMoveEffectSfx();

            // Play the animation.
            PlayOpponentAnimation("anim", 3);
        }

        // Plays the opponent damage animation.
        public void PlayOpponentParalyzedAnimation()
        {
            PlayParalysisSfx();

            // TODO: play sound effect
            PlayOpponentAnimation("anim", 4);
        }

        // Plays the death animation for the opponent.
        public void PlayOpponentDeathAnimation()
        {
            // Should remain on this animation until the player goes back to the overworld.
            // As such, this doesn't call another function.

            // Play animation by changing the value.
            opponentAnimator.SetInteger("anim", 5);
        }
        // Plays the death animation for the opponent.
        public void PlayDefaultOpponentAnimation()
        {
            // Return to the default animation.
            opponentAnimator.SetInteger("anim", 0);
        }


        // Disables an animator after a certain amount of wait time.
        // If 'animatorOnly' is true, then the animator component is disabled.
        // If 'animatorOnly' is false, then the animator's GameObject is disabled.
        private IEnumerator AnimatorDisableDelayed(Animator animator, float waitTime, bool animatorOnly)
        {
            // While the operation is going.
            while (waitTime > 0.0F)
            {
                // Reduce by delta time.
                waitTime -= Time.deltaTime;

                // Tells the program to stall.
                yield return null;
            }

            // Checks what to disable.
            if(animatorOnly) // Component only.
            {
                animator.enabled = false;
            }
            else // Whole object.
            {
                animator.gameObject.SetActive(false);
            }
        }

        // A function called to set an int after the timer runs out.
        private IEnumerator AnimationSetIntegerDelayed(Animator animator, string parameter, float animTime, int value)
        {
            // The wait time for the animation.
            float waitTime = animTime;

            // While the operation is going.
            while (waitTime > 0.0F)
            {
                // Reduce by delta time.
                waitTime -= Time.deltaTime;

                // Tells the program to stall.
                yield return null;
            }

            // Sets the integer.
            animator.SetInteger(parameter, value);
        }

        // A function called to set a bool after a timer runs out.
        private IEnumerator AnimationSetBoolDelayed(Animator animator, string parameter, float animTime, bool value)
        {
            // The wait time for the animation.
            float waitTime = animTime;

            // While the operation is going.
            while (waitTime > 0.0F)
            {
                // Reduce by delta time.
                waitTime -= Time.deltaTime;

                // Tells the program to stall.
                yield return null;
            }

            // Changes the animator so that the animation goes back.
            animator.SetBool(parameter, value);
        }


        // Update is called once per frame
        void Update()
        {
            // If the text box is not visible.
            if (!textBox.IsVisible())
            {
                // Prevents the user from dying if this is the first battle they're doing.
                if(gameManager.useTutorial && !gameManager.tutorial.TextBoxIsVisible())
                {
                    // Loads first death tutorial.
                    // If the opponent is dead then the player will win the battle anyway.
                    if(gameManager.roomsCompleted == 0 && player.IsDead() && !opponent.IsDead())
                    {
                        // Need to reset these to finish the turn.
                        player.selectedMove = null;
                        opponent.selectedMove = null;

                        // Prevents the player from dying during the first battle.
                        player.SetHealthToMax();
                        UpdatePlayerHealthUI();

                        player.SetEnergyToMax();
                        UpdatePlayerEnergyUI();

                        // Tutorial for first battle death.
                        if(!gameManager.tutorial.clearedFirstBattleDeath)
                            gameManager.tutorial.LoadFirstBattleDeathTutorial();
                    }

                    // TODO: if this is used, the HP hits 0, then goes to 1.
                    // In practice this case should never be encountered.

                    // If it's the first turn and the opponent is dead, give them 1 HP back.
                    // The other tutorials won't happen if the enemy dies in one turn.
                    if (opponent.IsDead() && turnsTaken <= 1)
                    {
                        opponent.Health = 1;
                        UpdateOpponentUI();

                        // Making sure the moves are now all not selected.
                        player.selectedMove = null;
                        opponent.selectedMove = null;
                    }
                }

                // If both entities are alive do battle calculations.
                if (player.Health > 0 && opponent.Health > 0)
                {
                    // TUTORIAL CONTENT
                    // If using the tutorial, and a tutorial textbox isn't currently active.
                    if(gameManager.useTutorial && !gameManager.tutorial.TextBoxIsVisible())
                    {
                        // Grabs the tutorial object.
                        Tutorial trl = gameManager.tutorial;

                        // Loads tutorials.
                        // Loads the first move tutorial.
                        if (!trl.clearedFirstMove && turnsTaken != 0)
                        {
                            trl.LoadFirstMoveTutorial();
                        }
                        // Loads the critical damage tutorial.
                        else if (!trl.clearedCritical && gotCritical)
                        {
                            trl.LoadCriticalDamageTutorial();
                        }
                        // Loads the recoil damage tutorial.
                        else if (!trl.clearedRecoil && gotRecoil)
                        {
                            trl.LoadRecoilDamageTutorial();
                        }
                        // Loads the stat change tutorial.
                        else if (!trl.clearedStatChange && (player.HasStatModifiers() || opponent.HasStatModifiers()))
                        {
                            trl.LoadStatChangeTutorial();
                        }
                        // Loads the burn tutorial.
                        else if(!trl.clearedBurn && (player.burned || opponent.burned))
                        {
                            trl.LoadBurnTutorial();
                        }
                        // Loads the paralysis tutorial.
                        else if(!trl.clearedParalysis && (player.paralyzed || opponent.paralyzed))
                        {
                            trl.LoadParalysisTutorial();
                        }

                    }

                    // BATTLE CONTENT
                    // If the opponent isn't a treasure chest try to perform moves.
                    if (!(opponent is Treasure))
                    {
                        PerformMoves();
                    }
                    else // Checks to see if the prompt is visible.
                    {
                        // This is to fix a glitch where the treasure prompt would sometimes not appear.
                        // I don't know why that happens, but this should address it.
                        // TODO: don't show the prompt if the tutorial textbox is open?
                        if (!treasurePrompt.activeSelf)
                            treasurePrompt.SetActive(true);
                    }
                        
                }
                else
                {
                    // Checks if the battle end has been initialized.
                    if (!initBattleEnd)
                    {
                        // If the player is dead and the opponent is dead, the player is given 1 HP point.
                        // This means that if both of them die at the same time, the player always wins.
                        // This should make the game easier.
                        if (player.IsDead() && opponent.IsDead())
                            player.Health = 1;

                        // Returns to the overworld. TODO: account for game over.
                        // The player got a game over.
                        if (player.IsDead()) // game over
                        {
                            // Restores the opponent's health to max (stops both from dying on the same round.
                            opponent.Health = opponent.MaxHealth;

                            // textBox.pages.Clear();
                            textBox.ClearPages();

                            // Page for losing the game.
                            Page losePage = new Page(
                                BattleMessages.Instance.GetBattleLostMessage(),
                                BattleMessages.Instance.GetBattleLostSpeakKey()
                                );

                            // Play the battle lost jingle.
                            losePage.OnPageOpenedAddCallback(PlayBattleLostJng);

                            textBox.pages.Add(losePage);
                            textBox.SetPage(0);
                            textBox.OnTextBoxFinishedAddCallback(OnPlayerBattleLost);

                            DisablePlayerOptions();
                            textBox.Open();
                        }
                        else // The player won the fight.
                        {
                            // textBox.pages.Clear();
                            textBox.ClearPages();

                            // Checks the opponent type.
                            if (opponent is Treasure) // Is Treasure
                            {
                                textBox.pages.Add(new Page(
                                    BattleMessages.Instance.GetTakeTreasureMessage(),
                                    BattleMessages.Instance.GetTakeTreasureSpeakKey()
                                    ));
                            }
                            else if(opponent is Boss) // Final boss beaten.
                            {
                                // Boss page and callback.
                                Page bossPage = new Page(
                                    BattleMessages.Instance.GetBattleWonBossMessage(),
                                    BattleMessages.Instance.GetBattleWonBossSpeakKey()
                                    );

                                // Play jingle, close the textbox, and go onto the results screen.
                                bossPage.OnPageOpenedAddCallback(PlayBattleWonJng);
                                bossPage.OnPageOpenedAddCallback(PlayOpponentDeathAnimation);

                                bossPage.OnPageClosedAddCallback(textBox.Close);
                                bossPage.OnPageClosedAddCallback(gameManager.ToResultsScene);

                                // Adds the boss page. 
                                textBox.pages.Add(bossPage);

                                // Room has been compelted.
                                // This is put here because OnPlayerBattleWon is called after the game is set up to switch scenes.
                                // In said class the roomsCompleted count is increased, meaning that it wouldn't reach its final value...
                                // Before the game is over without being done manually here.
                                gameManager.roomsCompleted++;
                            }
                            else // Not Treasure
                            {
                                // The winning page.
                                Page winPage = new Page(
                                    BattleMessages.Instance.GetBattleWonMessage(),
                                    BattleMessages.Instance.GetBattleWonSpeakKey());

                                // Play the battle won jingle when the page is opened.
                                winPage.OnPageOpenedAddCallback(PlayBattleWonJng);
                                winPage.OnPageOpenedAddCallback(PlayOpponentDeathAnimation);

                                textBox.pages.Add(winPage);
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
                                // This updates the health and energy levels.
                                tempPage.OnPageOpenedAddCallback(gameManager.UpdateUI);

                                // Saves the old stats.
                                float oldMaxHp = player.MaxHealth;
                                float oldAtk = player.Attack;
                                float oldDef = player.Defense;
                                float oldSpd = player.Speed;
                                float oldMaxEng = player.MaxEnergy;

                                // Levels up te player.
                                // Checks if the opponent has a level-up specialty or not.
                                if(opponent is Enemy)
                                {
                                    // Special level up.
                                    player.LevelUp(opponent.statSpecial);
                                }
                                else
                                {
                                    // Standard level up.
                                    player.LevelUp();
                                }


                                // NOTE: no longer shows energy levels since those don't matter anymore.
                                // Adds page with the increases in stats.
                                textBox.pages.Add(new Page(
                                    gameManager.HealthString + " +" + Mathf.RoundToInt(player.MaxHealth - oldMaxHp).ToString() + "   |   " +
                                    gameManager.AttackString + " +" + Mathf.RoundToInt(player.Attack - oldAtk).ToString() + "\n" +
                                    gameManager.DefenseString + " +" + Mathf.RoundToInt(player.Defense - oldDef).ToString() + "   |   " +
                                    gameManager.SpeedString + " +" + Mathf.RoundToInt(player.Speed - oldSpd).ToString()
                                    ));

                                // Adds page with new stats.
                                textBox.pages.Add(new Page(
                                    gameManager.LevelString + " = " + player.Level.ToString() + "\n" +
                                    gameManager.HealthString + " = " + Mathf.RoundToInt(player.MaxHealth).ToString() + "   |   " +
                                    gameManager.AttackString + " = " + Mathf.RoundToInt(player.Attack).ToString() + "\n" +
                                    gameManager.DefenseString + " = " + Mathf.RoundToInt(player.Defense).ToString() + "   |   " +
                                    gameManager.SpeedString + " = " + Mathf.RoundToInt(player.Speed).ToString()
                                    ));

                                
                            }

                            // Checks to see if a new move should be learned.
                            bool learningMove = (Random.Range(0.0F, 1.0F) <= NEW_MOVE_CHANCE || opponent is Treasure);

                            // If this is the tutorial, and it's the first room cleared, always give the player a new move.
                            if(learningMove == false)
                            {
                                // Learn a new move.
                                if (gameManager.useTutorial && gameManager.roomsCompleted == 0)
                                    learningMove = true;
                            }

                            // If the boss was beaten the game will end, so the player won't learn a new move.
                            if (opponent is Boss)
                                learningMove = false;
                            

                            // Checks to see if the player will be learning a new move.
                            // If the opponet was a treasure box the player will always get the chance to learn a new move.
                            if (learningMove)
                            {
                                Page newMovePage = new Page(
                                    BattleMessages.Instance.GetLearnMoveMessage(),
                                    BattleMessages.Instance.GetLearnMoveSpeakKey()
                                    );

                                newMovePage.OnPageClosedAddCallback(OnLearningNewMove);
                                textBox.pages.Add(newMovePage);

                                // Needed for the learn move panel to skip through.
                                // This page is active when learning the move.
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

                // Turns these tutorial variables to false so that they are reset.
                gotCritical = false;
                gotRecoil = false;
            }

            // If the text should match the bars.
            if(gameManager.syncTextToBars)
            {
                // Opponent Health Scrolling Text
                // Checks if the opponent health bar is transitioning.
                if (opponentHealthBar.IsTransitioning())
                {
                    opponentHealthText.text = Mathf.Ceil(opponentHealthBar.GetSliderValueAsPercentage() * opponent.MaxHealth).ToString() + "/" +
                        opponent.MaxHealth.ToString();

                    // The health is transitioning.
                    opponentHealthTransitioning = true;
                }
                else if (opponentHealthTransitioning) // Transition done.
                {
                    // Set to exact value.
                    opponentHealthText.text = Mathf.Ceil(opponent.Health).ToString() + "/" + opponent.MaxHealth.ToString();

                    opponentHealthTransitioning = false;
                }
            }
            

        }


    }
}