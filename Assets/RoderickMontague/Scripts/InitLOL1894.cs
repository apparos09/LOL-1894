using LoLSDK;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitLOL1894 : MonoBehaviour
{
    // Awake is called when the script is being loaded
    private void Awake()
    {
        // the sdk for the LoL component.
        ILOLSDK sdk;

        // checks the platform the editor is running in.
#if UNITY_EDITOR
        sdk = new LoLSDK.MockWebGL();
#elif UNITY_WEBGL
        sdk = new LoLSDK.WebGL();

#endif

        // initialization
        LOLSDK.Init(sdk, "lol_1894.battle-bot-training-sim");

        // // event handlers
        // LOLSDK.Instance.StartGameReceived += new StartGameReceivedHandler(StartGame);
        // LOLSDK.Instance.GameStateChanged += new GameStateChangedHandler(gameState => Debug.Log(gameState));
        // LOLSDK.Instance.QuestionsReceived += new QuestionListReceivedHandler(questionList => Debug.Log(questionList));
        // LOLSDK.Instance.LanguageDefsReceived += new LanguageDefsReceivedHandler(LanguageUpdate);
    }

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadScene("TitleScene");
    }

    // called for starting the game.
    private void StartGame(string startGameJSON)
    {
        if (string.IsNullOrEmpty(startGameJSON))
            return;

        JSONNode startGamePayload = JSON.Parse(startGameJSON);
        // Capture the language code from the start payload. Use this to switch fonts
        // _langCode = startGamePayload["languageCode"];
    }

    // function called for saving the game.
    private void Save()
    {

    }

    // function called after saving.
    private void OnSaveResult()
    {

    }

    private void LanguageUpdate(string _langNode)
    {
        // if (string.IsNullOrEmpty(langJSON))
        //     return;
        // 
        // _langNode = JSON.Parse(langJSON);
        // 
        // TextDisplayUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
