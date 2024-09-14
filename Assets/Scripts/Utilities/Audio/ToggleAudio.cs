using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RM_BBTS
{
    // Plays audio when clicking a toggle.
    public class ToggleAudio : MonoBehaviour
    {
        // The button this script is for.
        public Toggle toggle;

        // THe audio for the user inputs.
        public MenuAudio menuAudio;

        // Awake is called when the script instance is being loaded.
        private void Awake()
        {
            // Moved here in case the button has not been set enabled before the game was closed.

            // Button not set.
            if (toggle == null)
            {
                // Tries to grab the button from the parent object.
                gameObject.TryGetComponent<Toggle>(out toggle);
            }

            // Listener for the tutorial toggle.
            toggle.onValueChanged.AddListener(delegate
            {
                OnValueChanged(toggle.isOn);
            });
        }

        // Start is called before the first frame update
        void Start()
        {
            // ...
        }


        // Called when the toggle is clicked.
        private void OnValueChanged(bool isOn)
        {
            menuAudio.PlayButtonSoundEffect();
        }

        // Script is destroyed.
        private void OnDestroy()
        {
            // Remove the listener for onValueChanged.
            toggle.onValueChanged.RemoveListener(OnValueChanged);
        }
    }
}