using LoLSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_BBTS
{
    // A page in a textbox.
    public class Page
    {
        // The text for the page.
        public string text = "";

        // The key for the text to be read with text-to-speech. This may differ from what the text actually says.
        public string speakKey = "";

        // The callback for the page.
        public delegate void PageCallback();

        // The callback for opening the page.
        private PageCallback pageOpenCallback;

        // The callback for closing the page.
        private PageCallback pageCloseCallback;

        // Generates the empty page.
        public Page()
        {
            text = "";
            speakKey = "";
        }

        // Adds a page with text.
        public Page(string text)
        {
            this.text = text;
            speakKey = "";
        }

        // Adds a page with text and a speak key.
        public Page(string text, string speakKey)
        {
            this.text = text;
            this.speakKey = speakKey;
        }

        // Reads the page.
        public void SpeakPage()
        {
            // Checks for text-to-speech automatically.
            if (GameSettings.Instance.UseTextToSpeech && LOLSDK.Instance.IsInitialized)
            {
                // Checks if the speak key has been set.
                if (speakKey != "")
                    TextToSpeech.Instance.SpeakText(speakKey);
            }
        }

        // Stops the page from being spoken.
        public void StopSpeakingPage()
        {
            // Checks for text-to-speech automatically.
            if (GameSettings.Instance.UseTextToSpeech && LOLSDK.Instance.IsInitialized)
            {
                // Checks if the speak key has been set, and if it's the one being spoken.
                if (speakKey != "" && TextToSpeech.Instance.CurrentSpeakKey == speakKey)
                    TextToSpeech.Instance.StopSpeakText();
            }

        }

        // Add a callback for when the page is opened.
        public void OnPageOpenedAddCallback(PageCallback callback)
        {
            pageOpenCallback += callback;
        }

        // Remove a callback for when the page is opened.
        public void OnPageOpenedRemoveCallback(PageCallback callback)
        {
            pageOpenCallback -= callback;
        }

        // Add a callback for when the page is closd.
        public void OnPageClosedAddCallback(PageCallback callback)
        {
            pageCloseCallback += callback;
        }

        // Remove a callback for when the page is closed.
        public void OnPageClosedRemoveCallback(PageCallback callback)
        {
            pageCloseCallback -= callback;
        }

        // Called when the page is opened.
        public virtual void OnPageOpened()
        {
            // Trigger the callbacks.
            if (pageOpenCallback != null)
                pageOpenCallback();

            // Use text-to-speech to speak the page content.
            SpeakPage();
        }

        // Called when the page is closed.
        public virtual void OnPageClosed()
        {
            // Trigger the callbacks.
            if (pageCloseCallback != null)
                pageCloseCallback();
        }
    }
}