using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RM_BBTS
{
    // Manages the battle operations for the game. This becomes active when the game enters a battle state.
    public class BattleManager : GameState
    {
        // Becomes 'true' when the overworld is initialized.
        public bool initialized = false;

        // the manager for the game.
        public GameplayManager gameManager;

        // The door
        public Door door;

        // The textbox
        public TextBox textBox;

        [Header("Battle")]

        // The player.
        public Player player;

        // The opponent for the player.
        // TODO: should a new opponent be generated everytime? Should really just cycle through some pre-build objects.
        public BattleEntity opponent;

        // The sprite for the opponent.
        public SpriteRenderer opponentSprite;

        // Base objects that are activated for battle.
        public Enemy enemyBase;
        public Treasure treasureBase;
        public Boss bossBase;

        // [Header("Battle/Mechanics")]
        // // The move the player has selected.
        // public Move playerMove;
        // 
        // // The move the opponent has selected.
        // public Move opponentMove;

        [Header("UI")]
        // The user interface.
        public GameObject ui;

        // The turn text. Each entry is a different page.
        public List<Page> turnText;

        // The player's move page.
        public Page playerMovePage;

        // The opponent's move page.
        public Page opponentMovePage;

        [Header("UI/Player")]

        // Move 0 (index 0) button.
        [Tooltip("The button for using Player Move 0, which is at index [0].")]
        public Button move0Button;
        public TMP_Text move0Text;

        // Move 1 (index 1) button.
        [Tooltip("The button for using Player Move 1, which is at index [1].")]
        public Button move1Button;
        public TMP_Text move1Text;

        // Move 2 (index 2) button.
        [Tooltip("The button for using Player Move 2, which is at index [2].")]
        public Button move2Button;
        public TMP_Text move2Text;

        // Move 3 (index 3) button.
        [Tooltip("The button for using Player Move 3, which is at index [3].")]
        public Button move3Button;
        public TMP_Text move3Text;

        // Charge Button
        public Button chargeButton;

        // Run Button
        public Button runButton;

        [Header("UI/Other")]

        // The health bar for the opponent.
        public ProgressBar opponentHealthBar;

        // TODO: this will not be shown n the final game.
        public TMP_Text opponentHealthText;

        // Start is called before the first frame update
        void Start()
        {
            // // enemy base not set, so make a base.
            // if(enemyBase == null)
            // {
            //     GameObject go = new GameObject("Enemy Base");
            //     enemyBase = go.AddComponent<Enemy>();
            //     go.transform.parent = gameObject.transform;
            // }
            // 
            // // treasure base not set, so make a base.
            // if (treasureBase == null)
            // {
            //     GameObject go = new GameObject("Treasure Base");
            //     treasureBase = go.AddComponent<Treasure>();
            //     go.transform.parent = gameObject.transform;
            // }
            // 
            // // enemy base not set, so make a base.
            // if (bossBase == null)
            // {
            //     GameObject go = new GameObject("Boss Base");
            //     bossBase = go.AddComponent<Boss>();
            //     go.transform.parent = gameObject.transform;
            // }

            // Turns off the bases.
            enemyBase.gameObject.SetActive(false);
            treasureBase.gameObject.SetActive(false);
            bossBase.gameObject.SetActive(false);

            // Initializes the list.
            turnText = new List<Page>();

            // When the textbox disappears the turn is over, so call this function.
            textBox.OnTextBoxFinishedAddCallback(OnTurnOver);

            // Close the textbox when the player is done.
            textBox.closeOnEnd = true;
        }

        // This function is called when the object becomes enabled and active
        private void OnEnable()
        {
            if (ui != null)
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
            initialized = true;

            // Sets the battle entity from the door.
            // opponent = null; // TODO: comment out.
            
            // Checks the type of entity.
            switch(door.battleEntity.id)
            {
                case battleEntityId.treasure: // treasure
                    opponent = treasureBase;
                    break;
                case battleEntityId.boss: // boss
                    opponent = bossBase;
                    break;
                default: // enemy
                    opponent = enemyBase;
                    break;
                    
            }

            // Opponent has been set.
            if(opponent != null)
            {
                opponent.LoadBattleData(door.battleEntity);
                opponentSprite.sprite = opponent.sprite;
            }    
                

            // Checks move activity to see if the player can use it or not.
            // Also changes the move name on the display.

            // Move 0
            // move0Button.interactable = player.Move0 != null;
            move0Text.text = (player.Move0 != null) ? player.Move0.Name : "-";

            // Move 1
            // move1Button.interactable = player.Move1 != null;
            move1Text.text = (player.Move1 != null) ? player.Move1.Name : "-";

            // Move 2
            // move2Button.interactable = player.Move2 != null;
            move2Text.text = (player.Move2 != null) ? player.Move2.Name : "-";

            // Move 3
            // move3Button.interactable = player.Move3 != null;
            move3Text.text = (player.Move3 != null) ? player.Move3.Name : "-";

            // Checks if the player has a full charge.
            // chargeButton.interactable = !player.HasFullCharge();

            // Changes the 'interactable' toggle for the buttons.
            RefreshPlayerOptions();

            // Updates the interface.
            UpdateUI();
        }

        // Called when the mouse hovers over an object.
        public override void OnMouseHovered(GameObject hoveredObject)
        {
            throw new System.NotImplementedException();
        }

        // Called when the mouse interacts with an entity.
        public override void OnMouseInteract(GameObject heldObject)
        {

        }

        // Called when the user's touch interacts with an entity.
        public override void OnTouchInteract(GameObject touchedObject, Touch touch)
        {

        }

        // Called with the object that was received with the interaction.
        protected override void OnInteractReceive(GameObject gameObject)
        {
            throw new System.NotImplementedException();
        }

        // Sets player controls to interactable or not. RefreshPlayerOptions is also called to disable buttons that do nothing. 
        public void SetPlayerOptionsAvailable(bool interactable)
        {
            move0Button.interactable = interactable;
            move1Button.interactable = interactable;
            move2Button.interactable = interactable;
            move3Button.interactable = interactable;
            
            chargeButton.interactable = interactable;
            runButton.interactable = interactable;

            // If all were turned on, check to see if some should stay off.
            if(interactable)
                RefreshPlayerOptions();
        }

        // Enables the player options.
        public void EnablePlayerOptions()
        {
            SetPlayerOptionsAvailable(true);
        }

        // Disables the player options.
        public void DisablePlayerOptions()
        {
            SetPlayerOptionsAvailable(false);
        }

        public void RefreshPlayerOptions()
        {
            // Checks move activity to see if the player can use it or not.
            // Also changes the move name on the display.

            // Enables/disables various buttons.

            // Move 0 
            if(player.Move0 != null)
                move0Button.interactable = player.Move0.Energy <= player.Energy;
            else
                move0Button.interactable = false;

            // Move 1
            if (player.Move1 != null)
                move1Button.interactable = player.Move1.Energy <= player.Energy;
            else
                move1Button.interactable = false;

            // Move 2 
            if (player.Move2 != null)
                move2Button.interactable = player.Move2.Energy <= player.Energy;
            else
                move2Button.interactable = false;

            // Move 3
            if (player.Move3 != null)
                move3Button.interactable = player.Move3.Energy <= player.Energy;
            else
                move3Button.interactable = false;

            // Checks if the player has a full charge.
            chargeButton.interactable = !player.HasFullCharge();
        }

        // Updates the battle visuals.
        // If 'playerTurn' is true, then the update is coming from the player's turn.
        // If false, it's coming from the enemy's turn.
        private void AddVisualUpdateCallbacks(bool playerTurn)
        {
            // If there are pages to attach callbacks too.
            if(turnText.Count > 0)
            {
                if(playerTurn) // Player turn
                {
                    turnText[turnText.Count - 1].OnPageClosedAddCallback(UpdateUI);
                    turnText[turnText.Count - 1].OnPageClosedAddCallback(gameManager.UpdateUI);
                }
                else
                {
                    // turnText[turnText.Count - 1].OnPageClosedAddCallback(gameManager.UpdatePlayerHealthUI);
                    turnText[turnText.Count - 1].OnPageClosedAddCallback(gameManager.UpdateUI);
                }
            }
        }

        // Called to perform the player's move.
        private void PerformPlayerMove()
        {
            player.selectedMove.Perform(player, opponent, this);
        }

        // Called to perform the opponent's move.
        private void PerformOpponentMove()
        {
            opponent.selectedMove.Perform(opponent, player, this);
        }

        // Performs the two moves.
        public void PerformMoves()
        {
            // Both sides have selected a move.
            if (player.selectedMove != null && opponent.selectedMove != null)
            {
                // Checks who goes first.
                bool playerFirst = false;

                // TODO: account for status effects.

                // Clears out the past text.
                turnText.Clear();

                // Determines who goes first.
                if (player.Speed > opponent.Speed) // player first
                    playerFirst = true;
                else if (player.Speed < opponent.Speed) // opponent first
                    playerFirst = false;
                else // random
                    playerFirst = Random.Range(0, 2) == 1;


                // Loads the selected moves.
                // The two pages for the player and the opponent.

                // Adds the player's move.
                playerMovePage = new Page(player.displayName + " used " + player.selectedMove.Name + "!");
                playerMovePage.OnPageOpenedAddCallback(PerformPlayerMove);

                // Adds the opponent's move.
                opponentMovePage = new Page(opponent.displayName + " used " + opponent.selectedMove.Name + "!");
                opponentMovePage.OnPageOpenedAddCallback(PerformOpponentMove);

                // Places the pages in order.
                if(playerFirst)
                {
                    turnText.Add(playerMovePage);
                    turnText.Add(opponentMovePage);
                }
                else
                {
                    turnText.Add(opponentMovePage);
                    turnText.Add(playerMovePage);
                }
                

                // Show the textbox.
                // TODO: hide player move controls.
                textBox.ReplacePages(turnText);
                textBox.Open();

                // Disable the player options since the textbox is open.
                DisablePlayerOptions();
            }
            else
            {
                // Gets the moves from the player and the opponent.
                player.OnBattleTurn(); // does nothing right now.

                // opponent.
                opponent.OnBattleTurn(); // calculates next move (evenutally)
            }
        }

        
        // Called when the player attempts to run away. TODO: have the enemy's move still go off if the run fails.
        public void RunAway()
        {
            // Becomes 'true' if the run attempt was successful.
            bool success = false;

            // Overrides the selected move.
            player.selectedMove = MoveList.Instance.RunMove;

            // If there's no opponent then the player can always run away.
            if (opponent == null)
            {
                success = true;
            }
            // If there is an opponent there the player may be unable to leave.
            else
            {
                // There's a 1/2 chance of running away.
                success = (Random.Range(0, 2) == 1);
            }

            // Returns to the overworld if the run was successful.
            if (success)
                ToOverworld();
            else
                Debug.Log("Run failed.");
        }

        // Called when the turn is over.
        private void OnTurnOver()
        {
            player.selectedMove = null;
            opponent.selectedMove = null;

            playerMovePage = null;
            opponentMovePage = null;

            EnablePlayerOptions();
        }

        // Called when potentially learning a new move.
        public void OnLearningNewMove()
        {

        }

        // Goes to the overworld.
        public void ToOverworld()
        {
            gameManager.UpdateUI();
            gameManager.EnterOverworld();
        }

        // Updates the user interface.
        public void UpdateUI()
        {
            // TODO: remove the safety checK??
            opponentHealthBar.SetValue(opponent.Health / opponent.MaxHealth);
            opponentHealthText.text = opponent.Health.ToString() + "/" + opponent.MaxHealth.ToString();
        }

        // Update is called once per frame
        void Update()
        {
            // If the text box is not visible.
            if(!textBox.IsVisible())
            {
                // If both entities are alive do battle calculations.
                if (player.Health > 0 && opponent.Health > 0)
                {
                    PerformMoves();
                }
                else
                {
                    // Returns to the overworld. TODO: account for game over.
                    // The player got a game over.
                    if (player.Health <= 0) // game over
                    {
                        gameManager.OnGameOver();
                        ToOverworld();
                    }
                    else // The player won the fight.
                    {
                        player.LevelUp();
                        ToOverworld();
                    }
                }
            }
            
        }

        
    }
}