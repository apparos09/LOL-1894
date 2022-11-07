using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// The results manager.

namespace RM_BBTS
{
    // The results manager.
    public class ResultsManager : MonoBehaviour
    {
        // The rooms cleared text.
        public TMPro.TMP_Text roomsClearedText;

        // The total time text.
        public TMPro.TMP_Text totalTimeText;

        // The total turns text.
        public TMPro.TMP_Text totalTurnsText;

        // Move 0 display text.
        public TMPro.TMP_Text move0Text;

        // Move 1 display text.
        public TMPro.TMP_Text move1Text;

        // Move 2 display text.
        public TMPro.TMP_Text move2Text;

        // Move 3 display text.
        public TMPro.TMP_Text move3Text;

        // Start is called before the first frame update
        void Start()
        {
            // Finds the results object.
            ResultsData rd = FindObjectOfType<ResultsData>();

            // Results object has been found.
            if(rd != null)
            {
                // Rooms cleared.
                roomsClearedText.text = "Rooms Cleared: " + rd.roomsCleared.ToString() + " / " + rd.totalRooms.ToString();

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
                    totalTimeText.text = "Total Time: " + minutes.ToString("00") + ":" + seconds.ToString("00");

                }

                // Total turns.
                totalTurnsText.text = "Total Turns: " + rd.totalTurns.ToString();

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

        // Update is called once per frame
        void Update()
        {

        }
    }
}