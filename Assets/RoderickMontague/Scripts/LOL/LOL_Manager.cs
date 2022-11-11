using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_BBTS
{
    // a class for the LOL
    public class LOL_Manager : MonoBehaviour
    {
        // the instance of the game settings.
        private static LOL_Manager instance;

        // Language definition for translation.
        private JSONNode defs;
        
        // The save system for the game.
        public SaveSystem saveSystem;
        
        // The text-to-speech object.
        public TextToSpeech textToSpeech;

        // private constructor so that only one accessibility object exists.
        private LOL_Manager()
        {
            // ...
        }

        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            // This object should not be destroyed.
            DontDestroyOnLoad(this);

            // If the text-to-speech component is not set, try to get it.
            if(textToSpeech == null)
            {
                // Tries to get the component.
                if(!TryGetComponent<TextToSpeech>(out textToSpeech))
                {
                    // Adds the component.
                    textToSpeech = gameObject.AddComponent<TextToSpeech>();
                }
            }

            // If the save system speech component is not set, try to get it.
            if (saveSystem == null)
            {
                // Tries to get a component.
                if (!TryGetComponent<SaveSystem>(out saveSystem))
                {
                    // Adds the component.
                    saveSystem = gameObject.AddComponent<SaveSystem>();
                }
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // returns the instance of the accessibility.
        public static LOL_Manager Instance
        {
            get
            {
                // Checks to see if the instance exists. If it doesn't, generate an object.
                if (instance == null)
                {
                    // Makes a new settings object.
                    GameObject go = new GameObject("Accessibility");

                    // Adds the instance component to the new object.
                    instance = go.AddComponent<LOL_Manager>();
                }

                // returns the instance.
                return instance;
            }
        }

        // Gets the text from the language file.
        public string GetText(string key)
        {
            // Gets the language definitions.
            if(defs == null)
                defs = SharedState.LanguageDefs;

            // Returns the text.
            if (defs != null)
                return defs[key];
            else
                return "";
        }
    }
}

