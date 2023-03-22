using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SimpleJSON;
using TMPro;
using LoLSDK;
using UnityEngine.UI;

namespace RM_BBTS
{
    // General manager for the gameplay.
    public class GameplayManager : GameState
    {
        // the state of the game.
        private gameState state = gameState.none;

        // the manager for the overworld.
        public OverworldManager overworld;

        // the manager for the battle.
        public BattleManager battle;

        // the input from the mouse and touch screen.
        public MouseTouchInput mouseTouchInput;

        // The player for the game.
        public Player player;

        // The tutorial object.
        public Tutorial tutorial;

        // If set to 'true' then the tutorial is used.
        public bool useTutorial = true;

        // Becomes set to 'true' when postStart has been called.
        private bool calledPostStart = false;

        // The name of the results scene.
        public const string RESULTS_SCENE_NAME = "ResultsScene";

        [Header("Game Stats")]

        // The score for the game.
        public int score = 0;

        // The total rooms in the game.
        // I just made a function for this since it's equal to the total amount of doors.
        // public int roomsTotal = 0;

        // The total amount of completed rooms.
        public int roomsCompleted = 0;
        
        // The amount of rooms completed for the enemies to level up.
        public const int ROOMS_PER_LEVEL_UP = 2; // Originally 3

        // The last time the enemies were leveled up (is room the player is on).
        public int lastEnemyLevelUps = -1;

        // Shows how many times evolution waves have been done
        public int evolveWaves = 0;

        // The maximum amount of evolution waves allowed in a single game.
        public const int EVOLVE_WAVES_MAX = 2;

        // String labels for each stat (used for translation).
        private string levelString = "Level";
        private string healthString = "Health";
        private string attackString = "Attack";
        private string defenseString = "Defense";
        private string speedString = "Speed";
        private string energyString = "Energy";

        // Move characteristics.
        private string rankString = "Rank";
        private string powerString = "Power";
        private string accuracyString = "Accuracy";
        private string descriptionString = "Description";

        // Score label.
        private string scoreString = "Score";

        [Header("Game Stats/Time")]

        // The total amount of turns completed.
        public int totalTurnsPassed = 0;

        // The time the game has been going for.
        // This uses deltaTime, which is in seconds.
        public float gameTimer = 0.0F;

        // If set to 'true' the game timer is paused.
        public bool pausedTimer = false;

        // Pauses the timer when the tutorial box is active.
        public bool pauseTimerWhenTutorial = true;

        [Header("UI")]
        // A back panel used to cover up gameplay UI.
        public GameObject backPanel;

        // The text that's displayed during saving.
        public TMP_Text saveFeedbackText;

        [Header("UI/Stats Window")]

        // Title text for stats button.
        public TMP_Text statsButtonText;

        // The player stats window.
        public PlayerStatsWindow statsWindow;

        [Header("UI/Save Prompt")]
        // The save button.
        public Button saveButton;

        // The save button text.
        public TMP_Text saveButtonText;

        // The save window.
        public GameObject savePrompt;

        // The save prompt text.
        public TMP_Text savePromptText;

        // The speak key for the save prompt.
        private const string SAVE_PROMPT_TEXT_KEY = "sve_msg_prompt";

        // The save and continue text.
        public TMP_Text saveAndContinueText;

        // The save and quit text.
        public TMP_Text saveAndQuitText;

        // The back button text.
        public TMP_Text savePromptBackText;

        [Header("UI/Settings Window")]
        // Title text for settings button.
        public TMP_Text settingsButtonText;

        // The settings window.
        public SettingsMenu settingsWindow;

        [Header("UI/Main Menu Prompt")]
        // The main menu button.
        public Button mainMenuButton;

        // The main menu button text.
        public TMP_Text mainMenuButtonText;

        // The main menu window.
        public GameObject mainMenuPrompt;

        // The key for the main menu prompt.
        private const string MAIN_MENU_PROMPT_TEXT_KEY = "mmu_msg_prompt";

        // The prompt text for the main menu.
        public TMP_Text mainMenuPromptText;

        // The main menu (to title screen) confirmation text.
        public TMP_Text mainMenuYesText;

        // The continue game confirmation text.
        public TMP_Text mainMenuNoText;

        [Header("UI/Info Prompt")]
        // The info button.
        public Button infoButton;

        // The info button text.
        public TMP_Text infoButtonText;

        // The info window.
        public GameObject infoWindow;



        [Header("UI/Game")]

        // The text box used for general messages.
        // This is used for the tutorial, which also saves this object.
        public TextBox textBox;

        // The battle number text.
        public TMPro.TMP_Text battleNumberText;

        // Determines if text transitions are being used so that they sync with the progress bars.
        // If not, change the text instantly. If so, update the bars.
        public bool syncTextToBars = true;

        // The health bar for the player.
        public ProgressBar playerHealthBar;

        // The text for the player's health
        public TMP_Text playerHealthText;

        // Becomes set to 'true' when the player's health is transitioning.
        private bool playerHealthTransitioning = false;

        // The health bar for the player.
        public ProgressBar playerEnergyBar;

        // The text for the player's enegy
        public TMP_Text playerEnergyText;

        // The amount of decimal places used for displaying percentages.
        public const int DISPLAY_DECIMAL_PLACES = 2;

        // Becomes set to 'true' when the player's energy is transitioning.
        private bool playerEnergyTransitioning = false;

        [Header("Audio")]
        // The source for the background music audio source.
        public AudioManager audioManager;

        // // The bgm for the overworld.
        // public AudioClip overworldBgm;
        // 
        // // Battle bgm.
        // public AudioClip battleBgm;
        // 
        // // The boss BGM.
        // public AudioClip bossBgm;
        // 
        // // The jingle for winning a battle.
        // public AudioClip battleWonJng;
        // 
        // // The jingle for losing a battle.
        // public AudioClip battleLostJng;

        [Header("Animations")]

        // The overworld background
        public SpriteRenderer overworldBackground;

        // The battle background
        public SpriteRenderer battleBackground;

        // The background animator.
        public Animator battleBackgroundAnimator;

        // Transitions should be used.
        public bool useTransitions = true;

        // The scene transition object.
        public SceneTransition sceneTransition;

        // The room transition object.
        public RoomTransition roomTransition;

        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            // Turns on the overworld object and its ui.
            overworld.gameObject.SetActive(false);
            overworld.ui.SetActive(false);

            // Turns off the battle object and its ui.
            battle.gameObject.SetActive(false);
            battle.ui.SetActive(false);

            // Tutorial settings.
            if(FindObjectOfType<GameSettings>() != null)
                useTutorial = GameSettings.Instance.UseTutorial;

            // Translation
            JSONNode defs = SharedState.LanguageDefs;

