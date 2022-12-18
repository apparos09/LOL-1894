using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The namespace for the tutorial.
namespace RM_BBTS
{
    // The tutorial script.
    public class Tutorial : MonoBehaviour
    {
        // The manager for the game.
        public GameplayManager gameManager;

        // The textbox for the tutorial.
        public TextBox textBox;

        // Automatically shows the textbox when text is loaded.
        public bool showTextboxOnLoad;

        [Header("Tutorial Steps")]

        // The definitions from the json file.
        JSONNode defs;

        // Set to 'true' when the intro text has been loaded up.
        public bool clearedIntro = false;

        // Cleared the battle tutorial.
        public bool clearedBattle = false;

        // Sub-battle tutorials.
        public bool clearedFirstMove = false;
        public bool clearedCritical = false;
        public bool clearedRecoil = false;
        public bool clearedStatChange = false;
        public bool clearedBurn = false;
        public bool clearedParalysis = false;
        public bool clearedFirstBattleDeath = false;

        // Cleared the overworld tutorial.
        public bool clearedOverworld = false;

        // Cleared the treasure tutorial.
        public bool clearedTreasure = false;

        // Cleared the boss tutorial.
        public bool clearedBoss = false;
        
        // Set to 'true' whe the game over tutorial has been loaded up.
        public bool clearedGameOver = false;

        // Awake is called when the script is being loaded
        private void Awake()
        {
            textBox.closeOnEnd = true;

            // Adds the callbacks.
            textBox.OnTextBoxOpenedAddCallback(OnTutorialStart);
            textBox.OnTextBoxClosedAddCallback(OnTutorialEnd);

            // Loads the language definitions.
            defs = SharedState.LanguageDefs;
        }

        // // Start is called before the first frame update
        // void Start()
        // {
        //     
        // }

        // Opens the textbox.
        public void OpenTextbox()
        {
            textBox.Open();
        }

        // Closes the textbox.
        public void CloseTextbox()
        {
            textBox.Close();
        }

        // Returns 'true' if the textbox is visible.
        public bool TextBoxIsVisible()
        {
            return textBox.IsVisible();
        }

        // Loads the tutorial
        private void LoadTutorial(ref List<Page> pages)
        {
            // Clears out the textbox and adds in the new pages.
            textBox.ClearPages();
            textBox.pages = pages;
            textBox.SetPage(0);


            // Opens the textbox.
            if (showTextboxOnLoad)
                textBox.Open();
        }

        // Loads the intro tutorial.
        public void LoadIntroTutorial()
        {
            // If the intro tutorial is loaded from a saved game the doors will not be locked.
            // This would make the message inaccruate; however, this scenario should never naturally happen.
            // The user can only save after the portion where the tutorial intro has been registered as being cleared.
            // As such, there shouldn't be a need to fix this inaccuracy. 
            // If it comes down to it, you could lock down the other buttons so that the player can't save during the tutorial.

            // Page Object
            List<Page> pages = new List<Page>();

            // Pages
            if(defs != null) // Translation
            {
                pages.Add(new Page(defs["trl_intro_00"], "trl_intro_00"));
                pages.Add(new Page(defs["trl_intro_01"], "trl_intro_01"));
                pages.Add(new Page(defs["trl_intro_02"], "trl_intro_02"));
                pages.Add(new Page(defs["trl_intro_03"], "trl_intro_03"));
                pages.Add(new Page(defs["trl_intro_04"], "trl_intro_04"));
                pages.Add(new Page(defs["trl_intro_05"], "trl_intro_05"));
                pages.Add(new Page(defs["trl_intro_06"], "trl_intro_06"));
                pages.Add(new Page(defs["trl_intro_07"], "trl_intro_07"));
            }
            else // Default
            {
                pages.Add(new Page("<Before we begin, here’s a quick tip. Pressing the forward button while the text is scrolling will automatically show the rest of the text for the current page.>"));
                pages.Add(new Page("<Once all the page text is displayed, the forward button will move onto the next page, or close the textbox if there are no more pages. Now, with all that out of the way…>"));
                pages.Add(new Page("<Welcome to the battle simulator, Battle Bot! Your job is to beat the boss of this simulation, who is behind the red door. But first, there are some things you should know about the simulation.>"));
                pages.Add(new Page("<This is the overworld, which is where you pick a door to go through. Once you win your first battle, all the other doors will be unlocked.>"));
                pages.Add(new Page("<To the left is your health, and to the right is your energy. These will be explained further once you get into a battle.>"));
                pages.Add(new Page("<At the bottom is your current score, which increases by a variable amount for every room that you clear.>"));
                pages.Add(new Page("<And at the top are various options that you can select, some of which may be disabled depending on the state of the game.>"));
                pages.Add(new Page("<With all that explained, please select one of the open doors to attempt your first battle!>"));

            }

            // Loads the pages.
            LoadTutorial(ref pages);

            // Intro tutorial loaded.
            clearedIntro = true;

        }

