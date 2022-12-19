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
        public int roomsPerLevelUp = 3;

        // The last time the enemies were leveled up (is room the player is on).
        public int lastEnemyLevelUps = -1;

        // Shows how many times evolution waves have been done
        public int evolveWaves = 0;

        // String labels for each stat (used for translation).
        private string levelString = "<Level>";
        private string healthString = "<Health>";
        private string attackString = "<Attack>";
        private string defenseString = "<Defense>";
        private string speedString = "<Speed>";
        private string energyString = "<Energy>";

        // Move characteristics.
        private string rankString = "<Rank>";
        private string powerString = "<Power>";
        private string accuracyString = "<Accuracy>";
        private string descriptionString = "<Description>";

        [Header("Game Stats/Time")]

        // The total amount of turns completed.
        public int turnsPassed = 0;

        // The time the game has been going for.
        // This uses deltaTime, which is in seconds.
        public float gameTimer = 0.0F;

        // If set to 'true' the game timer is paused.
        public bool pausedTimer = false;

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
        private string savePromptTextKey = "sve_msg_prompt";

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
        // The quit button text.
        public TMP_Text mainMenuButtonText;

        // The quit window.
        public GameObject mainMenuPrompt;

        // The key for the main menu prompt.
        private string mainMenuPromptTextKey = "mmu_msg_prompt";

        // The prompt text for the main menu.
        public TMP_Text mainMenuPromptText;

        // The main menu (to title screen) confirmation text.
        public TMP_Text mainMenuYesText;

        // The continue game confirmation text.
        public TMP_Text mainMenuNoText;

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
        // Transitions should be used.
        public bool useTransitions = true;

        // The scene transition object.
        public SceneTransition sceneTransition;

        // A game object used to transition between states.
        public Animator stateTransition;

        // The image of the state transition used for colour changes.
        public Image stateTransitionImage;

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
                savePromptText.text = defs[savePromptTextKey];
                saveAndContinueText.text = defs["kwd_saveContinue"];
                saveAndQuitText.text = defs["kwd_saveQuit"];
                savePromptBackText.text = defs["kwd_back"];

                // SETTINGS WINDOW //
                settingsButtonText.text = defs["kwd_settings"];

                // TITLE SCREEN PROMPT
                mainMenuButtonText.text = defs["kwd_mainMenu"];
                mainMenuPromptText.text = defs[mainMenuPromptTextKey];
                mainMenuYesText.text = defs["kwd_returnToMainMenu"];
                mainMenuNoText.text = defs["kwd_returnToGame"];

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
            if (saveFeedbackText != null)
            {
                saveFeedbackText.text = string.Empty;
                LOLManager.Instance.saveSystem.feedbackText = saveFeedbackText;
            }

            // Initialize
            Initialize();           
        }

        // A function called after the all awake and start functions have been called.
        // This is needed so that everything is initialized properly.
        void PostStart()
        {
            // If there is save data, load the saved game.
            if (LOLManager.Instance.saveSystem.loadedData != null)
            {
                // Load the data.
                LoadGame(LOLManager.Instance.saveSystem.loadedData);

                // Clear data.
                LOLManager.Instance.saveSystem.loadedData = null;
            }
            else
            {
                // TODO: uncomment and remove when not testing the save feature.
                // Loads the test data for the game.
                // LoadGameTest();
            }

            // Called so that game phase contnet gets updated.
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

                // If there are enough doors to lock, lock down some.
                // I did a +1 so that the boss room is also ignored.
                if(overworld.treasureDoors.Count + 1 != overworld.doors.Count)
                {
                    // The tutorial door count.
                    const int TRL_DOOR_COUNT = 3;

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

                    // Unlocks three random doors.
                    for (int n = 0; n < 3 && battleDoors.Count > 0; n++)
                    {
                        // Grabs a random index.
                        int randIndex = Random.Range(0, battleDoors.Count);

                        // Unlocks the door, and removes it from the list.
                        battleDoors[randIndex].Locked = false;
                        battleDoors.RemoveAt(randIndex);
                    }
                }

            }


            // Submits a base score of 0 with 0 battles completed.
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
            // Hide the back panel.
            backPanel.gameObject.SetActive(false);

            // Turn off all prompts.
            statsWindow.gameObject.SetActive(false);
            savePrompt.gameObject.SetActive(false);
            settingsWindow.gameObject.SetActive(false);
            mainMenuPrompt.gameObject.SetActive(false);

            // Enable mouse input.
            mouseTouchInput.gameObject.SetActive(true);
        }

        // Called when toggling a window or prompt.
        // 'Active' determines if the prompt is going to be active or not.
        private void OnToggleWindowOrPrompt(bool active)
        {
            // Change the back panel and mouse touch settings.
            backPanel.gameObject.SetActive(active);
            mouseTouchInput.gameObject.SetActive(!active);
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
                if (LOLSDK.Instance.IsInitialized && GameSettings.Instance.UseTextToSpeech && savePromptTextKey != "")
                {
                    // Read out the mssage.
                    LOLManager.Instance.textToSpeech.SpeakText(savePromptTextKey);
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
                if (LOLSDK.Instance.IsInitialized && GameSettings.Instance.UseTextToSpeech && mainMenuPromptTextKey != "")
                {
                    // Read out the mssage.
                    LOLManager.Instance.textToSpeech.SpeakText(mainMenuPromptTextKey);
                }
            }
        }

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

            // TODO: Don't count tutorial reading to game time.
            // pausedTimer = true;
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
                

            // TODO: Don't count tutorial reading to game time?
            // pausedTimer = false;
        }

        // Returns the amount of completed rooms.
        public int GetRoomsCompleted()
        {
            return roomsCompleted;
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
            if (completionRate < 0.33F)
                return 1;
            else if (completionRate < 0.66F)
                return 2;
            else
                return 3;

        }

        // Call this function to enter the overworld.
        public void EnterOverworld()
        {
            // TODO: play animation before transition.
            // if(useTransitions)


            battle.gameObject.SetActive(false);
            overworld.gameObject.SetActive(false);

            overworld.gameObject.SetActive(true);

            // The player has no move selected.
            player.selectedMove = null; // TODO: may not be needed.

            // Called upon returning to the overworld.
            overworld.OnOverworldReturn();

            // The intro text has already been shown, but not the overworld text.
            if (useTutorial && tutorial.clearedIntro && !tutorial.clearedOverworld)
            {
                tutorial.LoadOverworldTutorial();
            }
                
        }

        // Goes to the overworld with a transition.
        public void EnterOverworldWithTransition()
        {
            StartCoroutine(EnterStateWithTransition(gameState.overworld, null));
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

            // TODO: add entity for the opponent.

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
            // Changes the transition colour.
            if(door != null)
            {
                // Returns the door type color.
                stateTransitionImage.color = OverworldManager.GetDoorTypeColor(door.doorType);
            }

            StartCoroutine(EnterStateWithTransition(gameState.battle, door));
        }

        // A function called to turn off the damage animator once it has played.
        private IEnumerator EnterStateWithTransition(gameState newState, Door door)
        {
            // Gets the amount of wait time for each part of the animation finish.
            float waitTime = 0.0F;

            // Gets set to 'true' when the transition has happened.
            bool switched = false;

            // Activate the object so that the transition plays.
            // The GetCurrentAnimatorClipInfo() function won't work otherwise.
            stateTransition.gameObject.SetActive(true);

            // Plays the first part of the animation.
            stateTransition.SetInteger("animPart", 1);

            // Wait for the animation to finish (screen is fully covered when it does).
            waitTime = stateTransition.GetCurrentAnimatorClipInfo(0).Length / stateTransition.speed;

            // While the operation is going.
            while (waitTime > 0.0F)
            {
                // Reduce by delta time.
                waitTime -= Time.deltaTime;

                // Checks to see if the animation has not switched over yet.
                if(!switched && waitTime <= 0.0F)
                {
                    // Checks what state to switch to.
                    switch (newState)
                    {
                        case gameState.overworld: // Go to the overworld.
                            EnterOverworld();
                            break;
                        case gameState.battle: // Go to the battle.
                            EnterBattle(door);
                            break;
                    }

                    // Part 1 of the animation is over, so switch to part 2.
                    stateTransition.SetInteger("animPart", 2);
                    waitTime = stateTransition.GetCurrentAnimatorClipInfo(0).Length / stateTransition.speed;

                    // Switched state.
                    switched = true;
                }
            
                // Tells the program to stall.
                if(waitTime > 0.0F)
                    yield return null;
            }

            // Switch back to default.
            stateTransition.SetInteger("animPart", 1);

            // Deactives the object so that the transition animation stops.
            stateTransition.gameObject.SetActive(false);

            // TODO: the animation appears to break sometimes, and I'm not sure why.
            // It appears to happen because its been too short since the last transition?
            // I'm not sure why that would cause the visual bug though.

        }

        // Updates the UI
        public void UpdateUI()
        {
            UpdatePlayerHealthUI();
            UpdatePlayerEnergyUI();

            battleNumberText.text = (roomsCompleted + 1).ToString() + "/" + GetRoomsTotal().ToString();
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
            player.Health = player.MaxHealth;
            player.Energy = player.MaxEnergy;

            // TODO: restore enemy powers.
            overworld.gameOver = true;

            // Loads the game over tutorial.
            if (useTutorial && !tutorial.clearedGameOver)
                tutorial.LoadGameOverTutorial();
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
            results.totalTurns = turnsPassed;

            // Saves the level and final moves the player had.
            results.finalLevel = player.Level;
            results.move0 = (player.Move0 != null) ? player.Move0.Name : "-";
            results.move1 = (player.Move1 != null) ? player.Move1.Name : "-";
            results.move2 = (player.Move2 != null) ? player.Move2.Name : "-";
            results.move3 = (player.Move3 != null) ? player.Move3.Name : "-";

            // Submit progress to show that the game is complete.
            SubmitProgressComplete();

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

            // Converts the door save data.
            for (int i = 0; i < saveData.doorData.Length && i < overworld.doors.Count; i++)
            {
                // Store the save data.
                saveData.doorData[i] = overworld.doors[i].GenerateSaveData();
            }

            // Saves the tutorial content.
            saveData.clearedIntro = tutorial.clearedIntro;
            saveData.clearedBattle = tutorial.clearedBattle;
            saveData.clearedTreasure = tutorial.clearedTreasure;
            saveData.clearedOverworld = tutorial.clearedOverworld;
            saveData.clearedBoss = tutorial.clearedBoss;
            saveData.clearedGameOver = tutorial.clearedGameOver;

            // Save game results data.
            saveData.score = score;
            saveData.roomsCompleted = roomsCompleted;
            // saveData.roomsTotal = GetRoomsTotal();
            saveData.evolveWaves = evolveWaves;
            saveData.gameTime = gameTimer;
            saveData.turnsPassed = turnsPassed;

            // Send the save state.
            return saveData;
        }

        // Saves the game.
        public bool SaveGame(bool continueGame)
        {
            // Saves the game.
            bool success = LOLManager.Instance.saveSystem.SaveGame();

            // Was the save successful?
            if(success)
            {
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

            }

            return success;
            
        }

        // Called when the game should be saved and continued.
        public void SaveAndContinueGame()
        {
            // TODO: maybe post message instead of closing the window?
            // Hide the save prompt.
            if (savePrompt.gameObject.activeSelf)
                ToggleSavePrompt();

            // Save the game.
            SaveGame(true);
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
        public bool LoadGame(BBTS_GameData saveData)
        {
            // Checks current game state.
            if(state == gameState.battle) // Player is in the battle area, so go to the overworld.
            {
                // Return to the overoworld.
                battle.ToOverworld();
            }
            else if(state == gameState.none) // Game not initialized.
            {
                Debug.LogWarning("The game hasn't been initialized yet, so the data can't be loaded.");
                return false; 
            }

            // TODO: the player's health is loaded properly, but for some reason it gets reset to the max later.
            // I don't know why this happens.
            // Load the player's save data.
            player.LoadBattleSaveData(saveData.playerData);

            // Load the door data.
            for(int i = 0; i < saveData.doorData.Length && i < overworld.doors.Count; i++)
            {
                overworld.doors[i].LoadSaveData(saveData.doorData[i]);
            }

            // NOTE: the doors are all unlocked when the first battle begins (they are locked during the intro tutorial).
            // To avoid the doors staying locked from a saved tutorial game, the save button is disabled until the first battle is done.
            // This only goes for the tutorial though. The tutorial can't be turned on after the game starts normally...
            // So this exploit doesn't need to be addressed. 
            // By extension, the intro will always be cleared.

            // Saves the tutorial values.
            tutorial.clearedIntro = saveData.clearedIntro;
            tutorial.clearedBattle = saveData.clearedBattle;
            tutorial.clearedTreasure = saveData.clearedTreasure;
            tutorial.clearedOverworld = saveData.clearedOverworld;
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

            // Sets the evolve waves.
            evolveWaves = saveData.evolveWaves;
            
            gameTimer = saveData.gameTime;
            turnsPassed = saveData.turnsPassed;

            // Updates the UI.
            UpdateUI();

            // UI isn't updating properly.
            // playerHealthText.text = player.Health.ToString() + "/" + player.MaxHealth.ToString();
            // playerEnergyText.text = player.Energy.ToString() + "/" + player.MaxEnergy.ToString();

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
            LoadGame(saveData);
        }

        // Goes to the main menu.
        public void ToTitleScene()
        {
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

            // Checks for some mouse input.
            MouseTouchCheck();

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