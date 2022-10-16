using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoderickMontague_BattleBotTrainingSimulation
{
    // Provides controls for audio sources.
    // NOTE: for this to work the tag must be either "BGM" or 'SFX".
    public class AudioSourceControl : MonoBehaviour
    {
        // The audio source this script applies to.
        public AudioSource audioSource;

        // The maximum volume of the audio source, which is it's audio level at runtime.
        // This is private so that the volume can't be set out of the range.
        [Tooltip("The maximum volume for the audio source, which by default is the audio level at runtime unless it's preset.")]
        [Range(0.0F, 1.0F)]
        private float maxVolume = -1.0F;

        // Awake is called when the script instance is being loaded.
        private void Awake()
        {
            // If the audio source wans't set then try to grab the component.
            if (audioSource == null)
                audioSource = gameObject.GetComponent<AudioSource>();

            // The max volume has not been set.
            if (maxVolume < 0.0F)
                maxVolume = audioSource.volume;
        }

        // Start is called before the first frame update
        void Start()
        {
            // adjusts the audio.
            GameSettings.Instance.AdjustAudio(this);
        }

        // called when the object becomes active.
        private void OnEnable()
        {
            // adjusts the audio levels.
            GameSettings.Instance.AdjustAudio(this);
        }

        // The maximum volume variable.
        public float MaxVolume
        {
            get
            {
                return maxVolume;
            }

            set
            {
                maxVolume = Mathf.Clamp01(maxVolume);
                AdjustToGameSettings();
            }
        }

        // adjusts the audio controls using the game settings.
        public void AdjustToGameSettings()
        {
            GameSettings.Instance.AdjustAudio(this);
        }

        // // Update is called once per frame
        // void Update()
        // {
        //     
        // }
    }
}