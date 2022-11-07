using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RM_BBTS
{
    public class GameplayManager : GameState
    {
        // the state of the game.
        public gameState state;

        // the manager for the overworld.
        public OverworldManager overworld;

        // the manager for the battle.
        public BattleManager battle;

        // the input from the mouse and touch screen.
        public MouseTouchInput mouseTouchInput;

        // The player for the game.
        public Player player;

        // The total battles in the game.
        public int battlesTotal = 0;

        // The total amount of completed battles.
        public int battlesCompleted = 0;
        
        // The amount of battles completed for the enemies to level up.
        public int battlesPerLevelUp = 3;

        // The last time the enemies were leveled up (is room the player is on).
        public int lastEnemyLevelUps = -1;

        // Shows how many times evolution waves have been done
        public int evolveWaves = 0;

        // The tutorial object.
        public Tutorial tutorial;

        // If set to 'true' then the tutorial is used.
        public bool useTutorial = true;


        [Header("UI")]

        // The text box used for general messages.
        // This is used for the tutorial, which also saves this object.
        public TextBox textBox;

        // The battle number text.
        public TMPro.TMP_Text battleNumberText;

        // The health bar for the player.
        public ProgressBar playerHealthBar;

        // The text for the player's health (TODO: include a progress bar).
        public TMPro.TMP_Text playerHealthText;

        // The health bar for the player.
        public ProgressBar playerEnergyBar;

        // The text for the player's enegy (TODO: include a progress bar).
        public TMPro.TMP_Text playerEnergyText;

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
                useTutorial = GameSettings.Instance.useTutorial;
        }

        // Start is called before the first frame update
        void Start()
        {
            // Finds the mouse touch input object.
            if (mouseTouchInput == null)
                mouseTouchInput = FindObjectOfType<MouseTouchInput>();


            Initialize();
        }

        // Initializes the gameplay manager.
        public override void Initialize()
        {
            overworld.Initialize();
            overworld.gameObject.SetActive(true);
            state = gameState.overworld;

            // Saves the batle total.
            battlesTotal = OverworldManager.DOOR_COUNT;

            // Update the UI.
            UpdateUI();

            // The total amount of battles in the game.
            battlesTotal = overworld.doors.Count;

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

                // If there are enough doors to lock some down.
                if(overworld.treasureDoors.Count + 1 != overworld.doors.Count)
                {
                    // Copies the list.
                    List<Door> battleDoors = new List<Door>(overworld.doors);

                    // Locks and removes the special doors from the list.
                    for (int i = battleDoors.Count - 1; i >= 0; i--)
                    {
                        battleDoors[i].Locked = true; // Locks the door.

                        // Removes the special door from the list.
                        if (battleDoors[i].isBossDoor || battleDoors[i].isTreasureDoor)
                        {
                            battleDoors.RemoveAt(i); // Remove from list.
                        }
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
            // ...
        }

        // Called when the user's touch interacts with an entity.
        public override void OnTouchInteract(GameObject touchedObject, Touch touch)
        {
            // ...
        }

        // Called with the object that was received with the interaction.
        protected override void OnInteractReceive(GameObject gameObject)
        {
            throw new System.NotImplementedException();
        }

        // // Called when a level has been loaded.
        // private void OnLevelWasLoaded(int level)
        // {
        // }

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
                    switch (state)
                    {
                        case gameState.overworld:
                            overworld.OnTouchInteract(hitObject, touch);
                            break;

                        case gameState.battle:
                            battle.OnTouchInteract(hitObject, touch);
                            break;
                    }
                }
                
            }
            else
            {
                // Checks the state variable to see what kind of scene the game is in.
                switch (state)
                {
                    case gameState.overworld:
                        overworld.OnMouseInteract(hitObject);
                        break;

                    case gameState.battle:
                        battle.OnMouseInteract(hitObject);
                        break;
                }
            }

            // Print message for testing.
            // if(hitObject != null)
            //     Debug.Log("Hit Found");

        }

        // A function to call when a tutorial starts.
        public override void OnTutorialStart()
        {
            //// Checks the game state.
            //switch (state)
            //{
            //    case gameState.overworld: // overworld
            //        overworld.OnTutorialStart();
            //        break;

            //    case gameState.battle: // battle
            //        battle.OnTutorialStart();
            //        break;
            //}

            // Call both in case the state hasn't changed.
            overworld.OnTutorialStart();
            battle.OnTutorialStart();

            // Turns off the mouse touch input. 
            mouseTouchInput.gameObject.SetActive(false);
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
        }

        // Returns the phase of the game (1 = start, 2 = middle, 3 = end).
        // Each section is evenly split.
        public int GetGamePhase()
        {
            // The completion rate.
            float completionRate = battlesCompleted / (float)battlesTotal;

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
            battle.gameObject.SetActive(false);
            overworld.gameObject.SetActive(false);

            overworld.gameObject.SetActive(true);

            // The player has no move selected.
            player.selectedMove = null;

            // Called upon returning to the overworld.
            overworld.OnOverworldReturn();

            // The intro text has already been shown, but not the overworld text.
            if (useTutorial && tutorial.clearedIntro && !tutorial.clearedOverworld)
                tutorial.LoadOverworldTutorial();
        }

        // Call to enter the battle world.
        public void EnterBattle(Door door)
        {
            // TODO: comment this out in the final game.
            if(door.Locked)
            {
                Debug.Log("The door can't be opened.");
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

        // Goes to the results screen.
        public void ToResultsScreen()
        {
            // TODO: store battle data.
            SceneManager.LoadScene("ResultsScene");
        }

        // Updates the UI
        public void UpdateUI()
        {
            UpdatePlayerHealthUI();
            UpdatePlayerEnergyUI();

            battleNumberText.text = (battlesCompleted + 1).ToString() + " / " + battlesTotal.ToString();
        }
        
        // Updates the health bar UI.
        public void UpdatePlayerHealthUI()
        {
            playerHealthBar.SetValue(player.Health / player.MaxHealth);
            playerHealthText.text = player.Health.ToString() + "/" + player.MaxHealth.ToString();
        }

        // Updates the player energy UI.
        public void UpdatePlayerEnergyUI()
        {
            playerEnergyBar.SetValue(player.Energy / player.MaxEnergy);
            playerEnergyText.text = player.Energy.ToString() + "/" + player.MaxEnergy.ToString();
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

            // Checks for some mouse input.
            MouseTouchCheck();

            // Updates the player's UI.
            // UpdateUI();

            // Checks the state variable to see what kind of scene the game is in.
            switch (state)
            {
                case gameState.overworld:
                    break;

                case gameState.battle:
                    break;
            }

        }
    }
}