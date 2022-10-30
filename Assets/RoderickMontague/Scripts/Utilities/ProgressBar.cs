using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RM_BBTS
{
    // A script for a progress bar.
    public class ProgressBar : MonoBehaviour
    {
        // The slider bar that's being animated.
        public Slider bar;

        // The minimum value.
        public float minValue = 0.0F;

        // The maximum value.
        public float maxValue = 1.0F;

        // The value for the progress bar.
        public float value = 0.0F;

        // The starting value that's used for animation.
        private float startValue = 0.0F;

        // The scroll speed for the transitions.
        public float speed = 1.0F;

        // Set to 'true' if the bar is transitioning between values.
        private bool transitioning = false;

        // value t for interlation
        private float v_t = 0.0F;

        // Start is called before the first frame update
        void Start()
        {
            // Grabs the slider component from the object if this hasn't been set.
            if (bar == null)
                bar = GetComponent<Slider>();

            SetValue(1.0F);
        }

        // Returns the value.
        public float GetValue()
        {
            return value;
        }

        // TODO: account for a transition already in progress.
        // Sets the value for the progress bar.
        public void SetValue(float newValue, bool transition = true)
        {
            // If the minimum value is larger than the maximum value they will be swapped.
            if(minValue > maxValue)
            {
                float temp = minValue;
                minValue = maxValue;
                maxValue = temp;
            }

            // Applies the new value.
            startValue = value;
            value = Mathf.Clamp(newValue, minValue, maxValue);
            
            bar.minValue = minValue;
            bar.maxValue = maxValue;

            v_t = 0.0F;

            // If there should be a transition.
            if (transition)
            {
                // Transitioning.
                transitioning = true;
            }
            else
            {
                // Not transitioning.
                transitioning = false;

                // Changes the progress bar visual.
                startValue = value;
                bar.value = value;
            }
        }

        // Update is called once per frame
        void Update()
        {
            // If the start value is not equal to the set value.
            if(transitioning)
            {
                // Increases 't' and clamps it.
                v_t += Time.deltaTime * speed;
                v_t = Mathf.Clamp01(v_t);

                bar.value = Mathf.Lerp(startValue, value, v_t);

                // If the transition is complete.
                if (v_t >= 1.0F)
                {
                    v_t = 0.0F;
                    transitioning = false;
                }

            }
        }
    }
}