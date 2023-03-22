using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SimpleJSON;
using UnityEngine.SceneManagement;
using System.Data.Common;
using LoLSDK;
using UnityEngine.Timeline;

namespace RM_BBTS
{
    // Manages the battle operations for the game. This becomes active when the game enters a battle state.
    public class BattleManager : GameState
    {
        // Becomes 'true' when the overworld is initialized.
        private bool initialized = false;

        // Signifies that the post initialization function has been called.
        // This was used to fix a bug when initializing the battle from a saved game.
        // This gets set to false when 'Initialize()' is called, which triggers the post function call.
        private bool postInitialized = true;

        // the manager for the game.
        public GameplayManager gameManager;

        // The door
        public Door door;

        // The textbox
        public TextBox textBox;

        // The prompt for asking the player about the treasure.
        public GameObject treasurePrompt;

        // The panel for learning a new move, and the move to be offered.
        public LearnMove learnMovePanel;
        public MultipleMoveOffer multiMoveOfferPanel;
        private Move moveOffer;

        // Auto save the game when exiting the battle scene.
        public bool autoSaveOnExit = false;

        [Header("Battle")]

        // The player.
        public Player player;

        // The amount of turns the battle took, which is used to help calculate score.
        // This is about the amount of full turn rotations, not individual moves made.
        private int turnsPassed = 0;  

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

        // Saves the initial health and energy for the opponent when the battle got initialized.
        // This is to fix a bug where a load from a saved game did not keep the health and energy level.
        // Initial Health
        private float opponentInitHealth = -1;
        private float opponentInitMaxHealth = -1;
        // Initial Energy
        private float opponentInitEnergy = -1;
        private float opponentInitMaxEnergy = -1;

        // [Header("Battle/Mechanics")]
        // // The move the player has selected.
        // public Move playerMove;
        // 
        // // The move the opponent has selected.
        // public Move opponentMove;

        // The burn damage amount (1/16 damage).
        public const float BURN_DAMAGE = 0.0625F;

        // The chance to skip a turn if paralyzed.
        public const float PARALYSIS_SKIP_CHANCE = 0.2F;

        // The chance of learning a new move.
        private float NEW_MOVE_CHANCE = 1.00F; // 0.80

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
        private const string TREASURE_PROMPT_TEXT_KEY = "btl_msg_treasure";

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

        // Extra wait time for playing jingles.
        private const float JNG_EXTRA_WAIT_TIME = 0.5F;

        [Header("Animations")]
        // The player's animator.
        public Animator playerAnimator;

        // The image for the player animation (is recoloured as needed).
        public Image playerAnimationImage;

        // // Used to see if the player's object should be disabled when it's off, or just the component.
        // private const bool PLAYER_ANIM_DISABLE_OBJECT = true;
        // 
        // // The timer for player animations.
        // private TimerManager.Timer playerAnimTimer;

        // The opponent's animator.
        public Animator opponentAnimator;

        // // The timer for opponent animations.
        // private TimerManager.Timer opponentAnimTimer;
        // 
        // // Used to see if the opponent's object should be disabled when it's off, or just the component.
        // // I don't think I actually use this.
        // private const bool OPPONENT_ANIM_DISABLE_OBJECT = true;

        // A script used to cause the opponent object to float.
        public ObjectFloat opponentFloat;

        // Extra time for playing out animations.
        private float EXTRA_ANIM_TIME = 0.5F;

        // If set to 'true', the move animations are played.
        public const bool PLAY_IDLE_AND_MOVE_ANIMATIONS = true;

        // The animation manager for the moves.
        public MoveAnimationManager moveAnimation;

        // If the battle background should be used.
        public const bool USE_BATTLE_BACKGROUND = true;

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
            turnsPassed = 0;
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

            // // Initialize the timers.
            // playerAnimTimer = new TimerManager.Timer();
            // playerAnimTimer.tag = "player";
            // 
            // opponentAnimTimer = new TimerManager.Timer();
            // opponentAnimTimer.tag = "opponent";

