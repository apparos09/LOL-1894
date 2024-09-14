using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RM_BBTS
{
    // Dynamically scales TMP text so that it fits in the bounds.
    public class TextScaler : MonoBehaviour
    {
        // The text.
        public TMP_Text text;

        // If set to 'true', text wrapping is automatically disabled.
        public bool disableWrappingOnStart = true;

        // The maximum amount of characters before dynamic scaling is applied.
        public int charLimit = -1;

        [Header("Scaling")]
        // The base text scale.
        public Vector3 baseScale = Vector3.one;

        // Determines what to scale.
        public bool scaleX = true;
        public bool scaleY = false;
        public bool scaleZ = false;

        // If set to 'true', the starting scale of the text is used to set the baseScale.
        public bool autoSetBaseScale = true;

        [Header("Other")]
        
        // If set to 'true', the text scaler is automatically updated.
        public bool autoUpdate = true;

        // Start is called before the first frame update
        void Start()
        {
            // Autoset the text.
            if (text == null)
                text = GetComponent<TMP_Text>();

            // If text wrapping should be automatically disabled.
            if(disableWrappingOnStart)
                text.enableWordWrapping = false;

            // Auto sets the base scale.
            if (autoSetBaseScale)
                baseScale = text.transform.localScale;
        }

        // Scales the text when the script is enabled.
        private void OnEnable()
        {
            SetTextScale();
        }

        // Sets the text scale.
        public void SetTextScale()
        {
            // If the text length has exceeded the character limit.
            if (text.text.Length > charLimit)
            {
                // Gets the scale factor.
                float factor = (float)charLimit / text.text.Length;

                // The new scale.
                Vector3 newScale = baseScale;

                // Scale by x-factor.
                if (scaleX)
                    newScale.x *= factor;

                // Scale the y-factor.
                if (scaleY)
                    newScale.y *= factor;

                // Scale the z-factor.
                if (scaleZ)
                    newScale.z *= factor;

                // Set the text scale.
                text.transform.localScale = newScale;
            }
            else
            {
                // If the text scale is not set to its base, set it back to normal.
                if (text.transform.localScale != baseScale)
                {
                    // Set to the default.
                    text.transform.localScale = baseScale;
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            // If text should be auto-updated.
            if (autoUpdate)
                SetTextScale();
        }
    }
}