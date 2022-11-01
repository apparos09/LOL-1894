using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_BBTS
{
    public class Door : MonoBehaviour
    {
        // The sprite that the door uses.
        public SpriteRenderer sprite;

        // The sprite for the door being open.
        public Sprite openSprite;

        // The sprite for the door being closed.
        public Sprite closedSprite;

        // Determines if this is a boss door or not.
        public bool isBossDoor = false;

        // The battle entity behind the door.
        public BattleEntityData battleEntity;

        // Says whether the door is locked or not.
        public bool locked = false;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Called to lock/unlock the door.
        public bool Locked
        {
            get { return locked; }

            set
            {
                locked = value;

                // Changes the sprite.
                if (locked) // closed
                    sprite.sprite = closedSprite;
                else // open
                    sprite.sprite = openSprite;
            }
        }

        // The mouse_touch class will be used to send the door to the overworld manager.

        // Update is called once per frame
        void Update()
        {

        }
    }
}