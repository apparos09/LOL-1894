using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    // the game settings object.
    public GameSettings gameSettings;

    // the toggle for the text-to-speech toggle.
    public Toggle textToSpeechToggle;

    // the tutorial toggle.
    public Toggle tutorialToggle;

    // the bgm volume slider.
    public Slider bgmVolumeSlider;

    // the sound effect slider.
    public Slider sfxVolumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        gameSettings = GameSettings.Instance;

        // Current text-to-speech tutorial.
        textToSpeechToggle.isOn = gameSettings.TextToSpeech;

        // Listener for the text-to-speech.
        textToSpeechToggle.onValueChanged.AddListener(delegate {
            OnTextToSpeechChange(textToSpeechToggle);
        });

        // Current tutorial toggle setting.
        tutorialToggle.isOn = gameSettings.useTutorial;

        // Listener for the tutorial toggle.
        tutorialToggle.onValueChanged.AddListener(delegate {
             OnTutorialChange(tutorialToggle);
        });

        // Current bgm volume setting.
        bgmVolumeSlider.value = gameSettings.BgmVolume;

        // Listener for the bgm slider.
        bgmVolumeSlider.onValueChanged.AddListener(delegate {
            OnBgmVolumeChange(bgmVolumeSlider);
        });

        // Current sfx volume setting.
        sfxVolumeSlider.value = gameSettings.SfxVolume;

        // Listener for the sfx slider.
        sfxVolumeSlider.onValueChanged.AddListener(delegate {
            OnSfxVolumeChange(sfxVolumeSlider);
        });

    }

    // This function is called when the object becomes enabled and active
    private void OnEnable()
    {
        gameSettings = GameSettings.Instance;
    }


    // On the text-to-speech changes.
    public void OnTextToSpeechChange(Toggle toggle)
    {
        gameSettings.TextToSpeech = toggle.isOn;
    }

    // On the tutorial changes.
    public void OnTutorialChange(Toggle toggle)
    {
        gameSettings.useTutorial = toggle.isOn;
    }

    // on the bgm volume change.
    public void OnBgmVolumeChange(Slider slider)
    {
        gameSettings.BgmVolume = slider.value;
    }

    // on the sfx volume change.
    public void OnSfxVolumeChange(Slider slider)
    {
        gameSettings.SfxVolume = slider.value;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