        // Loads the battle tutorial.
        public void LoadBattleTutorial()
        {
            // Page Object
            List<Page> pages = new List<Page>();

            // Pages
            if(defs != null) // Translation
            {
                pages.Add(new Page(defs["trl_battle_00"], "trl_battle_00"));
                pages.Add(new Page(defs["trl_battle_01"], "trl_battle_01"));
                pages.Add(new Page(defs["trl_battle_02"], "trl_battle_02"));
                pages.Add(new Page(defs["trl_battle_03"], "trl_battle_03"));
                pages.Add(new Page(defs["trl_battle_04"], "trl_battle_04"));
                pages.Add(new Page(defs["trl_battle_05"], "trl_battle_05"));
                pages.Add(new Page(defs["trl_battle_06"], "trl_battle_06"));
                pages.Add(new Page(defs["trl_battle_07"], "trl_battle_07"));
                pages.Add(new Page(defs["trl_battle_08"], "trl_battle_08"));
                pages.Add(new Page(defs["trl_battle_09"], "trl_battle_09"));
            }    
            else // Default
            {
                pages.Add(new Page("<Welcome to your first battle, Battle Bot!>"));
                pages.Add(new Page("<To successfully win a battle, you must bring your opponent’s health down to 0. If your health hits 0, you will lose the battle and get a game over.>"));
                pages.Add(new Page("<You can hold up to 4 regular moves at a time, which together with the charge and run moves makes for a total of 6 battle options max.>"));
                pages.Add(new Page("<Selecting the run move has you attempt to flee from the battle, which always has a 50% chance of success. If you succeed, you will return to the overworld.>"));
                pages.Add(new Page("<Enemies retain their health and energy levels if you flee, but they will be completely restored if too many rooms are completed without finishing their battles. More on this later.>"));
                pages.Add(new Page("<Since this is the tutorial battle, the run option has been disabled. After this battle ends, the run option will always be available, even when battling the boss of the simulation.>"));
                pages.Add(new Page("<The charge move is used to charge your energy, which it restores by a fixed amount. Regular moves take energy to perform, so they cannot be used without enough energy.>"));
                pages.Add(new Page("<Speaking of which, regular moves all have different characteristics that determine how well they perform in battle. To view your full move information, check the stats window.>"));
                pages.Add(new Page("<As for the move buttons, they show the move names, and the current accuracy of every move. If you don’t have enough energy to perform a move, said move’s button will be disabled.>"));
                pages.Add(new Page("<That’s all for now, so on with the battle!>"));

            }

            // Loads the pages.
            LoadTutorial(ref pages);

            clearedBattle = true;
        }

