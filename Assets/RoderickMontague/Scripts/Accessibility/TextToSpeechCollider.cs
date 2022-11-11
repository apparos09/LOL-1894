using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_BBTS
{
// A collider used to do text-to-speech.
// This is used for UI.
    public class TextToSpeechCollider : MonoBehaviour
    {
        // The speak key for the text-to-speech.
        public string speakKey = "";

        // // OnMouseDown is called when the user has pressed the mouse button while over the GUIElement or Collider.
        // private void OnMouseDown()
        // {
        //     SpeakText();
        // }

        // OnMouseUpAsButton is only called when the mouse is released over the same GUIElement or Collider as it was pressed.
        private void OnMouseUpAsButton()
        {
            SpeakText();
        }

        // Speaks the text.
        public void SpeakText()
        {
            if(speakKey != "")
                TextToSpeech.Instance.SpeakText(speakKey);
        }
    }
}
