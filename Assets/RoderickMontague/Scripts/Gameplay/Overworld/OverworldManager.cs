using System.Collections.Generic;
using UnityEngine;

namespace RM_BBTS
{
    // The manager for the overworld.
    public class OverworldManager : MonoBehaviour
    {
        // Becomes 'true' when the overworld is initialized.
        public bool initialized = false;

        [Header("Doors")]
        // The list of doors.
        public List<Door> doors = new List<Door>();

        // The door prefab to be instantiated.
        public GameObject doorPrefab;

        // The amount of the doors.
        public const int DOOR_COUNT = 18;

        // The rows and colums for the doors in the overworld.
        // Currently set to a 6 x 3 (cols, rows) setup.
        public const int ROWS = 3;
        public const int COLUMNS = 6;

        // The reference object for placing a door.
        public GameObject doorParent;

        // the position offset for placing doors.
        public Vector3 doorPosOffset = new Vector3(2.0F, -2.0F, 0);

        // Start is called before the first frame update
        void Start()
        {
            // ...
        }

        // This function is called when the object becomes enabled and active
        private void OnEnable()
        {
            
        }

        // This function is called when the behaviour becomes disabled or inactive
        private void OnDisable()
        {
            
        }

        // Initializes the overworld.
        public void Initialize()
        {
            // Generates all the doors.
            for(int i = 0; i < DOOR_COUNT; i++)
            {
                // Goes through every column.
                for(int c = 0; c < COLUMNS; c++)
                {
                    // Goes through every row.
                    for (int r = 0; r < ROWS; r++)
                    {
                        // Instantiates the door.
                        GameObject doorObject = Instantiate(doorPrefab);

                        // Sets the parent as the door position reference. Set the local position back to 0 for repositioning.
                        doorObject.transform.parent = doorParent.transform;
                        doorObject.transform.localPosition = Vector3.zero;

                        // Translation calculation.
                        Vector3 tlate = Vector3.Scale(doorPosOffset, doorObject.transform.localScale);
                        tlate.Scale(new Vector3(c, r, 1));

                        // Translates the door.
                        doorObject.transform.Translate(tlate);

                        // Adds the door to the list and renames it.
                        doors.Add(doorObject.GetComponent<Door>());
                        doorObject.name = "Door " + doors.Count.ToString();

                        // TODO: add naming for boss door.
                        // TODO: add in enemy components.
                    }
                }
                
            }

            initialized = true;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}