            // Translate all of the string objects.
            if (defs != null)
            {
                // Windows/Prompts
                // STATS WINDOW
                statsButtonText.text = defs["kwd_stats"];

                // SAVE PROMPT //
                saveButtonText.text = defs["kwd_save"];
                savePromptText.text = defs[SAVE_PROMPT_TEXT_KEY];
                saveAndContinueText.text = defs["kwd_saveContinue"];
                saveAndQuitText.text = defs["kwd_saveQuit"];
                savePromptBackText.text = defs["kwd_back"];

                // SETTINGS WINDOW //
                settingsButtonText.text = defs["kwd_settings"];

                // MAIN MENU (TITLE SCREEN) PROMPT
                mainMenuButtonText.text = defs["kwd_mainMenu"];
                mainMenuPromptText.text = defs[MAIN_MENU_PROMPT_TEXT_KEY];
                mainMenuYesText.text = defs["kwd_returnToMainMenu"];
                mainMenuNoText.text = defs["kwd_returnToGame"];

                // INFO WINDOW
                infoButtonText.text = defs["kwd_info"];

                // String Labels
                // Moves
                levelString = defs["kwd_level"];
                healthString = defs["kwd_health"];
                attackString = defs["kwd_attack"];
                defenseString = defs["kwd_defense"];
                speedString = defs["kwd_speed"];
                energyString = defs["kwd_energy"];

                // Move characteristics.
                rankString = defs["kwd_rank"];
                powerString = defs["kwd_power"];
                accuracyString = defs["kwd_accuracy"];
                descriptionString = defs["kwd_description"];

                // The score string.
                scoreString = defs["kwd_score"];
            }
            else
            {
                // For marking text to show it's not loaded from the language file.
                LanguageMarker marker = LanguageMarker.Instance;

                // Windows/Prompts
                // STATS WINDOW
                marker.MarkText(statsButtonText);

                // SAVE PROMPT //
                marker.MarkText(saveButtonText);
                marker.MarkText(savePromptText);
                marker.MarkText(saveAndContinueText);
                marker.MarkText(saveAndQuitText);
                marker.MarkText(savePromptBackText);

                // SETTINGS WINDOW //
                marker.MarkText(settingsButtonText);

                // MAIN MENU (TITLE SCREEN) PROMPT
                marker.MarkText(mainMenuButtonText);
                marker.MarkText(mainMenuPromptText);
                marker.MarkText(mainMenuYesText);
                marker.MarkText(mainMenuNoText);

                // INFO WINDOW
                marker.MarkText(infoButtonText);
            }


            // Turns off the entrance animation if scene transitions shouldn't be used.
            sceneTransition.useSceneEnterAnim = useTransitions;
        }

        // Start is called before the first frame update
        void Start()
        {
            // // Finds the mouse touch input object.
            // if (mouseTouchInput == null)
            //     mouseTouchInput = FindObjectOfType<MouseTouchInput>();

            // Provides the save feedback text.
            if (saveFeedbackText != null && LOLSDK.Instance.IsInitialized)
            {
                saveFeedbackText.text = string.Empty;
                LOLManager.Instance.saveSystem.feedbackText = saveFeedbackText;
            }
            else
            {
                // Just empty out the string.
                saveFeedbackText.text = string.Empty;
            }

            // Initialize
            Initialize();           
        }

        // A function called after the all awake and start functions have been called.
        // This is needed so that everything is initialized properly.
        void PostStart()
        {
            // If there is save data, load the saved game.
            if (LOLManager.Instance.saveSystem.HasLoadedData())
            {
                // Load the data.
                // This function also updates the UI once the data is loaded.
                // This now does not allow completed game data to be reloaded.
                bool success = LoadGame(LOLManager.Instance.saveSystem.loadedData, false);

                // Clear data.
                LOLManager.Instance.saveSystem.ClearLoadedData();

                // Data successfully loaded.
                if(success)
                {
                    // Gets set to 'true' if a new tutorial was loaded.
                    bool loadedTutorial = false;

                    // If the tutorial textbox is open.
                    if (tutorial.TextBoxIsVisible())
                    {
                        // Closes the textbox.
                        tutorial.CloseTextBox();

                        // Stops the text-to-speech if it's active.
                        if (GameSettings.Instance.UseTextToSpeech)
                            LOLManager.Instance.textToSpeech.StopSpeakText();

                        // Checks to see if any oveworld tutorials need to be run.
                        // However, the player shouldn't be able to save before the intro happened.
                        // Due to where auto save occurs, the overworld tutorial hasn't necessarily been shown yet.
                        if (!tutorial.clearedIntro) // Intro
                        {
                            tutorial.LoadIntroTutorial();
                            loadedTutorial = true;
                        }
                        else if (!tutorial.clearedOverworld) // Overworld
                        {
                            tutorial.LoadOverworldTutorial();
                            loadedTutorial = true;
                        }

                        // There is a text-to-speech glitch that happens due to the textbox being open.
                        // It reads the textbox that was closed when it shouldn't, but there's no workaround there.

                        // The game over tutorial would also show up on the overworld.
                        // However, the game doesn't save after a game over, so it would end up getting triggered twice...
                        // If the user quits without saving. That's fine though.
                    }

                    // If a tutorial wasn't loaded, stop the speak text from the closed textbox.
                    if(!loadedTutorial && LOLSDK.Instance.IsInitialized && GameSettings.Instance.UseTextToSpeech)
                    {
                        // Stop the speak text does not work here, so you need to have it say something else.
                        // LOLManager.Instance.textToSpeech.StopSpeakText();

                        // Uses an alternate message to stop the closed textbox's text from being read.
                        // NOTE: this does not work when hosted though the LOL website itself.
                        LOLManager.Instance.textToSpeech.SpeakText("owd_loadSuccess_msg");
                    }
                        

                }
                
            }
            else
            {
                // TODO: uncomment and remove when not testing the save feature.
                // Loads the test data for the game.
                // LoadGameTest();
            }

            // Called so that game phase content gets updated.
            overworld.OnGamePhaseChange();

            // The post start function was called.
            calledPostStart = true;
        }