        // Loads the first move performed tutorial.
        public void LoadFirstMoveTutorial()
        {
            // Page Object
            List<Page> pages = new List<Page>();

            // Pages
            if (defs != null) // Translation
            {
                pages.Add(new Page(defs["trl_firstMove_00"], "trl_firstMove_00"));
                pages.Add(new Page(defs["trl_firstMove_01"], "trl_firstMove_01"));
                pages.Add(new Page(defs["trl_firstMove_02"], "trl_firstMove_02"));
                pages.Add(new Page(defs["trl_firstMove_03"], "trl_firstMove_03"));
                pages.Add(new Page(defs["trl_firstMove_04"], "trl_firstMove_04"));
                pages.Add(new Page(defs["trl_firstMove_05"], "trl_firstMove_05"));
                pages.Add(new Page(defs["trl_firstMove_06"], "trl_firstMove_06"));
            }
            else // Default
            {
                pages.Add(new Page("<You just completed your first turn of battle! Now that you’ve experienced how the battles work, some more move elements will be explained.>"));
                pages.Add(new Page("<A move’s rank determines how advanced said move is, which ranges from 1 to 3. The higher the number, the higher the rank. The rank has no effect in battle.>"));
                pages.Add(new Page("<A move’s power determines how strong said move is. If a move doesn’t have a power amount listed, then it either does no damage, or determines damage in a different way than usual.>"));
                pages.Add(new Page("<A move’s accuracy determines how likely it is to hit its target. A move with no accuracy listed either always succeeds or has its success rate determined by a different factor than usual.>"));
                pages.Add(new Page("<A move’s energy level determines what percentage of the battler’s energy is used to perform said move. If a move’s energy usage isn’t listed, then it either uses no energy, or its energy usage varies.>"));
                pages.Add(new Page("<Make sure to open the stats window if you ever want to see any of the information mentioned for one of your moves.>"));
                pages.Add(new Page("<That’s all for the move explanations, so on with the battle!>"));
            }

            // Loads the pages.
            LoadTutorial(ref pages);

            clearedFirstMove = true;
        }

        // Loads the critical damage tutorial.
        public void LoadCriticalDamageTutorial()
        {
            // Page Object
            List<Page> pages = new List<Page>();

            // Pages
            if (defs != null) // Translation
            {
                pages.Add(new Page(defs["trl_critical_00"], "trl_critical_00"));
                pages.Add(new Page(defs["trl_critical_01"], "trl_critical_01"));
                pages.Add(new Page(defs["trl_critical_02"], "trl_critical_02"));
            }
            else // Default
            {
                pages.Add(new Page("<You just encountered critical damage!>"));
                pages.Add(new Page("<Critical damage is a damage bonus that has a chance of being applied for every directly damaging move.>"));
                pages.Add(new Page("<Unless the move description states otherwise, every directly damaging move has the same chance of getting the critical bonus.>"));
            }

            // Loads the pages.
            LoadTutorial(ref pages);

            clearedCritical = true;
        }

        // Loads the recoil damage tutorial.
        public void LoadRecoilDamageTutorial()
        {
            // Page Object
            List<Page> pages = new List<Page>();

            // Pages
            if (defs != null) // Translation
            {
                pages.Add(new Page(defs["trl_recoil_01"], "trl_recoil_01"));
                pages.Add(new Page(defs["trl_recoil_00"], "trl_recoil_00"));
                pages.Add(new Page(defs["trl_recoil_02"], "trl_recoil_02"));
            }
            else // Default
            {
                pages.Add(new Page("<You just encountered recoil damage!>"));
                pages.Add(new Page("<Recoil damage is damage done to the user for successfully performing their move.>"));
                pages.Add(new Page("<A move’s description will state if it does recoil damage.>"));
            }

            // Loads the pages.
            LoadTutorial(ref pages);

            clearedRecoil = true;
        }

        // Loads the stat change tutorial.
        public void LoadStatChangeTutorial()
        {
            // Page Object
            List<Page> pages = new List<Page>();

            // Pages
            if (defs != null) // Translation
            {
                pages.Add(new Page(defs["trl_statChange_00"], "trl_statChange_00"));
                pages.Add(new Page(defs["trl_statChange_01"], "trl_statChange_01"));
                pages.Add(new Page(defs["trl_statChange_02"], "trl_statChange_02"));
            }
            else // Default
            {
                pages.Add(new Page("<You have encountered a stat change!>"));
                pages.Add(new Page("<Some moves can increase or decrease a battler’s stats, with said changes being in effect for the rest of the battle unless stated otherwise.>"));
                pages.Add(new Page("<Completing or fleeing from a battle will remove all stat changes for both you and your opponent.>"));
            }

            // TODO: STAT CHANGE

            // Loads the pages.
            LoadTutorial(ref pages);

            clearedStatChange = true;
        }

