using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_BBTS
{
    // Causes an object to float in place by interpolating its position.
    public class ObjectFloat : MonoBehaviour
    {
        // The reset position of the object float.
        [Header("The reset position of the object. This is set as the object's local position.")]
        public Vector3 resetPosition;

        // Automatically sets the reset position of the object.
        public bool autoSetResetPosition = true;

        // The high position of the object.
        public Vector3 highPosition;

        // The low position of the object.
        public Vector3 lowPosition;

        // If set to 'true', the low and high positions are automatically set.
        public bool autoSetLowHighPos = true;

        // The base position offset. This is only used if the low and high positions are automatically set.
        public float basePosOffset = 1.0F;

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
        private bool onHigh = true;

        // Start is called before the first frame update
        void Start()
        {
            // Sets the local position as the reset position.
            if (autoSetResetPosition)
                resetPosition = gameObject.transform.localPosition;


            // If the low and high positions should be automatically set.
            if (autoSetLowHighPos)
            {
                lowPosition = gameObject.transform.localPosition - new Vector3(0.0F, basePosOffset, 0.0F);
                highPosition = gameObject.transform.localPosition + new Vector3(0.0F, basePosOffset, 0.0F);
            }

            // Resets the process to start it.
            ResetProcess();
        }

        // Eases in and out of the provided positions.
        public Vector3 EaseInOutLerp(Vector3 start, Vector3 end, float t)
        {
            // ease in-out calculation
            float newT = (t < 0.5F) ? 2 * Mathf.Pow(t, 2) : -2 * Mathf.Pow(t, 2) + 4 * t - 1;

            // Use the lerp equation.
            Vector3 result = Vector3.Lerp(start, end, newT);
            
            // Return the result.
            return result;
        }

        // Starting values.
        public void ResetProcess()
        {
            // The start position is the low point.
            startPosition = lowPosition;

            // End point is the high point.
            endPosition = highPosition;

            // Calculates the rough T
            {
                // The current position of the object.
                Vector3 currPos = transform.position;

                // Calculates the t-value for all three location values.
                // If the start and end are the same, the value is left at 0. 
                float xT = 0, yT = 0, zT = 0;
            
                float sumT = 0.0F;
                int added = 0;
            
                // X
                if (highPosition.x != lowPosition.x)
                {
                    xT = Mathf.InverseLerp(lowPosition.x, highPosition.x, currPos.x);
                    sumT += xT;
                    added++;
                }
                    
                // Y
                if (highPosition.y != lowPosition.y)
                {
                    yT = Mathf.InverseLerp(lowPosition.y, highPosition.y, currPos.y);
                    sumT += yT;
                    added++;
                }
                    
                // Z
                if (highPosition.z != lowPosition.z)
                {
                    zT = Mathf.InverseLerp(lowPosition.z, highPosition.z, currPos.z);
                    sumT += zT;
                    added++;
                }
            
                // Calculates the final t.
                if (added != 0) // Average out the values.
                    sumT /= added;
                else // Set it to half.
                    sumT = 0.5F;
            
                // Set the value.
                time = sumT;
            }

            // Set the transformation's local position from the start.
            transform.localPosition = EaseInOutLerp(startPosition, endPosition, time);

            // The object is going towards the high point.
            onHigh = true;
        }

        // Sets the object to the reset position.
        public void SetObjectToResetPosition()
        {
            transform.localPosition = resetPosition;
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
                    if(onHigh) // At the high point, so now you're going to the low point.
                    {
                        onHigh = false;
                        startPosition = highPosition;
                        endPosition = lowPosition;
                    }
                    else// At the low point, so now you're going to the low point.
                    {
                        onHigh = true;
                        startPosition = lowPosition;
                        endPosition = highPosition;
                    }

                    // Reset the time.
                    time = 0.0F;
                }
            }
            
        }
    }
}