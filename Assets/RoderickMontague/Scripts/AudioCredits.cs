using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
        public string link1;
        public string link2;
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
            
            // The credit object.
            AudioCredit credit;

            // Standard
            // string copyright = "Confirmed by source to be copyright free for performance, distribution, " +
            //     "and modification for commercial purposes, even without permission.";
            // Game Sounds
            string copyrightGameSounds = "Confirmed by GameSounds.xyz to be copyright free for copying, performance, distribution, " +
                "and modification, even for commercial purposes, all without permission.";

            // Modification message.
            string modification = "Asset has been modified for usage in this game";

            // BACKGROUND MUSIC //
            // Title / Overworld / Results (BGM)
            credit = new AudioCredit();
            credit.song = "Ascension";
            credit.artist = "Ross Bugden";
            credit.collection = "Music - Ross Bugden";
            credit.source = "GameSounds.xyz";
            credit.link1 = "https://gamesounds.xyz/?dir=Music%20-%20Ross%20Bugden";

            credit.copyright =
                "\"Ascension\" (https://youtu.be/MzlEX7v0Qz0)" + "\n" +
                "Ross Bugden (https://soundcloud.com/rossbugden)" + "\n" +
                "Licensed under Creative Commons: By Attribution 4.0 International" + "\n" +
                "https://creativecommons.org/licenses/by/4.0/";
            credit.copyright += "\n" + modification;

            audioCredits.Add(credit);


            // Battle / Treasure (BGM)
            credit = new AudioCredit();
            credit.song = "Goodnightmare";
            credit.artist = "Kevin MacLeod";
            credit.collection = "Electronic";
            credit.source = "FreePD";
            credit.link1 = "https://freepd.com/electronic.php";

            credit.copyright = 
                "\"Goodnightmare\"" + "\n" +
                "Kevin MacLeod (incompetech.com)" + "\n" +
                "Licensed under Creative Commons: By Attribution 3.0" + "\n" +
                "http://creativecommons.org/licenses/by/3.0/";
            credit.copyright += "\n" + modification;

            audioCredits.Add(credit);

            // Boss (BGM)
            credit = new AudioCredit();
            credit.song = "Welcome to Chaos";
            credit.artist = "Ross Bugden";
            credit.collection = "Music - Ross Bugden";
            credit.source = "GameSounds.xyz";
            credit.link1 = "https://gamesounds.xyz/?dir=Music%20-%20Ross%20Bugden";

            credit.copyright =
                "\"Welcome to Chaos\" (https://youtu.be/q5w5VX4tAD4)" + "\n" +
                "Ross Bugden (https://soundcloud.com/rossbugden)" + "\n" +
                "Licensed under Creative Commons: By Attribution 4.0 International" + "\n" +
                "https://creativecommons.org/licenses/by/4.0/";
            credit.copyright += "\n" + modification;

            audioCredits.Add(credit);


            // JINGLES //
            // Battle Won (Jingle)
            credit = new AudioCredit();
            credit.song = "Heroic Adventure";
            credit.artist = "Rafael Krux";
            credit.collection = "Epic";
            credit.source = "FreePD, Orchestralis.net";
            credit.link1 = "https://freepd.com/epic.php";

            credit.copyright = 
                "\"HEROIC ADVENTURE\"" + "\n" +
                "Composed by Rafael Krux, Orchestralis.net" + "\n" +
                "Licensed under Creative Commons: By Attribution 4.0 International" + "\n" +
                "https://creativecommons.org/licenses/by/4.0/";
            credit.copyright += "\n" + modification;

            audioCredits.Add(credit);

            // Battle Lost (Jingle)
            credit = new AudioCredit();
            credit.song = "Stratosphere";
            credit.artist = "Kevin MacLeod";
            credit.collection = "Public Domain/Cinematic";
            credit.source = "GameSounds.xyz, FreePD";
            credit.link1 = "https://gamesounds.xyz/?dir=Public%20Domain/Cinematic";

            credit.copyright =
                "\"Stratosphere\"" + "\n" +
                "Kevin MacLeod (incompetech.com)" + "\n" +
                "Licensed under Creative Commons: By Attribution 3.0" + "\n" +
                "http://creativecommons.org/licenses/by/3.0/";
            credit.copyright += "\n" + modification;

            audioCredits.Add(credit);


            // SOUND EFFECTS //
            // Button Press (SFX)
            credit = new AudioCredit();
            credit.song = "Button 3";
            credit.artist = "Unknown Artist";
            credit.collection = "Sound Effects/Buttons";
            credit.source = "GameSounds.xyz";
            credit.link1 = "https://gamesounds.xyz/?dir=Sound%20Effects/Buttons";

            credit.copyright = copyrightGameSounds;
            credit.copyright += "\n" + modification;

            audioCredits.Add(credit);

            // Door Locked (SFX)
            credit = new AudioCredit();
            credit.song = "Door Knock 1";
            credit.artist = "Unknown Artist";
            credit.collection = "Sound Effects/Doors and Floors";
            credit.source = "GameSounds.xyz";
            credit.link1 = "https://gamesounds.xyz/?dir=Sound%20Effects/Doors%20and%20Floors";

            credit.copyright = copyrightGameSounds;
            credit.copyright += "\n" + modification;

            audioCredits.Add(credit);

            // Damage Given / Damage Taken (SFX)
            credit = new AudioCredit();
            credit.song = "Button 4";
            credit.artist = "Unknown Artist";
            credit.collection = "Sound Effects/Buttons";
            credit.source = "GameSounds.xyz";
            credit.link1 = "https://gamesounds.xyz/?dir=Sound%20Effects/Buttons";

            credit.copyright = copyrightGameSounds;
            credit.copyright += "\n" + modification;

            audioCredits.Add(credit);

            // Move Effect (SFX)
            credit = new AudioCredit();
            credit.song = "Button 1";
            credit.artist = "Unknown Artist";
            credit.collection = "Sound Effects/Buttons";
            credit.source = "GameSounds.xyz";
            credit.link1 = "https://gamesounds.xyz/?dir=Sound%20Effects/Buttons";

            credit.copyright = copyrightGameSounds;
            credit.copyright += "\n" + modification;

            audioCredits.Add(credit);
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