        // Initializes the gameplay manager.
        public override void Initialize()
        {
            overworld.Initialize();
            overworld.gameObject.SetActive(true);

            // Starting in the overworld state.
            state = gameState.overworld;

            // Update the UI.
            UpdateUI();

            // List<string> test = new List<string>() { "This is a test.", "This is only a test." };
            // // textBox.OnTextFinishedAddCallback(Test);
            // // textBox.OnTextFinishedRemoveCallback(Test);
            // textBox.ReplacePages(test);

            // Show the textbox automatically when loading text.
            tutorial.showTextboxOnLoad = true;

            // Load the intro tutorial.
            if (useTutorial && !tutorial.clearedIntro)
            {
                tutorial.LoadIntroTutorial();

                // The tutorial door count.
                const int TRL_DOOR_COUNT = 1;

                // If there are enough doors to lock, lock down some.
                // I did a +1 so that the boss room is also ignored.
                if (TRL_DOOR_COUNT > 0 && overworld.treasureDoors.Count + 1 != overworld.doors.Count)
                {
                    // Copies the list.
                    List<Door> battleDoors = new List<Door>(overworld.doors);
                    
                    // The list of unused doors (does not include boss and treasures).
                    // If the battleDoor list doesn't have enough doors, some are added back.
                    List<Door> unusedDoors = new List<Door>();

                    // Locks and removes the special doors from the list.
                    for (int i = battleDoors.Count - 1; i >= 0; i--)
                    {
                        battleDoors[i].Locked = true; // Locks the door.

                        // Removes the special door from the list.
                        // Boss and treasure rooms are locked by default.
                        if (battleDoors[i].isBossDoor || battleDoors[i].isTreasureDoor)
                        {
                            battleDoors.RemoveAt(i); // Remove from list.
                        }
                        // If it isn't a tutorial enemy, and if there are more than 3 potential options left...
                        // Remove the door from the list.
                        else if (!BattleEntityList.IsTutorialEnemy(battleDoors[i].battleEntity.id) && battleDoors.Count > TRL_DOOR_COUNT)
                        {
                            unusedDoors.Add(battleDoors[i]); // Remembers the removed door.
                            battleDoors.RemoveAt(i); // Remove from list.
                        }
                    }

                    // While the battle door count is less than the tutorial door count.
                    while(battleDoors.Count < TRL_DOOR_COUNT && unusedDoors.Count > 0)
                    {
                        // Adds an element back into the list, and removes it.
                        int index = Random.Range(0, unusedDoors.Count);
                        battleDoors.Add(unusedDoors[index]);
                        unusedDoors.RemoveAt(index);
                    }

                    // Unlocks (X) amount of random doors.
                    for (int n = 0; n < TRL_DOOR_COUNT && battleDoors.Count > 0; n++)
                    {
                        // Grabs a random index.
                        int randIndex = Random.Range(0, battleDoors.Count);

                        // TODO: test this.
                        // If the enemy is not a tutorial enemy, replace it with one.
                        if (!BattleEntityList.IsTutorialEnemy(battleDoors[randIndex].battleEntity.id))
                        {
                            // Copies the level. 
                            uint oldLevel = battleDoors[randIndex].battleEntity.level;

                            // Generates a tutorial enemy to replace thi one.
                            battleDoors[randIndex].battleEntity = BattleEntityList.Instance.GenerateTutorialEnemy();

                            // Level up the new data if the level is less than the old level.
                            if(battleDoors[randIndex].battleEntity.level != oldLevel)
                            {
                                // How many times the enemy should level up.
                                uint times = 0;

                                // Checks which level is higher for proper subtraction.
                                if (oldLevel > battleDoors[randIndex].battleEntity.level)
                                    times = oldLevel - battleDoors[randIndex].battleEntity.level;
                                else if (oldLevel < battleDoors[randIndex].battleEntity.level)
                                    times = battleDoors[randIndex].battleEntity.level - oldLevel;

                                // Level up the entity.
                                battleDoors[randIndex].battleEntity = BattleEntity.LevelUpData(
                                    battleDoors[randIndex].battleEntity,
                                    battleDoors[randIndex].battleEntity.levelRate,
                                    battleDoors[randIndex].battleEntity.statSpecial,
                                    times);
                            }
                        }


                        // Unlocks the door, and removes it from the list.
                        battleDoors[randIndex].Locked = false;
                        battleDoors.RemoveAt(randIndex);
                    }
                }

            }


            // Submits a base score of 0 with 0 battles completed.
            // This is to kick off the start of the game.
            score = 0;
            roomsCompleted = 0;
            SubmitProgress();                
        }

        // public void Test()
        // {
        //     Debug.Log("Test");
        // }

        // Called when the mouse hovers over an object.
        public override void OnMouseHovered(GameObject hoveredObject)
        {
            // ...
        }

        // Called when the mouse interacts with an entity.
        public override void OnMouseInteract(GameObject heldObject)
        {
            //// Collider for text-to-speech (contains a function for OnMouseDown).
            //TextToSpeechCollider ttsCol;

            //// Checks for the collider.
            //if (heldObject.TryGetComponent<TextToSpeechCollider>(out ttsCol))
            //{
            //    ttsCol.SpeakText();
            //}

            switch (state)
            {
                case gameState.overworld:
                    overworld.OnMouseInteract(heldObject);
                    break;

                case gameState.battle:
                    battle.OnMouseInteract(heldObject);
                    break;
            }
        }

        // Called when the user's touch interacts with an entity.
        public override void OnTouchInteract(GameObject touchedObject, Touch touch)
        {
            //// Collider for text-to-speech (contains a function for OnMouseDown).
            //TextToSpeechCollider ttsCol; 

            //// Checks for the collider.
            //if(touchedObject.TryGetComponent<TextToSpeechCollider>(out ttsCol))
            //{
            //    ttsCol.SpeakText();
            //}

            // Calls the touch interacts for the proper object.
            switch (state)
            {
                case gameState.overworld:
                    overworld.OnTouchInteract(touchedObject, touch);
                    break;

                case gameState.battle:
                    battle.OnTouchInteract(touchedObject, touch);
                    break;
            }
        }

        // Called with the object that was received with the interaction.
        protected override void OnInteractReceive(GameObject gameObject)
        {
            throw new System.NotImplementedException();
        }

        // Returns the level string.
        public string LevelString
        {
            get { return levelString; }
        }

        // Returns the health string.
        public string HealthString
        {
            get { return healthString; }
        }

        // Returns the attack string.
        public string AttackString
        {
            get { return attackString; }
        }

        // Returns the defense string.
        public string DefenseString
        {
            get { return defenseString; }
        }

        // Returns the speed string.
        public string SpeedString
        {
            get { return speedString; }
        }

        // Returns the energy string.
        public string EnergyString
        {
            get { return energyString; }
        }

        // Returns the rank string.
        public string RankString
        {
            get { return rankString; }
        }

        // Returns the power string.
        public string PowerString
        {
            get { return powerString; }
        }

        // Returns the accuracy string.
        public string AccuracyString
        {
            get { return accuracyString; }
        }

        // Returns the description string.
        public string DescriptionString
        {
            get { return descriptionString; }
        }

        // Returns the score string.
        public string ScoreString
        {
            get { return scoreString; }
        }

        // Sets the game stat.
        public void SetState(gameState newState)
        {
            // Sets the new state.
            state = newState;

            // State parameters.
            switch (state)
            {
                default:
                case gameState.none:
                    // Turn this on just in case, though this shouldn't be a problem.
                    saveButton.interactable = true;
                    break;

                case gameState.overworld: // Overworld

                    // If tutorial text isn't being shown, enable the save button.
                    // This may still be disabled elsewhere depending on where the tutorial is placed.
                    if (!tutorial.TextBoxIsVisible())
                        saveButton.interactable = true;
                    else
                        saveButton.interactable = false;

                    break;
                case gameState.battle: // Battle
                    // Can't save during battle.
                    saveButton.interactable = false;
                    break;
            }
        }

        // Sets to the overworld state.
        public void SetStateToOverworld()
        {
            SetState(gameState.overworld);
        }

        // Sets to the battle state.
        public void SetStateToBattle()
        {
            SetState(gameState.battle);
        }

        // Hides all the windows and prompts.
        private void HideAllWindowsAndPrompts()
        {
            // Time should move normally.
            Time.timeScale = 1.0F;

            // Hide the back panel.
            backPanel.gameObject.SetActive(false);

            // Turn off all prompts.
            statsWindow.gameObject.SetActive(false);
            savePrompt.gameObject.SetActive(false);
            settingsWindow.gameObject.SetActive(false);
            mainMenuPrompt.gameObject.SetActive(false);
            infoWindow.gameObject.SetActive(false);

            // Enable mouse input.
            mouseTouchInput.gameObject.SetActive(true);

            
            // Checks if the timer should be paused when the tutorial box is open.
            if (pauseTimerWhenTutorial)
            {
                // If the tutorial box isn't open, unpause the timer.
                if (!tutorial.TextBoxIsVisible())
                    pausedTimer = false;
            }
            else
            {
                // Enable timer.
                pausedTimer = false;
            }
        }

