using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RM_BBTS
{
    // NOTE: I tried changing the method for playing the animation, but it didn't fix it. I'm not sure why.

    // Used for scene transitions.
    // Attach this to a canvas object, and have the transition objects as children of that canvas.
    public class SceneTransition : MonoBehaviour
    {
        [Header("Scene Enter")]
        // Object for entering a scene.
        public GameObject sceneEnterObject;

        // The scene entrance animator.
        public Animator sceneEnterAnimator;

        // Scene entrance clip.
        public AnimationClip sceneEnterAnimClip;

        // Plays the scene enter animation.
        public bool useSceneEnterAnim = true;

        [Header("Scene Exit")]
        // The scene entrance animator.
        public Animator sceneExitAnimator;

        // Object for exiting a scene.
        public GameObject sceneExitObject;

        // Scene exit clip.
        public AnimationClip sceneExitAnimClip;

        // Uses the scene exit animation.
        public bool useSceneExitAnim = true;


        // Used to wait until a transition is done.
        private float waitTime = 0.0F;

        // The current clip being used.
        private AnimationClip currClip;

        // Then next scene to be transitioned to.
        private string nextScene = "";

        // Start is called before the first frame update
        void Start()
        {
            // Turn off the objects.
            sceneEnterObject.SetActive(false);
            sceneExitObject.SetActive(false);

            // Original
            // // Plays the entrance animation when entering the scene.
            // if(useSceneEnterAnim)
            // {
            //     sceneEnterObject.SetActive(useSceneEnterAnim);
            //     StartCoroutine(EndSceneEnterAnimation());
            // }

            // New - plays the scene enter animation.
            if (useSceneEnterAnim)
            {
                PlaySceneEnterAnimation();
            }

        }

        // Old
        // // Ends the scene enter animation by turning off the object.
        // private IEnumerator EndSceneEnterAnimation()
        // {
        //     // Turns on the object if the object is currently off.
        //     if(!sceneEnterObject.activeSelf)
        //         sceneExitObject.SetActive(true);
        // 
        //     // The wait time for the animation is equal to the length of the clip.
        //     float waitTime = sceneEnterAnimClip.length;
        // 
        //     // While the operation is going.
        //     while (waitTime > 0.0F)
        //     {
        //         // Reduce by delta time.
        //         waitTime -= Time.deltaTime;
        // 
        //         // Tells the program to stall.
        //         yield return null;
        //     }
        // 
        //     // Turn off the object.
        //     sceneEnterObject.SetActive(false);
        // }

        // Plays teh scene enter animation.
        public void PlaySceneEnterAnimation()
        {
            // Turn on the object.
            sceneEnterObject.SetActive(true);

            // Play the animation.
            sceneEnterAnimator.Play("Fade from Black Animation");

            // The wait time for the animation is equal to the length of the clip.
            waitTime = sceneEnterAnimClip.length;
            currClip = sceneEnterAnimClip;
        }

        // Loads a scene.
        public void LoadScene(string sceneName)
        {
            // Old
            // // Checks if the exit animation should be used.
            // if (useSceneExitAnim)
            //     StartCoroutine(LoadSceneWithAnimation(sceneName));
            // else
            //     SceneManager.LoadScene(sceneName);


            // New
            // Checks if the exit animation should be used.
            if (useSceneExitAnim)
                LoadSceneWithAnimation(sceneName);
            else
                SceneManager.LoadScene(sceneName);
        }


        // Old
        // // Delays starting the game so that an animation can play. The scene transitions if the animation finishes.
        // private IEnumerator LoadSceneWithAnimation(string sceneName)
        // {
        //     // NOTE: realistically you would use Asynchronous Scene Loading instead of this janky method.
        //     // But the game loads so fast that it isn't really needed to be set up.
        // 
        //     // Start the animation.
        //     // As a UI object, this also blocks all mouse/touch input from the user since it covers the whole screen.
        //     sceneExitObject.SetActive(true);
        // 
        //     // The wait time for the animation is equal to the length of the clip.
        //     float waitTime = sceneExitAnimClip.length;
        // 
        //     // While the operation is going.
        //     while (waitTime > 0.0F)
        //     {
        //         // Reduce by delta time.
        //         waitTime -= Time.deltaTime;
        // 
        //         // Tells the program to stall.
        //         yield return null;
        //     }
        // 
        //     // Loads the game scene.
        //     SceneManager.LoadScene(sceneName);
        // }


        // Loads the scene with the fade-in animation.
        private void LoadSceneWithAnimation(string sceneName)
        {
            // NOTE: realistically you would use Asynchronous Scene Loading instead of this janky method.
            // But the game loads so fast that it isn't really needed to be set up.
            
            // Start the animation.
            // As a UI object, this also blocks all mouse/touch input from the user since it covers the whole screen.
            // I do this so that nothing else can happen during a transition.
            sceneExitObject.SetActive(true);

            // Play the animation.
            sceneExitAnimator.Play("Fade to Black Animation");

            // Sets the next scene.
            nextScene = sceneName;

            // The wait time for the animation is equal to the length of the clip.
            waitTime = sceneExitAnimClip.length;
            currClip = sceneExitAnimClip;
        }

        // The scene enterance animation is finished.
        private void OnSceneEnterAnimationFinished()
        {
            // Turn off the scene enter object, and clear the clip.
            sceneEnterObject.SetActive(false);
            currClip = null;
        }

        // The scene exit animation is finished.
        private void OnSceneExitAnimationFinished()
        {
            // Turn off the scene exit object.
            // Since the scene transitions, the object is not turned off.
            // sceneExitObject.SetActive(false);

            // Clear the clip.
            currClip = null;

            // Load the next scene.
            SceneManager.LoadScene(nextScene);
        }


        // New

        // // This function is called after a new level was loaded.
        // private void OnLevelWasLoaded(int level)
        // {
        //     // Turn of the scene animations.
        //     sceneEnterObject.SetActive(false);
        //     sceneExitObject.SetActive(false);
        // 
        //     // Checks if the entrance animation should be used.
        //     sceneEnterObject.SetActive(useSceneEnterAnim);
        // }

        // Update is called once per frame
        private void Update()
        {
            // Reduces the wait time.
            if(waitTime > 0.0F)
            {
                // Reduce time.
                waitTime -= Time.deltaTime;

                // The wait time is over.
                if(waitTime <= 0.0F)
                {
                    waitTime = 0.0F;

                    // Checks the clip that the program was waiting for.
                    if(currClip == sceneEnterAnimClip)
                    {
                        OnSceneEnterAnimationFinished();
                    }
                    else if(currClip == sceneExitAnimClip)
                    {
                        OnSceneExitAnimationFinished();
                    }
                }
            }
        }
    }
}