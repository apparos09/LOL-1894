using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        // The definitions from the json file.
        private JSONNode defs;

        // Automatically shows the textbox when text is loaded.
        public bool showTextboxOnLoad;

        [Header("Tutorial Functions")]

        // The image of the speaker (helper bot).
        public Image speakerImage;

        // The diagram for showing how percentages work.
        public Image probPercentDiagram;

        // The probability fraction diagram.
        public Image probFractionDiagram;

        // The default text box position.
        public Vector3 textBoxDefaultPos;

        // Automatically sets the text box's default position.
        public bool autoSetTextBoxDefaultPos = true;

        // The position for a raised textbox.
        public Vector3 textBoxOffsetPos;

        // Automatically sets the position for an offset text box (puts it in the middle of the screen).
        public bool autoSetTextBoxOffsetPos = true;

        // The amount the textbox is shifted by to put it in the middle of the screen.
        protected const float TEXTBOX_SHIFT_Y = 85.0F;
        
        [Header("Tutorial Steps")]

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
            // These use the local position since that gives the text box's position in the Canvas.

            // Automatically sets the textbox position.
            if (autoSetTextBoxDefaultPos)
                textBoxDefaultPos = textBox.transform.localPosition;

            // Gets the offset position of the textbox. This needs to use local position to work.
            // For some reason this gets added twice at random, so I just manually set it now.
            if (autoSetTextBoxOffsetPos)
                textBoxOffsetPos = textBox.transform.localPosition + new Vector3(0, TEXTBOX_SHIFT_Y, 0);
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

        // Shows the speaker iamge.
        protected void ShowSpeakerImage()
        {
            speakerImage.gameObject.SetActive(true);
            probFractionDiagram.gameObject.SetActive(false);
            probPercentDiagram.gameObject.SetActive(false);
        }

        // Hides the speaker image.
        protected void HideSpeakerImage()
        {
            speakerImage.gameObject.SetActive(false);

            // It's unknown what the user wants to replace the image with...
            // So there is no other behaviour here.
        }

        // Shows the probability percent diagram.
        protected void ShowProbabilityPercentDiagram()
        {
            speakerImage.gameObject.SetActive(false);
            probFractionDiagram.gameObject.SetActive(false);
            probPercentDiagram.gameObject.SetActive(true);
        }

        // Hides the probability percent diagram.
        protected void HideProbabilityPercentDiagram()
        {
            speakerImage.gameObject.SetActive(true);
            probFractionDiagram.gameObject.SetActive(false);
            probPercentDiagram.gameObject.SetActive(false);
        }

        // Shows the probability fraction diagram.
        protected void ShowProbabilityFractionDiagram()
        {
            speakerImage.gameObject.SetActive(false);
            probFractionDiagram.gameObject.SetActive(true);
            probPercentDiagram.gameObject.SetActive(false);
        }

        // Hides the probability fraction diagram.
        protected void HideProbabilityFractionDiagram()
        {
            speakerImage.gameObject.SetActive(true);
            probFractionDiagram.gameObject.SetActive(false);
            probPercentDiagram.gameObject.SetActive(false);
        }


        // Moves the textbox up/down by the provided amount.
        protected void TranslateTextBoxY(float y)
        {
            textBox.transform.Translate(0, y, 0);
        }

        // Moves the textbox on the y-axis so that the score isn't covered for one page.
        protected void TranslateTextBoxUp()
        {
            TranslateTextBoxY(TEXTBOX_SHIFT_Y);
        }

        // Moves the textbox down so that it goes back to its original place.
        protected void TranslateTextBoxDown()
        {
            TranslateTextBoxY(-TEXTBOX_SHIFT_Y);
        }

        // Sets the text box to it's default position.
        public void SetTextBoxToDefaultPosition()
        {
            textBox.transform.localPosition = textBoxDefaultPos;
        }

        // Sets the text box to it's offset position.
        public void SetTextBoxToOffsetPosition()
        {
            textBox.transform.localPosition = textBoxOffsetPos;
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

            // The probability percentage page.
            Page probPercentPage;

            // The probability fraction page(s).
            Page probFractionPageStart;
            Page probFractionPageEnd;

            // The hud page for the intro, which needs to move the textbox so that the score is visible.
            Page hudPage;

            // Pages
            if (defs != null) // Translation
            {
                pages.Add(new Page(defs["trl_intro_00"], "trl_intro_00"));
                pages.Add(new Page(defs["trl_intro_01"], "trl_intro_01"));

                // Show/Hide the Probability Percent Diagram
                probPercentPage = new Page(defs["trl_intro_02"], "trl_intro_02");
                pages.Add(probPercentPage);

                // Show Probability Fraction Diagram
                probFractionPageStart = new Page(defs["trl_intro_03"], "trl_intro_03");
                pages.Add(probFractionPageStart);

                // Hide Probability Fraction Diagram
                probFractionPageEnd = new Page(defs["trl_intro_04"], "trl_intro_04");
                pages.Add(probFractionPageEnd);

                pages.Add(new Page(defs["trl_intro_05"], "trl_intro_05"));

                // Hud page requires moving the textbox, so save it.
                hudPage = new Page(defs["trl_intro_06"], "trl_intro_06");
                pages.Add(hudPage);

                pages.Add(new Page(defs["trl_intro_07"], "trl_intro_07"));
            }
            else // Default
            {
                pages.Add(new Page("Welcome to the battle simulator, Battle Bot! I'm Helper Bot, and I'll be teaching you battle strategies through this simulator! But first, you should know that these strategies revolve around the concept of probability."));
                pages.Add(new Page("Probability is a math subject where you assess the likelihood of an event. In decimal form, an event with a 0.00 chance will never happen, and an event with a 1.00 chance will always happen. The higher an event's chance, the more likely it is to happen."));
                
                // Probability Percentage Page
                probPercentPage = new Page("Probability can also be expressed in percentage form and fraction form. In percentage form, the same rules apply, but percentages are used instead of decimals, with 0% and 100% meaning 0.00 and 1.00 respectively.");
                pages.Add(probPercentPage);

                // Probability Fraction Page (Start)
                probFractionPageStart = new Page("In fraction form (x/y), the denominator (y) represents the size of the whole group, while the numerator (x) represents the size of a portion of the group.");
                pages.Add(probFractionPageStart);

                // Probability Fraction Page (End)
                probFractionPageEnd = new Page("The larger the portion (x) is in reference to the group size (y), the more likely the related event is. When (x) is equal to (y), it is the equivalent of 1.00 in decimal form. When (x) is equal to 0, it equates to 0.00 in decimal form.");
                pages.Add(probFractionPageEnd);

                pages.Add(new Page("With all that explained, welcome to the overworld! You need to beat the boss to finish the simulation, but they're behind that scary locked door! Looks like you'll have to go through a different door for now…"));

                // Hud Page
                // Hud page requires moving the textbox, so save it.
                hudPage = new Page("To the left is your health, to the right is your energy, at the bottom is your score, and at the top is the current round number, all of which I'll elaborate on later. There are also various buttons at the top, so check those out at your leisure.");
                pages.Add(hudPage);
                
                pages.Add(new Page("With all that covered, please select an open door to start your first battle!"));

            }


            // Show/hide the probability fraction diagram.
            probPercentPage.OnPageOpenedAddCallback(ShowProbabilityPercentDiagram);
            probPercentPage.OnPageClosedAddCallback(HideProbabilityPercentDiagram);

            // Show the probability diagram when the first fraction page is opened, and hide it when the last fraction page is closed.
            // The show and hide functions need to be called both times to make sure they show properly when...
            // Going back to a prior page.
            probFractionPageStart.OnPageOpenedAddCallback(ShowProbabilityFractionDiagram);
            probFractionPageStart.OnPageClosedAddCallback(HideProbabilityFractionDiagram);

            probFractionPageEnd.OnPageOpenedAddCallback(ShowProbabilityFractionDiagram);
            probFractionPageEnd.OnPageClosedAddCallback(HideProbabilityFractionDiagram);

            // Move the text box when the page opens so that the score is visible, and move it back when the page is closed.
            // This now moves to a fixed position since using Translate() caused problems.
            hudPage.OnPageOpenedAddCallback(SetTextBoxToOffsetPosition);
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
                pages.Add(new Page("Welcome to your first battle, Battle Bot! To win the battle, you must bring your opponent's health down to 0. But if the opponent brings your health down to 0, you'll get a game over! To attack your opponent, you must use your battle moves!"));
                pages.Add(new Page("You can have up to 4 battle moves at a time, which together with the run and charge moves makes for a total of 6 battle options! To learn a new move, you must accept a move offer, which is given out every time you clear a room."));
                pages.Add(new Page("The charge move is used to charge your energy, which is needed to perform your battle moves, while the run move has you attempt to flee from the battle."));
                pages.Add(new Page("If you flee from battle, your health and energy levels stay the same. Your opponent keeps their health and energy levels as well, but they will get restored if you clear too many rooms without defeating them."));
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
                pages.Add(new Page("A move's rank determines how advanced it is, a move's power determines how much damage it does, a move's accuracy determines how likely it is to hit its target, and a move's energy usage determines how much energy is needed to use said move."));
                pages.Add(new Page("If a move component isn't listed, then said move has unique behavior concerning said attribute. On that point, moves can have additional effects as well, which are always explained in their descriptions."));
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
                pages.Add(new Page("You have encountered critical damage! Critical damage multiplies the power of a move when it successfully hits its target. Unless a move's description states otherwise, every directly damaging move has the same critical damage chance."));
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
                pages.Add(new Page("You just encountered recoil damage! Some moves deal damage to the user when performed successfully. A move's description will state if it does recoil damage."));
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
                pages.Add(new Page("You have encountered a stat change! A battler's stats can be temporarily adjusted by certain moves, and by certain game events. These stat changes reset when the battle ends."));
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
            }
            else // Default
            {
                pages.Add(new Page("Look! The other doors have closed, but the boss door is still locked! I guess you'll have to clear all the other rooms before the boss accepts your challenge. Both you and your foes will get stronger as the game progresses, so good luck!"));
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
                pages.Add(new Page("This is a treasure room! If you take the treasure, you'll get a free level up, and a chance to learn 1 of 3 moves! If you don't take the treasure, the room will remain open, so you can always come back later."));
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
                pages.Add(new Page("Heads up! You're about to be asked a question. From this point forward, you'll get a question every time you clear a room."));
                pages.Add(new Page("If you answer correctly, you'll get a bonus that'll help you in the next room. But if you answer incorrectly, you'll get a penalty for the next room. Good luck!"));

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
            }
            else // Default
            {
                pages.Add(new Page("The game phase has changed! When the phase changes, you get permanent stat boosts, while the remaining enemies regain their health and energy. Enemies can also evolve from a phase change, which makes them stronger and gives them new moves. Good luck!"));

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
                pages.Add(new Page("Look! It's the boss of the simulation! Once you defeat them, the battle simulator will be complete! Good luck!"));
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
            }
            else // Default
            {
                pages.Add(new Page("You got a game over! Your health and energy have been restored, but some of your moves have changed, and the doors have switched places. The enemies have had their health and energy restored too, but their moves haven't changed. Good luck!"));
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