            // The defs are not set.
            if(defs != null)
            {
                // Translate the treasure prompt.
                // treasurePromptTextKey = "btl_msg_treasure"; // Set by default so that the text-to-speech can use it.
                treasurePromptText.text = defs[TREASURE_PROMPT_TEXT_KEY];
                treasureYesButtonText.text = defs["kwd_yes"];
                treasureNoButtonText.text = defs["kwd_no"];
            }
            else
            {
                LanguageMarker marker = LanguageMarker.Instance;

                marker.MarkText(chargeButtonText);
                marker.MarkText(runButtonText);
                
                marker.MarkText(move0NameText);
                marker.MarkText(move1NameText);
                marker.MarkText(move2NameText);
                marker.MarkText(move3NameText);

                marker.MarkText(treasurePromptText);
                marker.MarkText(treasureYesButtonText);
                marker.MarkText(treasureNoButtonText);

                marker.MarkText(opponentNameText);
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
            player.vulnerable = true; // The player can be damaged.
            // player.ResetStatModifiers(); // This is no longer done since questions can apply them.
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

                // Setting hte sprite and enabling the object.
                opponentSprite.sprite = opponent.sprite;
                opponentSprite.gameObject.SetActive(true);


                // The opponent can be damaged.
                opponent.vulnerable = true;

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
                // Closes the treasure chest.
                (opponent as Treasure).closed = true;

                // The player has no battle options since this isn't a fight.
                DisablePlayerOptions();

                // If the tutorial box isn't visible, turn on the treasure buttons.
                if (!gameManager.tutorial.textBox.IsVisible())
                {
                    // Enable the buttons for answering the question.
                    treasureYesButton.interactable = true;
                    treasureNoButton.interactable = true;

                    // Reads out the treasure prompt if the treasure tutorial isn't being shown.
                    if(LOLSDK.Instance.IsInitialized && GameSettings.Instance.UseTextToSpeech && TREASURE_PROMPT_TEXT_KEY != "")
                    {
                        // Speaks the text for the treasure prompt.
                        LOLManager.Instance.textToSpeech.SpeakText(TREASURE_PROMPT_TEXT_KEY);
                    }

                }
                
                // Hide health bar and health text
                opponentHealthBar.bar.gameObject.SetActive(false);
                opponentHealthText.gameObject.SetActive(false);

                // Show treasure prompt.
                treasurePrompt.gameObject.SetActive(true);

                // Replace the opponent sprite with the closed treasure chest sprite.
                if(treasureBase.closedSprite != null)
                {
                    opponent.sprite = treasureBase.closedSprite;
                    opponentSprite.sprite = opponent.sprite;
                }
            }
            else
            {
                // Show health bar, and update it.
                opponentHealthBar.bar.gameObject.SetActive(true);
                // opponentHealthBar.SetValue(opponent.Health / opponent.MaxHealth, false); // Moved

                // Show health text, and update it.
                opponentHealthText.gameObject.SetActive(true);
                // opponentHealthText.text = Mathf.Ceil(opponent.Health).ToString() + "/" + Mathf.Ceil(opponent.MaxHealth).ToString(); // Moved

                // Hide treasure prompt.
                treasurePrompt.gameObject.SetActive(false);
            }

            // Update the opponent health bar and health text.
            opponentHealthBar.SetValue(opponent.Health / opponent.MaxHealth, false);
            opponentHealthText.text = Mathf.Ceil(opponent.Health).ToString() + "/" + Mathf.Ceil(opponent.MaxHealth).ToString();

            // Saves the initial health and max health of the opponent.
            opponentInitHealth = opponent.Health;
            opponentInitMaxHealth = opponent.MaxHealth;

            // Saves the initial energy and max energy of the opponent.
            opponentInitEnergy = opponent.Energy;
            opponentInitMaxEnergy = opponent.MaxEnergy;

            // Plays the BGM based on the opponent.
            if (opponent is Boss)
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

            // Changes the background color for the animation.
            if(USE_BATTLE_BACKGROUND)
                gameManager.EnableBattleBackground("Blur", door.GetColor());


            // // Create the timers.
            // playerAnimTimer = new TimerManager.Timer();
            // opponentAnimTimer = new TimerManager.Timer();

            // Resets the turns passed and player damage taken since a new battle is being initialized. 
            turnsPassed = 0;
            playerDamageTaken = 0;

            // The battle has begun.
            initBattleEnd = false;

            // These events have not happened for this new battle. 
            gotCritical = false;
            gotRecoil = false;

            // Do not autosave unless the player actually wins.
            autoSaveOnExit = false;

            // No moves have been performed.
            order = 0;

            // No move has been learned for this battle yet, so set moveOffer to null, and learnedMove to false.
            moveOffer = null;
            learnedMove = false;

            // The battle has been initialized.
            // Also sets 'postInitialized' to false so that the post initialization function is called.
            initialized = true;
            postInitialized = false;
        }

