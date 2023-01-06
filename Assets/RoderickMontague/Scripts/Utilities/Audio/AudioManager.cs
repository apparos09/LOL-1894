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

        // The audio source for jingles.
        // This is tagged as a BGM so that it shares the same volume.
        public AudioSource jngSource;

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
            // TODO: there still appears to be an issue where the background music isn't reset properly.
            // This runs into conflict when you play a jingle and end up changing the background music.
            // You need to fix that.

            // Return the pitch to normal.
            if (resetPitch)
                bgmSource.pitch = 1.0F;

            // Stops the BGM, replaces it, then plays it from the beginning.
            bgmSource.Stop();
            bgmSource.clip = clip;
            bgmSource.Play();

            // The pitch shouldn't transition since the song has changed.
            pitchTimer = 0.0F;
        }

        // Plays the BGM and changes the pitch.
        public void PlayBackgroundMusic(AudioClip clip, float pitch)
        {
            // Changes the pitch.
            bgmSource.pitch = pitch;

            // Turn off the pitch timer since the audio has been replaced.
            // This may not be needed?
            pitchTimer = 0.0F;

            // Play the BGM.
            PlayBackgroundMusic(clip, false);
        }

        // Plays the sound effect.
        public void PlaySoundEffect(AudioClip clip)
        {
            sfxSource.PlayOneShot(clip);
        }


        // Plays a jingle.
        public void PlayJingle(AudioClip clip, bool resetPitch, float extraWaitTime = 0.0F)
        {   
            // Plays teh game jingle.
            if(jngSource != null) // New
            {
                // Checks if the BGM is playing.
                bool bgmPlaying = bgmSource.isPlaying;

                // Pauses the BGM.
                // Using Pause() caused problems, so Stop() is now used instead.
                if (bgmPlaying)
                    bgmSource.Stop();

                // Plays the jingle.
                jngSource.PlayOneShot(clip);

                // Plays the bgm after the clip is finished.
                if(bgmPlaying)
                {
                    // Wait for turning back on the BGM.
                    float waitTime = clip.length + extraWaitTime;

                    // Recommended that extra wait time is given since the BGM always seems to start too soon.

                    // Delayed play and revert the pitch back when the BGM starts up again.
                    bgmSource.PlayDelayed(waitTime);
                }
                    
            }
            else // Old
            {
                // Reuses the BGM audio source and stops the BGM audio for a moment.
                // This is unreliable since the audio often starts up before the jingle is finished.

                // Stops the BGM and reset the pitch.
                // The audio must be stopped, otherwise it will start automatically when PlayOneShot() is called.
                // The song restarts when using the Stop() function, but it isn't a big deal.
                // bgmSource.Pause();
                bgmSource.Stop();
                
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
                // Extra wait time can be provided if the game should wait longer.
                bgmSource.PlayDelayed(clip.length + ((extraWaitTime < 0.0F) ? 0.0F : extraWaitTime));
            }
        }

        // Stops the jingle audio from playing.
        // This only works if jingles are on a seperate audio source from the BGM.
        public void StopJingle()
        {
            // Checks if the jingle has been set.
            if (jngSource != null)
                jngSource.Stop();
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