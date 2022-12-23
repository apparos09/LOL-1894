using LoLSDK;
using SimpleJSON;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

// Initializes the LOL content and then enters the title screen.
// This code was taken from Loader.cs (a file from the LOL template content) and then modified.
namespace RM_BBTS
{
    public class InitGame : MonoBehaviour
    {
        // GAME //
        // Data for the game.
        BBTS_GameData gameData;

        // Becomes 'true' when the game has been initialized.
        public bool initGame = false;

        // The text for the game initialization.
        public TMP_Text initText;

        // LOL //
        // Relative to Assets /StreamingAssets/
        private const string languageJSONFilePath = "language.json";
        private const string questionsJSONFilePath = "questions.json";
        private const string startGameJSONFilePath = "startGame.json";

        // Use to determine when all data is preset to load to next state.
        // This will protect against async request race conditions in webgl.
        LoLDataType _receivedData;

        // This should represent the data you're expecting from the platform.
        // Most games are expecting 2 types of data, Start and Language.
        LoLDataType _expectedData = LoLDataType.START | LoLDataType.LANGUAGE;

        // // LOL - AutoSave //
        // // Added from the ExampleCookingGame. Used for feedback from autosaves.
        // WaitForSeconds feedbackTimer = new WaitForSeconds(2);
        // Coroutine feedbackMethod;
        // public TMP_Text feedbackText;

        [System.Flags]
        enum LoLDataType
        {
            START = 0,
            LANGUAGE = 1 << 0,
            QUESTIONS = 1 << 1
        }

        void Awake()
        {
            // Unity Initialization
            Application.targetFrameRate = 30; // 30 FPS
            Application.runInBackground = false; // Don't run in the background.

            // Use the tutorial by default.
            GameSettings.Instance.UseTutorial = true;

            // LOL Initialization
            // Create the WebGL (or mock) object
#if UNITY_EDITOR
            ILOLSDK sdk = new LoLSDK.MockWebGL();
#elif UNITY_WEBGL
            ILOLSDK sdk = new LoLSDK.WebGL();
#elif UNITY_IOS || UNITY_ANDROID
            ILOLSDK sdk = null; 
#endif

            // Initialize the object, passing in the WebGL
            LOLSDK.Init(sdk, "com.legends-of-learning.battle-bot-training-simulation");

            // Register event handlers
            LOLSDK.Instance.StartGameReceived += new StartGameReceivedHandler(HandleStartGame);
            LOLSDK.Instance.LanguageDefsReceived += new LanguageDefsReceivedHandler(HandleLanguageDefs);
            LOLSDK.Instance.QuestionsReceived += new QuestionListReceivedHandler(HandleQuestions);
            LOLSDK.Instance.GameStateChanged += new GameStateChangedHandler(HandleGameStateChange);

            // Used for player feedback. Not required by SDK.
            // LOLSDK.Instance.SaveResultReceived += OnSaveResult;


            // Mock the platform-to-game messages when in the Unity editor.
#if UNITY_EDITOR
            // UnityEditor.EditorGUIUtility.PingObject(this);
            LoadMockData();
#endif

            // Call GameIsReady before calling LoadState or using the helper method.
            // Then, tell the platform the game is ready.
            LOLSDK.Instance.GameIsReady();
            StartCoroutine(_WaitForData());

            // Helper method to hide and show the state buttons as needed.
            // Will call LoadState<T> for you.
            // Helper.StateButtonInitialize<CookingData>(newGameButton, continueButton, OnLoad);

            // TODO: take this out?
            // Shows that the game has been initialized?
            initGame = true;
        }

        // Start is called just before any of the Update methods is called the first time.
        public void Start()
        {
            // Taken out since this probably doesn't need to be translated.
            // // Grabs the language defs.
            // JSONNode defs = SharedState.LanguageDefs;
            // 
            // // Translate the text in case it's shown on screen from the game taking a while to initialize.
            // if (initText != null && defs != null)
            // {
            //     // Do this just in case 
            //     // initText.text = defs["kwd_loading"]; // Loading
            // }
        }

        private void OnDestroy()
        {
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
                return;
#endif
            // LOLSDK.Instance.SaveResultReceived -= OnSaveResult;
        }

        // Saves the game. This isn't used in the InitGame file.
        void Save()
        {
            LOLSDK.Instance.SaveState(gameData);
        }

