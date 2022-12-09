using LoLSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_BBTS
{
    // Runs the text-to-speech for the provided speak key.
    public class SpeakOnClick : MonoBehaviour
    {
        // The speak key for the object.
        public string speakKey = "";

        // Set the speak key and call the SpeakText function.
        public void SpeakKey(string newKey)
        {
            speakKey = newKey;
            SpeakText();
        }

        // Call this function to speak the text for the object.
        public void SpeakText()
        {
            // If the instance is initialized, use the text-to-speech.
            if(LOLSDK.Instance.IsInitialized && speakKey != "")
                LOLManager.Instance.textToSpeech.SpeakText(speakKey);
        }

        // This doesn't work if it's part of the UI. Use a button to call SpeakText() instead.
        // OnMouseDown is called when the user has pressed the mouse button while over the GUIElement or Collider.
        private void OnMouseDown()
        {
            SpeakText();
        }
    }
}