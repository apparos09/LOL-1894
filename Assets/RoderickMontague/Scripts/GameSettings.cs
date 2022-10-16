using LoLSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// the settings for the game, which is created in the init scene.
// the init isn't visited again, so this object will not be recreated.
namespace RoderickMontague_BattleBotTrainingSimulation
{
    public class GameSettings : MonoBehaviour
    {
        // the instance of the game settings.
        private static GameSettings instance;

        // the LOL SDK has been initialized.
        private bool initializedLOLSDK = false;

        [Header("Settings")]

        // use the text-to-speech options.
        public bool useTTS = true;

        // use the tutorial for the game.
        // this is only relevant when starting up the game scene.
        public bool useTutorial = true;

        // the tag for BGM objects.
        public const string BGM_TAG = "BGM";

        // the volume for the background music.
        private float bgmVolume = 1.0F;

        // the tag for the SFX objects.
        public const string SFX_TAG = "SFX";

        // the volume for the sound effects.
        private float sfxVolume = 1.0F;


        // the constructor.
        private GameSettings()
        {
            // Prevents multiple instances of this object being made.
            instance = this;
        }

        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            // This object should not be destroyed.
            DontDestroyOnLoad(this);


        }

        // Start is called before the first frame update
        void Start()
        {
            initializedLOLSDK = LOLSDK.Instance.IsInitialized;
        }

        // // This function is called when the object is enabled and active
        // private void OnEnable()
        // {
        //     SceneManager.sceneLoaded += OnSceneLoaded;
        // }
        // 
        // // This function is called when the behaviour becomes disabled or inactive
        // private void OnDisable()
        // {
        //     SceneManager.sceneLoaded -= OnSceneLoaded;
        // }

        // returns the instance of the game settings.
        public static GameSettings Instance
        {
            get
            {
                // checks to see if the instance exists. If it doesn't, generate an object.
                if (instance == null)
                {
                    // makes a new settings object.
                    GameObject go = new GameObject("Settings");

                    // adds the instance component to the new object.
                    instance = go.AddComponent<GameSettings>();
                }

                // returns the instance.
                return instance;
            }
        }

        // the LOL SDK has been initialized.
        public bool InitializedLOLSDK
        {
            get
            {
                // updates the variable.
                initializedLOLSDK = LOLSDK.Instance.IsInitialized;

                return initializedLOLSDK;
            }
        }

        // is text-to-speech being used?
        public bool TextToSpeech
        {
            get
            {
                return useTTS;
            }

            set
            {
                useTTS = value;
            }
        }

        // is the audio muted?
        public bool Mute
        {
            get
            {
                // is audio muted.
                return AudioListener.pause;
            }

            set
            {
                // mutes/unmutes all audio.
                AudioListener.pause = value;
            }
        }

        // setting the background volume.
        public float BgmVolume
        {
            get
            {
                return bgmVolume;
            }

            set
            {
                AdjustAllAudioLevels(Mathf.Clamp01(value), sfxVolume);
            }
        }

        // setting the sound effect volume.
        public float SfxVolume
        {
            get
            {
                return sfxVolume;
            }

            set
            {
                AdjustAllAudioLevels(bgmVolume, Mathf.Clamp01(value));
            }
        }

        // adjusts the audio source that's supplied through this function.
        // for this to work, it needs to have a usable tag and set source audio object.
        public void AdjustAudio(AudioSourceControl audio)
        {
            // checks which tag to use.
            if (audio.CompareTag(BGM_TAG)) // BGM
            {
                audio.audioSource.volume = audio.MaxVolume * bgmVolume;
            }
            else if (audio.CompareTag(SFX_TAG)) // SFX
            {
                audio.audioSource.volume = audio.MaxVolume * sfxVolume;
            }
            else // no recognizable tag.
            {
                Debug.LogAssertion("No recognizable audio tag has been set, so the audio can't be adjusted.");
            }
        }


        // applies the audio levels by using the saved audio settings.
        public void AdjustAllAudioLevels()
        {
            AdjustAllAudioLevels(bgmVolume, sfxVolume);
        }

        // adjusts the audio levels.
        public void AdjustAllAudioLevels(float newBgmVolume, float newSfxVolume)
        {
            // finds all the audio source controls.
            AudioSourceControl[] audios = FindObjectsOfType<AudioSourceControl>();

            // saves the bgm and sfx volume objects.
            bgmVolume = Mathf.Clamp01(newBgmVolume);
            sfxVolume = Mathf.Clamp01(newSfxVolume);

            // goes through each source.
            foreach (AudioSourceControl audio in audios)
            {
                AdjustAudio(audio);
            }

            // changing the values.
            bgmVolume = newBgmVolume;
            sfxVolume = newSfxVolume;

        }

        // // called when a scene is loaded.
        // public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        // {
        //     // adjusts the audio levels.
        //     AdjustAudioLevels(bgmVolume, sfxVolume);
        // }

    }
}