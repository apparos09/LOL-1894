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
        // Start is called before the first frame update
        void Start()
        {

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