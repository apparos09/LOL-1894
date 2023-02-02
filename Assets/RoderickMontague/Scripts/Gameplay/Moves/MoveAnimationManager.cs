using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RM_BBTS
{
    // The move animator ids.
    public enum moveAnim { none, circle };

    // The move animation manager.
    public class MoveAnimationManager : MonoBehaviour
    {
        // Animaton enum.
        // The animator for the move.
        public Animator animator;

        // The timer to automatically tell an animaton to stop if it hasn't already.
        private float animTimer = 0.0F;

        // Extra time to add to the anim timer.
        private const float ANIM_TIMER_EXTRA = 0.0F;

        // Set to call the move performance results once the animation is over.
        [HideInInspector()]
        public bool callMoveResults = false;

        // The move this animation is playing for, the user, the target, and the battle.
        // These are used to play the move results when the animation is done.
        protected Move move;
        protected BattleEntity user;
        protected BattleEntity target;
        protected BattleManager battle;

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

            // Changes the animation.
            animator.SetInteger("anim", (int)anim);

            // TODO: disable the forward button o the textbox when the animation plays.

            // Sets the animation timer.
            animTimer = animator.GetCurrentAnimatorStateInfo(0).length / animator.speed + ANIM_TIMER_EXTRA;


            // Disables the text box controls when playing the animation.
            if (disableTextBoxControlsWhenPlaying)
                textBox.DisableTextBoxControls();
        }

        // Plays the spiral animation.
        public void PlayTest()
        {
            // Plays the test animation.
            PlayAnimation(0);
            // TODO: change the animation number, then turn on the object.
        }

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
            animator.gameObject.SetActive(false);
        }

        // Sets the move for the animation.
        public void SetMove(Move move, BattleEntity user, BattleEntity target, BattleManager battle)
        {
            this.move = move;
            this.user = user;
            this.target = target;
            this.battle = battle;

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