using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RM_BBTS
{
    // the asynchronous scene loader visual
    public class AsyncSceneLoadVisual : MonoBehaviour
    {
        // Scene loader.
        public Slider slider;

        // Load operation.
        public AsyncSceneLoader loader;

        // Start is called before the first frame update
        void Start()
        {
            // tries to find the load operation.
            if (loader == null)
                loader = FindObjectOfType<AsyncSceneLoader>();

            // loader.LoadScene("TitleScene");
        }

        // Update is called once per frame
        void Update()
        {
            // if the load operation is going on.
            if (loader.IsLoading)
            {
                // changes the slider.
                slider.value = Mathf.Lerp(slider.minValue, slider.maxValue, loader.Progress);
            }
        }
    }
}