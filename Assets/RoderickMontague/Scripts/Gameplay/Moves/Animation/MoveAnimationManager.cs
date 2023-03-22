using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static RM_BBTS.Move;

namespace RM_BBTS
{
    // The move animator ids.
    public enum moveAnim { 
        none, blast1, blast2, burst1, colorWave1, crawl1, fill1, shield1, shootingStar1, 
        shot1, shot2, slash1, slash2, smack1, smack2, smack3, twister1, wave1, wave2, wave3 };

    // The move animation manager.
    public class MoveAnimationManager : MonoBehaviour
    {
        // Animaton enum.
        // The animator for the move.
        public Animator animator;

        // The image being animated (if being used in screen space).
        public Image animatedImage;

        // The sprite being animated (if being used in world space).
        public SpriteRenderer animatedSpriteRender;

        // The blank sprite, which is the default sprite
        public Sprite defaultSprite;

        // The default color for the image/sprite.
        public Color defaultColor = Color.white;

        // The variable to be changed.
        public const string ANIM_VAR = "anim";

        // Gets set to 'true' if the animation is running.
        private bool animPlaying = false;

        // The timer to automatically tell an animaton to stop if it hasn't already.
        private float animTimer = 0.0F;

        // Extra time to add to the anim timer.
        private const float ANIM_TIMER_EXTRA = 0.60F;

        // Set to call the move performance results once the animation is over.
        [HideInInspector()]
        public bool callMoveResults = false;

        // The move this animation is playing for, the user, the target, and the battle.
        // These are used to play the move results when the animation is done.
        protected Move move;
        protected BattleEntity user;
        protected BattleEntity target;
        protected BattleManager battle;

        // The effects that are played for the user and the target.
        protected moveEffect userEffect = moveEffect.none;
        protected moveEffect targetEffect = moveEffect.none;

        // Determines if the animation should be flipped.
        protected bool flip;

        [Header("Audio")]
        // The audio for the move animation.
        public AudioSource audioSource;

        // Sound Effects
        [Header("Audio/Sound Effects")]
        public AudioClip pingHighSfx;
        public AudioClip pingLowSfx;
        public AudioClip laserSfx;
        public AudioClip warp01Sfx;
        public AudioClip warp02Sfx;
        public AudioClip warp03Sfx;
        public AudioClip riverSfx;
        public AudioClip clongSfx;
        public AudioClip creepingVinesSfx;
        public AudioClip smackSfx;
        public AudioClip whooshHighSfx;
        public AudioClip whooshLowSfx;
        public AudioClip windSfx;

        [Header("Text Box")]
        // A textbox that might be showing when the battle animation is playing.
        public TextBox textBox;
        // Disables the text box buttons when playing the animation.
        public bool disableTextBoxControlsWhenPlaying = true;

        // Start is called before the first frame update
        void Start()
        {
            // Debug.Log("Test");
            // PlayTest();
        }

        // Returns 'true' if the animation is playing.
        public bool AnimationIsPlaying()
        {
            return animPlaying;
        }

        // Plays the animation.
        public void PlayAnimation(moveAnim anim)
        {
            // // No animaton to play.
            // if (anim == moveAnim.none)
            //     return;

            // // Changes the animation.
            // switch(anim)
            // {
            //     case moveAnim.smack1: // Smack Animation
            //         animator.SetInteger(ANIM_VAR, 1);
            //         break;
            // }

            // Stop the current animation if it is playing.
            if (animPlaying)
                StopAnimation();

            // Just reuses the enum value.
            animator.SetInteger(ANIM_VAR, (int)anim);

            // Sets the animation color.
            if (move != null)
            {
                // If the image is being used.
                if(animatedImage != null)
                {
                    // Give move color.
                    animatedImage.color = move.animationColor;

                    // Images don't have a built-in flip feature, so this needs to be done instead.
                    animatedImage.transform.localScale = (flip) ? new Vector3(-1.0F, 1.0F, 1.0F) : Vector3.one;
                }

                // If the sprite is being used.
                if(animatedSpriteRender != null)
                {
                    // Give move color.
                    animatedSpriteRender.color = move.animationColor;

                    // Flips the sprite.
                    animatedSpriteRender.flipX = flip;
                }
                    
            }

            // Turn on the animator object. This function call replays the animation.
            // animator.gameObject.SetActive(true);
            // TODO: Doesn't seem to do anything, so take it out.
            // animator.enabled = true;

            // Animation is running. 
            animPlaying = true;

            // Gets the current time for the animation, plus extra time. 
            // Just going by the clip legnth doesn't allow the animation to play out fully. 
            // Goes by the state length if the clip does not exist. 
            if (animator.GetCurrentAnimatorClipInfo(0).Length > 0)
                animTimer = (animator.GetCurrentAnimatorClipInfo(0)[0].clip.length + ANIM_TIMER_EXTRA) / animator.speed;
            else
                animTimer = (animator.GetCurrentAnimatorStateInfo(0).length + ANIM_TIMER_EXTRA) / animator.speed;


            // Disables the text box controls when playing the animation.
            if (disableTextBoxControlsWhenPlaying)
            {
                // Disable the controls.
                textBox.DisableTextBoxControls();

                // Pause the auto timer so that it doesn't continue until the animation is done.
                textBox.autoNextTimerPaused = true;
            }
                
        }

