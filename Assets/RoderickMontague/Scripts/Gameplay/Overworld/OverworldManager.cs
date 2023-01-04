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

        // The total amount of rooms in the game.
        public const int ROOM_COUNT = 15;

        // The boss door.
        public Door bossDoor = null;

        // THe treasure doors.
        public List<Door> treasureDoors = null; 

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

        // Determines if the boss can only be fought at the end of the game (true) or not (false).
        [Tooltip("If true, the boss can only be fought at the end of the game.")]
        public bool bossAtEnd = true;

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

        // The door type integers for setting the animations.
        public List<int> doorTypes = new List<int>();

        // The boss door type integer.
        public int bossDoorType = 0;

        [Header("Game Question")]
        // The game question manager.
        public GameQuestionManager gameQuestion;

        // The increment for the round spacing for asking questions.
        [Tooltip("The multiple used to determine what round to ask questions on.")]
        public int questionRoundInc = 3;

        // The next round that a question will be asked on.
        [Tooltip("The next round the question will be asked on (roomsCompleted + 1). This is set to questionRoundInc when the overworld is initialized.")]
        public int nextQuestionRound = 0;

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
            // The room count and door count don't match.
            if (doors.Count != ROOM_COUNT)
            {
                Debug.LogWarning("The door count does not match the room count! Will cause save issues.");
            }
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

                // If the boss should be locked until the end, lock the door.
                if (bossAtEnd)
                    bossDoor.Locked = true;
            }

            // Switches to the phase 1 color.
            if (usePhaseColors)
                background.color = phase1Color;


            // Prepares for when the question will be asked.
            nextQuestionRound = questionRoundInc;

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
            // Touched object was set to null for some reason.
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
                    // PlayDoorLockedSfx();
                }
                else
                {
                    // Enters the battle.
                    // Checks if transitions should be used for knowing which function to call.
                    if(gameManager.useTransitions)
                        gameManager.EnterBattleWithTransition(door);
                    else
                        gameManager.EnterBattle(door);
                }
            }
        }

        // A function to call when a tutorial starts.
        public override void OnTutorialStart()
        {
            // Disables the question if one is being asked.
            if (gameQuestion.QuestionIsRunning())
                gameQuestion.DisableQuestion();

        }

        // A function to call when a tutorial ends.
        public override void OnTutorialEnd()
        {
            // Enables the question if one is being asked.
            if (gameQuestion.QuestionIsRunning())
                gameQuestion.EnableQuestion();

        }

        // Retunrs 'true' if the overworld is initialized.
        public bool Initialized
        {
            get { return initialized; }
        }
        
        // Returns the amount of doors.
        public int GetDoorCount()
        {
            return doors.Count;
        }

        // Gets the unlocked door sprite.
        public void SetDoorSpritesByDoorType(Door door)
        {
            // Index of door sprites.
            int index = 0;

            // Boss door locked and unlocked sprite.
            if (door.isBossDoor)
            {
                door.doorType = bossDoorType; // Sets type.
                door.unlockedSprite = bossDoorUnlockedSprite;
                door.lockedSprite = bossDoorLockedSprite;
                return;
            }

            // Set to the default if the door type is invalid.
            if (!doorTypes.Contains(door.doorType))
                door.doorType = 0;

            // Finds the index.
            if(doorTypes.Contains(door.doorType))
            {
                // Gets the index.
                index = doorTypes.IndexOf(door.doorType);

                // Sets the values.
                door.unlockedSprite = doorUnlockedSprites[index];
                door.lockedSprite = doorLockedSprites[index];
            }
        }

        // Gets the color for the dedicated door type.
        public static Color GetDoorTypeColor(int doorType)
        {
            // Color object.
            Color color;

            // Checks the door type.
            switch (doorType)
            {
                case 0: // default (white)
                default:
                    color = Color.white;
                    break;
                case 1: // boss door (red)
                    color = Color.red;
                    break;
                case 2: // blue
                    color = Color.blue;
                    break;
                case 3: // yellow
                    color = Color.yellow;
                    break;
                case 4: // green (it seems like it should be purple, but it's not).
                    color = Color.green;
                    break;
                case 5: // purple (it seems like it should be green, but it's not).
                    color = new Color(0.627F, 0.125F, 0.941F);
                    break;
            }

            return color;
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
                door.doorType = bossDoorType;

                // Saves boss door. The most recent door is considered the boss door.
                bossDoor = door;
            }
            else if(door.isTreasureDoor) // Treasure Door
            {
                door.battleEntity = BattleEntityList.Instance.GenerateBattleEntityData(battleEntityId.treasure);
            }
            else // Normal Door
            {
                // Test (always loads the same neemy)
                // door.battleEntity = BattleEntityList.Instance.GenerateBattleEntityData(battleEntityId.ufo);

                // Generates a random enemy (base version).

                // FINAL - I don't think I'll use random weights.
                // door.battleEntity = BattleEntityList.Instance.GenerateRandomEnemy(true, false, true); // Random weights.
                door.battleEntity = BattleEntityList.Instance.GenerateRandomEnemy(true, true, true); // No random weights.

                // TESTING 
                // door.battleEntity = BattleEntityList.Instance.GenerateRandomEnemy(false, false, true);

            }

            // Randomizes the door image if it's not a boss door.
            if (!door.isBossDoor && doorLockedSprites.Count != 0 && doorLockedSprites.Count == doorUnlockedSprites.Count)
            {
                // Generates a random door image.
                // Starts at 1 so that the white (default) door.
                int index = Random.Range(1, doorLockedSprites.Count);

                // Replaces the sprites.
                door.unlockedSprite = doorUnlockedSprites[index];
                door.lockedSprite = doorLockedSprites[index];
                // door.SetDoorOpenAnimation(doorTypes[index]);
                door.doorType = doorTypes[index];

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

            // If the boss should only be available at the end.
            if (bossAtEnd)
            {
                // If the player is on the last room, unlock the boss door.
                // Otherwise, lock the boss door.
                if(gameManager.GetRoomsCompleted() + 1 == gameManager.GetRoomsTotal())
                {
                    // Unlocks the door if it is locked.
                    if (bossDoor.Locked)
                        bossDoor.Locked = false;

                }
                else
                {
                    // Locks the door if it is unlocked.
                    if (!bossDoor.Locked)
                        bossDoor.Locked = true;
                }
            }

            // Asking a question of the question round number has been reached or surpassed.
            if(gameManager.GetCurrentRoomNumber() >= nextQuestionRound)
            {
                // Ask a random question, and increase the next round counter.
                gameQuestion.AskRandomQuestion();
                nextQuestionRound += questionRoundInc;
            }

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

                        // TODO: maybe don't restore it entirely?
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
                        if(background.color != phase1Color)
                            background.color = phase1Color;
                        break;
                    case 2: // Phase 2 (Middle)
                        if (background.color != phase2Color)
                            background.color = phase2Color;
                        break;
                    case 3: // Phase 3 (End)
                        if (background.color != phase3Color)
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

            // List of 4 index spots.
            List<int> moveIndexes = new List<int>() { 0, 1, 2, 3 };

            // Removes two indexes.
            moveIndexes.Remove(Random.Range(0, moveIndexes.Count));
            moveIndexes.Remove(Random.Range(0, moveIndexes.Count));

            // Goes through each move index.
            foreach(int moveIndex in moveIndexes)
            {  
                // Becomes set to 'true' when a move has been found.
                bool foundMove = false;
                // If too many attempts are made, then it sticks with the move generated (avoids duplicate moves).
                int attempts = 0;
                // Maximum amount of attempts.
                const int ATTEMPTS_MAX = 5;

                do
                {
                    // The generated move.
                    Move move = null;

                    // Checks that the move exists in the player's list.
                    if (player.moves[moveIndex] != null)
                    {
                        // Grabs the move rank, and replaces the move.
                        switch (player.moves[moveIndex].Rank)
                        {
                            case 1: // R1
                                move = MoveList.Instance.GetRandomRank1Move();
                                break;
                            case 2: // R2
                                move = MoveList.Instance.GetRandomRank2Move();
                                break;
                            case 3: // R3
                                move = MoveList.Instance.GetRandomRank3Move();
                                break;
                            default: // Not applicable rank.
                                move = MoveList.Instance.GetRandomMove();
                                break;
                        }
                    }
                    else
                    {
                        // The player doesn't have a move at this index, so it can't be replaced.
                        // Break out of this do-while loop.
                        break;
                    }

                    // Made an attempt to find a move.
                    attempts++;

                    // Checks to make sure the player doesn't already have the move.
                    // Handles duplicate moves.
                    if(player.HasMove(move) && attempts <= ATTEMPTS_MAX)
                    {
                        // Player already has this move.
                        // Try generating another move.
                        foundMove = false;
                    }
                    else
                    {
                        // Player does not have this move...
                        // Or the attempts have been maxed out.
                        player.moves[moveIndex] = move;
                        foundMove = true;
                    }

                } while (!foundMove && attempts > ATTEMPTS_MAX);
                
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
                // Gives the player a basic attack move if they don't have one.
                if(moveIndexes.Count > 0)
                {
                    player.moves[moveIndexes[Random.Range(0, moveIndexes.Count)]] = 
                        MoveList.Instance.GenerateMove(moveId.kablam);
                }
                else // Replace the player's first move.
                {
                    player.Move0 = MoveList.Instance.GenerateMove(moveId.kablam);
                }
                    
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