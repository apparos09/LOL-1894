using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RM_BBTS
{
    public class GameplayManager : GameState
    {
        // the state of the game.
        public gameState state;

        // the manager for the overworld.
        public OverworldManager overworld;

        // the manager for the battle.
        public BattleManager battle;

        // the input from the mouse and touch screen.
        public MouseTouchInput mouseTouchInput;

        // The text box used for the game.
        public TextBox textBox;

        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            // Turns on the overworld object and its ui.
            overworld.gameObject.SetActive(false);
            overworld.ui.SetActive(false);

            // Turns off the battle object and its ui.
            battle.gameObject.SetActive(false);
            battle.ui.SetActive(false);
        }

        // Start is called before the first frame update
        void Start()
        {
            // Finds the mouse touch input object.
            if (mouseTouchInput == null)
                mouseTouchInput = FindObjectOfType<MouseTouchInput>();


            Initialize();
        }

        // Initializes the gameplay manager.
        public override void Initialize()
        {
            overworld.Initialize();
            overworld.gameObject.SetActive(true);
            state = gameState.overworld;


            List<string> test = new List<string>() { "This is a test.", "This is only a text." };
            textBox.LoadPageText(test);
        }

        // Called when the mouse hovers over an object.
        public override void OnMouseHovered(GameObject hoveredObject)
        {
            // ...
        }

        // Called when the mouse interacts with an entity.
        public override void OnMouseInteract(GameObject heldObject)
        {
            // ...
        }

        // Called when the user's touch interacts with an entity.
        public override void OnTouchInteract(GameObject touchedObject, Touch touch)
        {
            // ...
        }

        // // Called when a level has been loaded.
        // private void OnLevelWasLoaded(int level)
        // {
        // }

        // Checks the mouse and touch to see if there's any object to use.
        public void MouseTouchCheck()
        {
            // The object that was interacted with.
            GameObject hitObject;

            // TODO: hovered check.


            // Tries grabbing the mouse object.
            hitObject = mouseTouchInput.mouseHeldObject;

            // The hit object was not found from the mouse, so check the touch instead.
            if (hitObject == null)
            {
                // Grabs the last object in the list.
                if (mouseTouchInput.touchObjects.Count > 0)
                {
                    // Grabs the index.
                    int index = mouseTouchInput.touchObjects.Count - 1;

                    // Saves the hit object.
                    hitObject = mouseTouchInput.touchObjects[index];
                    Touch touch = mouseTouchInput.currentTouches[index];

                    // Checks the state variable to see what kind of scene the game is in.
                    // Calls the appropriate touch interaction.
                    switch (state)
                    {
                        case gameState.overworld:
                            overworld.OnTouchInteract(hitObject, touch);
                            break;

                        case gameState.battle:
                            battle.OnTouchInteract(hitObject, touch);
                            break;
                    }
                }
                
            }
            else
            {
                // Checks the state variable to see what kind of scene the game is in.
                switch (state)
                {
                    case gameState.overworld:
                        overworld.OnMouseInteract(hitObject);
                        break;

                    case gameState.battle:
                        battle.OnMouseInteract(hitObject);
                        break;
                }
            }

            // Print message for testing.
            // if(hitObject != null)
            //     Debug.Log("Hit Found");

        }

        // Called with the object that was received with the interaction.
        protected override void OnInteractReceive(GameObject gameObject)
        {
            throw new System.NotImplementedException();
        }


        // Call this function to enter the overworld.
        public void EnterOverworld()
        {
            battle.gameObject.SetActive(false);
            overworld.gameObject.SetActive(false);

            overworld.gameObject.SetActive(true);
        }

        // Call to enter the battle world.
        public void EnterBattle(Door door)
        {
            // TODO: comment this out in the final game.
            if(door.locked)
            {
                Debug.Log("The door can't be opened.");
                return;
            }

            overworld.gameObject.SetActive(false);
            battle.gameObject.SetActive(false);

            // Initialize the battle scene.
            battle.door = door;
            battle.Initialize();

            // Activates the battle object.
            battle.gameObject.SetActive(true);

        }

        

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

            // Checks for some mouse input.
            MouseTouchCheck();


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