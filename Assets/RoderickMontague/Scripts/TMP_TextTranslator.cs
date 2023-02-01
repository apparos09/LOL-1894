using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using LoLSDK;

namespace RM_BBTS
{
    // Translates a TMP_Text object's text using the provided key.
    public class TMP_TextTranslator : MonoBehaviour
    {
        // The text object.
        public TMP_Text text;

        // The translation key.
        public string key = "";

        // Start is called before the first frame update
        void Start()
        {
            // Grabs the text mesh pro component from the object this script is attached to.
            if (text == null)
                text = GetComponent<TextMeshPro>();

            // If the SDK is initialized.
            if(LOLSDK.Instance.IsInitialized && text != null)
            {
                text.text = LOLManager.Instance.GetLanguageText(key);
            }
        }

        // Speaks out the provided text.
        public void SpeakText()
        {

            // Checks if the TTS is set up, if the TTS is active, and if the key string exists.
            if(LOLSDK.Instance.IsInitialized && GameSettings.Instance.UseTextToSpeech && key != string.Empty)
            {
                // Read out the text.
                LOLManager.Instance.textToSpeech.SpeakText(key);
            }
        }
    }
}