        // Called when toggling a window or prompt.
        // 'Active' determines if the prompt is going to be active or not.
        private void OnToggleWindowOrPrompt(bool active)
        {
            // If a prompt is being turned on, set the time scale to 0 so that game events do not happen.
            // If a prompt is being turned off, set time scale to 1 so that game events do happen.
            Time.timeScale = (active) ? 0.0F : 1.0F;

            // Change the back panel and mouse touch settings.
            backPanel.gameObject.SetActive(active);
            mouseTouchInput.gameObject.SetActive(!active);

            // Checks if the timer should be paused when the tutorial box is open.
            if (pauseTimerWhenTutorial)
            {
                // If the tutorial box isn't open, switch the pause timer.
                if (!tutorial.TextBoxIsVisible())
                    pausedTimer = active;
            }
            else
            {
                // The timer doesn't run when the windows are open.
                pausedTimer = active;
            }
        }

        // Opens the player stats window.
        public void TogglePlayerStatsWindow()
        {
            // Gets the change in activity.
            bool active = !statsWindow.gameObject.activeSelf;

            // Hides all the windows and prompts.
            HideAllWindowsAndPrompts();

            // Shows (or hides) the specific window/prompt.
            statsWindow.gameObject.SetActive(active);

            // Called since the window/prompt is being toggled.
            OnToggleWindowOrPrompt(active);

            // This could be simplified, but this is easier to read.
            // Checks if the timer should be paused when the tutorial box is open.
            if(pauseTimerWhenTutorial)
            {
                // If the tutorial textbox is not open, run the timer.
                if (!tutorial.TextBoxIsVisible())
                    pausedTimer = false;
            }
            else
            {
                // The stats window does NOT pause the timer like the other windows. 
                pausedTimer = false;
            }
            
        }


        // Toggles the save prompt.
        public void ToggleSavePrompt()
        {
            // Gets the change in activity.
            bool active = !savePrompt.gameObject.activeSelf;

            // Hides all the windows and prompts.
            HideAllWindowsAndPrompts();

            // Shows (or hides) the specific window/prompt.
            savePrompt.gameObject.SetActive(active);

            // Called since the window/prompt is being toggled.
            OnToggleWindowOrPrompt(active);

            // If the save prompt is being shown.
            if (active)
            {
                // If the SDK is initialized, text-to-speech is being used, and the speak key has been set.
                if (LOLSDK.Instance.IsInitialized && GameSettings.Instance.UseTextToSpeech && SAVE_PROMPT_TEXT_KEY != "")
                {
                    // Read out the mssage.
                    LOLManager.Instance.textToSpeech.SpeakText(SAVE_PROMPT_TEXT_KEY);
                }
            }
        }

        // Opens the settings window.
        public void ToggleSettingsWindow()
        {
            // Gets the change in activity.
            bool active = !settingsWindow.gameObject.activeSelf;

            // Hides all the windows and prompts.
            HideAllWindowsAndPrompts();

            // Shows (or hides) the specific window/prompt.
            settingsWindow.gameObject.SetActive(active);

            // Called since the window/prompt is being toggled.
            OnToggleWindowOrPrompt(active);
        }


        // Toggles the main menu prompt.
        public void ToggleMainMenuPrompt()
        {
            // Gets the change in activity.
            bool active = !mainMenuPrompt.gameObject.activeSelf;

            // Hides all the windows and prompts.
            HideAllWindowsAndPrompts();

            // Shows (or hides) the specific window/prompt.
            mainMenuPrompt.gameObject.SetActive(active);

            // Called since the window/prompt is being toggled.
            OnToggleWindowOrPrompt(active);

            // If the main menu prompt is being shown.
            if (active)
            {
                // If the SDK is initialized, text-to-speech is being used, and the speak key has been set.
                if (LOLSDK.Instance.IsInitialized && GameSettings.Instance.UseTextToSpeech && MAIN_MENU_PROMPT_TEXT_KEY != "")
                {
                    // Read out the mssage.
                    LOLManager.Instance.textToSpeech.SpeakText(MAIN_MENU_PROMPT_TEXT_KEY);
                }
            }
        }

        // Opens the info window.
        public void ToggleInfoWindow()
        {
            // Gets the change in activity.
            bool active = !infoWindow.gameObject.activeSelf;

            // Hides all the windows and prompts.
            HideAllWindowsAndPrompts();

            // Shows (or hides) the specific window/prompt.
            infoWindow.gameObject.SetActive(active);

            // Called since the window/prompt is being toggled.
            OnToggleWindowOrPrompt(active);
        }


        // BACKGROUNDS //
        // Shows the overworld background.
        public void EnableOverworldBackground()
        {
            battleBackgroundAnimator.StopPlayback();
            battleBackground.gameObject.SetActive(false);
            battleBackground.sprite = null;
            battleBackground.color = Color.white;

            overworldBackground.gameObject.SetActive(true);
        }

        // Show the battle background.
        public void EnableBattleBackground(string stateName, Color color)
        {
            // Turn off overworld background.
            overworldBackground.gameObject.SetActive(false);

            // Turn on battle background.
            battleBackground.gameObject.SetActive(true);

            // Sets the background colour, and reduces the brightest of the background slightly.
            Color bgColor = color * 0.99F;
            color.a = 1.0F;
            battleBackground.color = bgColor;

            // Play the background animation.
            battleBackgroundAnimator.Play(stateName);
        }

        // MOUSE

        // Checks the mouse and touch to see if there's any object to use.
        public void MouseTouchCheck()
        {
            // The object that was interacted with.
            GameObject hitObject;

            // TODO: hovered check.


            // Tries grabbing the mouse object.
            hitObject = mouseTouchInput.mouseHeldObject;

            // The hit object was not found from the mouse, so check the touch instead.
            if (hitObject == null)
            {
                // Grabs the last object in the list.
                if (mouseTouchInput.touchObjects.Count > 0)
                {
                    // Grabs the index.
                    int index = mouseTouchInput.touchObjects.Count - 1;

                    // Saves the hit object.
                    hitObject = mouseTouchInput.touchObjects[index];
                    Touch touch = mouseTouchInput.currentTouches[index];

                    // Checks the state variable to see what kind of scene the game is in.
                    // Calls the appropriate touch interaction.
                    // NOTE: for some reason this wasn't being set.
                    if(hitObject != null)
                        OnTouchInteract(hitObject, touch);

                    // switch (state)
                    // {
                    //     case gameState.overworld:
                    //         overworld.OnTouchInteract(hitObject, touch);
                    //         break;
                    // 
                    //     case gameState.battle:
                    //         battle.OnTouchInteract(hitObject, touch);
                    //         break;
                    // }
                }
                
            }
            else
            {
                // Checks the state variable to see what kind of scene the game is in.
                OnMouseInteract(hitObject);

                // switch (state)
                // {
                //     case gameState.overworld:
                //         overworld.OnMouseInteract(hitObject);
                //         break;
                // 
                //     case gameState.battle:
                //         battle.OnMouseInteract(hitObject);
                //         break;
                // }
            }

            // Print message for testing.
            // if(hitObject != null)
            //     Debug.Log("Hit Found");

        }

        // A function to call when a tutorial starts.
        public override void OnTutorialStart()
        {
            // Call both in case the state hasn't changed.
            // TODO: maybe call only one.
            overworld.OnTutorialStart();
            battle.OnTutorialStart();

            // Turns off the mouse touch input. 
            mouseTouchInput.gameObject.SetActive(false);

            // Disables the save button.
            saveButton.interactable = false;

            // The timer is paused when the tutorial text is being displayed.
            if(pauseTimerWhenTutorial)
                pausedTimer = true;
        }

