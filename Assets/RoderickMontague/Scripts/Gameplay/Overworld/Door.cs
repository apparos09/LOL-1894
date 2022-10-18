using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_BBTS
{
    public class Door : MonoBehaviour
    {
        // The sprite that the door uses.
        public SpriteRenderer sprite;

        // Determines if this is a boss door or not.
        public bool isBossDoor = false;

        // The battle entity behind the door.
        public BattleEntity battleEntity;

        // Says whether the door is locked or not.
        public bool locked = true;

        // Start is called before the first frame update
        void Start()
        {

        }

        // The mouse_touch class will be used to send the door to the overworld manager.

        // Update is called once per frame
        void Update()
        {

        }
    }
}