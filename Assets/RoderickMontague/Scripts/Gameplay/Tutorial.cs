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

        // Set to 'true' when the intro text has been loaded up.
        public bool clearedIntro = false;

        // Cleared the battle tutorial.
        public bool clearedBattle = false;

        // Cleared the treasure tutorial.
        public bool clearedTreasure = false;

        // Cleared the overworld tutorial.
        public bool clearedOverworld = false;

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
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Loads the tutorial
        private void LoadTutorial(ref List<Page> pages)
        {
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
            // Page Object
            List<Page> pages = new List<Page>();

            // Pages
            pages.Add(new Page("Welcome to the simulation game, Battle Bot!"));
            pages.Add(new Page("Your job is to beat the boss of the simulation."));
            pages.Add(new Page("But first, I'd recommend you do some smaller battles to level-up and get new moves."));
            pages.Add(new Page("To the left is your health, which causes a game over if it hits 0."));
            pages.Add(new Page("To the right is your energy, which is needed to perform moves."));
            pages.Add(new Page("That will be explained more when you get into battle."));
            pages.Add(new Page("Speaking of which, click or touch a door to enter it."));
            pages.Add(new Page("The rest of the rooms will be unlocked once you clear a battle."));

            // Loads the pages.
            LoadTutorial(ref pages);

            clearedIntro = true;
        }

        // Loads the battle tutorial.
        public void LoadBattleTutorial()
        {
            // Page Object
            List<Page> pages = new List<Page>();

            // Pages
            pages.Add(new Page("Welcome to the battle phase of the game!"));
            pages.Add(new Page("Your job is to make your opponent's health hit 0."));
            pages.Add(new Page("If your health hits 0 then you will get a game over."));
            pages.Add(new Page("To attack your opponent, select one of your moves."));
            pages.Add(new Page("You can hold up to 4 moves at a time, and might be able to get a new move when you level-up."));
            pages.Add(new Page("You can open the info panel to how what each of your moves does."));
            pages.Add(new Page("If you don't have enough energy to perform a move it will be unavailable."));
            pages.Add(new Page("You'll need to hit the charge option to gain energy, which takes a turn."));
            pages.Add(new Page("You can also try your hand at running away if you wish to retreat, but it may fail."));

            // Loads the pages.
            LoadTutorial(ref pages);

            clearedBattle = true;
        }

        // Loads the treasure tutorial.
        public void LoadTreasureTutorial()
        {
            // Page Object
            List<Page> pages = new List<Page>();

            // Pages
            pages.Add(new Page("This is a treasure room!"));
            pages.Add(new Page("You get a free level-up, health restore, and energy restore if you collect the treasure."));
            pages.Add(new Page("If you leave the treasure you can come back and open it later."));

            // Loads the pages.
            LoadTutorial(ref pages);

            clearedTreasure = true;
        }

        // Loads the overworld tutorial, whichs hows after you clear a room.
        public void LoadOverworldTutorial()
        {
            // Page Object
            List<Page> pages = new List<Page>();

            // Pages
            pages.Add(new Page("Now that you've cleared your first room the boss is open."));
            pages.Add(new Page("The game ends when you beat the boss, so clear as many rooms as you think you need."));
            pages.Add(new Page("The more rooms you clear the stronger you'll become."));
            pages.Add(new Page("Enemies will get stronger the longer the game goes on, so keep that in mind."));
            pages.Add(new Page("On with the game."));

            // Loads the pages.
            LoadTutorial(ref pages);

            clearedOverworld = true;
        }

        // Loads for the boss tutorial.
        public void LoadBossTutorial()
        {
            // Page Object
            List<Page> pages = new List<Page>();

            // Pages
            pages.Add(new Page("Welcome to the boss room!"));
            pages.Add(new Page("All you need is to beat the boss to win the game!"));

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
            pages.Add(new Page("You got a game over"));
            pages.Add(new Page("All living enemies have had their health and energy restored."));
            pages.Add(new Page("Your health and energy have also been restored, but your moves have been randomized."));

            // Loads the pages.
            LoadTutorial(ref pages);

            clearedGameOver = true;
        }

        // Called when the tutorial starts, which is when the textbox is opened.
        public void OnTutorialStart()
        {
            gameManager.mouseTouchInput.gameObject.SetActive(false);
        }

        // Called when the tutorial starts, which is when the textbox is closed.
        public void OnTutorialEnd()
        {
            gameManager.mouseTouchInput.gameObject.SetActive(true);
        }

    }
}