        // A function to call when a tutorial ends.
        public override void OnTutorialEnd()
        {
            // // Checks the game state.
            // switch (state)
            // {
            //     case gameState.overworld: // overworld
            //         overworld.OnTutorialEnd();
            //         break;
            // 
            //     case gameState.battle: // battle
            //         battle.OnTutorialEnd();
            //         break;
            // }

            // Call both in case the state hasn't changed.
            overworld.OnTutorialEnd();
            battle.OnTutorialEnd();

            // Turns off the mouse touch input. 
            mouseTouchInput.gameObject.SetActive(true);

            // If in the overworld, enable the save button.
            // The player can't save if they haven't completed the tutorial battle (while in the tutorial).
            if (state == gameState.overworld) // Might be able to save during overworld.
            {
                // The save button won't be activated if the first tutorial battle hasn't been cleared.
                if(useTutorial && roomsCompleted == 0)
                    saveButton.interactable = false;
                else
                    saveButton.interactable = true;
            }
            else if (state == gameState.battle) // Can't save during battle.
            {
                saveButton.interactable = false;
            }


            // The timer is paused when the tutorial text is displayed.
            if (pauseTimerWhenTutorial)
                pausedTimer = false;
        }

        // Called when a question is given to the user.
        public void OnQuestionStart()
        {
            // Disables the mouse.
            mouseTouchInput.gameObject.SetActive(false);

            // Disables the save button and the main menu button.
            saveButton.interactable = false;
            mainMenuButton.interactable = false;
        }

        // Called when a question has been finished.
        public void OnQuestionEnd()
        {
            mouseTouchInput.gameObject.SetActive(true);

            // If in the overworld, enable the save button.
            // If in a battle, disable the save button.
            // The player can't save during battles.
            if (state == gameState.overworld)
            {
                // In overworld, so activate save button.
                saveButton.interactable = true;
            }
            else if (state == gameState.battle)
            {
                // In battle, so keep save button disabled.
                saveButton.interactable = false;
            }

            // Enables the main menu button.
            mainMenuButton.interactable = true;

            // The 'useTutorial' object in this class could also be referenced, but I don't think it matters.
            if(GameSettings.Instance.UseTutorial)
            {
                // Loads the stat change tutorial if it hasn't happened already.
                if (player.HasStatModifiers() && !tutorial.clearedStatChange)
                    tutorial.LoadStatChangeTutorial();
            }

            // Save that the player answered a question.
            SaveAndContinueGame();
        }

        // Returns the amount of completed rooms.
        public int GetRoomsCompleted()
        {
            return roomsCompleted;
        }

        // Checks if the game is complete.
        public bool IsGameComplete()
        {
            // Checks if the amount of rooms completed is equal to the room total.
            // If it is, then the game is complete.
            bool result = GetRoomsCompleted() == GetRoomsTotal();

            // Returns the result.
            return result;
        }

        // Gets the number of the current round.
        public int GetCurrentRoomNumber()
        {
            return roomsCompleted + 1;
        }

        // Returns the total amount of rooms.
        public int GetRoomsTotal()
        {
            // Total amount of rooms.
            int roomsTotal = overworld.GetDoorCount();

            // Grabs for the door count to make sure it's consistent with what's acutally there.
            // TODO: this shouldn't be needed, so maybe take this out?
            if (overworld.doors.Count != 0 && overworld.doors.Count != roomsTotal)
                roomsTotal = overworld.doors.Count;

            // Checks to see if the room total is accurate.
            if(roomsTotal != OverworldManager.ROOM_COUNT)
            {
                Debug.LogWarning("The game's set room count does not match to the actual amount of doors in the list.");
            }

            return roomsTotal;

        }

        // Returns the phase of the game (1 = start, 2 = middle, 3 = end).
        // Each section is evenly split.
        public int GetGamePhase()
        {
            // The completion rate.
            float completionRate = roomsCompleted / (float)GetRoomsTotal();

            // Returns the game phase.
            if (completionRate < OverworldManager.PHASE_2_THRESOLD)
                return 1;
            else if (completionRate < OverworldManager.PHASE_3_THRESOLD)
                return 2;
            else
                return 3;

        }

        // Call this function to enter the overworld.
        public void EnterOverworld(bool battleWon)
        {
            // TODO: play animation before transition.
            // if(useTransitions)


            battle.gameObject.SetActive(false);
            overworld.gameObject.SetActive(false);

            overworld.gameObject.SetActive(true);

            // The player has no move selected.
            player.selectedMove = null; // TODO: may not be needed.

            // Called upon returning to the overworld.
            overworld.OnOverworldReturn(battleWon);

            // The intro text has already been shown, but not the overworld text.
            if (useTutorial && tutorial.clearedIntro && !tutorial.clearedOverworld)
            {
                tutorial.LoadOverworldTutorial();
            }
                
        }

        // Goes to the overworld with a transition.
        public void EnterOverworldWithTransition(bool battleWon)
        {
            // New
            roomTransition.TransitionToOverworld(null, battleWon);
        }

        // Call to enter the battle world.
        public void EnterBattle(Door door)
        {
            // TODO: play animation before transition.
            // if(useTransitions)


            // Can't enter a locked door.
            if (door.Locked)
            {
                return;
            }

            overworld.gameObject.SetActive(false);
            battle.gameObject.SetActive(false);

            // Initialize the battle scene.
            battle.door = door;
            battle.Initialize();

            // Activates the battle object.
            battle.gameObject.SetActive(true);

            // Loads tutorials.
            if(useTutorial)
            {
                // If it's a treasure, load that tutorial.
                if (door.isTreasureDoor)
                {
                    if (!tutorial.clearedTreasure)
                        tutorial.LoadTreasureTutorial();
                }
                // If it's a boss door, load the boss tutorial.
                else if (door.isBossDoor)
                {
                    if (!tutorial.clearedBoss)
                        tutorial.LoadBossTutorial();
                }
                else // It's a regular door, so load that tutorial.
                {
                    // If it's the battle tutorial being loaded.
                    if (!tutorial.clearedBattle)
                    {
                        // Load the battle tutorial.
                        tutorial.LoadBattleTutorial();

                        // Unlocks all the doors since the first battle has started.
                        // The treasure and boss doors are both locked until a battle room is attempted.
                        foreach(Door lockedDoor in overworld.doors)
                        {
                            // Entity is alive, so unlock the door.
                            if(lockedDoor.battleEntity.health != 0)
                            {
                                lockedDoor.Locked = false;
                            }
                            
                        }
                    }
                        
                }

            }
        }

        // Goes to the battle with a transition.
        public void EnterBattleWithTransition(Door door)
        {
            // New
            roomTransition.TransitionToBattle(door);
        }

        // Updates the UI
        public void UpdateUI()
        {
            UpdatePlayerHealthUI();
            UpdatePlayerEnergyUI();

            // Updates the battle number text.
            // This prevents the numerator from overtaking the denominator when the game ends.
            int currRoom = (roomsCompleted + 1 > GetRoomsTotal()) ? roomsCompleted : roomsCompleted + 1;
            int roomsTotal = GetRoomsTotal();
            battleNumberText.text = currRoom.ToString() + "/" + roomsTotal.ToString();
        }
        
        // Updates the health bar UI.
        public void UpdatePlayerHealthUI()
        {
            playerHealthBar.SetValue(player.Health / player.MaxHealth);

            // If false, the text is changed instantly. If true, the text is not updated here.
            // This prevents the final number from flashing for a frame.
            if(!syncTextToBars)
            {
                SetPlayerHealthText();
            }
                
        }

