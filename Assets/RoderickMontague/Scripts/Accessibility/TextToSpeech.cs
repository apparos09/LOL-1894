using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoLSDK;


// The namespace should be <YourCompany> <GameName>.
// TODO: switch to instance.
namespace RM_BBTS
{
    public class TextToSpeech : MonoBehaviour
    {
        // The instance of the text-to-speech.
        private static TextToSpeech instance;

        // The audio source for the text-to-speech.
        public AudioSource ttsAudioSource;

        // The current key being read. It is set to "" if there isn't one.
        private string currentKey = "";

        // Constructor
        private TextToSpeech()
        {

        }

        // Awake is called when the script instance is being loaded
        void Awake()
        {

            // if(Application.platform == RuntimePlatform.WebGLPlayer)
            // {
            //     // only do if initializing for the first time.
            //     // ILOLSDK webGL = new LoLSDK.MockWebGL();
            //     // 
            //     // LOLSDK.Init(webGL, "understand_probability_game");
            // 
            //     ReadLine("This is a test");
            // }

            // Checks for the instance.
            if (instance == null)
            {
                instance = this;
            }

            // checks if the SDK has been initialized.
            if (!LOLSDK.Instance.IsInitialized)
                Debug.LogError("The SDK has not been initialized.");

            // if the audio source has not been set, then add an audio source component.
            if (ttsAudioSource == null)
                ttsAudioSource = gameObject.AddComponent<AudioSource>();

        }

        // Returns the instance of the text-to-speech object.
        public static TextToSpeech Instance
        {
            get
            {
                // Checks to see if the instance exists. If it doesn't, generate an object.
                if (instance == null)
                {
                    instance = FindObjectOfType<TextToSpeech>(true);

                    // Generate new instance if an existing instance was not found.
                    if (instance == null)
                    {
                        // Makes a new settings object.
                        GameObject go = new GameObject("Text-to-Speech");

                        // Adds the instance component to the new object.
                        instance = go.AddComponent<TextToSpeech>();
                    }

                }

                // returns the instance.
                return instance;
            }
        }

        // Returns 'true' if the text-to-speech is reading something.
        public bool IsTextBeingRead
        {
            get { return ttsAudioSource.isPlaying; }
        }

        // Gets the key of the text being read. If no text is being read then it will return (""). 
        public string GetTextKey()
        {
            return currentKey;
        }

        // Reads the text based on the provided key.
        public void SpeakText(string key)
        {
#if UNITY_EDITOR
            // Gets the language code.
            string languageCode = SharedState.StartGameData["languageCode"];

            // Gets the text based on the provided key.
            string text = SharedState.LanguageDefs[key];

            // There is no text to read, so return.
            if (text == null || text == "")
                return;

            // Stops any current text-to-speech audio.
            ttsAudioSource.Stop();

            // Saves the text key.
            currentKey = key;

            // Speaks the clip of text requested from using this MonoBehaviour as the coroutine owner.
            ((ILOLSDK_EDITOR)LOLSDK.Instance.PostMessage).SpeakText(
                text,
                clip => { 
                    ttsAudioSource.clip = clip; 
                    ttsAudioSource.Play(); 
                },
                this,
                languageCode);
#else
        // Reads the text using the provided key.
		LOLSDK.Instance.SpeakText(key);
#endif
        }

        // Cancels the text that's being read by the text-to-speech.
        public void CancelSpeakText()
        {
#if UNITY_EDITOR
            ttsAudioSource.Stop();
#endif
            ((ILOLSDK_EXTENSION)LOLSDK.Instance.PostMessage).CancelSpeakText();
        }

        // Update is called every frame, if the MonoBehaviour is enabled
        private void Update()
        {
            // If the current key has been set.
            if(currentKey != "")
            {
                // If the text is not being read, set the current key to blank.
                if (!IsTextBeingRead)
                    currentKey = "";
            }
        }
    }
}
