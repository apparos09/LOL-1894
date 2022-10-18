using System.Collections.Generic;
using UnityEngine;

namespace RM_BBTS
{
    // The manager for the overworld.
    public class OverworldManager : GameState
    {
        // Becomes 'true' when the overworld is initialized.
        public bool initialized = false;

        public GameplayManager gameManager;

        // The object that was selected in the overworld.
        public GameObject selectedObject;

        [Header("Doors")]
        // The list of doors.
        public List<Door> doors = new List<Door>();

        // The amount of the doors.
        public const int DOOR_COUNT = 18;

        // // The door prefab to be instantiated.
        // public GameObject doorPrefab;
        // 
        // // TODO: remove these variables when the door is finished.
        // // The rows and colums for the doors in the overworld.
        // // Currently set to a 6 x 3 (cols, rows) setup.
        // public const int ROWS = 3;
        // public const int COLUMNS = 6;
        // 
        // // The reference object for placing a door.
        // public GameObject doorParent;
        // 
        // // the position offset for placing doors.
        // public Vector3 doorPosOffset = new Vector3(2.0F, -2.0F, 0);

        // Start is called before the first frame update
        void Start()
        {
            // ...
        }

        // This function is called when the object becomes enabled and active
        private void OnEnable()
        {
            // ...
        }

        // This function is called when the behaviour becomes disabled or inactive
        private void OnDisable()
        {
            // ...
        }

        // Initializes the overworld.
        public override void Initialize()
        {
            // This is used to generate doors for setting up the scene.
            // This should be done commented out when doors are added manually.

            // {
            //     // Goes through every column.
            //     for(int c = 0; c < COLUMNS && doors.Count < DOOR_COUNT; c++)
            //     {
            //         // Goes through every row.
            //         for (int r = 0; r < ROWS && doors.Count < DOOR_COUNT; r++)
            //         {
            //             // Instantiates the door.
            //             GameObject doorObject = Instantiate(doorPrefab);
            // 
            //             // Sets the parent as the door position reference. Set the local position back to 0 for repositioning.
            //             doorObject.transform.parent = doorParent.transform;
            //             doorObject.transform.localPosition = Vector3.zero;
            // 
            //             // Translation calculation.
            //             Vector3 tlate = Vector3.Scale(doorPosOffset, doorObject.transform.localScale);
            //             tlate.Scale(new Vector3(c, r, 1));
            // 
            //             // Translates the door.
            //             doorObject.transform.Translate(tlate);
            // 
            //             // Adds the door to the list and renames it.
            //             doors.Add(doorObject.GetComponent<Door>());
            //             doorObject.name = "Door " + doors.Count.ToString("00");
            // 
            //             // TODO: add naming for boss door.
            //             // TODO: add in enemy components.
            //         }
            //     }
            //     
            // }

            // // Adds all the doors to the list.
            // if (doors.Count == 0)
            // {
            //     // Includes inactive doors since they may not be visible right now.
            //     doors.AddRange(FindObjectsOfType<Door>(true));
            // }    
                
            // Goes through each door and gives them an entity behind them.
            foreach(Door door in doors)
            {
                // generates a room for the door.
                GenerateRoom(door);
            }

            initialized = true;
        }

        // Called when the mouse hovers over an object.
        public override void OnMouseHovered(GameObject hoveredObject)
        {
            // ...
        }

        // Called when the mouse interacts with an entity.
        public override void OnMouseInteract(GameObject heldObject)
        {
            selectedObject = heldObject;
        }

        // Called when the user's touch interacts with an entity.
        public override void OnTouchInteract(GameObject touchedObject, Touch touch)
        {
            selectedObject = touchedObject;

            // If the object is not set to null, and has been t
            if(selectedObject != null && touch.tapCount > 1)
            {

            }
        }

        // Called with the object that was received with the interaction.
        protected override void OnInteractReceive(GameObject gameObject)
        {
            throw new System.NotImplementedException();
        }

        // Generates a room for the door.
        private void GenerateRoom(Door door)
        {
            // ...
            door.locked = false;
        }

        // Update is called once per frame
        void Update()
        {

        }

        
    }
}