        // Sets the player's health text.
        public void SetPlayerHealthText()
        {
            // This is always shown as a whole number, rounded up.
            playerHealthText.text =
                    Mathf.Ceil(player.Health).ToString() + "/" + Mathf.Ceil(player.MaxHealth).ToString();
        }

        // Updates the player energy UI.
        public void UpdatePlayerEnergyUI()
        {
            playerEnergyBar.SetValue(player.Energy / player.MaxEnergy);

            // If false, the text is changed instantly. If true, the text is not updated here.
            // This prevents the final number from flashing for a frame.
            if (!syncTextToBars)
            {
                // Now just shows the percentage.
                // playerEnergyText.text = player.Energy.ToString() + "/" + player.MaxEnergy.ToString();
                SetPlayerEnergyText();
            }
                
        }

        // Sets the player's health text.
        public void SetPlayerEnergyText()
        {
            playerEnergyText.text =
                    (player.Energy / player.MaxEnergy * 100.0F).ToString("F" + DISPLAY_DECIMAL_PLACES.ToString()) + "%";
        }

        

        // OTHER //

        // Called when the player gets a game over.
        public void OnGameOver()
        {
            // Return health and energy levels to max.
            player.SetHealthToMax();
            player.SetEnergyToMax();

            // If the bars are called to update while transitioning, they jump to their end result instantly.
            // As such, these UI calls were commented out, since they're called elsewhere.
            // The code above for setting to max could probably also be commented out...
            // But it's not causing any issues.

            // // Update the UI for the player's health and energy. 
            // UpdatePlayerHealthUI();
            // UpdatePlayerEnergyUI();

            // Enemy powers are restored in the OnOverworldReturnGameOver() function. 
            overworld.gameOver = true;


            // Moved to OnOverworldReturnGameOver() so that it doesn't show up until the transition is done. 
            // // Loads the game over tutorial. 
            // if (useTutorial && !tutorial.clearedGameOver) 
            //     tutorial.LoadGameOverTutorial(); 
        }


        // Submits the current game progress.
        public void SubmitProgress()
        {
            LOLManager.Instance.SubmitProgress(score, roomsCompleted);
        }

        // Submits the game progress complete.
        public void SubmitProgressComplete()
        {
            LOLManager.Instance.SubmitProgressComplete(score);
        }

        // Goes to the results scene.
        public void ToResultsScene()
        {
            // Set up the results object. It will be kept when transitioning to the next scene.
            GameObject resultsObject = new GameObject();
            ResultsData results = resultsObject.AddComponent<ResultsData>();
            DontDestroyOnLoad(resultsObject);

            // Score - extra 500 points for completing the game.
            score += 1000;
            results.finalScore = score;

            // Rooms total.
            results.roomsCompleted = roomsCompleted;
            results.roomsTotal = GetRoomsTotal();

            // Time and turns.
            results.totalTime = gameTimer;
            results.totalTurns = totalTurnsPassed;

            // Copies the amount of questions used, and a version with no repeats.
            results.questionsUsed = overworld.gameQuestion.GetQuestionsUsedCount(false);
            results.questionsUsedNoRepeats = overworld.gameQuestion.GetQuestionsUsedCount(true);

            // Copies the amount of correct responses, and a version with no repeats.
            results.questionsCorrect = overworld.gameQuestion.GetQuestionResultsCorrect(false);
            results.questionsCorrectNoRepeats = overworld.gameQuestion.GetQuestionResultsCorrect(true);

            // Saves the level and final moves the player had.
            // The player levels up after the boss battle, so the provided level is subtracted by 1.
            results.finalLevel = player.Level - 1;

            // Saves the final moves.
            results.move0 = (player.Move0 != null) ? player.Move0.Name : "-";
            results.move1 = (player.Move1 != null) ? player.Move1.Name : "-";
            results.move2 = (player.Move2 != null) ? player.Move2.Name : "-";
            results.move3 = (player.Move3 != null) ? player.Move3.Name : "-";
            
            // Submit progress to show that the game is complete.
            SubmitProgressComplete();

            // Saves the game before loading the results screen.
            SaveAndContinueGame();

            // Sets the last save as the loaded data.
            // This gets overwritten anyway if the player is saving like normal.
            LOLManager.Instance.saveSystem.SetLastSaveAsLoadedData();

            // Clears out the saves.
            // Taken out so that the game shows the results screen if attempt to continue.
            // This only applies when loading from the title scene, not the init scene.
            
            // NOTE: this only happens once. If the player attempts to continue again once the game is over, a new game will start.
            // As such, I have decided to leave this in, even though it undoes SetLastSaveAsLoadedData.
            LOLManager.Instance.saveSystem.ClearLoadedAndLastSaveData();

            // Go to the results scene.
            if (useTransitions) // Transition
                sceneTransition.LoadScene(RESULTS_SCENE_NAME);
            else // Direct
                SceneManager.LoadScene(RESULTS_SCENE_NAME);
        }

        // Generates the save data.
        public BBTS_GameData GenerateSaveData()
        {
            // Generates the save data.
            BBTS_GameData saveData = new BBTS_GameData();

            // Gets the player save data.
            saveData.playerData = player.GenerateBattleEntitySaveData();

            // If the door data array is not initialized, initialize it.
            if (saveData.doorData == null)
                saveData.doorData = new DoorSaveData[OverworldManager.ROOM_COUNT];

            // Converts the door save data.
            for (int i = 0; i < saveData.doorData.Length && i < overworld.doors.Count; i++)
            {
                // Store the save data.
                saveData.doorData[i] = overworld.doors[i].GenerateSaveData();
            }

            // Saves the tutorial content.
            saveData.clearedIntro = tutorial.clearedIntro;
            saveData.clearedBattle = tutorial.clearedBattle;
            saveData.clearedFirstMove = tutorial.clearedFirstMove;
            saveData.clearedCritical = tutorial.clearedCritical;
            saveData.clearedRecoil = tutorial.clearedRecoil;
            saveData.clearedStatChange = tutorial.clearedStatChange;
            saveData.clearedBurn = tutorial.clearedBurn;
            saveData.clearedParalysis = tutorial.clearedParalysis;
            saveData.clearedFirstBattleDeath = tutorial.clearedFirstBattleDeath;     
            saveData.clearedOverworld = tutorial.clearedOverworld;
            saveData.clearedTreasure = tutorial.clearedTreasure;
            saveData.clearedQuestion = tutorial.clearedQuestion;
            saveData.clearedPhase = tutorial.clearedPhase;
            saveData.clearedBoss = tutorial.clearedBoss;
            saveData.clearedGameOver = tutorial.clearedGameOver;

            // Save game results data.
            saveData.score = score;
            saveData.roomsCompleted = roomsCompleted;
            // saveData.roomsTotal = GetRoomsTotal();

            // Question information.
            // Question countdown information.
            saveData.questionCountdown = overworld.questionCountdown;

            // Copies the asked questions into the array in the save system object.
            {
                // Grabs the two lists.
                List<int> questionsUsed = overworld.gameQuestion.GetQuestionsUsed(false);
                List<bool> questionResults = overworld.gameQuestion.GetQuestionResults(false);

                // Copies the used questions into the data array.
                for (int i = 0; i < saveData.questionsUsed.Length; i++)
                {
                    // Copies the question into the list if it exists.
                    // If it doesn't exist, -1 is used to mark an empty space.
                    if (i < questionsUsed.Count)
                        saveData.questionsUsed[i] = questionsUsed[i];
                    else
                        saveData.questionsUsed[i] = -1;

                }

                // Copies the question results into the data array.
                for (int i = 0; i < saveData.questionResults.Length; i++)
                {
                    // Copies the result into the list if it exists.
                    // The length of the questionsUsed array (which is the same length of this array)...
                    // Is used to know what spots are default entries, as those are set to false.
                    if (i < questionResults.Count)
                        saveData.questionResults[i] = questionResults[i];
                    else
                        saveData.questionResults[i] = false;

                }
            }

            // Game stat information.
            saveData.evolveWaves = evolveWaves;
            saveData.gameTime = gameTimer;
            saveData.turnsPassed = totalTurnsPassed;

            // Saves whether the game has been completed or not.
            saveData.complete = IsGameComplete();

            // Final flag to indicate that the data is safe to read.
            // This is done last so that any errors that occur will cause this line to be skipped.
            saveData.valid = true;

            // Send the save state.
            return saveData;
        }

