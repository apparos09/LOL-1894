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
    public bool useTTS = true;

    // use the tutorial for the game.
    // this is only relevant when starting up the game scene.
    public bool useTutorial = true;

    // the volume for the background music.
    private float bgmVolume = 1.0F;

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
            AdjustAudioLevels(Mathf.Clamp01(value), sfxVolume);
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
            AdjustAudioLevels(bgmVolume, Mathf.Clamp01(value));
        }
    }


    // applies the audio levels by using the saved audio settings.
    public void ApplyAudioLevels()
    {
        AdjustAudioLevels(bgmVolume, sfxVolume);
    }

    // adjusts the audio levels for the game.
    // by default, the audio is at 100% for everything.
    public void AdjustAudioLevels(float newBgmVolume, float newSfxVolume)
    {
        // adjusts the audio levels. Since this is not the first time the audio is being adjusted, the audio will be adjusted.
        AdjustAudioLevels(false, SceneManager.GetActiveScene(), newBgmVolume, newSfxVolume);
    }

    // adjusts the audio levels.
    // firstTime: this is the first time the audio is being adjusted for the scene.
    // TODO: this is NOT efficient for a menu component. Make sure you change it or find a workaround.
    private void AdjustAudioLevels(bool firstTime, Scene scene, float newBgmVolume, float newSfxVolume)
    {
        // TODO: maybe split them into two seperate functions for BGM and SFX?
        // TODO: create loading scene.

        // finds all the audio sources.
        AudioSource[] sources = FindObjectsOfType<AudioSource>();

        // goes through each source.
        foreach(AudioSource source in sources)
        {
            // if the game object's scene is not set to this scene.
            if (source.gameObject.scene != scene)
                continue;

            // old and new volume objects.
            float oldVol = -1, newVol = -1;

            // checks the audio type
            if(source.tag == "BGM") // background music
            {
                oldVol = bgmVolume;
                newVol = newBgmVolume;
            }
            else if(source.tag == "SFX") // sound effect.
            {
                oldVol = sfxVolume;
                newVol = newSfxVolume;
            }
            else // no recognizable tag found, so audio levels can't be adjusted.
            {
                continue;
            }

            // if this is NOT the first time the audio is being adjusted in this scene...
            // then the audio levels need to be set to max first.
            if (!firstTime)
            {
                // returns to max audio level
                source.volume /= oldVol;
            }

            // adjusts the audio source.
            source.volume *= newVol;
        }

        // changing the values.
        bgmVolume = newBgmVolume;
        sfxVolume = newSfxVolume;

    }

    // called when a scene is loaded.
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // adjusts the audio levels.
        AdjustAudioLevels(true, scene, bgmVolume, sfxVolume);
    }

    

}
