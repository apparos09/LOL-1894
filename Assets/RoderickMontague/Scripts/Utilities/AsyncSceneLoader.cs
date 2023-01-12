using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RM_BBTS
{
    // Loads a scene asynchronously, which allows there to be a loading screen.
    public class AsyncSceneLoader : MonoBehaviour
    {
        // the scene that's being loaded.
        private string loadingScene = "";

        // Set to 'true' if the scene loader is loading.
        private bool isLoading = false;

        // How much has already been loaded.
        private float progress = 0.0F;

        // Returns the scene that's being loaded. If no scene is being loaded then the string will be blank ("").
        public string LoadingScene
        {
            get { return loadingScene; }
        }

        // Returns 'true' if the scene is now loading.
        public bool IsLoading
        {
            get { return isLoading; }
        }

        // Returns the current progress (0-1) range.
        public float Progress
        {
            get { return progress; }
        }

        // Public function to call for scene loading.
        public void LoadScene(string sceneName)
        {
            // TODO: check to see if a scene exists.

            // Spreads an operation across multiple frames.
            StartCoroutine(LoadSceneAsync(sceneName));
        }

        // Loads a scene asynchonously.
        private IEnumerator LoadSceneAsync(string sceneName)
        {
            // The asynchonrous operation.
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

            // the operation is now loading.
            isLoading = true;
            loadingScene = sceneName;

            // While the operation is going.
            while (!operation.isDone)
            {
                // Gets the current progress of the operation.
                // Unity starts off by loading the new assets, which goes from a (0 - 0.9) range.
                // Then Unity does activation, deleting the old content and loading the new content (0.9 - 1).
                // The activation phase goes straight to 1.0F instead of doing anything in-between.
                // As such, we divide by 0.9F so that it goes from [0-1] percentage wise.
                progress = Mathf.Clamp01(operation.progress / 0.9F);

                // Tells the program to stall the operation and return controls back to Unity.
                yield return null;
            }

            // the scene has finished loading, so change these values.
            isLoading = false;
            loadingScene = "";
        }



    }
}