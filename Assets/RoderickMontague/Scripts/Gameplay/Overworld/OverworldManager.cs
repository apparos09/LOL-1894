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
        // NOTE: if you change the room count, change the amount of doors in the list, and change the phase percentages.
        public const int ROOM_COUNT = 10;

        // The phase 2 and phase 3 thresholds. This is below (3/10 and 6/10) so that the phase changes after 3 rooms cleared.
        public const float PHASE_2_THRESOLD = 0.29F; // 0.33
        public const float PHASE_3_THRESOLD = 0.59F; // 0.66

        // The boss door.
        public Door bossDoor = null;

        // THe treasure doors.
        public List<Door> treasureDoors = null; 

        // The amount of treasures for the game.
        public const int TREASURE_COUNT = 3; // 2

        /*
         * Determines the game boss. Any number other than 0 is only used for testing.
         * 0 = Varies (Random)
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

        // Used to determine if unevolved entities should level up when a new phase begins.
        const bool LEVEL_UP_FOR_PHASE_IF_UNEVOLVED = true;

        // The amount of levels an unevolved entity gets on a new phase (only used if LEVEL_UP_FOR_PHASE_IF_UNEVOLVED is true).
        const int PHASE_LEVEL_UP_IF_UNEVOLVED = 1;

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

        // Asks questions if set to true.
        public bool askQuestions = true;

        // The wait time (in rounds) for automatically asking a question.
        [Tooltip("The wait time (in rounds) to ask a question.")]
        public int questionWaitTime = 3;

        // Counts down to the next time a question will be asked.
        [Tooltip("The countdown for asking the user a question.")]
        public int questionCountdown = 0;

        // Starts asking questions from this round onwards.
        public int askQuestionsFromRound = 0;

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

        // The bgm for having a question.
        public AudioClip questionBgm;

        // The sound effect for entering a door.
        public AudioClip doorEnterSfx;

        // The sound effect for a locked door.
        public AudioClip doorLockedSfx;

        // The question correct SFX.
        public AudioClip questionCorrectSfx;

        // The question incorrect SFX.
        public AudioClip questionIncorrectSfx;

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
            questionCountdown = questionWaitTime;

            // Updates the UI.
            UpdateUI();

            // Plays the overworld BGM.
            PlayOverworldBgm();

            // Enables the overworld background.
            gameManager.EnableOverworldBackground();

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
            // I don't think a tutorial is run when a question is shown, but just to be sure, this handles it.
            // Disables the question if one is being asked.
            // NOTE: you removed the activeSelf check to fix a problem.
            if (gameQuestion.QuestionIsRunning())
            {
                gameQuestion.DisableQuestion();
                gameQuestion.gameObject.SetActive(false);
            }

        }

        // A function to call when a tutorial ends.
        public override void OnTutorialEnd()
        {
            // I don't think a tutorial is run when a question is shown, but just to be sure, this handles it.
            // Enables the question if one is being asked.
            // NOTE: you removed the activeSelf check to fix a re-activation problem.
            if (gameQuestion.QuestionIsRunning())
            {
                // Enable the question.
                gameQuestion.EnableQuestion(true);
                gameQuestion.gameObject.SetActive(true);

                // Makes sure that the mouse touch input is still off since there's a question.
                gameManager.mouseTouchInput.gameObject.SetActive(false);
            }

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
            // Function moved to the door class.
            return Door.GetDoorTypeColor(doorType);
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
                (uint)Random.Range(1, GameplayManager.ROOMS_PER_LEVEL_UP + 1));


            // TODO: randomize the enemy being placed behind the door.
            // door.battleEntity = BattleEntityList.Instance.GenerateBattleEntityData(battleEntityId.unknown);
            // door.battleEntity = BattleEntityList.Instance.GenerateBattleEntityData(battleEntityId.treasure);
            // door.battleEntity = BattleEntityList.Instance.GenerateBattleEntityData(battleEntityId.boss);

            // Make sure the battle entity is parented to the door.
            // TODO: have algorithm for generating enemies.

            // Makes sure the door sprite is updated to match the provided images.
            door.UpdateSprite();

        }


        // Asks the user a random question.
        public void AskQuestion()
        {
            // Asks the question.
            gameQuestion.AskRandomQuestion();
        }

        // Checks if a question will be asked when returning to the overworld.
        public bool AskingQuestionOnOverworldEnter(bool battleWon)
        {
            // Checks various conditions to see if a question will be asked.
            /*
             * Checks various conditions to see if a question will be asked.
             * [1] Checks if questions will be asked.
             * [2] Checks if the battle has been won (questions are asked if a battle is won).
             * [3] Checks if the rooms completed count has risen enough to start asking questions.
             * [4] Checks if the questionCountdown has reached 0 (time to ask question).
             */

            bool result = 
                askQuestions &&
                battleWon &&
                (gameManager.GetRoomsCompleted() >= askQuestionsFromRound) &&
                (questionCountdown - 1 <= 0);

            return result;
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
                    audioManager.PlayBackgroundMusic(overworldBgm, 1.0F);
                    break;

                case 2: // Faster
                    audioManager.PlayBackgroundMusic(overworldBgm, 1.2F);
                    break;

                case 3: // Faster
                    audioManager.PlayBackgroundMusic(overworldBgm, 1.4F);
                    break;
            }


        }

        // Plays the overworld bgm.
        public void PlayQuestionBgm()
        {
            // Plays the question bgm.
            gameManager.audioManager.PlayBackgroundMusic(questionBgm);
        }

        // Play the door enter SFX.
        public void PlayDoorEnterSfx()
        {
            // Grabs the audio manager.
            AudioManager audioManager = gameManager.audioManager;

            // Plays the door enter SFX.
            audioManager.PlaySoundEffect(doorEnterSfx);
        }

        // Play the door locked SFX.
        public void PlayDoorLockedSfx()
        {
            // Grabs the audio manager.
            AudioManager audioManager = gameManager.audioManager;

            // Plays the door locked SFX.
            audioManager.PlaySoundEffect(doorLockedSfx);
        }

        // Plays the question correct SFX.
        public void PlayQuestionCorrectSfx()
        {
            gameManager.audioManager.PlaySoundEffect(questionCorrectSfx);
        }

        // Plays the question incorrect SFX.
        public void PlayQuestionIncorrectSfx()
        {
            gameManager.audioManager.PlaySoundEffect(questionIncorrectSfx);
        }

        // Called when returning to the overworld.
        public void OnOverworldReturn(bool battleWon)
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

            // If set to 'true', questions are asked.
            if(askQuestions)
            {
                // If the questions should now start being asked.
                if(gameManager.GetRoomsCompleted() >= askQuestionsFromRound)
                {
                    // Subtracts from the countdown if a battle was completed.
                    // NOTE: this won't be asked on the first turn since this function isn't called until a battle is completed.
                    // This will show up on every following turn though.ns.
                    if (battleWon)
                    {
                        questionCountdown--;

                        // If the countdown has reached (or fallen below) 0, ask a question.
                        if (questionCountdown <= 0)
                        {
                            AskQuestion();
                            questionCountdown = questionWaitTime;

                            // Triggers the tutorial for asking questions.
                            if (GameSettings.Instance.UseTutorial)
                            {
                                // If the phase tutorial hasn't been loaded yet.
                                if (!gameManager.tutorial.clearedQuestion && !gameManager.tutorial.TextBoxIsVisible())
                                {
                                    // Load the phase tutorial.
                                    gameManager.tutorial.LoadQuestionTutorial();
                                }
                            }

                            // If a tutorial is running, disalbe the question for now.
                            if (gameManager.tutorial.TextBoxIsVisible())
                            {
                                gameQuestion.DisableQuestion();
                                gameQuestion.gameObject.SetActive(false);

                                // Re-reads the current page so that the tutorial TTS dialogue doesn't override it.
                                gameManager.tutorial.SpeakCurrentPage();
                            }
                        }
                    }
                }

            }
            

            // Update the UI for the overworld.
            UpdateUI();

            // Plays the overworld BGM.
            PlayOverworldBgm();

            // Show the overworld background.
            gameManager.EnableOverworldBackground();
        }

        // Called when the game phase changes.
        public void OnGamePhaseChange()
        {
            // Evolves the entities.
            int phase = gameManager.GetGamePhase();

            // Gets set to 'true' when the phase changes.
            bool phaseChanged = false;

            // NOTE: entities only evolve when the door is unlocked.
            // Since doors never become unlocked after they are locked (not counting the battle tutorial)...
            // This shouldn't cause any problems. The boss stats are simply high enough to meet the player's final level.

            // Time to level up enemies if 'true'.
            // If no rooms have been completed, then nothing happens.
            if (gameManager.roomsCompleted % GameplayManager.ROOMS_PER_LEVEL_UP == 0 && gameManager.roomsCompleted != 0)
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
                                (uint)GameplayManager.ROOMS_PER_LEVEL_UP
                                );
                        }
                    }


                    gameManager.lastEnemyLevelUps = gameManager.roomsCompleted;
                }
            }

            // This is an attempt to fix a bug where enemies either weren't evolving, or reverted back to prior evolution stages.
            // This bug only happened when I went down from 15 doors to 12 doors (as far as I can tell)...
            // But I don't know what caused it.
            if (gameManager.evolveWaves < GameplayManager.EVOLVE_WAVES_MAX)
            {
                // If in the middle phase, and no evolutions have happened.
                // If in the end phase, and the evolutions have not been run a second time.
                if ((phase == 2 && gameManager.evolveWaves == 0) || (phase == 3 && gameManager.evolveWaves == 1))
                {
                    // The phase is changing.
                    phaseChanged = true;

                    // Goes through each door.
                    foreach (Door door in doors)
                    {
                        // Only evolve the entity if the door is unlocked.
                        // It helps save on evolution time.
                        if (!door.Locked)
                        {
                            // Evolve the entity if it has an evolution.
                            // If it can't evolve, raise it's level ith a basic level rate and no speciality. 
                            

                            // Checks if unevolved entities should receive a boost on a new phase or not.
                            if(LEVEL_UP_FOR_PHASE_IF_UNEVOLVED) // All Entities Receive Boost
                            {
                                // IF the entity can evolve, evolve it. If it can't evolve, just level it up.
                                if (BattleEntity.CanEvolve(door.battleEntity))
                                    door.battleEntity = BattleEntity.EvolveData(door.battleEntity);
                                else
                                    door.battleEntity = BattleEntity.LevelUpData(door.battleEntity, PHASE_LEVEL_UP_IF_UNEVOLVED);
                            }
                            else // No Boost for Unevolved Entities
                            {
                                // Evolve the entity. If the entity can't evolve, nothing happens.
                                door.battleEntity = BattleEntity.LevelUpData(door.battleEntity, 1.0F, BattleEntity.specialty.none, 1);
                            }


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

            // Triggers the tutorial.
            if(GameSettings.Instance.UseTutorial && phaseChanged)
            {
                // If the phase tutorial hasn't been loaded yet.
                if(!gameManager.tutorial.clearedPhase && !gameManager.tutorial.TextBoxIsVisible())
                {
                    // Load the phase tutorial.
                    gameManager.tutorial.LoadPhaseTutorial();
                }
            }
        }
        
        // Called when the player gets a game over.
        public void OnOverworldReturnGameOver()
        {
            // The new positions
            List<Vector3> doorLocs = new List<Vector3>();

            // Geta all door positions.
            foreach(Door door in doors)
            {
                // Restore health and energy to entity.
                // Old (restores to max)
                // door.battleEntity.health = door.battleEntity.maxHealth;
                // door.battleEntity.energy = door.battleEntity.maxEnergy;


                // Restore health and energy by a percentage of maxes.
                door.battleEntity.health += door.battleEntity.maxHealth * Enemy.GAME_OVER_HEALTH_RESTORE_PERCENT;
                door.battleEntity.energy += door.battleEntity.maxEnergy * Enemy.GAME_OVER_ENERGY_RESTORE_PERCENT;

                // Make the health and energy levels whole numbers.
                door.battleEntity.health = Mathf.Ceil(door.battleEntity.health);
                door.battleEntity.energy = Mathf.Ceil(door.battleEntity.energy);

                // Clamp the values so that they're within the bounds.
                door.battleEntity.health = Mathf.Clamp(door.battleEntity.health, 0, door.battleEntity.maxHealth);
                door.battleEntity.energy = Mathf.Clamp(door.battleEntity.energy, 0, door.battleEntity.maxEnergy);


                // Adds the local position to the list.
                doorLocs.Add(door.gameObject.transform.localPosition);
            }

            // Goes through each door again.
            for(int i = 0; i < doors.Count && doorLocs.Count != 0; i++)
            {
                // Grabs a random index.
                int randIndex = Random.Range(0, doorLocs.Count);

                // Re-positions the door.
                doors[i].gameObject.transform.localPosition = doorLocs[randIndex];

                // Removes position from list.
                doorLocs.RemoveAt(randIndex);
            }

            // Randomize player moves
            Player player = gameManager.player;

            // Boosts the player's stats if they get a game over.
            float boost = Player.GAME_OVER_BONUS_STAT_TOTAL / 4.0F;

            // Gives the playr a small stat boost upon getting a game over.
            player.SetHealthRelativeToMaxHealth(player.MaxHealth + boost);
            player.Attack += boost;
            player.Defense += boost;
            player.Speed += boost;

            // List of 4 index spots.
            List<int> moveIndexes = new List<int>() { 0, 1, 2, 3 };

            // Gets the game phase for determining how the randomization works. 
            int phase = gameManager.GetGamePhase();

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
                        switch (phase)
                        {
                            default: // Phase 1 - replace with move of the same rank. 
                            case 1:
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
                                break;

                            case 2: // Phase 2 - If rank 1 (or no rank) move, replace with rank 2 or rank 3 move. 
                                // Grabs the move rank, and replaces the move. 
                                switch (player.moves[moveIndex].Rank)
                                {
                                    default:
                                    case 1: // R1 
                                        // More likely to get rank 2 (6/10) over rank 3 (4/10). 
                                        move = Random.Range(1, 11) <= 6 ?
                                            MoveList.Instance.GetRandomRank2Move() :
                                            MoveList.Instance.GetRandomRank3Move();
                                        break;
                                    case 2: // R2 
                                        move = MoveList.Instance.GetRandomRank2Move();
                                        break;
                                    case 3: // R3 
                                        move = MoveList.Instance.GetRandomRank3Move();
                                        break;
                                }
                                break;

                            case 3: // Phase 3 - If rank 1, replace with a rank 3. 
                                // Grabs the move rank, and replaces the move. 
                                switch (player.moves[moveIndex].Rank)
                                {
                                    default:
                                    case 1: // R1 and R3 
                                    case 3:
                                        move = MoveList.Instance.GetRandomRank3Move();
                                        break;
                                    case 2: // R2 
                                        move = MoveList.Instance.GetRandomRank2Move();
                                        break;
                                }
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

            // Loads the game over tutorial. 
            if (gameManager.useTutorial && !gameManager.tutorial.clearedGameOver)
                gameManager.tutorial.LoadGameOverTutorial();


            // Sets the variable to false.
            gameOver = false;

        }


        // // Update is called once per frame
        // void Update()
        // {
        // 
        // }

        
    }
}