        // Called after the battle is initialized.
        private void PostInitalize()
        {
            // If the PostInitialize() function has already been called, do nothing.
            if (postInitialized)
                return;

            // Make sure the health wasn't overwritten.
            if(opponentInitHealth != -1 && opponentInitMaxHealth != -1)
            {
                // Update the health with the initial values.
                // Do it in this order in case there's a mismatch between the health and max health.
                opponent.MaxHealth = opponentInitMaxHealth;
                opponent.Health = opponentInitHealth;

                // Update the opponent health bar and health text.
                opponentHealthBar.SetValue(opponent.Health / opponent.MaxHealth, false);
                opponentHealthText.text = Mathf.Ceil(opponent.Health).ToString() + "/" + Mathf.Ceil(opponent.MaxHealth).ToString();
            }

            // Make sure the energy wasn't overwritten.
            if(opponentInitEnergy != -1 && opponentInitMaxEnergy != -1)
            {
                // Update the energy with the initial values.
                // Do it in this order in case there's a mismatch between the energy and max energy.
                opponent.MaxEnergy = opponentInitMaxEnergy;
                opponent.Energy = opponentInitEnergy;
            }

            // Play the opponent's default animation.
            PlayDefaultOpponentAnimation();

            // Play the idle animation. If there is no set idle animation, nothing will play.
            // This needs to be odne here since the sprite must be enabled first.
            if (PLAY_IDLE_AND_MOVE_ANIMATIONS)
                PlayOpponentIdleAnimation();


            // The function is finished, so don't call it again.
            postInitialized = true;
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
        public float TurnsPassed
        {
            get { return turnsPassed; }
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

        // Refreshes the options for the player.
        public void RefreshPlayerOptions()
        {
            // Checks move activity to see if the player can use it or not.
            // Also changes the move name on the display.

            // Enables/disables various buttons.
            if(player.HasNoEnergy()) // If 'true', the player has no energy.
            {
                // Turn off the four move buttons.
                move0Button.interactable = false;
                move1Button.interactable = false;
                move2Button.interactable = false;
                move3Button.interactable = false;
            }
            else // The player has energy, so check if any moves can be performed.
            {
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
            }



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

                // Percent Form
                // accuracy = Mathf.Clamp01(player.GetModifiedAccuracy(player.Move0.Accuracy)) * 100.0F;

                // Slots in the text.
                // move0AccuracyText.text = (player.Move0.useAccuracy) ? 
                //     accuracy.ToString("F" + GameplayManager.DISPLAY_DECIMAL_PLACES.ToString()) + "%" : "-";

                // Decimal Form
                // Get the accuracy.
                accuracy = player.GetModifiedAccuracy(player.Move0.Accuracy, true);

                // Slots in the text.
                move0AccuracyText.text = (player.Move0.useAccuracy) ?
                    accuracy.ToString("F" + GameplayManager.DISPLAY_DECIMAL_PLACES.ToString()) : "-";
            }
            else
            {
                move0AccuracyText.text = "-";
            }

            // Move 1
            if (player.Move1 != null)
            {
                // Calculates the accuracy to display.
                accuracy = player.GetModifiedAccuracy(player.Move1.Accuracy, true);

                // Slots in the text.
                move1AccuracyText.text = (player.Move1.useAccuracy) ? 
                    accuracy.ToString("F" + GameplayManager.DISPLAY_DECIMAL_PLACES.ToString()): "-";
            }
            else
            {
                move1AccuracyText.text = "-";
            }

            // Move 2
            if (player.Move2 != null)
            {
                // Calculates the accuracy to display.
                accuracy = player.GetModifiedAccuracy(player.Move2.Accuracy, true);

                // Slots in the text.
                move2AccuracyText.text = (player.Move2.useAccuracy) ? 
                    accuracy.ToString("F" + GameplayManager.DISPLAY_DECIMAL_PLACES.ToString()) : "-";
            }
            else
            {
                move2AccuracyText.text = "-";
            }

            // Move 3
            if (player.Move3 != null)
            {
                // Calculates the accuracy to display.
                accuracy = player.GetModifiedAccuracy(player.Move3.Accuracy, true);

                // Slots in the text.
                move3AccuracyText.text = (player.Move3.useAccuracy) ? 
                    accuracy.ToString("F" + GameplayManager.DISPLAY_DECIMAL_PLACES.ToString()): "-";
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

        // Generates a random float in the 0-1 range.
        public static float GenerateRandomFloat01()
        {
            // I'm doing it this way to try and improve the randomizer.
            // Originally it just randomized a float from 0.0 to 1.0, which may have been weighted poorly.

            // Generates a random value of [0, 100), which gives a value from 0 to 99.
            float value = Random.Range(0, 100);

            // Adds a float, which has the potential to increase the value to 100.0.
            value += Random.Range(0.0F, 1.0F);

            // Divides the value by 100 so that it's in a [0.0, 1.0] scale.
            value /= 100.0F;

            // Clamping the value to be sure nothing screwed up.
            value = Mathf.Clamp01(value);

            // Returns the result.
            return value;
        }

        // Called to perform the player's move.
        private void PerformPlayerMove()
        {
            order++;

            // Checks if the player is dead.
            // This addresses a problem where a move would go off even though the entity was already dead.
            if(player.IsDead()) // Dead, so don't do anything.
            {
                // If the text box is open, close it so that the game moves on.
                // If the textbox is closed (which should never happen), add a move failed page.
                if (textBox.IsVisible())
                {
                    // This fixes the error where the entity attempts to do a move even though it's dead.
                    // I don't know why EndTurnEarly doesn't consistently stop this, but this fix helps address it.
                    // There's still some changes that happen when they don't matter (e.g. burning an entity when it's already dead)...
                    // But I think it's okay to leave that.
                    textBox.Close();
                }
                else // Inserts a move failed page to skip over.
                {
                    // This should never be reached since this function is only called when the textbox is visible.
                    textBox.InsertAfterCurrentPage(Move.GetMoveFailedPage());
                }

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
                // If the text box is open, close it so that the game moves on.
                // If the textbox is closed (which should never happen), add a move failed page.
                if (textBox.IsVisible())
                {
                    // This fixes the error where the entity attempts to do a move even though it's dead.
                    // I don't know why EndTurnEarly doesn't consistently stop this, but this fix helps address it.
                    // There's still some changes that happen when they don't matter (e.g. burning an entity when it's already dead)...
                    // But I think it's okay to leave that.
                    textBox.Close();
                }  
                else // Inserts a move failed page to skip over.
                {
                    // This should never be reached since this function is only called when the textbox is visible.
                    textBox.InsertAfterCurrentPage(Move.GetMoveFailedPage());
                }

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

                // Clears out the past text.
                turnText.Clear();

                // Since these are only for the tutorial, this is fine.
                // // Set these variables to false so that they can be triggered by the new set of turns.
                gotCritical = false;
                gotRecoil = false;


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
                    turnSkip = GenerateRandomFloat01() <= PARALYSIS_SKIP_CHANCE;
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
                    turnSkip = GenerateRandomFloat01() <= PARALYSIS_SKIP_CHANCE;
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


                // Replace the pages, reset the auto timer, and open the textbox.
                textBox.ReplacePages(turnText);
                textBox.Open();

                // Disable the player options since the textbox is open.
                DisablePlayerOptions();

                // Adds to the amount of turns the battle has taken.
                turnsPassed++;

                // Add to the total turns counter.
                gameManager.totalTurnsPassed++;
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
                ToOverworld(false);
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
            int turnsPassedPar = 5;

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
            if(turnsPassed <= turnsPassedPar) // Took the expected amount of turns or less.
            {
                result += 100 * turnsPassedPar - turnsPassed;
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
                // If a question won't be asked, save the game.
                // If a question will be asked, don't save the game, as it would allow...
                // The player to skip the question if they shut the game down.
                if(!gameManager.overworld.AskingQuestionOnOverworldEnter(true))
                    gameManager.SaveAndContinueGame();
            }

            // Go to the overworld.
            ToOverworld(true);
        }

        // Called when the player has lost the battle.
        public void OnPlayerBattleLost()
        {
            textBox.OnTextBoxFinishedRemoveCallback(OnPlayerBattleLost);
            gameManager.OnGameOver();
            ToOverworld(false);
        }

        // Call this function to open the treasure.
        public void TakeTreasure()
        {
            // Hide prompt.
            treasurePrompt.gameObject.SetActive(false);

            // The "battle" is over.
            opponent.Health = 0;

            // Counts this as a turn to avoid tutorial trigger issues.
            turnsPassed++;

            // The treasure chest has been opened, so set 'closed' to false.
            treasureBase.closed = false;

            // Replaces the oponnent sprite with the treasure open sprite.
            if (treasureBase.openSprite != null)
            {
                // Overwrite the sprite with the open treasure chest.
                opponent.sprite = treasureBase.openSprite;
                opponentSprite.sprite = opponent.sprite;

                // Play the idle animation so that the sprite switches over.
                if(PLAY_IDLE_AND_MOVE_ANIMATIONS)
                    PlayOpponentIdleAnimation();
            }
                
        }

        // Call this function to leave the treasure (the treasure was not opened).
        public void LeaveTreasure()
        {
            // Hide prompt.
            treasurePrompt.gameObject.SetActive(false);

            // Return to the overworld.
            ToOverworld(false);
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

            // The phase.
            int phase = gameManager.GetGamePhase();

            // Runs a randomizer to see if a move of a random rank will be chosen.
            // The random rank being chosen.
            int randRank = (GenerateRandomFloat01() <= RANDOM_RANK_MOVE_CHANCE) ? -1 : phase;

            // Becomes 'true' if the move was found.
            bool moveFound = false;

            // The attempts to get a new move.
            int attempts = 0;

            // If the move offer has not been generated yet, generate the new move.
            if (moveOffer == null)
            {
                do
                {
                    // Checks the phase.
                    switch (randRank)
                    {
                        case 1: // beginning - 1
                            moveOffer = MoveList.Instance.GetRandomRank1Move();
                            break;
                        case 2: // middle - 2
                            moveOffer = MoveList.Instance.GetRandomRank2Move();
                            break;
                        case 3: // end - 3
                            moveOffer = MoveList.Instance.GetRandomRank3Move();
                            break;
                        default: // random
                            moveOffer = MoveList.Instance.GetRandomMove();
                            break;
                    }

                    // Checks if the player has the move already.
                    if (player.HasMove(moveOffer)) // Move is not valid.
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
            }


            // If the player has less than 4 moves, automatically learn the move.
            if (player.GetMoveCount() < 4)
            {
                // Adds the new move to the list.
                for (int i = 0; i < player.moves.Length; i++)
                {
                    if (player.moves[i] == null)
                    {
                        player.moves[i] = moveOffer;
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
                    BattleMessages.Instance.GetLearnMoveYesMessage(moveOffer.Name),
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
                learnMovePanel.SetLearningMove(moveOffer, false);
                learnMovePanel.LoadMoveInformation(); // Happens on enable (TODO: remove?)

                // Turn on the move panel, which also updates the move list.
                learnMovePanel.Activate(); // Turns on the object.
            }

            // Clear this out for the next move offer.
            moveOffer = null;
        }

        // Called when being offered multiple moves.
        public void OnMultipleMovesOffered()
        {
            // Hide the textbox so that it doesn't get in the way of the multi move offer pnale.
            textBox.Hide();

            // Activates the object - functions are called in OnEnable().
            multiMoveOfferPanel.Activate();
        }

        // Goes to the overworld.
        public void ToOverworld(bool battleWon)
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

            // Reset temporary variables.
            player.vulnerable = true;
            player.ResetStatModifiers();
            player.ResetStatuses();

            // Remove selected move from opponent.
            opponent.selectedMove = null;

            // Remove temporary opponent traits.
            opponent.vulnerable = true;
            opponent.ResetStatModifiers();
            opponent.ResetStatuses();

            // Closes the treasure base in case the opponent was a treasure chest.
            treasureBase.closed = true;

            // Save battle entity data.
            door.battleEntity = opponent.GenerateBattleEntityGameData();


            // // Stops the idle animation of the opponent (needs to be done before the sprite is disabled).
            // The sprite is no longer disabled, so this was taken out. It also prevents the animation from stopping mid transition.
            // if (PLAY_IDLE_AND_MOVE_ANIMATIONS)
            //     StopOpponentIdleAnimation();


            // NOTE: if you don't turn off the sprite here, the enemy snaps back to its starting position before...
            // The transition finishes. But if you don't do that, then the sprite disappears before the transition is done.
            // You chose the latter.

            // The default opponent animation is now triggered in PostInitialize(), and not when the battle is over.
            // This is because the sprite would be shown again if hidden from a game over. 

            // // Hide opponent sprite and reset the animation.
            // opponentSprite.gameObject.SetActive(false);
            // PlayDefaultOpponentAnimation();


            // // Removes the timers from the list, and pauses them so that they can't be triggered regardless.
            // // These timers are re-generated when a new battle begins.
            // TimerManager.Instance.RemoveTimer(playerAnimTimer);
            // playerAnimTimer.paused = true;
            // 
            // TimerManager.Instance.RemoveTimer(opponentAnimTimer);
            // opponentAnimTimer.paused = true;

            // Stops the jingle from playing before leaving the battle.
            // This is in case the jingle is still playing when the player goes back to the overworld.
            gameManager.audioManager.StopJingle();


            // Nullifies the move offer for the next round.
            moveOffer = null;

            // Prepare for next battle.
            gotCritical = false;
            gotRecoil = false;
            order = 0;
            learnedMove = false;

            // The battle gets initialized everytime one starts.
            initialized = false;
            // Set to false in Initialize() function so that the post function can be called.
            postInitialized = true;

            // Reset the initial health and energy variables.
            // This may not be needed, but this is just to be safe.
            opponentInitHealth = -1;
            opponentInitMaxHealth = -1;
            opponentInitEnergy = -1;
            opponentInitMaxEnergy = -1;

            // Go to the overworld.
            gameManager.UpdateUI();

            // Checks if transitions are being used to know the right function to call.
            if (gameManager.useTransitions)
                gameManager.EnterOverworldWithTransition(battleWon);
            else
                gameManager.EnterOverworld(battleWon);
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
                    audioManager.PlayBackgroundMusic(battleBgm, 1.0F);
                    break;

                case 2: // Faster
                    audioManager.PlayBackgroundMusic(battleBgm, 1.2F);
                    break;

                case 3: // Faster
                    audioManager.PlayBackgroundMusic(battleBgm, 1.4F);
                    break;
            }
        }

        // Plays the treasure bgm.
        public void PlayTreasureBgm()
        {
            // Slower version of the battle theme.
            gameManager.audioManager.PlayBackgroundMusic(battleBgm, 0.8F);
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
            gameManager.audioManager.PlayBackgroundMusic(
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

            gameManager.audioManager.PlayJingle(battleWonJng, false, JNG_EXTRA_WAIT_TIME);
        }

        // Plays the battle lost jingle.
        public void PlayBattleLostJng()
        {
            // This will play when the jingle is done.
            PlayBattleResultsBgm();

            gameManager.audioManager.PlayJingle(battleLostJng, false, JNG_EXTRA_WAIT_TIME);
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
            float animTime = (playerAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length + EXTRA_ANIM_TIME) / playerAnimator.speed;

            // Old - use corotuine
            // Turn off the animation.
            StartCoroutine(AnimatorDisableDelayed(playerAnimator, animTime, false));

            // This new method kept throwing errors. Not sure if I'll rectify that or not.
            // // New - use timer class.
            // // playerAnimTimer.tag = "player"; // Since I never change the tag, this is unneeded.
            // playerAnimTimer.maxTime = animTime;
            // playerAnimTimer.Set();
            // playerAnimTimer.paused = false;
            // 
            // // Add the callback.
            // playerAnimTimer.OnTimerFinishedAddCallback(AnimatorDisableDelayed);
            // 
            // // Give to the timer manager.
            // TimerManager.Instance.AddTimer(playerAnimTimer);
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
            float animTime = (opponentAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length + EXTRA_ANIM_TIME) / opponentAnimator.speed;
            // Debug.Log(animTime);

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

            // NOTE: sometimes another battle animation is still playing, and it causes the death animation not to play.
            // Or at least I think that's what causes the death animation not to play consistently.
            // It rarely happens, but it is a glitch that exists.
            // I'm not fixing it.

            // Play animation by changing the value.
            opponentAnimator.SetInteger("anim", 5);
        }
        // Plays the death animation for the opponent.
        public void PlayDefaultOpponentAnimation()
        {
            // Return to the default animation.
            opponentAnimator.SetInteger("anim", 0);
        }

        // ANIMATION //
        // Sets the idle animation for the enemy with the provided animator.
        public bool PlayOpponentIdleAnimation()
        {
            // Checks to see if the change was successful.
            bool success = true;

            // If set to 'false', the sprite does not float.
            bool floatSprite = true;

            // The speed settings.
            float fastSpeed = 0.75F;
            float midSpeed = 0.50F;
            float slowSpeed = 0.25F;

            // The speed the float plays at.
            float speed = slowSpeed;

            // Checks the ID.
            switch (opponent.id)
            {
                case battleEntityId.treasure:
                    // Checks if the opponent is a treasure chest.
                    if(opponent is Treasure)
                    {
                        // Checks if the treasure is closed, or open.
                        if ((opponent as Treasure).closed)
                            opponentAnimator.Play("BEY - Treasure - Close");
                        else
                            opponentAnimator.Play("BEY - Treasure - Open");

                    }
                    else
                    {
                        // If the opponent isn't a treasure chest, just play the close animation.
                        opponentAnimator.Play("BEY - Treasure - Close");
                    }

                    // The sprite shouldn't float.
                    floatSprite = false;

                    break;

                case battleEntityId.combatBot:
                    opponentAnimator.Play("BEY - Combat Bot - Idle");

                    speed = slowSpeed;

                    break;

                case battleEntityId.ufo1:
                    opponentAnimator.Play("BEY - UFO 1 - Idle");

                    speed = midSpeed;

                    break;

                case battleEntityId.ufo2:
                    opponentAnimator.Play("BEY - UFO 2 - Idle");

                    speed = slowSpeed;

                    break;

                case battleEntityId.ufo3:
                    opponentAnimator.Play("BEY - UFO 3 - Idle");

                    speed = midSpeed;

                    break;

                case battleEntityId.insect1:
                    opponentAnimator.Play("BEY - Insect 1 - Idle");

                    speed = fastSpeed;

                    break;

                case battleEntityId.insect2:
                    opponentAnimator.Play("BEY - Insect 2 - Idle");

                    speed = fastSpeed;

                    break;

                case battleEntityId.spaceGhost1:
                    opponentAnimator.Play("BEY - Space Ghost 1 - Idle");

                    speed = slowSpeed;

                    break;

                case battleEntityId.spaceGhost2:
                    opponentAnimator.Play("BEY - Space Ghost 2 - Idle");

                    speed = slowSpeed;

                    break;

                case battleEntityId.comet:
                    opponentAnimator.Play("BEY - Comet - Idle");

                    speed = midSpeed;

                    break;

                case battleEntityId.sunRock1:
                    opponentAnimator.Play("BEY - Sun Rock 1 - Idle");

                    floatSprite = false;

                    break;

                case battleEntityId.sunRock2:
                    opponentAnimator.Play("BEY - Sun Rock 2 - Idle");

                    speed = midSpeed;

                    break;

                case battleEntityId.moonRock1:
                    opponentAnimator.Play("BEY - Moon Rock 1 - Idle");

                    floatSprite = false;

                    break;

                case battleEntityId.moonRock2:
                    opponentAnimator.Play("BEY - Moon Rock 2 - Idle");

                    speed = midSpeed;

                    break;

                case battleEntityId.fireBot1:
                    opponentAnimator.Play("BEY - Fire Bot 1 - Idle");

                    speed = midSpeed;

                    break;

                case battleEntityId.fireBot2:
                    opponentAnimator.Play("BEY - Fire Bot 2 - Idle");

                    speed = midSpeed;

                    break;

                case battleEntityId.waterBot1:
                    opponentAnimator.Play("BEY - Water Bot 1 - Idle");

                    floatSprite = false;

                    break;

                case battleEntityId.waterBot2:
                    opponentAnimator.Play("BEY - Water Bot 2 - Idle");

                    speed = slowSpeed;

                    break;

                case battleEntityId.earthBot1:
                    opponentAnimator.Play("BEY - Earth Bot 1 - Idle");

                    floatSprite = false;

                    break;

                case battleEntityId.earthBot2:
                    opponentAnimator.Play("BEY - Earth Bot 2 - Idle");

                    speed = slowSpeed;

                    break;

                case battleEntityId.airBot1:
                    opponentAnimator.Play("BEY - Air Bot 1 - Idle");

                    speed = fastSpeed;

                    break;

                case battleEntityId.airBot2:
                    opponentAnimator.Play("BEY - Air Bot 2 - Idle");

                    speed = fastSpeed;

                    break;

                case battleEntityId.sharp1:
                    opponentAnimator.Play("BEY - Sharp 1 - Idle");

                    floatSprite = false;

                    break;

                case battleEntityId.sharp2:
                    opponentAnimator.Play("BEY - Sharp 2 - Idle");

                    floatSprite = false;

                    break;

                case battleEntityId.virusRed1:
                    opponentAnimator.Play("BEY - Red Virus 1 - Idle");

                    speed = midSpeed;

                    break;

                case battleEntityId.virusRed2:
                    opponentAnimator.Play("BEY - Red Virus 2 - Idle");

                    speed = midSpeed;

                    break;

                case battleEntityId.virusBlue1:
                    opponentAnimator.Play("BEY - Blue Virus 1 - Idle");

                    speed = fastSpeed;

                    break;

                case battleEntityId.virusBlue2:
                    opponentAnimator.Play("BEY - Blue Virus 2 - Idle");

                    speed = fastSpeed;

                    break;

                case battleEntityId.virusYellow1:
                    opponentAnimator.Play("BEY - Yellow Virus 1 - Idle");

                    speed = midSpeed;

                    break;

                case battleEntityId.virusYellow2:
                    opponentAnimator.Play("BEY - Yellow Virus 2 - Idle");

                    speed = midSpeed;

                    break;

                case battleEntityId.blackHole:
                    opponentAnimator.Play("BEY - Black Hole - Idle");

                    speed = slowSpeed;

                    break;

                case battleEntityId.planet1:
                    opponentAnimator.Play("BEY - Planet 1 - Idle");

                    speed = slowSpeed;

                    break;

                case battleEntityId.planet2:
                    opponentAnimator.Play("BEY - Planet 2 - Idle");

                    speed = slowSpeed;

                    break;

                default:
                    opponentAnimator.Play("No Idle");

                    floatSprite = false;
                    success = false;
                    break;
            }

            // Reset the object to its reset position, and resets the float process.
            opponentFloat.SetObjectToResetPosition();
            opponentFloat.ResetProcess();


            // If the sprite shouldn't float.
            if (!floatSprite)
            {
                // Float settings.
                opponentFloat.paused = true;
                opponentFloat.speed = 1.0F;
            }
            else
            {
                opponentFloat.paused = false;
                opponentFloat.speed = speed;
            }

            return success;
        }


        // Stops the opponent idle animation.
        public void StopOpponentIdleAnimation()
        {
            opponentAnimator.Play("No Idle");

            // Reset the floating animation.
            opponentFloat.paused = true;
            opponentFloat.ResetProcess();
            opponentFloat.SetObjectToResetPosition();
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

        // // Disables the animator as part of a callback from the timer.
        // private void AnimatorDisableDelayed(TimerManager.Timer timer)
        // {
        //     // Gets a lower version of the string.
        //     string strLower = timer.tag.ToLower();
        // 
        //     // Disable's the player's animator.
        //     if (strLower == "player")
        //     {
        //         // Determines if the component should be disabled, or the object.
        //         if (PLAYER_ANIM_DISABLE_OBJECT)
        //             playerAnimator.gameObject.SetActive(false);
        //         else
        //             playerAnimator.enabled = false;
        //     }
        //     // Disable's the opponent's animator.
        //     else if(strLower == "opponent")
        //     {
        //         // Determines if the component should be disabled, or the object.
        //         if (OPPONENT_ANIM_DISABLE_OBJECT)
        //             opponentAnimator.gameObject.SetActive(false);
        //         else
        //             opponentAnimator.enabled = false;
        //     }
        // 
        //     // Remove this callback now that the timer is done.
        //     timer.OnTimerFinishedRemoveCallback(AnimatorDisableDelayed);
        // }

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
            // If the PostInitialize() function hasn't been called, call it.
            if(!postInitialized)
            {
                PostInitalize();
            }

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
                        // Restore all health.
                        player.SetHealthToMax();
                        UpdatePlayerHealthUI();

                        // Restore all energy.
                        player.SetEnergyToMax();
                        UpdatePlayerEnergyUI();

                        // Reset statuses and stat modifiers.
                        player.ResetStatuses();
                        player.ResetStatModifiers();

                        // Tutorial for first battle death.
                        if (!gameManager.tutorial.clearedFirstBattleDeath)
                            gameManager.tutorial.LoadFirstBattleDeathTutorial();
                    }

                    // TODO: if this is used, the HP hits 0, then goes to 1.
                    // In practice this case should never be encountered.

                    // If it's the first turn and the opponent is dead, give them 1 HP back.
                    // The other tutorials won't happen if the enemy dies in one turn.
                    if (gameManager.roomsCompleted == 0 && opponent.IsDead() && turnsPassed <= 1)
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
                        if (!trl.clearedFirstMove && turnsPassed != 0)
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

                            // Set the textbox settings.
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
                                uint oldLevel = player.Level;
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

                                // If this is the first battle, restore the player's health and energy to their max.
                                // Only for the first battle though.
                                if (gameManager.useTutorial && gameManager.roomsCompleted == 0)
                                {
                                    player.SetHealthToMax();
                                    player.SetEnergyToMax();
                                }

                                // NOTE: no longer shows energy levels since those don't matter anymore.
                                // Adds page with the increases in stats.
                                textBox.pages.Add(new Page(
                                    gameManager.LevelString + " +" + (player.Level - oldLevel).ToString() + "\n" +
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

                            // Score Page
                            {
                                // Calculates the battle score.
                                float battleScore = CalculateBattleScore();

                                // Adds a page for showing the battle score.
                                textBox.pages.Add(new Page(
                                    gameManager.ScoreString + " +" + Mathf.RoundToInt(battleScore).ToString() + "\n" +
                                    gameManager.ScoreString + " = " + Mathf.RoundToInt(gameManager.score + battleScore).ToString()
                                    ));
                            }

                            // Checks to see if a new move should be learned.
                            bool learningMove = (GenerateRandomFloat01() <= NEW_MOVE_CHANCE || opponent is Treasure);

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
                                // Checks if the opponent was a treasure or not.
                                // If it was, the player can learn from multiple moves.
                                if(opponent is Treasure) // Offer multiple moves.
                                {
                                    // Generates the page.
                                    Page multiMovePage = new Page(
                                        BattleMessages.Instance.GetMultipleMoveOfferMessage(),
                                        BattleMessages.Instance.GetMultipleMoveOfferSpeakKey()
                                        );

                                    // Add the callback, and put the page in the list.
                                    multiMovePage.OnPageClosedAddCallback(OnMultipleMovesOffered);
                                    textBox.pages.Add(multiMovePage);

                                    // Needed for the learn move panel to skip through.
                                    // This page is active when learning the move.
                                    textBox.pages.Add(new Page("..."));
                                }
                                else // Only offer one move.
                                {
                                    // Generates the page.
                                    Page newMovePage = new Page(
                                        BattleMessages.Instance.GetLearnMoveMessage(),
                                        BattleMessages.Instance.GetLearnMoveSpeakKey()
                                        );
                                    
                                    // Add the callback, and put the page in the list.
                                    newMovePage.OnPageClosedAddCallback(OnLearningNewMove);
                                    textBox.pages.Add(newMovePage);
                                    
                                    // Needed for the learn move panel to skip through.
                                    // This page is active when learning the move.
                                    textBox.pages.Add(new Page("..."));
                                }
                                
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