using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_BBTS
{
    // An audio reference struct.
    public struct AudioReference
    {
        public string name;
        public string artist;
        public string album;
        public string source;
        public string link1;
        public string link2;
    }

    // A script for loading in audio references.
    public class AudioReferences : MonoBehaviour
    {
        // The list of references.
        public List<AudioReference> references;

        // Start is called before the first frame update
        void Start()
        {
            // TODO: load in the audio references.
        }

        // Formats the audio reference.
        public string FromatReference(AudioReference audioRef)
        {

            // TODO: put reference into proper format.

            return string.Empty;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}