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

        [Header("Main Menu")]
        // Menu
        public GameObject mainMenu;

        // Start
        public Button newGameButton;
        public TMP_Text newGameButtonText;
        public Button continueButton;
        public TMP_Text continueButtonText;

        // Info
        public GameObject infoMenu;
        public TMP_Text infoButtonText;

        // Controls
        public GameObject controlsMenu;
        public TMP_Text controlsButtonText;

        // Settings
        public GameObject settingsMenu;
        public TMP_Text settingsButtonText;

        [Header("Controls Submenu")]
        // The controls title text.
        public TMP_Text controlsTitleText;

        // The text for the controls description.
        public TMP_Text controlsInstructText;

        // The text for the controls description.
        public TMP_Text controlsDescText;

        // The back button text for the controls sebmenu.
        public TMP_Text controlsBackButtonText;

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
            if(defs != null)
            {
                // Main Menu
                // startButtonText.text = defs["kwd_start"];
                newGameButtonText.text = defs["kwd_newGame"];
                continueButtonText.text = defs["kwd_continue"];

                infoButtonText.text = defs["kwd_info"];
                controlsButtonText.text = defs["kwd_controls"];
                settingsButtonText.text = defs["kwd_settings"];

                // Controls Menu
                controlsTitleText.text = defs["kwd_controls"];
                controlsInstructText.text = defs["mnu_controls_instruct"];
                controlsDescText.text = defs["mnu_controls_desc"];
                controlsBackButtonText.text = defs["kwd_back"];
            }


            // Checks for initialization
            if (!settings.InitializedLOLSDK)
            {
                Debug.LogError("LOL SDK NOT INITIALIZED.");

                // Do not allow the button to be used.
                continueButton.interactable = false;
                
                settings.AdjustAllAudioLevels();
            }
            else
            {
                // NOTE: the buttons disappear for a frame if there is no save state.
                // It doesn't effect anything, but it's jarring visually.
                // Initialize the game.
                if(newGameButton != null && continueButton != null)
                    lolManager.saveSystem.Initialize(newGameButton, continueButton);

                // TODO: if the continue button is made invisible, just turn it on and disable it instead?
                // Maybe change this?
                if(!continueButton.gameObject.activeSelf) // No save available.
                {
                    continueButton.gameObject.SetActive(true);
                    continueButton.interactable = false;
                }
                else // Save available.
                {
                    continueButton.interactable = true;
                }

                // LOLSDK.Instance.SubmitProgress();
            }

        }

        // Start is called before the first frame update
        void Start()
        {
            // TODO: so it seems that you need to save the data for the game using the SDK.
            // I say you load up the save data during the GameInit phase, but don't actually apply it unless 'Continue' is presed.
            // You'll also need to setup an autosave async feature.
        }

        // Starts the game.
        public void StartGame()
        {
            SceneManager.LoadScene("GameScene");
        }

        // Starts a new game.
        public void StartNewGame()
        {
            // Clear out the loaded data.
            LOLManager.Instance.saveSystem.loadedData = null;
            StartGame();
        }

        // Continues a saved game.
        public void ContinueGame()
        {
            // If there is no loaded data.
            if(LOLManager.Instance.saveSystem.loadedData == null)
            {
                Debug.LogWarning("No save data found. New game to be loaded.");
                StartNewGame();
            }
            else // Loaded data will be inplemented by the gameplay manager when entering the scene.
            {
                StartGame();
            }            
        }


        // Toggles the info menu.
        public void ToggleInfoMenu()
        {
            infoMenu.gameObject.SetActive(!infoMenu.gameObject.activeSelf);
            mainMenu.gameObject.SetActive(!mainMenu.gameObject.activeSelf);
        }

        // Toggles the controls menu.
        public void ToggleControlsMenu()
        {
            controlsMenu.gameObject.SetActive(!controlsMenu.gameObject.activeSelf);
            mainMenu.gameObject.SetActive(!mainMenu.gameObject.activeSelf);
        }

        // Toggles the settings menu.
        public void ToggleSettingsMenu()
        {
            settingsMenu.gameObject.SetActive(!settingsMenu.gameObject.activeSelf);
            mainMenu.gameObject.SetActive(!mainMenu.gameObject.activeSelf);
        }

        // Clears out the save. This is only for testing, and the button for this should not be shown in the final game.
        public void ClearSave()
        {
            LOLManager.Instance.saveSystem.lastSave = null;
            LOLManager.Instance.saveSystem.loadedData = null;

            continueButton.interactable = false;
        }

        // // Update is called once per frame
        // void Update()
        // {
        // 
        // }
    }
}