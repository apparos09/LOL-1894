using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RM_BBTS
{
    // The move animator ids.
    public enum moveAnim { none, blast1, blast2, burst1, colorWave1, crawl1, fill1, shield1, shootingStar1, shot1, shot2, slash1, slash2, smack1, smack2, smack3, twister1, wave1, wave2, wave3 };

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

        private const string ANIM_VAR = "anim";

        // The timer to automatically tell an animaton to stop if it hasn't already.
        private float animTimer = 0.0F;

        // Extra time to add to the anim timer.
        private const float ANIM_TIMER_EXTRA = 100.0F;

        // Set to call the move performance results once the animation is over.
        [HideInInspector()]
        public bool callMoveResults = false;

        // The move this animation is playing for, the user, the target, and the battle.
        // These are used to play the move results when the animation is done.
        protected Move move;
        protected BattleEntity user;
        protected BattleEntity target;
        protected BattleManager battle;

        // Determines if the animation should be flipped.
        protected bool flip;

        [Header("Audio")]
        // The audio for the move animation.
        public AudioSource audioSource;

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

        // Plays the animation.
        public void PlayAnimation(moveAnim anim)
        {
            // // No animaton to play.
            // if (anim == moveAnim.none)
            //     return;

            // Turn on the animator object.
            animator.gameObject.SetActive(true);


            // // Changes the animation.
            // switch(anim)
            // {
            //     case moveAnim.smack1: // Smack Animation
            //         animator.SetInteger(ANIM_VAR, 1);
            //         break;
            // }

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

            // Sets the animation timer.
            animTimer = animator.GetCurrentAnimatorStateInfo(0).length / animator.speed + ANIM_TIMER_EXTRA;


            // Disables the text box controls when playing the animation.
            if (disableTextBoxControlsWhenPlaying)
                textBox.DisableTextBoxControls();
        }

        // // Plays the spiral animation.
        // public void PlayTest()
        // {
        //     // Plays the test animation.
        //     PlayAnimation(0);
        //     // TODO: change the animation number, then turn on the object.
        // }

        // Called when the animation is finished.
        public void StopAnimation()
        {
            // Show the performance results since the animation is finished.
            if (callMoveResults)
                move.ShowPerformanceResults(user, target, battle);

            // Disables the text box controls when playing the animation.
            if (disableTextBoxControlsWhenPlaying)
                textBox.EnableTextBoxControls();

            // Turn off the animator object.
            // animator.SetInteger(ANIM_VAR, 0);

            // Resets hte image color.
            if (animatedImage != null)
            {
                // Reset colour.
                animatedImage.color = Color.white;

                // Reset flip.
                // Images don't have a built-in flip feature, so this needs to be done instead.
                animatedImage.transform.localScale = (flip) ? new Vector3(-1.0F, 1.0F, 1.0F): Vector3.one;
            }
                
            // Resets the sprite renderer color.
            if (animatedSpriteRender != null)
            {
                // Reset the colour, and the flip.
                animatedSpriteRender.color = Color.white;
                animatedSpriteRender.flipX = false;
            }

            animator.gameObject.SetActive(false);
        }

        // Sets the move for the animation.
        public void SetMove(Move move, BattleEntity user, BattleEntity target, BattleManager battle, bool flip)
        {
            this.move = move;
            this.user = user;
            this.target = target;
            this.battle = battle;
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
            if (animTimer > 0.0F)
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