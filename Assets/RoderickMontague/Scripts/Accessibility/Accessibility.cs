using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The namespace should be <YourCompany> <GameName>.
namespace RM_BBTS
{
    // a class for the accessibility components
    public class Accessibility : MonoBehaviour
    {
        // the instance of the game settings.
        private static Accessibility instance;

        // the text-to-speech object.
        public TextToSpeech textToSpeech;

        // private constructor so that only one accessibility object exists.
        private Accessibility()
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
                textToSpeech = GetComponent<TextToSpeech>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // returns the instance of the accessibility.
        public static Accessibility Instance
        {
            get
            {
                // Checks to see if the instance exists. If it doesn't, generate an object.
                if (instance == null)
                {
                    // Makes a new settings object.
                    GameObject go = new GameObject("Accessibility");

                    // Adds the instance component to the new object.
                    instance = go.AddComponent<Accessibility>();
                }

                // returns the instance.
                return instance;
            }
        }
    }
}

