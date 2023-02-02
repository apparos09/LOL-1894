using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

            // Enables/disables the speak key button.
            speakKeyButton.interactable = descriptionSpeakKey != string.Empty;
            speakKeyButton.gameObject.SetActive(descriptionSpeakKey != string.Empty);

            // Shows the symbol i the alpha value is not set to 0.
            symbol.gameObject.SetActive(symbol.sprite != null);
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