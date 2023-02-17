using RM_BBTS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RM_BTSS
{
    // The transition for the room.
    public class RoomTransition : MonoBehaviour
    {
        // The gameplay manager.
        public GameplayManager manager;

        // The animator.
        public Animator animator;

        // The image being animated.
        public Image animImage;

        // Clip for part 1.
        public AnimationClip part1Clip;

        // Clip for part 2.
        public AnimationClip part2Clip;

        // The wait time for the animation.
        private float animTimer = 0;

        // The extra wait time.
        private const float EXTRA_WAIT_TIME = 0.0F;

        // The part of the animation that's playing.
        // 0 = none, 1 = part 1, 2 = part 2.
        private int animPart = 0;

        // The state the transition is trasferring to.
        private gameState nextState;

        // The door being used for the transition.
        private Door door;

        // Checks to see if the battle was won or lost.
        private bool battleWon;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Gets the animation wait time. If 'part1' is true, then it's based on part 1. If it's false, then it's based on part 2.
        private float GetAnimationWaitTime(bool part1)
        {
            // The resulting time.
            float result = 0;

            // Gets the result. If the clips aren't set, then the length is gotten from the animator clip.
            if (part1Clip != null && part1)
                result = (part1Clip.length + EXTRA_WAIT_TIME) / animator.speed;
            else if (part2Clip != null && !part1)
                result = (part2Clip.length + EXTRA_WAIT_TIME) / animator.speed;
            else
                result = (animator.GetCurrentAnimatorClipInfo(0)[0].clip.length + EXTRA_WAIT_TIME) / animator.speed;

            // Return the result.
            return result;
        }

        // Transitions to the new game state.
        private void TransitionToNewState(gameState newState, Door door, bool battleWon)
        {
            // Set the state, the door, and the battle won.
            nextState = newState;
            this.door = door;
            this.battleWon = battleWon;

            // Change the image color.
            animImage.color = OverworldManager.GetDoorTypeColor(door.doorType);

            // The animation time.
            animTimer = 0.0F;

            // Plays the animation.
            animator.Play("Room Transition Animation - Part 1");

            // Gets the anim time, and sets it.
            animTimer = GetAnimationWaitTime(true);
            animPart = 1;
        }

        // Transition to the overworld.
        public void TransitionToOverworld(Door door)
        {
            TransitionToNewState(gameState.overworld, door, false);
        }

        // Transition to the battle.
        public void TransitionToBattle(Door door, bool battleWon)
        {
            TransitionToNewState(gameState.battle, door, false);
        }

        // Called when part 1 of the timer is finished.
        private void OnPart1TimerFinished()
        {
            // Plays the animation.
            animator.Play("Room Transition Animation - Part 2");

            // Gets the anim time, and sets it.
            animTimer = GetAnimationWaitTime(true);
            animPart = 2;
        }

        // Called when part 2 of the timer is finished.
        private void OnPart2TimerFinished()
        {

        }

        // Update is called once per frame
        void Update()
        {
            // Run the timer.
            if(animTimer > 0.0F)
            {
                // Reduce the timer.
                animTimer -= Time.deltaTime;

                // The timer has ended.
                if(animTimer <= 0.0F)
                {
                    // Checks which function to call.
                    switch(animPart)
                    {
                        case 1: // Part 1 Done.
                            OnPart1TimerFinished();
                            break;
                            
                        case 2: // Part 2 Done.
                            OnPart2TimerFinished();
                            break;
                    }
                }
            }
        }
    }
}