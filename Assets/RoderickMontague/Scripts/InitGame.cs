using LoLSDK;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

// Initializes the LOL content and then enters the title screen.
// This code was taken from Loader.cs (a file from the LOL template content) and then modified.
namespace RM_BBTS
{
    public class InitGame : MonoBehaviour
    {
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

            // LOL Initialization

            // Create the WebGL (or mock) object
#if UNITY_EDITOR
            ILOLSDK webGL = new LoLSDK.MockWebGL();
#elif UNITY_WEBGL
        ILOLSDK webGL = new LoLSDK.WebGL();
#endif

            // Initialize the object, passing in the WebGL
            LOLSDK.Init(webGL, "com.legends-of-learning.battle-bot-training-sim");

            // Register event handlers
            LOLSDK.Instance.StartGameReceived += new StartGameReceivedHandler(HandleStartGame);
            LOLSDK.Instance.LanguageDefsReceived += new LanguageDefsReceivedHandler(HandleLanguageDefs);
            LOLSDK.Instance.QuestionsReceived += new QuestionListReceivedHandler(HandleQuestions);
            LOLSDK.Instance.GameStateChanged += new GameStateChangedHandler(HandleGameStateChange);

            // Mock the platform-to-game messages when in the Unity editor.
#if UNITY_EDITOR
            LoadMockData();
#endif

            // Then, tell the platform the game is ready.
            LOLSDK.Instance.GameIsReady();
            StartCoroutine(_WaitForData());
        }

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
        void HandleGameStateChange(GameState gameState)
        {
            // Either GameState.Paused or GameState.Resumed
            Debug.Log("HandleGameStateChange");
        }

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