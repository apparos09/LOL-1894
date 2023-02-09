using SimpleJSON;
using System;
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

        // The default text box position.
        public Vector3 textBoxDefaultPos;

        // Automatically sets the text box's default position.
        public bool autoSetTextBoxDefaultPos = true;

        // The amount the textbox is shifted by to put it in the middle of the screen.
        protected const float TEXTBOX_SHIFT_Y = 100.0F;

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

        // Cleared the question tutorial.
        public bool clearedQuestion = false;

        // Cleared the phase tutorial.
        public bool clearedPhase = false;

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

        // Start is called before the first frame update
        void Start()
        {
            // Automatically sets the textbox position.
            if (autoSetTextBoxDefaultPos)
                textBoxDefaultPos = textBox.transform.position;
        }

        // Opens the textbox.
        public void OpenTextBox()
        {
            textBox.Open();
        }

        // Closes the textbox.
        public void CloseTextBox()
        {
            textBox.Close();
        }

        // Returns 'true' if the textbox is visible.
        public bool TextBoxIsVisible()
        {
            return textBox.IsVisible();
        }

        // Moves the textbox up so that the score isn't covered for one page.
        protected void TranslateTextBoxUp()
        {
            textBox.transform.Translate(0, TEXTBOX_SHIFT_Y, 0);
        }

        // Moves the textbox down so that it goes back to its original place.
        protected void TranslateTextBoxDown()
        {
            textBox.transform.Translate(0, -TEXTBOX_SHIFT_Y, 0);
        }

        // Sets the text box to it's default position.
        public void SetTextBoxToDefaultPosition()
        {
            textBox.transform.position = textBoxDefaultPos;
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

            // THe hud page for the intro, which needs to move the textbox so that the score is visible.
            Page hudPage;

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

                // Hud page has unique behaviour, so save it.
                // pages.Add(new Page(defs["trl_intro_07"], "trl_intro_07"));
                hudPage = new Page(defs["trl_intro_07"], "trl_intro_07");
                pages.Add(hudPage);

                pages.Add(new Page(defs["trl_intro_08"], "trl_intro_08"));
            }
            else // Default
            {
                pages.Add(new Page("Before we begin, I have a quick tip for you. Pressing the forward button while the text is scrolling will automatically show the rest of the text for the current page."));
                pages.Add(new Page("If there is no more text, you’ll move onto the next page, or close the textbox if there are no pages left. With all that out of the way…"));
                pages.Add(new Page("Welcome to the battle simulator, Battle Bot! I’m Helper Bot, and I’ll be teaching you battle strategies through this simulator! But first, you should know that these strategies revolve around the concept of probability."));
                pages.Add(new Page("Probability is a math subject where you assess the likelihood of an event. In decimal form, an event with a 0.00 chance will never happen, and an event with a 1.00 chance will always happen. The higher an event’s chance, the more likely it is to happen."));
                pages.Add(new Page("Probability can also be in expressed in percentage form or fraction form. In percentage form, the same rules apply, but percentages are used instead of decimals, with 0% and 100% meaning 0.00 and 1.00 respectively."));
                pages.Add(new Page("In fraction form (x/y), the larger (x) is compared to (y), the more likely the event is. (y) equates to 1.00 for the fraction, with (x) acting as the chance value. The event chance is 0.00 when (x) is equal to 0 and is 1.00 when (x) is equal to (y)."));
                pages.Add(new Page("With all that explained, welcome to the overworld! You need to beat the boss to finish the simulation, but they’re behind that scary locked door! Looks like you’ll have to go through a different door for now…"));

                // Hud page has unique behaviour, so save it.
                // pages.Add(new Page("To the left is your health, to the right is your energy, at the bottom is your score, and at the top is the current round number, all of which I’ll elaborate on later. There are also various buttons at the top, so check those out at your leisure."));
                hudPage = new Page("To the left is your health, to the right is your energy, at the bottom is your score, and at the top is the current round number, all of which I’ll elaborate on later. There are also various buttons at the top, so check those out at your leisure.");
                pages.Add(hudPage);
                
                pages.Add(new Page("With all that covered, please select an open door to start your first battle!"));

            }

            // Move the text box so thatthe score is visible.
            hudPage.OnPageOpenedAddCallback(TranslateTextBoxUp);

            // This caused problems, so now the textbox is just set to it's default position instead of translated back down.
            // hudPage.OnPageClosedAddCallback(TranslateTextBoxDown);
            hudPage.OnPageClosedAddCallback(SetTextBoxToDefaultPosition);

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
            }    
            else // Default
            {
                pages.Add(new Page("Welcome to your first battle, Battle Bot! To win the battle, you must bring your opponent’s health down to 0. But if the opponent brings your health down to 0, you’ll get a game over! To attack your opponent, you must use your battle moves!"));
                pages.Add(new Page("You can have up to 4 battle moves at a time, which together with the run and charge moves makes for a total of 6 battle options! To learn a new move, you must accept a move offer, which is given out every time you clear a room."));
                pages.Add(new Page("The charge and run moves are special options that cannot be replaced. The charge move is used to charge your energy, which you need to perform your battle moves, while the run move has you attempt to flee from the battle."));
                pages.Add(new Page("If you run from battle, your health and energy levels stay the same. Your opponent keeps their health and energy levels as well, but they will get restored if you clear too many rooms without defeating them."));
                pages.Add(new Page("With all that covered, time to try out one of your moves! If you want to know what a move does, click the stats button!"));
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
            }
            else // Default
            {
                pages.Add(new Page("You just finished your first turn of battle! As you can see, moves have different characteristics that determine how they perform in battle. Every move has at least 4 components, which are as follows: rank, power, accuracy, and energy usage."));
                pages.Add(new Page("A move’s rank determines how advanced it is, a move’s power determines how much damage it does, a move’s accuracy determines how likely it is to hit its target, and a move’s energy usage determines how much energy is needed to use said move."));
                pages.Add(new Page("If a move does not list one of the four components, then it has unique behaviour that concerns said attribute. On that point, moves can have additional effects as well, which are always explained in their descriptions."));
                pages.Add(new Page("Make sure to check the stats window and info window if you ever need more information on your moves! With all that explained, on with the battle!"));
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
            }
            else // Default
            {
                pages.Add(new Page("You have encountered critical damage! Critical damage multiplies the power of a move when it successfully hits its target. Unless a move’s description states otherwise, every directly damaging move has the same critical damage chance."));
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
                pages.Add(new Page(defs["trl_recoil_00"], "trl_recoil_00"));
            }
            else // Default
            {
                pages.Add(new Page("You just encountered recoil damage! Some moves deal damage to the user when performed successfully. A move’s description will state if it does recoil damage."));
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
            }
            else // Default
            {
                pages.Add(new Page("You have encountered a stat change! A battler’s stats can be temporarily adjusted by certain moves, and by certain game events. These stat changes reset when the battle ends."));
            }

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
            }
            else // Default
            {
                pages.Add(new Page("You have encountered burn status! A battler inflicted with burn status will take damage every turn. Burn status wears off when the battle ends, for both you and your opponent."));
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
            }
            else // Default
            {
                pages.Add(new Page("You have encountered paralysis status! A battler inflicted with paralysis will move slower and has a chance of missing their turn. Once the battle ends, both you and your opponent are cured of paralysis."));
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
            }
            else // Default
            {
                pages.Add(new Page("Since this is your first battle, the simulation has completely restored your health and energy! After this battle is over, you will get a game over if you lose all your health."));
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
            }
            else // Default
            {
                pages.Add(new Page("Look! The other doors have opened! But the boss door is still locked… I guess you’ll have to clear out all the other rooms before the boss accepts your challenge. But that shouldn’t be a problem."));
                pages.Add(new Page("As the simulation progresses, you’ll grow stronger, and learn even better moves! Your foes will probably get stronger too, but I’m sure you can handle it! And remember, if you don’t understand something, check out the stats and info windows! Good luck!"));
            }
            
            // Loads the pages.
            LoadTutorial(ref pages);

            clearedOverworld = true;

            // TODO: don't ask questions until the overworld tutorial is given.
            // gameManager.overworld.askQuestions = true;
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
            }
            else // Default
            {
                pages.Add(new Page("This is a treasure room! If you take the treasure, you’ll get a free level up, and a chance to learn 1 of 3 moves! If you don’t take the treasure, the room will remain open, so you can always come back later."));
            }

            // Loads the pages.
            LoadTutorial(ref pages);

            clearedTreasure = true;
        }

        // Loads the question tutorial.
        public void LoadQuestionTutorial()
        {
            // Page Object
            List<Page> pages = new List<Page>();

            // Pages
            if (defs != null) // Translation
            {
                pages.Add(new Page(defs["trl_question_00"], "trl_question_00"));
                pages.Add(new Page(defs["trl_question_01"], "trl_question_01"));
            }
            else // Default
            {
                pages.Add(new Page("Heads up! You’re about to be asked a question. From this point forward, you’ll get a question every time you clear a room."));
                pages.Add(new Page("If you answer correctly, you’ll get a bonus that’ll help you in the next room. But if you answer wrong, you’ll get a penalty for the next room. Good luck!"));

            }

            // Loads the pages.
            LoadTutorial(ref pages);

            clearedQuestion = true;
        }

        // Loads the phase tutorial.
        public void LoadPhaseTutorial()
        {
            // Page Object
            List<Page> pages = new List<Page>();

            // Pages
            if (defs != null) // Translation
            {
                pages.Add(new Page(defs["trl_phase_00"], "trl_phase_00"));
                pages.Add(new Page(defs["trl_phase_01"], "trl_phase_01"));
            }
            else // Default
            {
                pages.Add(new Page("It looks like you’ve entered a new phase of the simulation! A phase change seems to happen every time you complete a certain number of rooms… Interesting!"));
                pages.Add(new Page("Hmm… It appears that a phase change gives you a permanent stat boost! Cool! But it also appears that foes can evolve and learn new moves from a phase change, which is less cool… Well, I’m sure you can handle it! Good luck!"));

            }

            // Loads the pages.
            LoadTutorial(ref pages);

            clearedPhase = true;
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
            }
            else // Default
            {
                pages.Add(new Page("Look! It’s the boss of the simulation! Once you defeat them, the battle simulator will be complete! Good luck!"));
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
            }
            else // Default
            {
                pages.Add(new Page("You lost the battle and got a game over! But don’t worry, you can try again! Hmm… It appears that your health and energy have been completely restored, but that some of your battle moves are now different! And on another note…"));
                pages.Add(new Page("The rooms you cleared are still locked, so I don’t think you have to redo them. I’m guessing the remaining enemies got their health and energy back though… Anyway, this is just a small setback. Remember, you’ve got this! Good luck!"));
            }

            // Loads the pages.
            LoadTutorial(ref pages);

            clearedGameOver = true;
        }

        // Reads the current page of the tutorial box using TTS. This does not check if TTS is enabled.
        public void SpeakCurrentPage()
        {
            textBox.CurrentPage.SpeakPage();
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