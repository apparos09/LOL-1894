using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using LoLSDK;

namespace RM_BBTS
{
    // A marker used to mark text that is not loaded from the language file.
    public class LanguageMarker : MonoBehaviour
    {
        // the instance of the language marker.
        private static LanguageMarker instance;

        // The color used for marking text that wasn't repalced with language file content. 
        [HideInInspector]
        public Color noLoadColor = Color.red;

        // If the text colour should be changed.
        public const bool CHANGE_TEXT_COLOR = true;

        // The constructor
        private LanguageMarker()
        {
            // ...
        }

        // Awake is called when the script is loaded.
        private void Awake()
        {
            // Instance saving.
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
                return;
            }
        }

        // Returns the instance of the language marker.
        public static LanguageMarker Instance
        {
            get
            {
                // Checks to see if the instance exists. If it doesn't, generate an object.
                if (instance == null)
                {
                    // Makes a new settings object.
                    GameObject go = new GameObject("Language Marker (singleton)");

                    // Adds the instance component to the new object.
                    instance = go.AddComponent<LanguageMarker>();
                }

                // returns the instance.
                return instance;
            }
        }

        // Marks the provided text object.
        public void MarkText(TMP_Text text)
        {
            // If the text color should be changed.
            if(CHANGE_TEXT_COLOR)
                text.color = noLoadColor;
        }

        // Translates the text using the provided key.
        // If the language file isn't loaded, then the text is marked using the noLoad colour.
        public bool TranslateText(TMP_Text text, string key, bool markIfFailed)
        {
            // Checks if the SDK has been initialized. 
            if(LOLSDK.Instance.IsInitialized)
            {
                text.text = LOLManager.Instance.GetLanguageText(key);
                return true;
            }
            else
            {
                // If the text should be marked if failed.
                if (markIfFailed)
                    MarkText(text);

                return false;
            }

        }
    }
}