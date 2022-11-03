using System.Collections.Generic;
using UnityEngine;

namespace RM_BBTS
{
    // The manager for the overworld.
    public class OverworldManager : GameState
    {
        // Becomes 'true' when the overworld is initialized.
        public bool initialized = false;

        // The gameplay manager.
        public GameplayManager gameManager;

        

        // The object that was selected in the overworld.
        // public GameObject selectedObject;

        [Header("Doors")]
        // The list of doors.
        public List<Door> doors = new List<Door>();

        // The amount of the doors.
        public const int DOOR_COUNT = 18;

        // The amount of treasures for the game.
        public const int TREASURE_COUNT = 3;

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

        [Header("UI")]
        
        // The user interface.
        public GameObject ui;

        // Start is called before the first frame update
        void Start()
        {
            // ...
        }

        // This function is called when the object becomes enabled and active
        private void OnEnable()
        {
            if(ui != null)
                ui.SetActive(true);
        }

        // This function is called when the behaviour becomes disabled or inactive
        private void OnDisable()
        {
            if(ui != null)
                ui.SetActive(false);
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
                
            // Initialize All Doors
            // Goes through each door and gives them an entity behind them.
            // foreach(Door door in doors)
            // {
            //     // generates a room for the door.
            //     GenerateRoom(door);
            // }

            // Initializes the doors (normal, treasure, and boss)
            {
                // Door initialization list.
                List<Door> doorInitList = new List<Door>(doors);

                // A random index.
                int randIndex = 0;

                // BOSS
                // The boss door
                Door bossDoor;

                randIndex = Random.Range(0, doorInitList.Count); // Grabs the random index.
                bossDoor = doorInitList[randIndex]; // Grab random door to make boss door.
                doorInitList.RemoveAt(randIndex); // Remove boss door from list.
                bossDoor.isBossDoor = true; // This is a boss door.
                GenerateRoom(bossDoor); // Generates the room.

                // TREASURES
                // The treasure doors.
                List<Door> treasureDoors = new List<Door>();

                // While there are still treasure doors.
                while(treasureDoors.Count < TREASURE_COUNT && treasureDoors.Count < doorInitList.Count)
                {
                    randIndex = Random.Range(0, doorInitList.Count); // random index
                    
                    Door treasureDoor = doorInitList[randIndex]; // Grab the door.
                    treasureDoors.Add(treasureDoor); // Add to list.
                    
                    doorInitList.RemoveAt(randIndex); // Remove the door from the overall list.
                    treasureDoor.isTreasureDoor = true; // This is a treasure door.
                    GenerateRoom(treasureDoor); // Generates the room.
                }

                // Initialize the rest of the doors.
                foreach(Door door in doorInitList)
                {
                    GenerateRoom(door);
                }

            }

            initialized = true;
        }

        // Called when the mouse hovers over an object.
        public override void OnMouseHovered(GameObject hoveredObject)
        {
            // ... highlight
        }

        // Called when the mouse interacts with an entity.
        public override void OnMouseInteract(GameObject heldObject)
        {
            // selectedObject = heldObject;

            OnInteractReceive(heldObject);
        }

        // Called when the user's touch interacts with an entity.
        public override void OnTouchInteract(GameObject touchedObject, Touch touch)
        {
            // selectedObject = touchedObject;

            // If the object is not set to null.

            // This is the first time the object has been tapped.
            if (touch.tapCount > 1)
            {
                // ENTER
            }
            // This is the first tap.
            else if (touch.tapCount == 1)
            {
                // HIGHLIGHT
            }

            OnInteractReceive(touchedObject);
        }

        // Called with the object that was received with the interaction.
        protected override void OnInteractReceive(GameObject gameObject)
        {
            // Door object.
            Door door = null;

            // Tries to grab the door component.
            if(gameObject.TryGetComponent<Door>(out door))
            {
                // Enters the battle.
                gameManager.EnterBattle(door);
            }
        }

        // Generates a room for the door.
        private void GenerateRoom(Door door)
        {
            // Unlock the door.
            door.Locked = false;

            // Checks the door type.
            if(door.isBossDoor) // Boss Door
            {
                door.battleEntity = BattleEntityList.Instance.GenerateBattleEntityData(battleEntityId.boss);
            }
            else if(door.isTreasureDoor) // Treasure Door
            {
                door.battleEntity = BattleEntityList.Instance.GenerateBattleEntityData(battleEntityId.treasure);
            }
            else // Normal Door
            {
                // TODO: generate normal enemy (need to RANDOMIZE)
                door.battleEntity = BattleEntityList.Instance.GenerateBattleEntityData(battleEntityId.ufo);
            }

            // Sets the level.
            door.battleEntity.level = (uint)Random.Range(1, gameManager.battlesPerLevelUp + 1);



            // TODO: randomize the enemy being placed behind the door.
            // door.battleEntity = BattleEntityList.Instance.GenerateBattleEntityData(battleEntityId.unknown);
            // door.battleEntity = BattleEntityList.Instance.GenerateBattleEntityData(battleEntityId.treasure);
            // door.battleEntity = BattleEntityList.Instance.GenerateBattleEntityData(battleEntityId.boss);

            // Make sure the battle entity is parented to the door.
            // TODO: have algorithm for generating enemies.

        }

        // Called when returning to the overworld.
        public void OnOverworldReturn()
        {
            // Evolves the entities.
            int phase = gameManager.GetGamePhase();

            // Time to level up enemies if 'true'
            if(gameManager.battlesCompleted % gameManager.battlesPerLevelUp == 0)
            {
                // The enemies haven't been leveled up yet.
                if(gameManager.lastEnemyLevelUps < gameManager.battlesCompleted)
                {
                    // Goes through each door.
                    foreach(Door door in doors)
                    {
                        // Only level up unlocked doors.
                        if(!door.Locked)
                        {
                            // Levels up the entity by the amount of battles per level up (the value is the same).
                            door.battleEntity = BattleEntity.LevelUpData(door.battleEntity, (uint)gameManager.battlesPerLevelUp);
                        }
                    }


                    gameManager.lastEnemyLevelUps = gameManager.battlesCompleted;
                }
            }

            // If in the middle phase, and no evolutions have happened.
            // If in the end phase, and the evolutions have not been run a second time.
            if ((phase == 2 && gameManager.evolveWaves == 0) || (phase == 3 && gameManager.evolveWaves == 1))
            {
                // Goes through each door.
                foreach(Door door in doors)
                {
                    // Only evolve the entity if the door is unlocked.
                    if (!door.Locked)
                        door.battleEntity = BattleEntity.Evolve(door.battleEntity);
                }

                gameManager.evolveWaves++;
            }

        }

        // Update is called once per frame
        void Update()
        {

        }

        
    }
}