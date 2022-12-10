using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RM_BBTS
{
    // Used for scene transitions.
    // Attach this to a canvas object, and have the transition objects as children of that canvas.
    public class SceneTransition : MonoBehaviour
    {
        // Object for entering a scene.
        public GameObject sceneEnterObject;

        // Scene entrance clip.
        public AnimationClip sceneEnterAnimClip;

        // Plays the scene enter animation.
        public bool useSceneEnterAnim = true;

        // Object for exiting a scene.
        public GameObject sceneExitObject;

        // Scene exit clip.
        public AnimationClip sceneExitAnimClip;

        // Uses the scene exit animation.
        public bool useSceneExitAnim = true;

        // Start is called before the first frame update
        void Start()
        {
            // Turn off the objects.
            sceneEnterObject.SetActive(false);
            sceneExitObject.SetActive(false);

            // Plays the entrance animation when entering the scene.
            if(useSceneEnterAnim)
            {
                sceneEnterObject.SetActive(useSceneEnterAnim);
                StartCoroutine(EndSceneEnterAnimation());
            }
                
        }

        // Ends the scene enter animation by turning off the object.
        private IEnumerator EndSceneEnterAnimation()
        {
            // Turns on the object if the object is currently off.
            if(!sceneEnterObject.activeSelf)
                sceneExitObject.SetActive(true);

            // The wait time for the animation is equal to the length of the clip.
            float waitTime = sceneEnterAnimClip.length;

            // While the operation is going.
            while (waitTime > 0.0F)
            {
                // Reduce by delta time.
                waitTime -= Time.deltaTime;

                // Tells the program to stall.
                yield return null;
            }

            // Turn off the object.
            sceneEnterObject.SetActive(false);
        }

        // Loads a scene.
        public void LoadScene(string sceneName)
        {
            // Checks if the exit animation should be used.
            if (useSceneExitAnim)
                StartCoroutine(LoadSceneWithAnimation(sceneName));
            else
                SceneManager.LoadScene(sceneName);
        }

        // Delays starting the game so that an animation can play. The scene transitions if the animation finishes.
        private IEnumerator LoadSceneWithAnimation(string sceneName)
        {
            // NOTE: realistically you would use Asynchronous Scene Loading instead of this janky method.
            // But the game loads so fast that it isn't really needed to be set up.

            // Start the animation.
            // As a UI object, this also blocks all mouse/touch input from the user since it covers the whole screen.
            sceneExitObject.SetActive(true);

            // The wait time for the animation is equal to the length of the clip.
            float waitTime = sceneExitAnimClip.length;

            // While the operation is going.
            while (waitTime > 0.0F)
            {
                // Reduce by delta time.
                waitTime -= Time.deltaTime;

                // Tells the program to stall.
                yield return null;
            }

            // Loads the game scene.
            SceneManager.LoadScene(sceneName);
        }

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
    }
}