        // Loads the burn status tutorial.
        public void LoadBurnTutorial()
        {
            // Page Object
            List<Page> pages = new List<Page>();

            // Pages
            if (defs != null) // Translation
            {
                pages.Add(new Page(defs["trl_burn_00"], "trl_burn_00"));
                pages.Add(new Page(defs["trl_burn_01"], "trl_burn_01"));
                pages.Add(new Page(defs["trl_burn_02"], "trl_burn_02"));
            }
            else // Default
            {
                pages.Add(new Page("<You have encountered burn status!>"));
                pages.Add(new Page("<Damage will be taken each turn when inflicted with burn status.>"));
                pages.Add(new Page("<Burn status wears off if you win the battle, or if you run away from the battle. This goes for you and your opponent.>"));
            }

            // Loads the pages.
            LoadTutorial(ref pages);

            clearedBurn = true;
        }

        // Loads the paralysis status tutorial.
        public void LoadParalysisTutorial()
        {
            // Page Object
            List<Page> pages = new List<Page>();

            // Pages
            if (defs != null) // Translation
            {
                pages.Add(new Page(defs["trl_paralysis_00"], "trl_paralysis_00"));
                pages.Add(new Page(defs["trl_paralysis_01"], "trl_paralysis_01"));
                pages.Add(new Page(defs["trl_paralysis_02"], "trl_paralysis_02"));
            }
            else // Default
            {
                pages.Add(new Page("<You have encountered the paralysis status!>"));
                pages.Add(new Page("<When inflicted with paralysis, a battler will move slower and has a chance of skipping a turn.>"));
                pages.Add(new Page("<Paralysis status wears off if you win the battle, or if you run away from the battle. This goes for you and your opponent.>"));
            }

            // Loads the pages.
            LoadTutorial(ref pages);

            clearedParalysis = true;
        }

        // Loads the first battle death tutorial.
        public void LoadFirstBattleDeathTutorial()
        {
            // Page Object
            List<Page> pages = new List<Page>();

            // Pages
            if (defs != null) // Translation
            {
                pages.Add(new Page(defs["trl_firstBattleDeath_00"], "trl_firstBattleDeath_00"));
                pages.Add(new Page(defs["trl_firstBattleDeath_01"], "trl_firstBattleDeath_01"));
            }
            else // Default
            {
                pages.Add(new Page("<Since this is your first battle, the simulation has completely restored your health and energy.>"));
                pages.Add(new Page("<Keep in mind that this is a one-time thing. If you lose all your health after this battle is over, you will get a game over.>"));
            }

            // Loads the pages.
            LoadTutorial(ref pages);

            clearedFirstBattleDeath = true;
        }

        // Loads the overworld tutorial, whichs hows after you clear a room.
        public void LoadOverworldTutorial()
        {
            // Page Object
            List<Page> pages = new List<Page>();

            // Pages
            if (defs != null) // Translation
            {
                pages.Add(new Page(defs["trl_overworld_00"], "trl_overworld_00"));
                pages.Add(new Page(defs["trl_overworld_01"], "trl_overworld_01"));
                pages.Add(new Page(defs["trl_overworld_02"], "trl_overworld_02"));
                pages.Add(new Page(defs["trl_overworld_03"], "trl_overworld_03"));
                pages.Add(new Page(defs["trl_overworld_04"], "trl_overworld_04"));
                pages.Add(new Page(defs["trl_overworld_05"], "trl_overworld_05"));
                pages.Add(new Page(defs["trl_overworld_06"], "trl_overworld_06"));
                pages.Add(new Page(defs["trl_overworld_07"], "trl_overworld_07"));
                pages.Add(new Page(defs["trl_overworld_08"], "trl_overworld_08"));
                pages.Add(new Page(defs["trl_overworld_09"], "trl_overworld_09"));
            }
            else // Default
            {
                pages.Add(new Page("<Now that you’ve tried out a battle, all the rooms are open, including the boss room.>"));
                pages.Add(new Page("<As mentioned before, the game ends when you beat the boss, whom you can challenge at any time.>"));
                pages.Add(new Page("<You’ll get stronger and learn better moves the more rooms that you clear, so how you approach this task is up to you.>"));
                pages.Add(new Page("<As for the enemies, they will get stronger as the game progresses, and some can evolve into different forms. If an enemy can evolve, it will happen when a phase change occurs.>"));
                pages.Add(new Page("<A phase change happens when you complete enough rooms, with there being three phases in total. A phase change is signified by a change in the game background, and in the game music.>"));
                pages.Add(new Page("<Enemies regain all their health and energy when a phase change occurs, even if they don’t evolve. Each phase has an equal number of doors, so keep that in mind if you leave any battles unfinished.>"));
                pages.Add(new Page("<Also remember that running away from battle is always an option, even against the final boss.>"));
                pages.Add(new Page("<Going back to phases, you get permanent stat boosts when the phase changes, but your current health and energy levels stay the same. The stats screen reflects these changes as usual.>"));
                pages.Add(new Page("<Since you're currently in the first phase, the phase stat boosts will be applied a total of two separate times.>"));
                pages.Add(new Page("<That’s all for now, so on with the game!>"));
            }
            
            // Loads the pages.
            LoadTutorial(ref pages);

            clearedOverworld = true;
        }

