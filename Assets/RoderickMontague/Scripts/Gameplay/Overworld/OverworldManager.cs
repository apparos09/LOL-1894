using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_BBTS
{
    // The manager for the overworld.
    public class OverworldManager : MonoBehaviour
    {
        // The amount of the doors.
        public const int DOOR_COUNT = 18;

        // The rows and colums for the doors in the overworld.
        // Currently set to a 6 x 3 (cols, rows) setup.
        public const int ROWS = 3;
        public const int COLUMNS = 6;

        // The list of doors.
        public List<Door> doors = new List<Door>();

        // The door prefab to be instantiated.
        public GameObject doorPrefab;

        // Start is called before the first frame update
        void Start()
        {
            // ...
        }

        // Initializes the overworld.
        public void Initialize()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}