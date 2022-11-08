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
        public TMP_Text startButtonText;

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
            // set to 30 FPS
            Application.targetFrameRate = 30;

            // checks if LOL SDK has been initialized.
            GameSettings settings = GameSettings.Instance;

            // if the LOL SDK has been initialized...
            if (settings.InitializedLOLSDK)
            {
                JSONNode defs = SharedState.LanguageDefs;

                // Main Menu
                startButtonText.text = defs["kwd_start"];
                infoButtonText.text = defs["kwd_info"];
                controlsButtonText.text = defs["kwd_controls"];
                settingsButtonText.text = defs["kwd_settings"];

                // Controls Menu
                controlsTitleText.text = defs["kwd_controls"];
                controlsInstructText.text = defs["menu_controls_instruct"];
                controlsDescText.text = defs["menu_controls_desc"];
                controlsBackButtonText.text = defs["kwd_back"];
            }
            else
            {
                Debug.LogError("LOL SDK NOT INITIALIZED.");
                settings.AdjustAllAudioLevels();
            }

        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Starts the game.
        public void StartGame()
        {
            SceneManager.LoadScene("GameScene");
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

        // Update is called once per frame
        void Update()
        {

        }
    }
}