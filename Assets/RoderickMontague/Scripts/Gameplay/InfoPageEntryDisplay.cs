using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using LoLSDK;

namespace RM_BBTS
{
    // An entry for an info page.
    public class InfoPageEntryDisplay : MonoBehaviour
    {
        // Completely transparent black color.
        private Color blackAlpha0 = new Color(0, 0, 0, 0);

        // The text object for the entry name, and its speak key.
        public TMP_Text nameText;
        public string nameSpeakKey = string.Empty;

        // The text object for the description, and its speak key.
        public TMP_Text descriptionText;
        public string descriptionSpeakKey = string.Empty;

        // The image for the entry's symbol (where applicable).
        public Image symbol;

        // The color of the symbol.
        public Color symbolColor = new Color(0, 0, 0, 0);

        // The speak key button for the display.
        public Button speakKeyButton;

        // Start is called just before any of the Update methods are called for the first time.
        public void Start()
        {
            symbol.color = symbolColor;

            // If the SDK has not been initialized, mark the text.
            if(!LOLSDK.Instance.IsInitialized)
            {
                LanguageMarker.Instance.MarkText(nameText);
                LanguageMarker.Instance.MarkText(descriptionText);
            }
        }

        // Called when the script is enabled.
        private void OnEnable()
        {
            // Refreshes the speak button if this object was ever turned off.
            RefreshSpeakButton();
        }

        // Load the entry.
        public void LoadEntry(InfoPageEntry newEntry)
        {
            // Loads the information.
            nameText.text = newEntry.name;
            nameSpeakKey = newEntry.nameKey;

            descriptionText.text = newEntry.description;
            descriptionSpeakKey = newEntry.descriptionKey;

            symbol.sprite = newEntry.symbol;
            symbolColor = newEntry.symbolColor;
            symbol.color = symbolColor;

            // // Enables/disables the speak key button.
            // speakKeyButton.interactable = descriptionSpeakKey != string.Empty;
            // speakKeyButton.gameObject.SetActive(descriptionSpeakKey != string.Empty);

            // Refreshes the speak button to see if it can be used.
            RefreshSpeakButton();

            // Shows the symbol i the alpha value is not set to 0.
            symbol.gameObject.SetActive(symbol.sprite != null);
        }

        // Refreshes the speak button to see if it should be enabled.
        public void RefreshSpeakButton()
        {
            // Checks if the speak button should be read based on the game settings.
            if (!LOLSDK.Instance.IsInitialized || !GameSettings.Instance.UseTextToSpeech)
            {
                // The TTS isn't available, so disable the speak button.
                speakKeyButton.interactable = false;
            }
            else
            {
                // Check if there's information to read.
                speakKeyButton.interactable = descriptionSpeakKey != string.Empty;     
            }

            // Turn on/off the speak key button object if there is a description to be read.
            speakKeyButton.gameObject.SetActive(descriptionSpeakKey != string.Empty);
        }

        // Clear the entry.
        public void ClearEntry()
        {
            // Clears the information.
            nameText.text = string.Empty;
            nameSpeakKey = string.Empty;

            descriptionText.text = string.Empty;
            descriptionSpeakKey = string.Empty;

            symbol.sprite = null;
            symbolColor = blackAlpha0;
            symbol.color = symbolColor;

            // Disables the speak key button.
            speakKeyButton.interactable = false;
            speakKeyButton.gameObject.SetActive(false);
        }
    }
}