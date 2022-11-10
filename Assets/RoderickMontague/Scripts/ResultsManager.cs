using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SimpleJSON;
using LoLSDK;

namespace RM_BBTS
{
    // The results manager.
    public class ResultsManager : MonoBehaviour
    {
        // The title text.
        public TMPro.TMP_Text titleText;

        [Header("Stats")]
        // The rooms cleared text.
        public TMPro.TMP_Text roomsClearedText;

        // The total time text.
        public TMPro.TMP_Text totalTimeText;

        // The total turns text.
        public TMPro.TMP_Text totalTurnsText;

        // The text for the final level.
        public TMPro.TMP_Text finalLevelText;

        [Header("Stats/Move Text")]
        // Text for the move title.
        public TMPro.TMP_Text moveSubtitleText;

        // Move 0 display text.
        public TMPro.TMP_Text move0Text;

        // Move 1 display text.
        public TMPro.TMP_Text move1Text;

        // Move 2 display text.
        public TMPro.TMP_Text move2Text;

        // Move 3 display text.
        public TMPro.TMP_Text move3Text;

        [Header("Buttons")]
        // The finish button.
        public TMPro.TMP_Text finishButtonText;

        // Main menu button text.
        // TODO: hide this button when submitting the game.
        public TMPro.TMP_Text mainMenuButtonText;

        // Start is called before the first frame update
        void Start()
        {
            // The language defs.
            JSONNode defs = SharedState.LanguageDefs;

            // Labels for translation.
            string titleLabel = "<Results>";
            string roomsClearedLabel = "<Rooms Cleared>";
            string totalTimeLabel = "<Total Time>";
            string totalTurnsLabel = "<Total Turns>";
            string finalLevelLabel = "<Final Level>";
            string finalMovesLabel = "<Final Moves>";

            // The main menu title text.
            string finishLabel = "<Finish>";
            string mainMenuLabel = "<Main Menu>";

            // Translate title text.
            if(defs != null)
            {
                titleLabel = defs["kwd_results"];
                roomsClearedLabel = defs["kwd_roomsCleared"];
                totalTimeLabel = defs["kwd_totalTime"];
                totalTurnsLabel = defs["kwd_totalTurns"];
                finalLevelLabel = defs["kwd_finalLevel"];
                finalMovesLabel = defs["kwd_finalMoves"];

                finishLabel = defs["kwd_finish"];
                mainMenuLabel = defs["kwd_mainMenu"];
            }

            // Change out titles and buttons with translated label.
            titleText.text = titleLabel;
            moveSubtitleText.text = finalMovesLabel;

            finishButtonText.text = finishLabel;
            mainMenuButtonText.text = mainMenuLabel;

            // Change out button text with translated.

            // Finds the results object.
            ResultsData rd = FindObjectOfType<ResultsData>();

            // Results object has been found.
            if(rd != null)
            {
                // Final level
                finalLevelText.text = finalLevelLabel + ": " + rd.finalLevel.ToString();

                // Rooms cleared.
                roomsClearedText.text = roomsClearedLabel + ": " + rd.roomsCleared.ToString() + " / " + rd.totalRooms.ToString();

                // Total time.
                {
                    // ORIGINAL //
                    // Minutes. This rounds down to the nearest whole number.
                    // float minutes = Mathf.Floor(rd.totalTime / 60.0F); // original

                    // Seconds. This rounds up the remaining seconds to a whole number.
                    // float seconds = Mathf.Ceil(rd.totalTime - (minutes * 60.0F));

                    // NEW - USES MODULUS //
                    float seconds = Mathf.CeilToInt(rd.totalTime) % 60; // seconds
                    float minutes = Mathf.CeilToInt(rd.totalTime) - seconds; // minutes

                    // Sets the text.
                    totalTimeText.text = totalTimeLabel + ": " + minutes.ToString("00") + ":" + seconds.ToString("00");

                }

                // Total turns.
                totalTurnsText.text = totalTurnsLabel + ": " + rd.totalTurns.ToString();

                // Move text.
                move0Text.text = rd.move0;
                move1Text.text = rd.move1;
                move2Text.text = rd.move2;
                move3Text.text = rd.move3;

                // Destroy the object.
                Destroy(rd.gameObject);
            }
        }

        // Goes to the main menu.
        public void ToMainMenu()
        {
            SceneManager.LoadScene("TitleScene");
        }

        // Call this function to complete the game. This is called by the "finish" button.
        public void CompleteGame()
        {
            // The SDK has been initialized.
            if(LOLSDK.Instance.IsInitialized)
            {
                // Complete the game.
                LOLSDK.Instance.CompleteGame();
            }
            else
            {
                Debug.LogError("SDK NOT INITIALIZED");

                // Return to the main menu scene.
                ToMainMenu();
            }
            
        }
    }
}