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
        public AudioSource sfxSource;

        // TODO: having the jingle on a seperate audio source didn't work for delaying the BGM.
        // I think the pitch change caused a problem.
        // Either way, I'll just set the pitch back to what it was before after (X) amount of time.

        // The audio source for jingles.
        // This is tagged as a BGM so that it shares the same volume.
        // public AudioSource jngSource;

        // The pitch for the paused BGM.
        private float bgmPausedPitch = 1.0F;

        // The timer for changing the BGM pitch back to what it was before.
        private float pitchTimer = 0.0F;

        // Start is called before the first frame update
        void Start()
        {
            // // Sets the tag for the objects.
            // bgmSource.tag = GameSettings.BGM_TAG;
            // sfxSource.tag = GameSettings.SFX_TAG;
        }

        // Plays the provided BGM and resets the pitch.
        public void PlayBackgroundMusic(AudioClip clip, bool resetPitch = true)
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
            PlayBackgroundMusic(clip, false);
        }

        // Plays the sound effect.
        public void PlaySoundEffect(AudioClip clip)
        {
            sfxSource.PlayOneShot(clip);
        }

        // Plays a jingle.
        public void PlayJingle(AudioClip clip, bool resetPitch)
        {
            // Pause the BGM and reset the pitch.
            bgmSource.Pause();

            // NOTE: one way to fix this is to have a seperate audio object for jingles.
            // That way, the pitch can be retained when the song starts again.
            // That wasn't done here 

            // Resets the pitch so that the song now plays at normal speed.
            if (resetPitch)
            {
                bgmSource.pitch = 1.0F;
                pitchTimer = 0.0F;
            }
            else
            {
                // Saves the old pitch and sets the BGM back to normal.
                bgmPausedPitch = bgmSource.pitch;
                bgmSource.pitch = 1.0F;

                // Timer for chaging the pitch back.
                pitchTimer = clip.length;
            }
                
            // Play a one shot of this clip.
            bgmSource.PlayOneShot(clip);

            // Start playing the BGM again after this clip is finished.
            bgmSource.PlayDelayed(clip.length);
        }

        // Update is called once per frame
        void Update()
        {
            // Reduces the pitch timer.
            if(pitchTimer > 0.0F)
            {
                // Decrease timer.
                pitchTimer -= Time.deltaTime;

                // Timer has run out.
                if(pitchTimer <= 0.0F)
                {
                    // Pitch now set to normal.
                    bgmSource.pitch = bgmPausedPitch;

                    // Reset values.
                    pitchTimer = 0;
                    bgmPausedPitch = 0.0F;
                }
            }
        }
    }
}