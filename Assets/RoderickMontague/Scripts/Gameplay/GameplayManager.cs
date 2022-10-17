using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RM_BBTS
{
    public class GameplayManager : MonoBehaviour
    {
        // the state of the game.
        public enum gameState { none, overworld, battle }

        // the state of the game.
        public gameState state;

        // the manager for the overworld.
        public OverworldManager overworld;

        // the manager for the battle.
        public BattleManager battle;

        // the input from the mouse and touch screen.
        public MouseTouchInput mouseTouchInput;

        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            // turns on the overworld component.
            overworld.enabled = true;

            // turns off the battle component.
            battle.enabled = false;
        }

        // Start is called before the first frame update
        void Start()
        {
            // Tries to find the object.
            if (mouseTouchInput == null)
                mouseTouchInput = FindObjectOfType<MouseTouchInput>();

            // Adds a component.
            if (mouseTouchInput == null)
                mouseTouchInput = gameObject.AddComponent<MouseTouchInput>();
        }

        // Initializes the gameplay manager.
        public void Initialize()
        {

        }

        // // Called when a level has been loaded.
        // private void OnLevelWasLoaded(int level)
        // {
        // }

        // Update is called once per frame
        void Update()
        {
            // if(Input.touchCount != 0)
            // {
            //     Touch touch = Input.GetTouch(0);
            // 
            //     Debug.Log("Finger has touched screen. Tap Count: " + touch.tapCount);
            // 
            //     // // checks to see if the user has touched it.
            //     // if (touch.phase == TouchPhase.Began)
            //     // {
            //     //     // Debug.Log("Finger has touched screen.");
            //     // }
            // }

            // Checks how many touches there are.
            if (mouseTouchInput.currentTouches.Count > 0)
                Debug.Log("Touch Count: " + mouseTouchInput.currentTouches.Count);


            // Checks the state variable to see what kind of scene the game is in.
            switch (state)
            {
                case gameState.overworld:
                    break;

                case gameState.battle:
                    break;
            }

        }
    }
}