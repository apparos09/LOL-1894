using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoLSDK;


// The namespace should be <YourCompany> <GameName>.
namespace RM_BBTS
{
    public class TextToSpeech : MonoBehaviour
    {
        // The game settings instance.
        private GameSettings gameSettings;

        // The audio source for the text-to-speech.
        public AudioSource ttsAudioSource;

        // Start is called before the first frame update
        void Start()
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

            // Grabs the instance.
            gameSettings = GameSettings.Instance;

            // checks if the SDK has been initialized.
            if (!gameSettings.InitializedLOLSDK)
                Debug.LogError("The SDK has not been initialized.");

            // if the audio source has not been set, then add an audio source component.
            if (ttsAudioSource == null)
                ttsAudioSource = gameObject.AddComponent<AudioSource>();

        }

        // Returns 'true' if the text-to-speech is reading something.
        public bool IsTextBeingRead
        {
            get { return ttsAudioSource.isPlaying; }
        }

        // Reads the text based on th eprovided label.
        public void SpeakText(string label)
        {
#if UNITY_EDITOR
            // Gets the language code.
            string languageCode = SharedState.StartGameData["languageCode"];

            // Gets the text based on the provided label.
            string text = SharedState.LanguageDefs[label];

            // Stops any current text-to-speech audio.
            ttsAudioSource.Stop();
            
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
        // Reads the text using the provided label.
		LOLSDK.Instance.SpeakText(label);
#endif
        }

        // Cancels the text that's being read by the text-to-speech.
        private void CancelSpeakText()
        {
#if UNITY_EDITOR
            ttsAudioSource.Stop();
#endif
            ((ILOLSDK_EXTENSION)LOLSDK.Instance.PostMessage).CancelSpeakText();
        }
    }
}
