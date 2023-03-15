using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        private float minValue = 0.0F;

        // The maximum value.
        private float maxValue = 1.0F;

        // The value for the progress bar.
        private float value = 0.0F;

        // The scroll speed for the transitions.
        public float speed = 1.0F;

        // If 'true', the bar scrolls at a fixed speed.
        public bool fixedSpeed = false;

        // The starting value that's used for animation.
        private float startValue = 0.0F;

        // Set to 'true' if the bar is transitioning between values.
        private bool transitioning = false;

        // value t for interlation
        private float v_t = 0.0F;

        [Header("Bar Fill")]
        // The bar fill image.
        public Image barFill;

        // If 'true', the fill is hidden when the progress bar is at 0.
        public bool hideFillOnMin = true;

        // The fill alpha for hiding it when the bar slider is set to zero.
        [Range(0.0F, 1.0F)]
        public float fillAlpha = 1.0F;

        // If set to 'true', the fill alpha is automatically set on the start screen.
        public bool autoSetFillAlpha = true;

        [Header("Bar Fill/Multi-Color")]
        // If set to 'true', multi-colours are used. 
        public bool useMultiColor = false;

        // The colour for when the health bar is high (50% or more) 
        public Color highColor = Color.green;

        // The threshold for the bar entering the 'high-range' (low-end).
        [Tooltip("The threshold that must be surpassed to be in the high percentage range.")]
        public float highThreshold = 0.50F;

        // The colour for when the health bar is mid (25% or more) 
        public Color midColor = Color.yellow;

        // The threshold for the bar entering the 'mid range' (low-end).
        [Tooltip("The threshold that must be surpassed to be in the mid percentage range.")]
        public float midThreshold = 0.25F;

        // The colour for when the health bar is low (12.5% or less) 
        public Color lowColor = Color.red;

        // Start is called before the first frame update
        void Start()
        {
            // Grabs the slider component from the object if this hasn't been set.
            if (bar == null)
                bar = GetComponent<Slider>();

            // Gets the alpha of the bar fill colour.
            if (autoSetFillAlpha && barFill != null)
                fillAlpha = barFill.color.a;
        }

        // The minimum value.
        public float MinValue
        {
            get { return minValue; }

            set
            {
                minValue = value;
                SetValue(this.value);
            }
        }

        // The maximum value.
        public float MaxValue
        {
            get { return maxValue; }

            set
            {
                maxValue = value;
                SetValue(this.value);
            }
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
                // If currently transitioning, recalculate the current v_t value.
                if (transitioning)
                    v_t = Mathf.InverseLerp(startValue, value, bar.value);

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

                // Refreshes the color. 
                RefreshColor();
            }
        }

        // Returns the value in a 0.0 (0%) to 1.0 (100%) range.
        public float GetValueAsPercentage()
        {
            return Mathf.InverseLerp(minValue, maxValue, value);
        }

        // Gets the value from the slider UI object.
        public float GetSliderValue()
        {
            return bar.value;
        }

        // Gets the value from the slider UI object as a percentage.
        public float GetSliderValueAsPercentage()
        {
            return Mathf.InverseLerp(bar.minValue, bar.maxValue, bar.value);
        }

        // Checks if the progress bar is transitioning to another vlaue.
        public bool IsTransitioning()
        {
            return transitioning;
        }

        // Refreshes the color of the bar. If 'useMultiColor' is set to 'false', nothing happens. 
        public void RefreshColor()
        {
            // Don't use multi-color. 
            if (!useMultiColor)
                return;

            // The percentage. 
            float percent = value / maxValue;

            // If the bar is transitioning, use the bar's value instead of the destination value.
            if (transitioning)
                percent = bar.value / bar.maxValue;


            // Checks how full the bar is. 
            if (percent <= midThreshold) // less than or equal to 25% 
            {
                if (barFill.color != lowColor)
                    barFill.color = lowColor;
            }
            else if (percent <= highThreshold) // less than or equal to 50% 
            {
                if (barFill.color != midColor)
                    barFill.color = midColor;
            }
            else // Above 50% 
            {
                if (barFill.color != highColor)
                    barFill.color = highColor;
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

                // Checks if the bar should be moving at a fixed pace.
                if(fixedSpeed) // Fixed speed.
                {
                    // If the start value is less than the destination value then the bar is increasing.
                    if(value > startValue) // Increase
                    {
                        bar.value = Mathf.Lerp(minValue, maxValue, v_t);

                        // If the bar value has reached the desired value then it should stop moving.
                        if (bar.value >= value)
                        {
                            v_t = 1.0F;
                            bar.value = value;
                        }    
                            
                    }
                    else // Decrease
                    {
                        bar.value = Mathf.Lerp(maxValue, minValue, v_t);

                        // If the bar value has reached the desired value then it should stop moving.
                        if (bar.value <= value)
                        {
                            v_t = 1.0F;
                            bar.value = value;
                        }
                            
                    }
                }
                else // Not moving at a fixed speed.
                {
                    bar.value = Mathf.Lerp(startValue, value, v_t);
                }

                // If the transition is complete.
                if (v_t >= 1.0F)
                {
                    v_t = 0.0F;
                    transitioning = false;
                }

                // Refresh the bar color. 
                RefreshColor();
            }
            else
            {
                // if the bar value does not match.
                if (bar.value != value)
                {
                    bar.minValue = minValue;
                    bar.maxValue = maxValue;
                    bar.value = value;
                }
            }

            // Makes sure the bar value stays within the bounds.
            if(bar.value < bar.minValue || bar.value > bar.maxValue)
                bar.value = Mathf.Clamp(bar.value, bar.minValue, bar.maxValue);


            // If the bar fill should be hidden when the bar is at 0.
            // This is needed because it won't be gone when set to 0 with how you have it set up.
            if(hideFillOnMin)
            {
                // Checks to see if the bar fill should be hidden.
                if (bar.value <= bar.minValue && barFill.color.a != 0.0F) // Hide bar fill.
                {
                    // Creates a new colour variable with an alpha of 0.
                    Color newColor = barFill.color;
                    newColor.a = 0.0F;

                    // Replaces the bar colour with the new colour.
                    barFill.color = newColor;
                }
                else if (bar.value > bar.minValue && barFill.color.a == 0.0F && fillAlpha != 0.0F) // Show bar fill.
                {
                    // The program doesn't bother if the fill alpha is set to 0.0F.
                    // Creates a new colour variable, giving it the fill alpha.
                    Color newColor = barFill.color;
                    newColor.a = fillAlpha;

                    // Replaces the bar colour with the new colour.
                    barFill.color = newColor;
                }
            }

            
        }
    }
}