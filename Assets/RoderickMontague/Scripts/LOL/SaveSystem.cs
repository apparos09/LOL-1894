using LoLSDK;
using SimpleJSON;
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
        // Becomes set to 'true' to indicate that there is data to be read.
        public bool valid = false;

        // Marks whether the data is from a completed game or not (game was finished if 'complete' is set to 'true').
        // If the game was completed, start a new game instead.
        public bool complete = false;

        // The player's data.
        public BattleEntitySaveData playerData;

        // The save data for the doors in the game.
        // This also holds the data for each entity.
        // public List<DoorSaveData> doorData;

        // The door save data.
        public DoorSaveData[] doorData = new DoorSaveData[OverworldManager.ROOM_COUNT];

        // Triggers for the tutorial for the game.
        public bool clearedIntro; // Intro tutorial.
        public bool clearedBattle; // Battle tutorial.
        public bool clearedFirstMove; // First move tutorial.
        public bool clearedCritical; // Critical tutorial.
        public bool clearedRecoil; // Recoil tutorial.
        public bool clearedStatChange; // Stat change tutorial.
        public bool clearedBurn; // Burn tutorial.
        public bool clearedParalysis; // Paralysis tutorial.
        public bool clearedFirstBattleDeath; // First battle death tutorial.
        public bool clearedOverworld; // Overworld tutorial.
        public bool clearedTreasure; // Treasure tutorial.
        public bool clearedQuestion; // Question tutorial.
        public bool clearedPhase; // Phase tutorial.
        public bool clearedBoss; // Boss tutorial.
        public bool clearedGameOver; // Game over tutorial.

        // TODO: saving rooms total may not be needed.

        // Results data at the time of the save.
        public int score = 0; // Score
        public int roomsCompleted = 0; // Rooms cleared by the player.

        // The next question round.
        public int questionCountdown = 0;

        // The amount of used questions and the results for said questions.
        public int[] questionsUsed = new int[OverworldManager.ROOM_COUNT];
        public bool[] questionResults = new bool[OverworldManager.ROOM_COUNT];

        // The serializer does NOT like integer arrays or lists for some reason.
        // As such, each one had to be stored as a seperate variable.

        // Not needed since this is a fixed value.
        // public int roomsTotal = 0; // Total rooms cleared.

        public int evolveWaves = 0; // Evolution waves.
        public float gameTime = 0.0F; // Total game time.
        public int turnsPassed = 0; // Total turns.

    }

    // Used to save the game.
    public class SaveSystem : MonoBehaviour
    {
        // The game data.
        // The last game save. This is only for testing purposes.
        public BBTS_GameData lastSave;

        // The data that was loaded.
        public BBTS_GameData loadedData;

        // The manager for the game.
        public GameplayManager gameManager;

        // LOL - AutoSave //
        // Added from the ExampleCookingGame. Used for feedback from autosaves.
        WaitForSeconds feedbackTimer = new WaitForSeconds(2);
        Coroutine feedbackMethod;
        public TMP_Text feedbackText;

        // The string shown when having feedback.
        private string feedbackString = "Saving Data";

        // The string key for the feedback.
        private const string FEEDBACK_STRING_KEY = "sve_msg_savingGame";

        // Start is called before the first frame update
        void Start()
        {
            // Sets the save result to the instance.
            LOLSDK.Instance.SaveResultReceived += OnSaveResult;

            // Gets the language definition.
            JSONNode defs = SharedState.LanguageDefs;

            // Sets the save complete text.
            if (defs != null)
                feedbackString = defs[FEEDBACK_STRING_KEY];
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

        // Sets the last bit of saved data to the loaded data object.
        public void SetLastSaveAsLoadedData()
        {
            loadedData = lastSave;
        }

        // Clears out the last save and the loaded data object.
        public void ClearLoadedAndLastSaveData()
        {
            lastSave = null;
            loadedData = null;
        }

        // Saves data.
        public bool SaveGame()
        {
            // The game manager does not exist if false.
            if(!IsGameManagerSet())
            {
                Debug.LogWarning("The Game Manager couldn't be found.");
                return false;
            }

            // Determines if saving wa a success.
            bool success = false;

            // Generates the save data.
            BBTS_GameData savedData = gameManager.GenerateSaveData();

            // Stores the most recent save.
            lastSave = savedData;

            // If the instance has been initialized.
            if (LOLSDK.Instance.IsInitialized)
            {
                // Makes sure that the feedback string is set.
                if(FEEDBACK_STRING_KEY != string.Empty)
                {
                    // Gets the language definition.
                    JSONNode defs = SharedState.LanguageDefs;

                    // Sets the feedback string if it wasn't already set.
                    if (feedbackString != defs[FEEDBACK_STRING_KEY])
                        feedbackString = defs[FEEDBACK_STRING_KEY];
                }
               

                // Send the save state.
                LOLSDK.Instance.SaveState(savedData);
                success = true;
            }
            else // Not initialized.
            {
                Debug.LogError("The SDK has not been initialized. Improper save made.");
                success = false;
            }

            return success;
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
            feedbackMethod = StartCoroutine(Feedback(feedbackString));
        }

        // Feedback while result is saving.
        IEnumerator Feedback(string text)
        {
            // Only updates the text that the feedback text was set.
            if(feedbackText != null)
                feedbackText.text = text;

            yield return feedbackTimer;
            
            // Only updates the content if the feedback text has been set.
            if(feedbackText != null)
                feedbackText.text = string.Empty;
            
            // nullifies the feedback method.
            feedbackMethod = null;
        }

        // Checks if the game has loaded data.
        public bool HasLoadedData()
        {
            // Used to see if the data is available.
            bool result;

            // Checks to see if the data exists.
            if (loadedData != null) // Exists.
            {
                // Checks to see if the data is valid.
                result = loadedData.valid;
            }
            else // No data.
            {
                // Not readable.
                result = false;
            }
                
            // Returns the result.
            return result;
        }

        // Removes the loaded data.
        public void ClearLoadedData()
        {
            loadedData = null;
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
                Debug.LogError("Game gameManager not found.");
                return;
            }

            // TODO: this automatically loads the game if the continue button is pressed.
            // If there is no data to load, the button is gone. 
            // You should move the buttons around to accomidate for this.
            // LoadGame();
        }

        
    }
}