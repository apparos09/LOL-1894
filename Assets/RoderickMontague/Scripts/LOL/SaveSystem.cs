using LoLSDK;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RM_BBTS
{
    // The battle bot training sim data.
    [System.Serializable]
    public class BBTS_GameData
    {
        // The player's data.
        public BattleEntitySaveData playerData;

        // The save data for the doors in the game.
        // This also holds the data for each entity.
        // public List<DoorSaveData> doorData;

        // The door save data.
        public DoorSaveData[] doorData = new DoorSaveData[18];


        // Triggers for the tutorial for the game.
        public bool clearedIntro; // Intro tutorial.
        public bool clearedBattle; // Battle tutorial.
        public bool clearedTreasure; // Treasure tutorial.
        public bool clearedOverworld; // Overworld tutorial.
        public bool clearedBoss; // Boss tutorial.
        public bool clearedGameOver; // Game over tutorial.

        // TODO: saving rooms total may not be needed.

        // Results data at the time of the save.
        public int score = 0; // Score
        public int roomsCompleted = 0; // Rooms cleared by the player.
        public int roomsTotal = 0; // Total rooms cleared.
        public float gameTime = 0.0F; // Total game time.
        public int turnsPassed = 0; // Total turns.

    }

    // Used to save the game.
    public class SaveSystem : MonoBehaviour
    {
        // The game data.
        // The data that was saved.
        // private BBTS_GameData savedData;

        // The data that was loaded.
        public BBTS_GameData loadedData;

        // The manager for the game.
        public GameplayManager gameManager;

        // LOL - AutoSave //
        // Added from the ExampleCookingGame. Used for feedback from autosaves.
        WaitForSeconds feedbackTimer = new WaitForSeconds(2);
        Coroutine feedbackMethod;
        public TMP_Text feedbackText;

        // Start is called before the first frame update
        void Start()
        {
            LOLSDK.Instance.SaveResultReceived += OnSaveResult;
        }

        // Set save and load operations.
        public void Initialize(Button newGameButton, Button continueButton)
        {
            // Makes the continue button disappear if there is no data to load. 
            Helper.StateButtonInitialize<BBTS_GameData>(newGameButton, continueButton, OnLoadData);
        }

        // Checks if the game manager has been set.
        private bool IsGameManagerSet()
        {
            if (gameManager == null)
                gameManager = FindObjectOfType<GameplayManager>(true);

            // Game manager does not exist.
            if (gameManager == null)
            {
                Debug.LogWarning("The Game Manager couldn't be found.");
                return false;
            }

            return true;
        }

        // Saves data.
        public bool SaveGame()
        {
            // If the instance has been initialized.
            if(!LOLSDK.Instance.IsInitialized)
            {
                Debug.LogError("The SDK has not been initialized. Save failed.");
                return false;
            }

            // The game manager does not exist if false.
            if(!IsGameManagerSet())
            {
                Debug.LogWarning("The Game Manager couldn't be found.");
                return false;
            }

            // Generates the save data.
            BBTS_GameData savedData = new BBTS_GameData();

            // Gets the player save data.
            savedData.playerData = gameManager.player.GenerateBattleEntitySaveData();

            // Converts the door save data.
            for(int i = 0; i < savedData.doorData.Length && i < gameManager.overworld.doors.Count; i++)
            {
                // Store the save data.
                savedData.doorData[i] = gameManager.overworld.doors[i].GenerateSaveData();
            }

            // Saves the tutorial content.
            savedData.clearedIntro = gameManager.tutorial.clearedIntro;
            savedData.clearedBattle = gameManager.tutorial.clearedBattle;
            savedData.clearedTreasure = gameManager.tutorial.clearedTreasure;
            savedData.clearedOverworld = gameManager.tutorial.clearedOverworld;
            savedData.clearedBoss = gameManager.tutorial.clearedBoss;
            savedData.clearedGameOver = gameManager.tutorial.clearedGameOver;

            // Save game results data.
            savedData.score = gameManager.score;
            savedData.roomsCompleted = gameManager.roomsCompleted;
            savedData.roomsTotal = gameManager.roomsTotal;
            savedData.gameTime = gameManager.gameTimer;
            savedData.turnsPassed = gameManager.turnsPassed;

            // Send the save state.
            LOLSDK.Instance.SaveState(savedData);

            return true;

            // Helper.StateButtonInitialize<CookingData>(newGameButton, continueButton, OnLoad);
        }

        // Called for saving the result.
        private void OnSaveResult(bool success)
        {
            if (!success)
            {
                Debug.LogWarning("Saving not successful");
                return;
            }

            if (feedbackMethod != null)
                StopCoroutine(feedbackMethod);
            // ...Auto Saving Complete
            feedbackMethod = StartCoroutine(Feedback("sve_msg_saveComplete"));
        }

        // Feedback while result is saving.
        IEnumerator Feedback(string text)
        {
            feedbackText.text = text;
            yield return feedbackTimer;
            feedbackText.text = string.Empty;
            feedbackMethod = null;
        }

        // The gameplay manager now checks if there is loadedData. If so, then it will load in the data when the game starts.
        // // Loads a saved game. This returns 'false' if there was no data.
        // public bool LoadGame()
        // {
        //     // No loaded data.
        //     if(loadedData == null)
        //     {
        //         Debug.LogWarning("There is no saved game.");
        //         return false;
        //     }
        // 
        //     // TODO: load the game data.
        // 
        //     return true;
        // }

        // Called to load data from the server.
        private void OnLoadData(BBTS_GameData loadedGameData)
        {
            // Overrides serialized state data or continues with editor serialized values.
            if (loadedGameData != null)
            {
                loadedData = loadedGameData;
            }
            else // No game data found.
            {
                Debug.LogError("No game data found.");
                loadedData = null;
                return;
            }

            // TODO: save data for game loading.
            if(!IsGameManagerSet())
            {
                Debug.LogError("Game manager not found.");
                return;
            }

            // TODO: this automatically loads the game if the continue button is pressed.
            // If there is no data to load, the button is gone. 
            // You should move the buttons around to accomidate for this.
            // LoadGame();
        }

        
    }
}