using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SimpleJSON;
using LoLSDK;
using TMPro;

namespace RM_BBTS
{
    // The results manager.
    public class ResultsManager : MonoBehaviour
    {
        // The title text.
        public TMP_Text titleText;

        // The save feedback text for when the game ends.
        public TMP_Text saveFeedbackText;

        [Header("Stats")]

        // The final score.
        public TMP_Text scoreText;

        // The rooms cleared text.
        public TMP_Text roomsClearedText;

        // The total time text.
        public TMP_Text totalTimeText;

        // The total turns text.
        public TMP_Text totalTurnsText;

        // The questions correct text.
        public TMP_Text questionsCorrectText;

        // The questions asked (no repeats) text.
        public TMP_Text questionsAsked;

        // The text for the final level.
        public TMP_Text finalLevelText;

        [Header("Stats/Move Text")]
        // Text for the move title.
        public TMP_Text moveSubtitleText;

        // Move 0 display text.
        public TMP_Text move0Text;

        // Move 1 display text.
        public TMP_Text move1Text;

        // Move 2 display text.
        public TMP_Text move2Text;

        // Move 3 display text.
        public TMP_Text move3Text;

        [Header("Buttons")]
        // The finish button.
        public TMP_Text finishButtonText;

        [Header("Animations")]
        // If transitions should be used.
        public bool useTransitions = true;

        // The scene transition.
        public SceneTransition sceneTransition;

        // Awake is caleld when a script instance is being loaded.
        private void Awake()
        {
            // Turns off the entrance animation if scene transitions shouldn't be used.
            sceneTransition.useSceneEnterAnim = useTransitions;
        }

        // Start is called before the first frame update
        void Start()
        {
            // The language defs.
            JSONNode defs = SharedState.LanguageDefs;

            // Labels for translation.
            string titleLabel = "Results";
            string scoreLabel = "Final Score";
            string roomsClearedLabel = "Rooms Cleared";
            string totalTimeLabel = "Total Time";
            string totalTurnsLabel = "Total Turns";
            string questionsCorrectLabel = "Questions Correct";
            string questionsAskedLabel = "Questions Asked";
            string noRepeatsLabel = "No Repeats";
            string finalLevelLabel = "Final Level";
            string finalMovesLabel = "Final Moves";

            // The main menu title text.
            string finishLabel = "Finish";

            // The speak key for the title.
            string titleSpeakKey = "";

            // Translate title text.
            if (defs != null)
            {
                titleSpeakKey = "kwd_results";
                titleLabel = defs[titleSpeakKey];

                scoreLabel = defs["kwd_finalScore"];
                roomsClearedLabel = defs["kwd_roomsCleared"];
                totalTimeLabel = defs["kwd_totalTime"];
                totalTurnsLabel = defs["kwd_totalTurns"];
                questionsCorrectLabel = defs["kwd_questionsCorrect"];
                questionsAskedLabel = defs["kwd_questionsAsked"];
                noRepeatsLabel = defs["kwd_noRepeats"];
                finalLevelLabel = defs["kwd_finalLevel"];
                finalMovesLabel = defs["kwd_finalMoves"];

                finishLabel = defs["kwd_finish"];
            }
            else // If the language file isn't loaded, then mark the text objects.
            {
                LanguageMarker marker = LanguageMarker.Instance;

                marker.MarkText(titleText);
                marker.MarkText(saveFeedbackText);

                marker.MarkText(scoreText);
                marker.MarkText(roomsClearedText);
                marker.MarkText(totalTimeText);
                marker.MarkText(totalTurnsText);
                marker.MarkText(questionsCorrectText);
                marker.MarkText(questionsAsked);
                marker.MarkText(finalLevelText);
                marker.MarkText(moveSubtitleText);

                marker.MarkText(move0Text);
                marker.MarkText(move1Text);
                marker.MarkText(move2Text);
                marker.MarkText(move3Text);

                marker.MarkText(finishButtonText);

            }

            // Change out titles and buttons with translated label.
            titleText.text = titleLabel;
            moveSubtitleText.text = finalMovesLabel;

            finishButtonText.text = finishLabel;

            // Change out button text with translated.

            // Finds the results object.
            ResultsData rd = FindObjectOfType<ResultsData>();

            // Results object has been found.
            if(rd != null)
            {
                // Final score
                scoreText.text = scoreLabel + ": " + rd.finalScore;
                
                // Rooms cleared.
                roomsClearedText.text = roomsClearedLabel + ": " + rd.roomsCompleted.ToString() + "/" + rd.roomsTotal.ToString();

                // Total time.
                {
                    // Calculates the total time, limiting it to 99 miuntes and 59 seconds.
                    // Max Time = 60 * 99 + 59 = 5940 + 59 = 5999 [99:59]
                    float totalTime = Mathf.Clamp(rd.totalTime, 0, 5999.0F); // total time in seconds.

                    // NEW - USES MODULUS //
                    float minutes = Mathf.Floor(totalTime / 60.0F); // minutes (floor round to remove seconds).
                    float seconds = Mathf.Ceil(totalTime - (minutes * 60.0F)); // seconds (round up to remove nanoseconds).

                    // Sets the text.
                    totalTimeText.text = totalTimeLabel + ": " + minutes.ToString("00") + ":" + seconds.ToString("00");

                }

                // Total turns.
                totalTurnsText.text = totalTurnsLabel + ": " + rd.totalTurns.ToString();

                // Questions Correct Text
                questionsCorrectText.text = questionsCorrectLabel + ": " + 
                    rd.questionsCorrect.ToString() + "/" + rd.questionsUsed.ToString();

                // Questions Correct (No Repeats) Text
                // questionsAsked.text = questionsCorrectLabel + " (" + noRepeatsLabel + "): " +
                //     rd.questionsCorrectNoRepeats.ToString() + "/" + rd.questionsUsedNoRepeats.ToString();

                // Questions Asked (No Repeats) Text
                questionsAsked.text = questionsAskedLabel + " (" + noRepeatsLabel + "): " + 
                    rd.questionsUsedNoRepeats.ToString();

                // Final player level
                finalLevelText.text = finalLevelLabel + ": " + rd.finalLevel.ToString();

                // Move text.
                move0Text.text = rd.move0;
                move1Text.text = rd.move1;
                move2Text.text = rd.move2;
                move3Text.text = rd.move3;

                // Destroy the object.
                Destroy(rd.gameObject);
            }

            // Say the name of the title text.
            if(LOLSDK.Instance.IsInitialized && GameSettings.Instance.UseTextToSpeech && titleSpeakKey != "")
            {
                // Voice the title text.
                LOLManager.Instance.textToSpeech.SpeakText(titleSpeakKey);
            }

            // Provides the save feedback text.
            if (saveFeedbackText != null && LOLSDK.Instance.IsInitialized)
            {
                saveFeedbackText.text = string.Empty;
                LOLManager.Instance.saveSystem.feedbackText = saveFeedbackText;
            }
            else
            {
                // Just empty out the string.
                saveFeedbackText.text = string.Empty;
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
                // Logs the error.
                Debug.LogError("SDK NOT INITIALIZED. RETURNING TO MAIN MENU.");

                // Return to the main menu scene.
                ToMainMenu();
            }

            // Do not return to the main menu scene if running through the LOL platform.
            // This is because you can't have the game get repeated in the same session.
            // ToMainMenu();
        }
    }
}