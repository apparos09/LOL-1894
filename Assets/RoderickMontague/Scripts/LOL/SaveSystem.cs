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
    public class BBTS_Data
    {
        // The player's data.
        public BattleEntitySaveData playerData;

        // The save data for the doors in the game.
        // This also holds the data for each entity.
        public List<DoorSaveData> doorData;

        // Triggers for the tutorial for the game.
        public bool clearedIntro; // Intro tutorial.
        public bool clearedBattle; // Battle tutorial.
        public bool clearedTreasure; // Treasure tutorial.
        public bool clearedOverworld; // Overworld tutorial.
        public bool clearedBoss; // Boss tutorial.
        public bool clearedGameOver; // Game over tutorial.

        // Results data at the time of the save.
        public int roomsCleared; // Rooms cleared by the player.
        public int totalRooms; // Total rooms cleared.
        public float totalTime = 0.0F; // Total game time.
        public int totalTurns = 0; // Total turns.

    }

    // Used to save the game.
    public class SaveSystem : MonoBehaviour
    {
        // The game data.
        BBTS_Data gameData;

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

        // This function is called after a new level was loaded.
        private void OnLevelWasLoaded(int level)
        {
            // Saves the game manager.
            gameManager = FindObjectOfType<GameplayManager>(true);
        }

        // Set save and load operations.
        public void Initialize(Button newGameButton, Button continueButton)
        {
            Helper.StateButtonInitialize<BBTS_Data>(newGameButton, continueButton, OnLoadData);
        }

        // Saves data.
        private void SaveData()
        {
            // The data to be saved does not exist if not in the GameScene.
            if(SceneManager.GetActiveScene().name != "GameScene")
            {
                Debug.LogWarning("Data can only be saved in the GameScene.");
                return;
            }

            // Tries to find the gameplay manager.
            if(gameManager == null)
                gameManager = FindObjectOfType<GameplayManager>(true);

            // Game manager does not exist.
            if(gameManager == null)
            {
                Debug.LogWarning("The Game Manager couldn't be found.");
                return;
            }

            // TODO: save the game data.

            LOLSDK.Instance.SaveState(gameData);

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
            feedbackMethod = StartCoroutine(Feedback("autoSave"));
        }

        // Feedback while result is saving.
        IEnumerator Feedback(string text)
        {
            feedbackText.text = text;
            yield return feedbackTimer;
            feedbackText.text = string.Empty;
            feedbackMethod = null;
        }

        // Loads data.
        private void OnLoadData(BBTS_Data loadedGameData)
        {
            // Overrides serialized state data or continues with editor serialized values.
            if (loadedGameData != null)
                gameData = loadedGameData;
            else
                return;

            // TODO: save data for game loading.
        }

        
    }
}