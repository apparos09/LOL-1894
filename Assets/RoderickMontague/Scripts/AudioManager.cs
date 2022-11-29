using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_BBTS
{
    // An audio manager.
    public class AudioManager : MonoBehaviour
    {
        // These elements are seperate since they play at different pitches.
        // The BGM will have its pitch changed, but the SFX won't.

        // Sound for game music.
        public AudioSource bgmSource;

        // Source for sound effects.
        // Jingles are counted as sound effects, and thus share the same audio.
        public AudioSource sfxSource;

        // Start is called before the first frame update
        void Start()
        {
            // // Sets the tag for the objects.
            // bgmSource.tag = GameSettings.BGM_TAG;
            // sfxSource.tag = GameSettings.SFX_TAG;
        }

        // Plays the provided BGM and resets the pitch.
        public void PlayBgm(AudioClip clip, bool resetPitch = true)
        {
            // Return the pitch to normal.
            if (resetPitch)
                bgmSource.pitch = 1.0F;

            // Stops the BGM, replaces it, then plays it from the beginning.
            bgmSource.Stop();
            bgmSource.clip = clip;
            bgmSource.Play();
        }

        // Plays the BGM and changes the pitch.
        public void PlayBgm(AudioClip clip, float pitch)
        {
            // Change the pitch.
            bgmSource.pitch = pitch;

            // Play the BGM.
            PlayBgm(clip, false);
        }
    }
}