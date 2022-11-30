using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RM_BBTS
{
    public class ButtonAudio : MonoBehaviour
    {
        // The button this script is for.
        public Button button;

        // THe audio for the user inputs.
        public MenuAudio menuAudio;

        // // The audio source for the button operations.
        // public AudioSource audioSource;
        // 
        // // The clip for the button audio.
        // public AudioClip clickClip;

        // Start is called before the first frame update
        void Start()
        {
            // Button not set.
            if (button == null)
            {
                // Tries to grab the button from the parent object.
                gameObject.TryGetComponent<Button>(out button);
            }

            // Listener for the tutorial toggle.
            button.onClick.AddListener(delegate
            {
                OnClick();
            });
        }


        // Called when the button is clicked.
        private void OnClick()
        {
            // audioSource.PlayOneShot(clickClip);
            menuAudio.PlayButtonSoundEffect();
        }

        // Script is destroyed.
        private void OnDestroy()
        {
            // Remove the listener for onClick.
            button.onClick.RemoveListener(OnClick);
        }
    }
}