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
    public class TitleManager : MonoBehaviour
    {

        [Header("Menus")]
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

        [Header("Buttons")]
        public TMP_Text viewControlsButtonText;
        public TMP_Text viewOptionsButtonText;

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

                viewControlsButtonText.text = defs["viewControls"];
                viewOptionsButtonText.text = defs["viewOptions"];
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