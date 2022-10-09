using LoLSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// the settings for the game, which is created in the init scene.
// the init isn't visited again, so this object will not be recreated.
public class GameSettings : MonoBehaviour
{
    // the instance of the game settings.
    private static GameSettings instance;

    // the LOL SDK has been initialized.
    private bool initializedLOLSDK = false;

    [Header("Settings")]

    // use the text-to-speech options.
    public bool UseTTS = true;

    // use the tutorial for the game.
    public bool useTutorial = true;

    // the volume for the background music.
    private float bgmVolume;

    // the volume for the sound effects.
    private float sfxVolume;
    

    // the constructor.
    private GameSettings()
    {
        // Prevents multiple instances of this object being made.
    }

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // this object should not be called.
        DontDestroyOnLoad(this);

        
    }

    // Start is called before the first frame update
    void Start()
    {
        initializedLOLSDK = LOLSDK.Instance.IsInitialized;
    }

    // This function is called when the object is enabled and active
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // This function is called when the behaviour becomes disabled or inactive
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // returns the instance of the game settings.
    public GameSettings Instance
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
            return initializedLOLSDK;
        }
    }

    // Is text-to-speech being used?
    public bool TextToSpeech
    {
        get
        {
            return UseTTS;
        }

        set
        {
            UseTTS = value;
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

    // adjusts the audio levels for the game.
    // by default, the audio is at 100% for everything.
    public void AdjustAudioLevels()
    {
        // sets the audio levels.
        // AdjustAudioLevels(bgmVolume, sfxVolume);
    }

    // adjusts the audio levels.
    public void AdjustAudioLevels(float newBgmVolume, float newSfxVolume)
    {
        // return to 1.0F.
        // bgmVolume = 1.0F;
        // sfxVolume = 1.0F;
    }

    // called when a scene is loaded.
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // if either of the audio components are not at 100% adjust the audio levels.
        if (bgmVolume != 1.0F || sfxVolume != 1.0F)
            AdjustAudioLevels();
    }

    

}
