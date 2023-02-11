using LoLSDK;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

// the manager for the title scene.
namespace RM_BBTS
{
    // The manager for the title scene.
    public class TitleManager : MonoBehaviour
    {
        // The save text for the title manager.
        // If the user presses "save and quit", this text will be disabled on the title screen when the scene switches over.
        // This is because saving won't be done by the time the scene switches, so make sure this text is set.
        public TMP_Text saveFeedbackText;

        // The name of the game scene.
        public const string GAME_SCENE_NAME = "GameScene";

        // This variable is used to check i the tutorial setting should be overrided for a continued game.
        private bool overrideTutorial = false;

        // This variable holds the value for whether or not to use the tutorial in a continued game.
        private bool continueTutorial = false;

        [Header("Main Menu")]
        // Menu
        public GameObject mainMenu;

        // Start
        public Button newGameButton;
        public TMP_Text newGameButtonText;
        public Button continueButton;
        public TMP_Text continueButtonText;

        // Controls
        public GameObject controlsMenu;
        public TMP_Text controlsButtonText;

        // Settings
        public GameObject settingsMenu;
        public TMP_Text settingsButtonText;

        // Copyright
        public GameObject creditsMenu;
        public TMP_Text creditsButtonText;

        [Header("Controls Submenu")]
        // The controls title text.
        public TMP_Text controlsTitleText;

        // The text for the controls description.
        public TMP_Text controlsInstructText;

        // The text for the controls description.
        public TMP_Text controlsDescText;

        // THe key for the description text.
        private string controlsDescTextKey = "mnu_controls_desc";

        // The back button text for the controls sebmenu.
        public TMP_Text controlsBackButtonText;

        [Header("Animations")]
        // If 'true', transition animations are used.
        public bool useTransitions = true;

        // The transition object.
        public SceneTransition sceneTransition;

