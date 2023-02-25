using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_BBTS
{
    // Causes an object to float in place by interpolating its position.
    public class ObjectFloat : MonoBehaviour
    {
        // The high position of the object.
        public Vector3 highPosition;

        // The low position of the object.
        public Vector3 lowPosition;

        // If set to 'true', the low and high positions are automatically set.
        public bool autoSetLowHighPos = true;

        // The base position offset. This is only used if the low and high positions are automatically set.
        public float basePosOffset = 10.0F;

        // If set to 'true', the animation is paused.
        public bool paused = false;

        // The speed of the animation.
        public float speed = 1.0F;

        // The start point for the object float.
        private Vector3 startPosition;

        // The end point of the object float.
        private Vector3 endPosition;

        // The time value used for the interpolation.
        private float time = 0.0F;

        // Determines if the object is working towards the high point or the low point.
        private bool onHigh = false;

        // Start is called before the first frame update
        void Start()
        {
            // If the low and high positions should be automatically set.
            if(autoSetLowHighPos)
            {
                lowPosition = gameObject.transform.localPosition - new Vector3(0.0F, -basePosOffset, 0.0F);
                highPosition = gameObject.transform.localPosition - new Vector3(0.0F, +basePosOffset, 0.0F);
            }

            // Resets the process to start it.
            ResetProcess();
        }

        // Eases in and out of the provided positions.
        public Vector3 EaseInOutLerp(Vector3 start, Vector3 end, float t)
        {
            // TODO: implement proper equation.
            Vector3 result = Vector3.Lerp(start, end, t);
            
            return result;
        }

        // Starting values.
        public void ResetProcess()
        {
            // The start position is half-way between low and high.
            startPosition = (lowPosition + highPosition) / 2.0F;

            // End point is the high point.
            endPosition = highPosition;

            // Since we're starting in the middle, put time to 0.5.
            time = 0.5F;

            // The object is going towards the high point.
            onHigh = true;
        }

        // Update is called once per frame
        void Update()
        {
            // If the timer isn't paused.
            if(!paused)
            {
                // Increment the timer.
                time += Time.deltaTime * speed;
                time = Mathf.Clamp01(time);

                // Change the object's position.
                transform.localPosition = EaseInOutLerp(startPosition, endPosition, time);

                // If the end of the line has been reached.
                if(time >= 1.0F)
                {
                    // Checks if the object is going towards the high point or low point.
                    if(onHigh) // High
                    {
                        onHigh = false;
                        startPosition = lowPosition;
                        endPosition = highPosition;
                    }
                    else // Low
                    {
                        onHigh = true;
                        startPosition = highPosition;
                        endPosition = lowPosition;
                    }

                    // Reset the time.
                    time = 0.0F;
                }
            }
            
        }
    }
}