using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RM_BBTS
{
    public class SettingsMenu : MonoBehaviour
    {
        // the game settings object.
        public GameSettings gameSettings;

        // The title for the game.
        public TMPro.TMP_Text titleText;

        // The text for the back button.
        public TMPro.TMP_Text backButtonText;

        [Header("Volume")]

        // the bgm volume slider.
        public Slider bgmVolumeSlider;

        // The title text for the bgm.
        public TMPro.TMP_Text bgmLabelText;

        // The sound effect slider.
        public Slider sfxVolumeSlider;
        
        // The title for the SFX.
        public TMPro.TMP_Text sfxLabelText;

        [Header("Misc")]
        // the toggle for the text-to-speech toggle.
        public Toggle textToSpeechToggle;

        // The text for the text to speech toggle.
        public TMPro.TMP_Text textToSpeechLabel;

        // the tutorial toggle.
        public Toggle tutorialToggle;

        // The text for the text to speech toggle.
        public TMPro.TMP_Text tutorialLabel;

        // Start is called before the first frame update
        void Start()
        {
            // Save the instance.
            gameSettings = GameSettings.Instance;

            // Current bgm volume setting.
            bgmVolumeSlider.value = gameSettings.BgmVolume;

            // // Listener for the bgm slider.
            // bgmVolumeSlider.onValueChanged.AddListener(delegate
            // {
            //     OnBgmVolumeChange(bgmVolumeSlider);
            // });

            // Current sfx volume setting.
            sfxVolumeSlider.value = gameSettings.SfxVolume;

            // // Listener for the sfx slider.
            // sfxVolumeSlider.onValueChanged.AddListener(delegate
            // {
            //     OnSfxVolumeChange(sfxVolumeSlider);
            // });

            // Current text-to-speech tutorial.
            textToSpeechToggle.isOn = gameSettings.TextToSpeech;

            // // Listener for the text-to-speech.
            // textToSpeechToggle.onValueChanged.AddListener(delegate
            // {
            //     OnTextToSpeechChange(textToSpeechToggle);
            // });

            // Current tutorial toggle setting.
            tutorialToggle.isOn = gameSettings.useTutorial;

            // // Listener for the tutorial toggle.
            // tutorialToggle.onValueChanged.AddListener(delegate
            // {
            //     OnTutorialChange(tutorialToggle);
            // });


            // Translation.
            JSONNode defs = SharedState.LanguageDefs;

            // Translate the dialogue.
            if(defs != null)
            {
                titleText.text = defs["kwd_settings"];
                bgmLabelText.text = defs["kwd_bgmVolume"];
                sfxLabelText.text = defs["kwd_sfxVolume"];
                textToSpeechLabel.text = defs["kwd_textToSpeech"];
                tutorialLabel.text = defs["kwd_tutorial"];
                backButtonText.text = defs["kwd_back"];
            }    

        }

        // This function is called when the object becomes enabled and active
        private void OnEnable()
        {
            gameSettings = GameSettings.Instance;
        }

        public void OnTextSpeechChange()
        {
            OnTextToSpeechChange(textToSpeechToggle);
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
            gameSettings.BgmVolume = Mathf.InverseLerp(slider.minValue, slider.maxValue, slider.value);
        }

        // on the sfx volume change.
        public void OnSfxVolumeChange(Slider slider)
        {
            gameSettings.SfxVolume = Mathf.InverseLerp(slider.minValue, slider.maxValue, slider.value);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}