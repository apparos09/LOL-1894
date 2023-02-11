using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RM_BBTS
{
    // Runs timers, calling the provided function wehn they're done.
    public class TimerManager : MonoBehaviour
    {
        // The timer.
        public struct Timer
        {
            // An optional name for the timer.
            public string name;

            // An optional tag for the timer.
            public string tag;

            // The maximum time.
            public float maxTime;

            // The current timer.
            public float currTime;

            // The callback for when the timer finishes.
            TimerCallback callback;
            
        }

        // The instance of the timer manager.
        private static TimerManager instance;

        // The list of timers.
        public List<Timer> timers = new List<Timer>();

        // CALLBACKS
        // a callback for when all the text has been gone through.
        public delegate void TimerCallback();

        // Callback for when the timer is done.
        private TimerCallback timerDoneCallback;

        // Constructor
        private TimerManager()
        {
            // ...
        }

        // Awake is called when the script is loaded.
        private void Awake()
        {
            // Instance saving.
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
                return;
            }
        }

        // // Start is called before the first frame update
        // void Start()
        // {
        // }

        // Gets the instance of the timer manager.
        public static TimerManager Instance
        {
            get
            {
                // Generates the instance if it isn't set.
                if (instance == null)
                {
                    // Searches for the instance if it is not set.
                    instance = FindObjectOfType<TimerManager>(true);

                    // No instance found, so make a new object.
                    if (instance == null)
                    {
                        GameObject go = new GameObject("Battle Entity List");
                        instance = go.AddComponent<TimerManager>();
                    }

                }

                return instance;
            }

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}