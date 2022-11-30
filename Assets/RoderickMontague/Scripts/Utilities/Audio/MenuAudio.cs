using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_BBTS
{
    // The audio for user inputs.
    public class MenuAudio : MonoBehaviour
    {
        // The audio source.
        public AudioSource audioSource;

        // The button sound effect.
        public AudioClip buttonClickClip;

        // TODO: play the button at different pitches for other parts of the menu.
        // Plays the button sound effect.
        public void PlayButtonSoundEffect()
        {
            audioSource.PlayOneShot(buttonClickClip);
        }
    }
}