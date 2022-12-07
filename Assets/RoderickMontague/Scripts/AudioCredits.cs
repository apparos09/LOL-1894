using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_BBTS
{
    // An audio reference struct.
    public struct AudioCredit
    {
        public string song;
        public string artist;
        public string collection;
        public string source;
        public string link;
        public string copyright;
    }

    // A script for loading in audio references.
    public class AudioCredits : MonoBehaviour
    {
        // The list of references.
        public List<AudioCredit> audioCredits;

        // Awake is called when the script instance is loaded.
        private void Awake()
        {
            audioCredits = new List<AudioCredit>();

            // TEST
            AudioCredit test = new AudioCredit();
            test.song = "Battle Bot Mix";
            test.artist = "mecha-rm";
            test.collection = "Battle Bot";
            test.collection = "Original";
            test.link = "N/A";
            test.copyright = "N/A";

            audioCredits.Add(test);
        }

        // Start is called before the first frame update
        void Start()
        {
            // TODO: load in the audio references.
        }

        // Returns the page count for the audio references.
        public int GetCreditCount()
        {
            return audioCredits.Count;
        }

        // Checks if the index is in the page range.
        public bool IndexInBounds(int index)
        {
            return (index >= 0 && index < audioCredits.Count);
        }

        // Returns a reference.
        public AudioCredit GetCredit(int index)
        {
            // Returns the requested audio credit, or a blank one if out of bounds.
            if (IndexInBounds(index))
                return audioCredits[index];
            else
                return new AudioCredit();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}