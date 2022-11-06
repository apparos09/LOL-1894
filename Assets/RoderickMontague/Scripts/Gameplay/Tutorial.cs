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

        // public bool showedOverworld;
        // 
        // public bool showedBattle;
        // 
        // public bool showed;

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
            pages.Add(new Page("Welcome to the simulation, Battle Bot!"));
            pages.Add(new Page("Your job is to beat the boss of the simulation."));
            pages.Add(new Page("But first, I'd recommend you do some smaller battles to level-up and get new moves."));
            pages.Add(new Page("To the left is your health, and to the right is your energy."));
            pages.Add(new Page("That will be explained more when you get into battle."));
            pages.Add(new Page("Speaking of which, click or touch a door to enter it."));
            pages.Add(new Page("The boss room will be opened up after you clear one room."));

            // Loads the pages.
            LoadTutorial(ref pages);

            clearedIntro = true;
        }

        // Loads the overworld tutorial.
        public void LoadOverworldTutorial()
        {
            List<Page> pages = new List<Page>();
        }

        // Loads the battle tutorial.
        public void LoadBattleTutorial()
        {
            List<Page> pages = new List<Page>();
        }

        // Loads for the boss tutorial.
        public void LoadBossTutorial()
        {
            List<Page> pages = new List<Page>();
        }

        // Loads the game over tutorial.
        public void LoadGameOverTutorial()
        {
            List<Page> pages = new List<Page>();
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