        // Awake is called when the script instance is loaded.
        private void Awake()
        {
            // Checks if LOL SDK has been initialized.
            GameSettings settings = GameSettings.Instance;

            // Gets an instance of the LOL manager.
            LOLManager lolManager = LOLManager.Instance;

            // Language
            JSONNode defs = SharedState.LanguageDefs;

            // Translate text.
            if (defs != null)
            {
                // Main Menu
                // startButtonText.text = defs["kwd_start"];
                newGameButtonText.text = defs["kwd_newGame"];
                continueButtonText.text = defs["kwd_continue"];

                controlsButtonText.text = defs["kwd_controls"];
                settingsButtonText.text = defs["kwd_settings"];
                creditsButtonText.text = defs["kwd_licenses"];

                // Controls Menu
                controlsTitleText.text = defs["kwd_controls"];
                controlsInstructText.text = defs["mnu_controls_instruct"];
                controlsDescText.text = defs[controlsDescTextKey];
                controlsBackButtonText.text = defs["kwd_back"];
            }
            else
            {
                // Mark all of the text.
                LanguageMarker marker = LanguageMarker.Instance;

                marker.MarkText(saveFeedbackText);

                marker.MarkText(newGameButtonText);
                marker.MarkText(continueButtonText);

                marker.MarkText(controlsButtonText);
                marker.MarkText(settingsButtonText);
                marker.MarkText(creditsButtonText);

                marker.MarkText(controlsTitleText);
                marker.MarkText(controlsInstructText);
                marker.MarkText(controlsDescText);
                marker.MarkText(controlsBackButtonText);
            }

            // Checks for initialization
            if (LOLSDK.Instance.IsInitialized)
            {
                // NOTE: the buttons disappear for a frame if there is no save state.
                // It doesn't effect anything, but it's jarring visually.
                // As such, the Update loop keeps them both on.


                // Set up the game initializations.
                if (newGameButton != null && continueButton != null)
                    lolManager.saveSystem.Initialize(newGameButton, continueButton);


                // Don't disable the continue button, otherwise the save data can't be loaded.
                // Enables/disables the continue button based on if there is loaded data or not.
                // continueButton.interactable = lolManager.saveSystem.HasLoadedData();
                // Continue button is left alone.

                // Since the player can't change the tutorial settings anyway when loaded from InitScene...
                // These are turned off just as a safety precaution. 
                // This isn't needed since the tutorial is activated by default if going from InitScene...
                // And can't be turned off.
                // overrideTutorial = true;
                // continueTutorial = true;

                // LOLSDK.Instance.SubmitProgress();
            }
            else
            {
                Debug.LogError("LOL SDK NOT INITIALIZED.");

                // You can save and go back to the menu, so the continue button is usable under that circumstance.
                if(LOLManager.Instance.saveSystem.HasLoadedData()) // Game has loaded data.
                {
                    // Tutorial should be overwritten.
                    overrideTutorial = true;

                    // Checks if the intro was cleared.
                    if (LOLManager.Instance.saveSystem.loadedData.clearedIntro)
                    {
                        // If the intro was cleared, then that means the tutorial was on last time.
                        continueTutorial = true;
                    }
                    else
                    {
                        // If the intro wasn't cleared, then the tutorial was disabled last time.
                        continueTutorial = false;
                    }

                    // Activate continue button.
                    continueButton.interactable = true;
                }
                else // No loaded data.
                {
                    // Disable continue button.
                    continueButton.interactable = false;
                }


                // Have the button be turned on no matter what for testing purposes.
                continueButton.interactable = true;

                // Adjust the audio settings since the InitScene was not used.
                settings.AdjustAllAudioLevels();
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            //  SceneManager.LoadScene("ResultsScene");

            // Sets the save text.
            if (saveFeedbackText != null && LOLSDK.Instance.IsInitialized)
            {
                saveFeedbackText.text = string.Empty;
                LOLManager.Instance.saveSystem.feedbackText = saveFeedbackText;
            }
            else
            {
                // Just empty out the string.
                saveFeedbackText.text = string.Empty;

                // Mark this as debug text.
                LanguageMarker.Instance.MarkText(saveFeedbackText);
            }
        }

        // Starts the game (general function for moving to the GameScene).
        public void StartGame()
        {
            // If transitions should be used, do a delayed game start.
            if (useTransitions)
                sceneTransition.LoadScene(GAME_SCENE_NAME);
            else
                SceneManager.LoadScene(GAME_SCENE_NAME);

        }

        // Starts a new game.
        public void StartNewGame()
        {
            // Clear out the loaded data and last save if the LOLSDK has been initialized.
            LOLManager.Instance.saveSystem.ClearLoadedAndLastSaveData();

            StartGame();
        }

        // Continues a saved game.
        public void ContinueGame()
        {
            // // Original
            // // Checks if there is loaded data.
            // if (LOLManager.Instance.saveSystem.HasLoadedData()) // Game has loaded data, so start game.
            // {
            //     // If the user's tutorial settings should be overwritten, do so.
            //     if (overrideTutorial)
            //         GameSettings.Instance.UseTutorial = continueTutorial;
            // 
            //     StartGame();
            //     
            // }
            // else // No data, so start new game.
            // {
            //     Debug.LogWarning("No save data found. New game to be loaded.");
            //     StartNewGame();
            // }

            // New
            // NOTE: a callback is setup onclick to load the save data.
            // Since that might happen after this function is processed...
            // It no longer checks for loaded data at this stage.

            // If the user's tutorial settings should be overwritten, do so.
            if (overrideTutorial)
                GameSettings.Instance.UseTutorial = continueTutorial;

            // Starts the game.
            StartGame();
        }

        // Toggles the controls menu.
        public void ToggleControlsMenu()
        {
            bool active = !controlsMenu.gameObject.activeSelf;
            controlsMenu.gameObject.SetActive(active);
            mainMenu.gameObject.SetActive(!active);

            // If the controls menu has been opened.
            if(active)
            {
                // Play the controls description.
                if(LOLSDK.Instance.IsInitialized && GameSettings.Instance.UseTextToSpeech && controlsDescTextKey != "")
                {
                    // Voice the text.
                    LOLManager.Instance.textToSpeech.SpeakText(controlsDescTextKey);
                }
            }
        }

        // Toggles the settings menu.
        public void ToggleSettingsMenu()
        {
            settingsMenu.gameObject.SetActive(!settingsMenu.gameObject.activeSelf);
            mainMenu.gameObject.SetActive(!mainMenu.gameObject.activeSelf);
        }

        // Toggles the credits menu.
        public void ToggleCreditsMenu()
        {
            creditsMenu.gameObject.SetActive(!creditsMenu.gameObject.activeSelf);
            mainMenu.gameObject.SetActive(!mainMenu.gameObject.activeSelf);
        }

        // Clears out the save.
        // TODO: This is only for testing, and the button for this should not be shown in the final game.
        public void ClearSave()
        {
            LOLManager.Instance.saveSystem.lastSave = null;
            LOLManager.Instance.saveSystem.loadedData = null;

            continueButton.interactable = false;
        }

        // Update is called once per frame
        void Update()
        {
            // The transitions block the UI input, so these buttons cannot be pressed once the scene starts transitioning.
            // Makes sure the new game button is kept on when the save system turns it off.
            if(!newGameButton.gameObject.activeSelf)
                newGameButton.gameObject.SetActive(true);

            // Makes sure the continue button is kept on when the save system turns it off.
            if (!continueButton.gameObject.activeSelf)
                continueButton.gameObject.SetActive(true);
        }
    }
}