        // Loads the treasure tutorial.
        public void LoadTreasureTutorial()
        {
            // Page Object
            List<Page> pages = new List<Page>();

            // Pages
            if(defs != null) // Translation
            {
                pages.Add(new Page(defs["trl_treasure_00"], "trl_treasure_00"));
                pages.Add(new Page(defs["trl_treasure_01"], "trl_treasure_01"));
                pages.Add(new Page(defs["trl_treasure_02"], "trl_treasure_02"));
            }
            else // Default
            {
                pages.Add(new Page("<This is a treasure room!>"));
                pages.Add(new Page("<If you take the treasure, you get a free level up, some heath restored, some energy restored, and an opportunity to learn a new move.>"));
                pages.Add(new Page("<If you don’t take the treasure, the room will remain open, so you can always pick up the treasure later.>"));

            }

            // Loads the pages.
            LoadTutorial(ref pages);

            clearedTreasure = true;
        }

        // Loads for the boss tutorial.
        public void LoadBossTutorial()
        {
            // Page Object
            List<Page> pages = new List<Page>();

            // Pages
            if(defs != null) // Translation
            {
                pages.Add(new Page(defs["trl_boss_00"], "trl_boss_00"));
                pages.Add(new Page(defs["trl_boss_01"], "trl_boss_01"));
            }
            else // Default
            {
                pages.Add(new Page("<Welcome to the boss room!>"));
                pages.Add(new Page("<All you need to do is beat the boss to win the game! Good luck!>"));
            }

            // Loads the pages.
            LoadTutorial(ref pages);

            clearedBoss = true;
        }

        // Loads the game over tutorial.
        public void LoadGameOverTutorial()
        {
            // Page Object
            List<Page> pages = new List<Page>();

            // Pages
            if (defs != null) // Translation
            {
                pages.Add(new Page(defs["trl_gameOver_00"], "trl_gameOver_00"));
                pages.Add(new Page(defs["trl_gameOver_01"], "trl_gameOver_01"));
                pages.Add(new Page(defs["trl_gameOver_02"], "trl_gameOver_02"));
                pages.Add(new Page(defs["trl_gameOver_03"], "trl_gameOver_03"));
                pages.Add(new Page(defs["trl_gameOver_04"], "trl_gameOver_04"));
            }
            else // Default
            {
                pages.Add(new Page("<You lost the battle and got a game over, so there’s some things that you should know.>"));
                pages.Add(new Page("<Once a room is completed, it will stay completed no matter what. So, you don’t have to redo any rooms you already cleared. The open doors have had their positions randomized though.>"));
                pages.Add(new Page("<As for the enemies, they have all had their health and energy fully restored. However, the enemies themselves and their moves have not changed.>"));
                pages.Add(new Page("<And as for you, your health and energy have been completely restored, but some of your moves have been randomized. Make sure to check the stats window to see your new moves.>"));
                pages.Add(new Page("<That's all for now, so good luck!>"));
            }

            // Loads the pages.
            LoadTutorial(ref pages);

            clearedGameOver = true;
        }

        // Called when the tutorial starts, which is when the textbox is opened.
        public void OnTutorialStart()
        {
            // gameManager.mouseTouchInput.gameObject.SetActive(false);
            gameManager.OnTutorialStart();
        }

        // Called when the tutorial starts, which is when the textbox is closed.
        public void OnTutorialEnd()
        {
            // gameManager.mouseTouchInput.gameObject.SetActive(true);
            gameManager.OnTutorialEnd();
        }

    }
}