        // // Plays the spiral animation.
        // public void PlayTest()
        // {
        //     // Plays the test animation.
        //     PlayAnimation(0);
        //     // TODO: change the animation number, then turn on the object.
        // }


        // Plays a sound.
        public void PlaySound(AudioClip clip)
        {
            // Plays the provided audio clip.
            if (audioSource != null && clip != null)
                audioSource.PlayOneShot(clip);
        }

        // Plays a specific sound.
        public void PlayPingHighSfx()
        {
            PlaySound(pingHighSfx);
        }

        // Plays a specific sound.
        public void PlayPingLowSfx()
        {
            PlaySound(pingLowSfx);
        }

        // Plays a specific sound.
        public void PlayLaserSfx()
        {
            PlaySound(laserSfx);
        }
        // Plays a specific sound.
        public void PlayWarp01Sfx()
        {
            PlaySound(warp01Sfx);
        }

        // Plays a specific sound.
        public void PlayWarp02Sfx()
        {
            PlaySound(warp02Sfx);
        }

        // Plays a specific sound.
        public void PlayWarp03Sfx()
        {
            PlaySound(warp03Sfx);
        }

        // Plays a specific sound.
        public void PlayRiverSfx()
        {
            PlaySound(riverSfx);
        }

        // Plays a specific sound.
        public void PlayClongSfx()
        {
            PlaySound(clongSfx);
        }

        // Plays a specific sound.
        public void PlayCreepingVinesSfx()
        {
            PlaySound(creepingVinesSfx);
        }

        // Plays a specific sound.
        public void PlaySmackSfx()
        {
            PlaySound(smackSfx);
        }

        // Plays a specific sound.
        public void PlayWhooshHighSfx()
        {
            PlaySound(whooshHighSfx);
        }

        // Plays a specific sound.
        public void PlayWhooshLowSfx()
        {
            PlaySound(whooshLowSfx);
        }

        // Plays a specific sound.
        public void PlayWindSfx()
        {
            PlaySound(windSfx);
        }

        // Stops the audio.
        public void StopAudio()
        {
            // Stop the audio.
            if (audioSource != null)
                audioSource.Stop();
        }

        // Called when the animation is finished.
        public void StopAnimation()
        {
            // Show the performance results since the animation is finished.
            if (callMoveResults)
                move.ShowPerformanceResults(user, target, battle, userEffect, targetEffect);

            // Disables the text box controls when playing the animation.
            if (disableTextBoxControlsWhenPlaying)
            {
                // Enable the textbox controls.
                textBox.EnableTextBoxControls();

                // Unpause the auto timer.
                textBox.autoNextTimerPaused = false;

                // Reset the timer to the max. If it isn't enabled, it won't run anyway.
                textBox.SetAutoNextTimerToMax();
            }
                

            // Turn off the animator object.
            animator.SetInteger(ANIM_VAR, (int)moveAnim.none);

            // Resets hte image color.
            if (animatedImage != null)
            {
                // Reset sprite and colour.
                animatedImage.sprite = defaultSprite;
                animatedImage.color = defaultColor;

                // Reset flip.
                // Images don't have a built-in flip feature, so this needs to be done instead.
                animatedImage.transform.localScale = (flip) ? new Vector3(-1.0F, 1.0F, 1.0F): Vector3.one;
            }
                
            // Resets the sprite renderer color.
            if (animatedSpriteRender != null)
            {
                // Reset the sprite, the colour, and the flip.
                animatedSpriteRender.sprite = defaultSprite;
                animatedSpriteRender.color = defaultColor;
                animatedSpriteRender.flipX = false;
            }

            // This cuts off different audio events, but it can't be helped.
            // Stops any remaining move audio from playing.
            StopAudio();

            // animation is no longer running, and neither is the timer.
            animPlaying = false;
            animTimer = 0.0F;

            // Debug.Log("Finished");

            // animator.gameObject.SetActive(false);
            // animator.enabled = false;
        }

        // Sets the move for the animation.
        public void SetMove(Move move, BattleEntity user, BattleEntity target, BattleManager battle,
            moveEffect userEffect, moveEffect targetEffect, bool flip)
        {
            this.move = move;
            this.user = user;
            this.target = target;
            this.battle = battle;

            this.userEffect = userEffect;
            this.targetEffect = targetEffect;

            this.flip = flip;

            callMoveResults = true;
        }

        // Clears the move information.
        public void ClearMove()
        {
            move = null;
            user = null;
            target = null;
            battle = null;

            callMoveResults = false;
        }


        // Update is called once per frame
        void Update()
        {
            // Operates a timer for automatically ending an animation.
            if (animPlaying && animTimer > 0.0F)
            {
                // Reduces the timer.
                animTimer -= Time.deltaTime;

                // Sets the timer to 0.0F.
                if (animTimer <= 0.0F)
                {
                    animTimer = 0.0f;
                    StopAnimation();
                }
            }
        }
    }
}