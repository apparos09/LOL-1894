using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RM_BBTS
{
    // The manager for the overworld.
    public class OverworldManager : GameState
    {
        // Becomes 'true' when the overworld is initialized.
        private bool initialized = false;

        // The gameplay manager.
        public GameplayManager gameManager;

        // The object that was selected in the overworld.
        // public GameObject selectedObject;

        // Gets set to 'true' when the game is in a game over state.
        // If 'true', the game over function will be called when the player returns to the overworld.
        public bool gameOver;

        // The background.
        [Header("Background")]
        // The background for the game.
        public SpriteRenderer background;

        // The three phase colours.
        public Color phase1Color = Color.white;
        public Color phase2Color = Color.white;
        public Color phase3Color = Color.white;

        // Uses the phase colors, changing the background when the phase changes.
        public bool usePhaseColors = true;

        [Header("Doors")]
        // The list of doors.
        public List<Door> doors = new List<Door>();

        // The boss door.
        public Door bossDoor = null;

        // THe treasure doors.
        public List<Door> treasureDoors = null; 

        // The amount of the doors.
        public const int DOOR_COUNT = 18;

        // The amount of treasures for the game.
        public const int TREASURE_COUNT = 3;

        /*
         * Determines the game boss. Any number other than 0 is only used for testing.
         * 0 = Varies
         * 1 = Combat Bot (ATTACK/DEFAULT)
         *  - Default option.
         * 2 = Comet (SPEED)
         * 3 = Vortex (DEFENSE)
         */
        private const int GAME_BOSS = 0;

        // These are used for choosing the final boss.        
        private int noneSpecials = 0;
        private int healthSpecials = 0;
        private int attackSpecials = 0;
        private int defenseSpecials = 0;
        private int speedSpecials = 0;
        private int energySpecials = 0;

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

        [Header("Doors/Sprites")]

        // The list of unlocked and locked door sprites (does NOT include the boss door).
        public List<Sprite> doorUnlockedSprites;
        public List<Sprite> doorLockedSprites;

        // The unlocked and locked boss door sprites.
        public Sprite bossDoorUnlockedSprite;
        public Sprite bossDoorLockedSprite;

        [Header("UI")]
        
        // The user interface.
        public GameObject ui;

        // The score text for the overworld.
        public TMP_Text scoreText;

        // The amount of digits the score has. If it surpasses this amount it will add more digits.
        // It can probably just be 5 digits.
        public const int SCORE_DIGITS = 10;

        [Header("Audio")]
        // The bgm for the overworld.
        public AudioClip overworldBgm;

        // The sound effect for a locked door.
        public AudioClip doorLockedSfx;

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
            // The overworld is only initialized once, so this function should NOT be called more than once.
            // Either way, this function stops it from being called twice.
            if(initialized)
            {
                return;
            }

            // In the overworld state.
            gameManager.SetStateToOverworld();

            // Initializes the doors (normal, treasure, and boss)
            {
                // Door initialization list.
                List<Door> doorInitList = new List<Door>(doors);

                // A random index.
                int randIndex = 0;

                // BOSS (PART 1)
                // The boss door
                randIndex = Random.Range(0, doorInitList.Count); // Grabs the random index.
                bossDoor = doorInitList[randIndex]; // Grab random door to make boss door.
                doorInitList.RemoveAt(randIndex); // Remove boss door from list.
                
                // Replaces the sprite in the 'GenerateRoom' function for consistency.
                bossDoor.isBossDoor = true; // This is a boss door.

                // MOVED to the end so that the enemy can vary.
                // GenerateRoom(bossDoor); // Generates the room.

                // TREASURES
                // The treasure doors.
                treasureDoors = new List<Door>();

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


                // Initialize the basic doors.
                foreach(Door door in doorInitList)
                {
                    GenerateRoom(door);

                    // Checks the stat specialty of the generated enemy.
                    switch (door.battleEntity.statSpecial)
                    {
                        case BattleEntity.specialty.none: // No special.
                            noneSpecials++;
                            break;
                        case BattleEntity.specialty.health: // Health special.
                            healthSpecials++;
                            break;
                        case BattleEntity.specialty.attack: // Attack special.
                            attackSpecials++;
                            break;
                        case BattleEntity.specialty.defense: // Defense special.
                            defenseSpecials++;
                            break;
                        case BattleEntity.specialty.speed: // Speed special.
                            speedSpecials++;
                            break;

                    }
                }


                // BOSS (PART 2)
                // This will choose the boss based on the other enemies.
                GenerateRoom(bossDoor);

                // Note: this was probably caused just by an error from a missing object.
                // // For some reason the doors sometimes don't get replaced. This should fix it.
                // bossDoor.unlockedSprite = bossDoorUnlockedSprite;
                // bossDoor.lockedSprite = bossDoorLockedSprite;
            }

            // Switches to the phase 1 color.
            if (usePhaseColors)
                background.color = phase1Color;

            // Updates the UI.
            UpdateUI();

            // Plays the overworld BGM.
            PlayOverworldBgm();

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
                if(door.Locked)
                {
                    // Plays the door SFX.
                    PlayDoorLockedSfx();
                }
                else
                {
                    // Enters the battle.
                    gameManager.EnterBattle(door);
                }
            }
        }

        // A function to call when a tutorial starts.
        public override void OnTutorialStart()
        {

        }

        // A function to call when a tutorial ends.
        public override void OnTutorialEnd()
        {

        }

        // Retunrs 'true' if the overworld is initialized.
        public bool Initialized
        {
            get { return initialized; }
        }

        // Generates a room for the door.
        private void GenerateRoom(Door door)
        {
            // Unlock the door.
            door.Locked = false;

            // Checks the door type.
            if(door.isBossDoor) // Boss Door
            {
                // The boss to be generated.
                int boss = GAME_BOSS;

                // The maximum special value.
                int specialMax = Mathf.Max(noneSpecials, healthSpecials, attackSpecials, defenseSpecials, speedSpecials, energySpecials);

                // Checks if the boss choice should vary.
                if(boss == 0)
                {
                    if (specialMax == noneSpecials || specialMax == healthSpecials || specialMax == attackSpecials) // Attack Boss
                    {
                        boss = 1;
                    }
                    else if (specialMax == defenseSpecials) // Defense Boss
                    {
                        boss = 3;
                    }
                    else if (specialMax == speedSpecials) // Speed Boss
                    {
                        boss = 2;
                    }
                    else
                    {
                        boss = 1; // Default boss.
                    }
                }

                // Checks what game boss to use.
                // Uses this function.
                // 1 = Combat Bot (Default), 2 = Comet, 3 = Blackhole
                door.battleEntity = BattleEntityList.Instance.GenerateBoss(boss);

                // switch(boss)
                // {
                //     case 1: // Combatbot (combatbot) - Attack/Health/Default
                //     default:
                //         door.battleEntity = BattleEntityList.Instance.GenerateBoss(1);
                //         break;
                //     case 2: // Comet (comet) - Speed
                //         door.battleEntity = BattleEntityList.Instance.GenerateBattleEntityData(battleEntityId.comet);
                //         break;
                //     case 3: // Vortex (blackhole) - Defense
                //         door.battleEntity = BattleEntityList.Instance.GenerateBattleEntityData(battleEntityId.blackHole);
                //         break;
                // }

                // Replaces the sprites.
                door.unlockedSprite = bossDoorUnlockedSprite;
                door.lockedSprite = bossDoorLockedSprite;

                // Saves boss door. The most recent door is considered the boss door.
                bossDoor = door;
            }
            else if(door.isTreasureDoor) // Treasure Door
            {
                door.battleEntity = BattleEntityList.Instance.GenerateBattleEntityData(battleEntityId.treasure);
            }
            else // Normal Door
            {
                // Test
                // door.battleEntity = BattleEntityList.Instance.GenerateBattleEntityData(battleEntityId.ufo);

                // Generates a random enemy (base version).

                // TODO: switch to the final version after implementing more enemies.
                // FINAL - I don't think I'll use random weights.
                // door.battleEntity = BattleEntityList.Instance.GenerateRandomEnemy(true, false, true); // Random rates.
                door.battleEntity = BattleEntityList.Instance.GenerateRandomEnemy(true, true, true); // No random rates.

                // TESTING 
                // door.battleEntity = BattleEntityList.Instance.GenerateRandomEnemy(false, false, true);

            }

            // Randomizes the door image if it's not a boss door.
            if (!door.isBossDoor && doorLockedSprites.Count != 0 && doorLockedSprites.Count == doorUnlockedSprites.Count)
            {
                // Generates a random door image.
                int index = Random.Range(0, doorLockedSprites.Count);

                // Replaces the sprites.
                door.unlockedSprite = doorUnlockedSprites[index];
                door.lockedSprite = doorLockedSprites[index];
            }

            // Sets the level.
            door.battleEntity = BattleEntity.LevelUpData(
                door.battleEntity, 
                door.battleEntity.levelRate,
                door.battleEntity.statSpecial, 
                (uint)Random.Range(1, gameManager.roomsPerLevelUp + 1));


            // TODO: randomize the enemy being placed behind the door.
            // door.battleEntity = BattleEntityList.Instance.GenerateBattleEntityData(battleEntityId.unknown);
            // door.battleEntity = BattleEntityList.Instance.GenerateBattleEntityData(battleEntityId.treasure);
            // door.battleEntity = BattleEntityList.Instance.GenerateBattleEntityData(battleEntityId.boss);

            // Make sure the battle entity is parented to the door.
            // TODO: have algorithm for generating enemies.

            // Makes sure the door sprite is updated to match the provided images.
            door.UpdateSprite();

        }

        // Updates the UI for the overworld.
        public void UpdateUI()
        {
            // Set the base score.
            scoreText.text = gameManager.score.ToString("D" + SCORE_DIGITS.ToString());
        }

        // Plays the overworld bgm.
        public void PlayOverworldBgm()
        {
            // Gets the phase of the game.
            int phase = gameManager.GetGamePhase();

            // Grabs the audio manager.
            AudioManager audioManager = gameManager.audioManager;

            // Checks the phase for playing the BGM.
            switch (phase)
            {
                default:
                case 1: // Normal Speed
                    audioManager.PlayBgm(overworldBgm, 1.0F);
                    break;

                case 2: // Faster
                    audioManager.PlayBgm(overworldBgm, 1.2F);
                    break;

                case 3: // Faster
                    audioManager.PlayBgm(overworldBgm, 1.4F);
                    break;
            }


        }

        // Play the door locked SFX.
        public void PlayDoorLockedSfx()
        {
            // Grabs the audio manager.
            AudioManager audioManager = gameManager.audioManager;

            // Plays the door locked SFX.
            audioManager.PlaySoundEffect(doorLockedSfx);
        }

        // Called when returning to the overworld.
        public void OnOverworldReturn()
        {
            // Currently in the overworld.
            gameManager.SetStateToOverworld();

            // Rearranges the doors.
            if (gameOver)
                OnOverworldReturnGameOver();

            // Calls the function for OnGamePhaseChange().
            OnGamePhaseChange();

            // Update the UI for the overworld.
            UpdateUI();

            // Plays the overworld BGM.
            PlayOverworldBgm();

        }

        // Called when the game phase changes.
        public void OnGamePhaseChange()
        {
            // Evolves the entities.
            int phase = gameManager.GetGamePhase();

            // Time to level up enemies if 'true'.
            // If no rooms have been completed, then nothing happens.
            if (gameManager.roomsCompleted % gameManager.roomsPerLevelUp == 0 && gameManager.roomsCompleted != 0)
            {
                // The enemies haven't been leveled up yet.
                if (gameManager.lastEnemyLevelUps < gameManager.roomsCompleted)
                {
                    // Goes through each door.
                    foreach (Door door in doors)
                    {
                        // Only level up unlocked doors.
                        if (!door.Locked)
                        {
                            // Levels up the entity by the amount of battles per level up (the value is the same).
                            door.battleEntity = BattleEntity.LevelUpData(
                                door.battleEntity,
                                door.battleEntity.levelRate,
                                door.battleEntity.statSpecial,
                                (uint)gameManager.roomsPerLevelUp
                                );
                        }
                    }


                    gameManager.lastEnemyLevelUps = gameManager.roomsCompleted;
                }
            }

            // If in the middle phase, and no evolutions have happened.
            // If in the end phase, and the evolutions have not been run a second time.
            if ((phase == 2 && gameManager.evolveWaves == 0) || (phase == 3 && gameManager.evolveWaves == 1))
            {
                // Goes through each door.
                foreach (Door door in doors)
                {
                    // Only evolve the entity if the door is unlocked.
                    // It helps save on evolution time.
                    if (!door.Locked)
                    {
                        door.battleEntity = BattleEntity.EvolveData(door.battleEntity);

                        // Restore health and energy levels to max even if the entity didn't evolve.
                        door.battleEntity.health = door.battleEntity.maxHealth;
                        door.battleEntity.energy = door.battleEntity.maxEnergy;

                    }

                }

                // Entities evolved.
                gameManager.evolveWaves++;

                // Gives the new phase bonus.
                gameManager.player.ApplyNewPhaseBonus();
            }

            // Change the background image colours.
            if (usePhaseColors)
            {
                // Check the phase.
                switch (phase)
                {
                    case 1: // Phase 1 (Start)
                        background.color = phase1Color;
                        break;
                    case 2: // Phase 2 (Middle)
                        background.color = phase2Color;
                        break;
                    case 3: // Phase 3 (End)
                        background.color = phase3Color;
                        break;
                    default: // White
                        background.color = Color.white;
                        break;
                }
            }
        }
        
        // Rearranges the doors.
        public void OnOverworldReturnGameOver()
        {
            // TODO: don't move the boss door.
            // The new positions
            List<Vector3> doorLocs = new List<Vector3>();

            // Geta all door positions.
            foreach(Door door in doors)
            {
                // Restore health and energy to entity.
                door.battleEntity.health = door.battleEntity.maxHealth;
                door.battleEntity.energy = door.battleEntity.maxEnergy;

                // Adds the position to the list.
                doorLocs.Add(door.gameObject.transform.position);
            }

            // Goes through each door again.
            for(int i = 0; i < doors.Count && doorLocs.Count != 0; i++)
            {
                // Grabs a random index.
                int randIndex = Random.Range(0, doorLocs.Count);

                // Re-positions the door.
                doors[i].transform.position = doorLocs[randIndex];

                // Removes position from list.
                doorLocs.RemoveAt(randIndex);
            }

            // Randomize player moves
            Player player = gameManager.player;

            // Go through each move.
            for(int i = 0; i < player.moves.Length; i++)
            {
                // Move has been set.
                if (player.moves[i] != null)
                {
                    // Grabs the rank.
                    int rank = player.moves[i].Rank;

                    // Replaces the move.
                    switch(rank)
                    {
                        case 1: // R1
                            player.moves[i] = MoveList.Instance.GetRandomRank1Move();
                            break;
                        case 2: // R2
                            player.moves[i] = MoveList.Instance.GetRandomRank2Move();
                            break;
                        case 3: // R3
                            player.moves[i] = MoveList.Instance.GetRandomRank3Move();
                            break;
                    }
                }
            }

            // Checks to see if the player has an attacking move.
            bool playerHasAttack = false;

            // Checks each move.
            foreach(Move move in player.moves)
            {
                // The player has an attacking move.
                if(move.Power != 0)
                {
                    playerHasAttack = true;
                    break;
                }
            }

            // The player does not have an attacking move, so give them one.
            if(!playerHasAttack)
            {
                // Gives the player a basic attack move.
                player.Move0 = MoveList.Instance.GenerateMove(moveId.kablam);
            }


            gameOver = false;

        }


        // // Update is called once per frame
        // void Update()
        // {
        // 
        // }

        
    }
}