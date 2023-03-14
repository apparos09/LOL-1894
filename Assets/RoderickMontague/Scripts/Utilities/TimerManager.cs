using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static RM_BBTS.TextBox;

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

            // Checks if the timer is paused.
            public bool paused;

            // CALLBACKS
            // a callback for when all the text has been gone through.
            public delegate void TimerCallback(Timer timer);

            // Callback for when the timer is done.
            private TimerCallback finishedCallback;

            // The callback for when the timer finishes.
            private TimerCallback callback;

            // Set the current time to the max time.
            public void Set()
            {
                currTime = maxTime;
            }

            // Add a timer callback.
            public void OnTimerFinishedAddCallback(TimerCallback callback)
            {
                finishedCallback += callback;
            }

            // Removes a timer callback.
            public void OnTimerFinishedRemoveCallback(TimerCallback callback)
            {
                finishedCallback -= callback;
            }

            // Called when the timer is finished.
            public void OnTimerFinished()
            {
                // The timer finished callback.
                if (finishedCallback != null)
                {
                    Timer timer = this;
                    finishedCallback(timer);
                }
            }

        }

        // The instance of the timer manager.
        private static TimerManager instance;

        // The list of timers.
        public List<Timer> timers = new List<Timer>();

        // Removes the timers when they're finished.
        public bool removeWhenFinished = true;

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
                        GameObject go = new GameObject("Timer Manager (singleton)");
                        instance = go.AddComponent<TimerManager>();
                    }

                }

                return instance;
            }

        }

        // Returns the amount of timers.
        public int GetTimerCount()
        {
            return timers.Count;
        }

        // Adds the timer. Timers are removed when they're finished.
        public Timer AddTimer(float maxTime)
        {
            // The timer object.
            Timer timer = new Timer();
            timer.name = "";
            timer.tag = "";
            timer.maxTime = maxTime;
            timer.currTime = timer.maxTime;
            timer.paused = false;

            // Adds the timer to the list.
            AddTimer(timer);

            // Returns the new timer.
            return timer;
        }

        // Adds the timer. Timers are removed when they're finished.
        public Timer AddTimer(string name, string tag, float maxTime, bool paused)
        {
            // The timer object.
            Timer timer = new Timer();
            timer.name = name;
            timer.tag = tag;
            timer.maxTime = maxTime;
            timer.currTime = timer.maxTime;
            timer.paused = paused;

            // Adds the timer to the list.
            AddTimer(timer);

            // Returns the new timer.
            return timer;
        }

        // Adds the timer. Timers are removed when they're finished.
        public void AddTimer(Timer timer)
        {
            // If this timer isn't already in the list, add it.
            if(!timers.Contains(timer))
                timers.Add(timer);
        }

        // Removes the timer of the list.
        public void RemoveTimer(Timer timer)
        {
            if(timers.Contains(timer))
                timers.Remove(timer);
        }

        // Returns the index of the timer. Returns -1 if not in list.
        public int IndexOfTimer(Timer timer)
        {
            if(timers.Contains(timer))
            {
                return timers.IndexOf(timer);
            }
            else
            {
                return -1;
            }
        }

        // Called when a timer is finished.
        private void OnTimerFinished(Timer timer)
        {
            // Remove the timer from the list.
            if(removeWhenFinished)
                RemoveTimer(timer);

            // The timer is finished, so call the callback.
            timer.OnTimerFinished();
        }

        // Update is called once per frame
        void Update()
        {
            // Goes through each timer.
            for(int i = 0; i < timers.Count; i++)
            {
                // If the timer isn't paused.
                if (!timers[i].paused)
                {
                    // Reduces the current time of the timer.
                    Timer timer = timers[i];
                    timer.currTime -= Time.deltaTime;

                    // Put it back in the list.
                    timers[i] = timer;

                    // The timer has finished.
                    if(timer.currTime <= 0.0F)
                        OnTimerFinished(timers[i]);
                }
            }
        }
    }
}