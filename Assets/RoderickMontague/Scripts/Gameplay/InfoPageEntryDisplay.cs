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

        // The text object for the entry name.
        public TMP_Text nameText;

        // The text object for the description.
        public TMP_Text descriptionText;

        // The image for the entry's symbol (where applicable).
        public Image symbol;

        // The color of the symbol.
        public Color symbolColor = new Color(0, 0, 0, 0);


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
            descriptionText.text = newEntry.description;

            symbol.sprite = newEntry.symbol;
            symbolColor = newEntry.symbolColor;
            symbol.color = symbolColor;

            // Shows the symbol i the alpha value is not set to 0.
            symbol.gameObject.SetActive(symbol.sprite != null);
        }

        // Clear the entry.
        public void ClearEntry()
        {
            // Loads the information.
            nameText.text = string.Empty;
            descriptionText.text = string.Empty;

            // TODO: check to make sure the sprite is actually made invisible.
            symbol.sprite = null;
            symbolColor = blackAlpha0;
            symbol.color = symbolColor;
        }
    }
}