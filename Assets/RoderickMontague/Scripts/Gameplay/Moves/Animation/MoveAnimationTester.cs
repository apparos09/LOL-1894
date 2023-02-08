using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_BBTS
{
    // Tests out animations for the move.
    public class MoveAnimationTester : MonoBehaviour
    {
        // The move animation that info is sent to.
        public MoveAnimationManager moveAnimation;

        // The animation.
        public moveAnim anim = moveAnim.none;

        // The first and last animations.
        public const moveAnim FIRST_ANIM = moveAnim.none;
        public const moveAnim LAST_ANIM = moveAnim.wave3;

        // // Start is called before the first frame update
        // void Start()
        // {
        // 
        // }

        // Plays the animation.
        public void PlayAnimation()
        {
            // if (moveAnimation != null)
            //     moveAnimation.PlayAnimation(anim);

            moveAnimation.PlayAnimation(anim);
            // currentStateHash = moveAnimation.animator.GetCurrentAnimatorStateInfo(0).fullPathHash;

            // Debug.Log(moveAnimation.animator.GetCurrentAnimatorStateInfo(0).shortNameHash);
            // moveAnimation.animator.SetInteger(MoveAnimationManager.ANIM_VAR, (int)anim);
        }

        // Plays the next animation.
        public void NextAnimation()
        {
            int value = (int)anim;
            value++;

            if (value > (int)LAST_ANIM)
                value = (int)FIRST_ANIM;

            anim = (moveAnim)value;
            PlayAnimation();
        }

        // Replays the animation (doesn't work).
        public void ReplayAnimation()
        {
            // This doesn't cause the animation to replay.
            // moveAnimation.animator.enabled = false;
            // moveAnimation.animator.enabled = true;

            // Return to default.
            // moveAnimation.animator.SetInteger(MoveAnimationManager.ANIM_VAR, 0);

            // moveAnimation.animator.play(;

            // moveAnimation.animator.GetCurrentAnimatorStateInfo(0).nameHash;

            // Return to the other animaton on the next frame.
            // callPlayInUpdate = true;
            moveAnimation.PlayAnimation(anim);
        }

        // Plays the previous animation.
        public void PrevAnimation()
        {
            int value = (int)anim;
            value--;

            if (value < (int)FIRST_ANIM)
                value = (int)LAST_ANIM;

            anim = (moveAnim)value;
            PlayAnimation();
        }

        // // Update is called once per frame
        // void Update()
        // {
        // }
    }
}