        // Saves the game.
        public bool SaveGame(bool continueGame)
        {
            // Saves the game.
            bool success = LOLManager.Instance.saveSystem.SaveGame();

            // NOTE: a message is printed to show that the save failed if the game hasn't been initialized.
            // As such, that message is not repeated.

            // If the game should be continued, or should be quit.
            if (continueGame)
            {
                // Turn off the save prompt object if it is visible.
                if (savePrompt.gameObject.activeSelf)
                    ToggleSavePrompt();
            }
            else
            {
                // Go to the title scene.
                ToTitleScene();
            }

            return success;
            
        }

        // Called when the game should be saved and continued.
        public void SaveAndContinueGame()
        {
            // Hide the save prompt.
            if (savePrompt.gameObject.activeSelf)
                ToggleSavePrompt();

            // Save the game.
            SaveGame(true);
        }

        // Called to save and continue the game using a button.
        public void SaveAndContinueGameButton()
        {
            SaveAndContinueGame();

            // TODO: should I keep this?
            // // Play a voice cue to read out the save message text.
            // if (LOLSDK.Instance.IsInitialized && GameSettings.Instance.UseTextToSpeech)
            //     LOLManager.Instance.textToSpeech.SpeakText("sve_msg_savingGame");
        }

        // Called when the game should be saved and quit.
        public void SaveAndQuitGame()
        {
            // TODO: maybe post message instead of closing the window?
            // Hide the save prompt.
            if (savePrompt.gameObject.activeSelf)
                ToggleSavePrompt();

            // Save the game.
            SaveGame(false);
        }

        // Loads the game using the provided save data.
        // If 'allowCompletedGame' is 'true', this function loads the data, even if said data's game was finished.
        // If 'allowCompletedGame' is false, then the program will not load the data if its game was already completed.
        public bool LoadGame(BBTS_GameData saveData, bool allowCompletedGame)
        {
            // Null data check.
            if (saveData == null)
            {
                Debug.LogError("No data sent.");
                return false;
            }

            // Checks if the data is marked as valid.
            // If it isn't valid, then it will not be read.
            if (!saveData.valid)
            {
                Debug.LogError("Data is not marked as valid. Data was not read.");
                return false;
            }

            // If the program should not load in completed games.
            if(!allowCompletedGame)
            {
                // If the amount of rooms completed matches the amount of rooms total, a new game is started.
                // The 'complete' variable tracks this information.
                if (saveData.complete)
                    return false;

            }

            // Checks current game state.
            if(state == gameState.battle) // Player is in the battle area, so go to the overworld.
            {
                // Return to the overworld. The battle hasn't been completed, so set 'battleWon' to false.
                battle.ToOverworld(false);
            }
            else if(state == gameState.none) // Game not initialized.
            {
                Debug.LogWarning("The game hasn't been initialized yet, so the data can't be loaded.");
                return false; 
            }

            // Load the player's save data.
            player.LoadBattleSaveData(saveData.playerData);

            // DOORS
            // Clears out the boss door and the treasure door list.
            overworld.bossDoor = null;
            overworld.treasureDoors.Clear();

            // If there is door data to pull from.
            if(saveData.doorData != null)
            {
                // Loads the door data to replace the existing doors.
                for (int i = 0; i < saveData.doorData.Length && i < overworld.doors.Count; i++)
                {
                    // Loads the save data.
                    overworld.doors[i].LoadSaveData(saveData.doorData[i]);

                    // Found the boss door, so save it.
                    if (overworld.doors[i].isBossDoor)
                        overworld.bossDoor = overworld.doors[i];

                    // Found a treasure door, so add it to the list.
                    if (overworld.doors[i].isTreasureDoor)
                        overworld.treasureDoors.Add(overworld.doors[i]);
                }
            }

            // For some reason the boss door would be left unlocked when loading in game...
            // That had the tutorial enabled. This is a patch work fix of that.
            // Cites the save data because the roomsCompleted content hasn't been loaded into the game's variable yet.
            if (overworld.bossAtEnd && saveData.roomsCompleted != GetRoomsTotal() - 1)
            {
                // Locks the boss door if it's not the final round.
                overworld.bossDoor.Locked = true;
            }
            else
            {
                // Unlock the boss door.
                // Since this is from the tutorial room.
                overworld.bossDoor.Locked = false;
            }                

            // NOTE: the doors are all unlocked when the first battle begins (they are locked during the intro tutorial).
            // To avoid the doors staying locked from a saved tutorial game, the save button is disabled until the first battle is done.
            // This only goes for the tutorial though. The tutorial can't be turned on after the game starts normally...
            // So this exploit doesn't need to be addressed. 
            // By extension, the intro will always be cleared.

            // Loads in the tutorial triggers.
            tutorial.clearedIntro = saveData.clearedIntro;
            tutorial.clearedBattle = saveData.clearedBattle;
            tutorial.clearedFirstMove = saveData.clearedFirstMove;
            tutorial.clearedCritical = saveData.clearedCritical;
            tutorial.clearedRecoil = saveData.clearedRecoil;
            tutorial.clearedStatChange = saveData.clearedStatChange;
            tutorial.clearedBurn = saveData.clearedBurn;
            tutorial.clearedParalysis = saveData.clearedParalysis;
            tutorial.clearedFirstBattleDeath = saveData.clearedFirstBattleDeath;
            tutorial.clearedOverworld = saveData.clearedOverworld;
            tutorial.clearedTreasure = saveData.clearedTreasure;
            tutorial.clearedQuestion = saveData.clearedQuestion;
            tutorial.clearedPhase = saveData.clearedPhase;
            tutorial.clearedBoss = saveData.clearedBoss;
            tutorial.clearedGameOver = saveData.clearedGameOver;

            // If the tutorial is being used.
            if(useTutorial)
            {
                // If the tutorial textbox is open, close it.
                if (tutorial.textBox.IsVisible())
                    tutorial.textBox.Close();

                // The game shouldn't be saved without the intro being shown anyway, but just in case...

                // If the tutorial intro was not cleared, start it up.
                if (!tutorial.clearedIntro)
                    tutorial.LoadIntroTutorial();

                // If the textbox isn't visible, and there are pages, open it.
                if(!tutorial.textBox.IsVisible() && tutorial.textBox.pages.Count != 0)
                {
                    // Opens the textbox.
                    tutorial.textBox.Open();
                }
            }

            // Sets the game data values.
            score = saveData.score;
            roomsCompleted = saveData.roomsCompleted;

            // Rooms total isn't sent over since that value shouldn't changed.

            // Sets the question information.
            // Loads the question countdown information.
            overworld.questionCountdown = saveData.questionCountdown;

            // Copies the question content into the question manager.
            {
                // Questions used list, and the question results.
                List<int> questionsUsed = new List<int>();
                List<bool> questionResults = new List<bool>();

                // Goes through each used question and result, which are the same length.
                for(int i = 0; i < saveData.questionsUsed.Length; i++)
                {
                    // Checks if the question number is valid (negatives are marked as invalid).
                    if (saveData.questionsUsed[i] >= 0)
                    {
                        // Adds the save data contnet to the lists.
                        questionsUsed.Add(saveData.questionsUsed[i]);
                        questionResults.Add(saveData.questionResults[i]);
                    }

                    // NOTE: there shouldn't be any data after the first -1 marker, but all spots are checked anyway.
                    // TODO: maybe have it break after the first negative marker?
                }

                // Replaces the questions used and results lists.
                overworld.gameQuestion.ReplaceQuestionsUsedList(questionsUsed, questionResults);
            }

            // Sets the evolve waves.
            evolveWaves = saveData.evolveWaves;
            
            gameTimer = saveData.gameTime;
            totalTurnsPassed = saveData.turnsPassed;

            // Updates the UI in general.
            UpdateUI();

            // Updates the overworld UI since the player starts there.
            overworld.UpdateUI();

            // Restarts the overworld BGM so that it matches the game phase (will play at default pitch otherwise).
            overworld.PlayOverworldBgm();

            // UI isn't updating properly.
            // playerHealthText.text = player.Health.ToString() + "/" + player.MaxHealth.ToString();
            // playerEnergyText.text = player.Energy.ToString() + "/" + player.MaxEnergy.ToString();

            // If the player is attempted to continue a finished game, go straight to the results screen.
            // This is because the data may still exist for the player.
            if (roomsCompleted == GetRoomsTotal())
                ToResultsScene();

            return true;
        }

