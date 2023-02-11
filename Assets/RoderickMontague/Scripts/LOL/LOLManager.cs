using LoLSDK;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_BBTS
{
    // a class for the LOL
    public class LOLManager : MonoBehaviour
    {
        // the instance of the LOL manager.
        private static LOLManager instance;

        // Language definition for translation.
        private JSONNode defs;
        
        // The save system for the game.
        public SaveSystem saveSystem;
        
        // The text-to-speech object.
        public TextToSpeech textToSpeech;

        // The maixmum progress points for the game.
        // Currently, progress is based on the amount of doors cleared in the game.
        const int MAX_PROGRESS = OverworldManager.ROOM_COUNT; // same as room count.

        // private constructor so that only one accessibility object exists.
        private LOLManager()
        {
            // ...
        }

        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            // This is the instance.
            if (instance == null)
                instance = this;

            // This object should not be destroyed.
            DontDestroyOnLoad(this);

            // The LOLSDK version is the one you use.
            // It is automatically being used already, but I wanted to make a note of this...
            // Since you didn't realize you had to do it this way at the time.
            // LOLSDK.DontDestroyOnLoad(this);

            // If the text-to-speech component is not set, try to get it.
            if (textToSpeech == null)
            {
                // Tries to get the component.
                if(!TryGetComponent<TextToSpeech>(out textToSpeech))
                {
                    // Adds the text-to-speech component.
                    textToSpeech = gameObject.AddComponent<TextToSpeech>();
                }
            }

            // If the save system speech component is not set, try to get it.
            if (saveSystem == null)
            {
                // Tries to get a component.
                if (!TryGetComponent<SaveSystem>(out saveSystem))
                {
                    // Adds the component.
                    saveSystem = gameObject.AddComponent<SaveSystem>();
                }
            }
        }

        // // Start is called before the first frame update
        // void Start()
        // {
        // 
        // }

        // Returns the instance of the accessibility.
        public static LOLManager Instance
        {
            get
            {
                // Checks to see if the instance exists. If it doesn't, generate an object.
                if (instance == null)
                {
                    // Makes a new settings object.
                    GameObject go = new GameObject("LOL Manager");

                    // Adds the instance component to the new object.
                    instance = go.AddComponent<LOLManager>();
                }

                // returns the instance.
                return instance;
            }
        }

        // Gets the text from the language file.
        public string GetLanguageText(string key)
        {
            // Gets the language definitions.
            if(defs == null)
                defs = SharedState.LanguageDefs;

            // Returns the text.
            if (defs != null)
                return defs[key];
            else
                return "";
        }

        // Submits progress for the game.
        // The value overrides the last progress value submitted, and must not go over the max.
        // NOTE: the value will be REPLACED, not added to.
        public void SubmitProgress(int score, int currentProgress)
        {
            // SDK not initialized.
            if(!LOLSDK.Instance.IsInitialized)
            {
                Debug.LogWarning("The SDK is not initialized. No data was submitted.");
                return;
            }

            // Clamps the current progress.
            currentProgress = Mathf.Clamp(currentProgress, 0, MAX_PROGRESS);

            // Submit the progress.
            LOLSDK.Instance.SubmitProgress(score, currentProgress, MAX_PROGRESS);
        }

        // Submits progress to show that the game is complete.
        public void SubmitProgressComplete(int score)
        {
            // Submits the final score.
           SubmitProgress(score, MAX_PROGRESS);
        }
    }
}