        // // On save result.
        // void OnSaveResult(bool success)
        // {
        //     if (!success)
        //     {
        //         Debug.LogWarning("Saving not successful");
        //         return;
        //     }
        // 
        //     if (feedbackMethod != null)
        //         StopCoroutine(feedbackMethod);
        //     // ...Auto Saving Complete
        //     feedbackMethod = StartCoroutine(Feedback("autoSave"));
        // }

        // Waits for the data and then loads the scene.
        IEnumerator _WaitForData()
        {
            yield return new WaitUntil(() => (_receivedData & _expectedData) != 0);
            SceneManager.LoadScene("TitleScene", LoadSceneMode.Single);
        }

        // Start the game here
        void HandleStartGame(string json)
        {
            SharedState.StartGameData = JSON.Parse(json);
            _receivedData |= LoLDataType.START;
        }


        // Use language to populate UI
        void HandleLanguageDefs(string json)
        {
            JSONNode langDefs = JSON.Parse(json);

            // Example of accessing language strings
            // Debug.Log(langDefs);
            // Debug.Log(langDefs["welcome"]);

            SharedState.LanguageDefs = langDefs;
            _receivedData |= LoLDataType.LANGUAGE;
        }

        // Store the questions and show them in order based on your game flow.
        void HandleQuestions(MultipleChoiceQuestionList questionList)
        {
            Debug.Log("HandleQuestions");
            SharedState.QuestionList = questionList;
            _receivedData |= LoLDataType.QUESTIONS;
        }

        // Handle pause / resume
        void HandleGameStateChange(LoLSDK.GameState gameState)
        {
            // Either GameState.Paused or GameState.Resumed
            Debug.Log("HandleGameStateChange");
        }

        /// <summary>
        /// This is the setting of your initial state when the scene loads.
        /// The state can be set from your default editor settings or from the
        /// users saved data after a valid save is called.
        /// </summary>
        /// <param name="loadedGameData"></param>
        void OnLoad(BBTS_GameData loadedGameData)
        {
            // Overrides serialized state data or continues with editor serialized values.
            if (loadedGameData != null)
            {
                gameData = loadedGameData;
                LOLManager.Instance.saveSystem.loadedData = loadedGameData;
            }
            else
            {
                return;
            }
               
            // Becomes set to 'true' when the game data has been loaded.
            initGame = true;
        }

        // Gets translated text.
        public string GetTranslatedText(string key)
        {
            // Gets the definitions.
            JSONNode defs = SharedState.LanguageDefs;

            // Get content.
            if (defs != null)
                return defs[key];
            else
                return "";
        }

        // // Not quite sure what this does.
        // IEnumerator Feedback(string text)
        // {
        //     feedbackText.text = text;
        //     yield return feedbackTimer;
        //     feedbackText.text = string.Empty;
        //     feedbackMethod = null;
        // }

        private void LoadMockData()
        {
#if UNITY_EDITOR
            // Load Dev Language File from StreamingAssets

            string startDataFilePath = Path.Combine(Application.streamingAssetsPath, startGameJSONFilePath);
            string langCode = "en";

            Debug.Log(File.Exists(startDataFilePath));

            if (File.Exists(startDataFilePath))
            {
                string startDataAsJSON = File.ReadAllText(startDataFilePath);
                JSONNode startGamePayload = JSON.Parse(startDataAsJSON);
                // Capture the language code from the start payload. Use this to switch fonts
                langCode = startGamePayload["languageCode"];
                HandleStartGame(startDataAsJSON);
            }

            // Load Dev Language File from StreamingAssets
            string langFilePath = Path.Combine(Application.streamingAssetsPath, languageJSONFilePath);
            if (File.Exists(langFilePath))
            {
                string langDataAsJson = File.ReadAllText(langFilePath);
                // The dev payload in language.json includes all languages.
                // Parse this file as JSON, encode, and stringify to mock
                // the platform payload, which includes only a single language.
                JSONNode langDefs = JSON.Parse(langDataAsJson);
                // use the languageCode from startGame.json captured above
                HandleLanguageDefs(langDefs[langCode].ToString());
            }

            // Load Dev Questions from StreamingAssets
            string questionsFilePath = Path.Combine(Application.streamingAssetsPath, questionsJSONFilePath);
            if (File.Exists(questionsFilePath))
            {
                string questionsDataAsJson = File.ReadAllText(questionsFilePath);
                MultipleChoiceQuestionList qs =
                    MultipleChoiceQuestionList.CreateFromJSON(questionsDataAsJson);
                HandleQuestions(qs);
            }
#endif
        }
    }
}