        // Loads a saved game as a test.
        private void LoadGameTest()
        {
            // Generates save data.
            BBTS_GameData saveData = GenerateSaveData();

            // Makes some changes.
            saveData.playerData.level = 5;
            saveData.playerData.maxHealth = 999;
            saveData.playerData.health = 600;

            saveData.playerData.maxEnergy = 999;
            saveData.playerData.energy = 600;
            saveData.playerData.move0 = MoveList.Instance.GetRandomMove().Id;

            // Goes through each door data.
            for(int i = 0; i < saveData.doorData.Length; i++)
            {
                // Regular door.
                if(!saveData.doorData[i].locked && 
                    !saveData.doorData[i].isBossDoor && !saveData.doorData[i].isTreasureDoor)
                {
                    bool lockDoor = Random.Range(0, 2) == 0;

                    // If the door should be locked.
                    if (lockDoor)
                    {
                        saveData.doorData[i].locked = lockDoor;

                        saveData.roomsCompleted++;
                        saveData.score += 200; 
                    }
                    
                }

                // Door type test.
                // This will get overwritten for the boss door since it's unique.
                saveData.doorData[i].doorType = 2;
            }

            // Change tutorial settings to test them.
            saveData.clearedIntro = false;
            saveData.clearedBattle = false;
            saveData.clearedOverworld = true;
            saveData.clearedTreasure = true;
            saveData.clearedBoss = true;
            saveData.clearedGameOver = true;
            useTutorial = true;

            saveData.score += 100;
            // saveData.roomsTotal = GetRoomsTotal();
            // saveData.evolveWaves = 1;
            saveData.gameTime = 120;
            saveData.turnsPassed = saveData.roomsCompleted;

            // Loads the save data.
            // Since this is just a test, completed games are allowed.
            LoadGame(saveData, true);
        }

        // Goes to the main menu.
        public void ToTitleScene()
        {
            // Sets the last save as the loaded data.
            LOLManager.Instance.saveSystem.SetLastSaveAsLoadedData();

            // Goes to the title scene.
            SceneManager.LoadScene("TitleScene");
        }

        // Update is called once per frame
        void Update()
        {
            // if(Input.touchCount != 0)
            // {
            //     Touch touch = Input.GetTouch(0);
            // 
            //     Debug.Log("Finger has touched screen. Tap Count: " + touch.tapCount);
            // 
            //     // // checks to see if the user has touched it.
            //     // if (touch.phase == TouchPhase.Began)
            //     // {
            //     //     // Debug.Log("Finger has touched screen.");
            //     // }
            // }

            // // Checks how many touches there are.
            // if (mouseTouchInput.currentTouches.Count > 0)
            //     Debug.Log("Touch Count: " + mouseTouchInput.currentTouches.Count);

            // Call the post start game function.
            if (!calledPostStart)
                PostStart();

            
            // Checks if the mouse input object is active.
            // This is used to correct cases where the mouseTouch is activated when it shouldn't be.
            if(mouseTouchInput.gameObject.activeSelf)
            {
                // If the tutorial is running, disable the mouse touch input.
                if (tutorial.TextBoxIsVisible())
                    mouseTouchInput.gameObject.SetActive(false);

                // If a question is running, disable the mouse touch input.
                if (overworld.gameQuestion.QuestionIsRunning())
                    mouseTouchInput.gameObject.SetActive(false);

            }

            // If the mouse touch input is active, check for the mouse touch.
            if(mouseTouchInput.isActiveAndEnabled)
            {
                // Checks for some mouse input.
                MouseTouchCheck();
            }

            // Updates the player's UI.
            // UpdateUI();

            // // Checks the state variable to see what kind of scene the game is in.
            // switch (state)
            // {
            //     case gameState.overworld:
            //         break;
            // 
            //     case gameState.battle:
            //         break;
            // }

            // Increases the game timer.
            if(!pausedTimer)
            {
                gameTimer += Time.deltaTime;
            }

            // If text transitions are being used.
            if(syncTextToBars)
            {
                // Checks if the health bar is transitioning.
                if (playerHealthBar.IsTransitioning())
                {
                    playerHealthText.text = Mathf.Ceil(playerHealthBar.GetSliderValueAsPercentage() * player.MaxHealth).ToString() + "/" +
                        Mathf.Ceil(player.MaxHealth).ToString();

                    // The health is transitioning.
                    playerHealthTransitioning = true;
                }
                else if (playerHealthTransitioning) // Transition done.
                {
                    // Set to exact value.
                    SetPlayerHealthText();

                    playerHealthTransitioning = false;
                }

                // Checks if the energy bar is transitioning.
                if (playerEnergyBar.IsTransitioning())
                {
                    // Percentage value.
                    // playerEnergyText.text = 
                    //     Mathf.Round(playerEnergyBar.GetSliderValueAsPercentage() * player.MaxEnergy).ToString() + "/" +
                    //     player.MaxEnergy.ToString();

                    // Now displays as a percentage value.
                    playerEnergyText.text = 
                        (playerEnergyBar.GetSliderValueAsPercentage() * 100).ToString(
                            "F" + DISPLAY_DECIMAL_PLACES.ToString()) + "%";

                    // The energy is transitioning.
                    playerEnergyTransitioning = true;
                }
                else if (playerEnergyTransitioning)  // Transition done.
                {
                    // Set to exact value.
                    SetPlayerEnergyText();

                    playerEnergyTransitioning = false;
                }
            }


            
        }
    }
}