using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_BBTS
{
    // A class for the enemeies.
    public class Enemy : BattleEntity
    {
        // private struct MoveOption
        // {
        //     public Move move;
        // 
        // 
        // }

        // The restore percentage for health and energy on a game over.
        public const float GAME_OVER_HEALTH_RESTORE_PERCENT = 1.0F;
        public const float GAME_OVER_ENERGY_RESTORE_PERCENT = 1.0F;

        [Header("Other")]

        // Holds a reference to the player.
        public Player player;

        // Sets to use the AI.
        private bool useAI = true;

        // The maximum amount of steps used for predictions.
        protected uint stepsMax = 3;

        // becomes 'true', when the enemy is in the process of selecting a move.
        private bool selectingMove;

        // Start is called before the first frame update
        new void Start()
        {
            base.Start();
        }

        // Called when a battle turn is happening.
        public override void OnBattleTurn()
        {
            // Calls the parent's battle turn function.
            base.OnBattleTurn();

            // Move not selected yet.
            if (selectedMove == null)
                SelectMove();
        }

        // Called to have the enemy select a move. This has the enemy choose the move for themself.
        public void SelectMove()
        {
            // Not using the AI.
            if(!useAI)
            {
                // Selects move0, or otherwise it selected charge.
                if (Move0.Usable(this))
                    selectedMove = Move0;
                else
                    SelectCharge();

                // selectedMove = Move0;

                return;
            }

            // TODO: implement AI
            // StartCoroutine(DecideNextMove());

            // Checks if the enemy needs to change their energy.
            // If the enemy has no energy, then they MUST charge their energy.
            // If the enemy does have energy, then it needs to check if any move can be performed.
            // If no move can be performed, then the enemy must change their energy.
            bool needCharge = (Energy <= 0.0F);

            // Check if any move can be performed without charging to start off.
            // This will always make the entity select charge if they don't have any other usable moves.
            if(!needCharge)
            {
                // Checks if a charge is needed.
                // This needs to be set to true by default for the check.
                needCharge = true;

                // Goes through each move.
                for (int i = 0; i < moves.Length; i++)
                {
                    // Move has been set.
                    if (moves[i] != null)
                    {
                        // Checks if the move can be used.
                        needCharge = !(moves[i].Usable(this));
                    }

                    // A move can be used without charging.
                    if (!needCharge)
                        break;

                }
            }

            // The enemy needs to charge their energy, so select this move.
            if (needCharge)
            {
                SelectCharge();
                return;
            }

            // Determines the move options the player has.
            List<Move> moveOptions = new List<Move>();

            // Adds the charge move to the list.
            if (!HasFullCharge())
                moveOptions.Add(MoveList.Instance.ChargeMove);

            // Adds the standard moves to the list.
            foreach(Move move in moves)
            {
                // Adds the moves.
                if(move != null)
                {
                    // If the move can be performed.
                    // If it's a run move (default move), then it is not included since it does nothing for enemies.
                    if(move.Usable(this) && move.Id != moveId.run)
                        moveOptions.Add(move);
                }
            }

            // If thre are no move options just select' charge'.
            if(moveOptions.Count == 0)
            {
                SelectCharge();
                return;
            }
            else // Chooses a random move.
            {
                // TODO: improve the AI on this.

                // Selects the random move.
                int randIndex = Random.Range(0, moveOptions.Count);
                selectedMove = moveOptions[randIndex];

                // Returns.
                return;
            }


            // Runs a move step and provides a list of moves.
            // selectedMove = RunMoveStep(moveOptions, 0, 1);
        }

        // // Runs a step on the provided move.
        // private Move RunMoveStep(List<Move> movesOptions, int moveIndex, int step)
        // {
        //     // Checks if the player will likely go first.
        //     // TODO: check for speed adjustments.
        //     bool playerFirst = player.Speed >= Speed;
        // 
        // 
        // }

        // // Called to decide the next move.
        // private IEnumerator DecideNextMove()
        // {
        //     bool choseMove = false;
        // 
        //     while(!choseMove)
        //     {
        //         yield return null;
        //     }
        // 
        //     selectedMove = Move0;
        // }

        // Called when the player has gotten a game over.
        public void OnGameOver()
        {
            // Restores the player's health and energy levels. This rounds up to a whole number.
            // HEALTH and ENERGY
            RestoreHealth(GAME_OVER_HEALTH_RESTORE_PERCENT);
            RestoreEnergy(GAME_OVER_ENERGY_RESTORE_PERCENT);
        }


        // ANIMATION //
        // Sets the idle animation for the enemy with the provided animator.
        public bool SetEnemyIdleAnimation(Animator animator)
        {
            // Checks to see if the change was successful.
            bool success = true;

            // Checks the ID.
            switch (id)
            {
                case battleEntityId.combatBot:
                    animator.Play("BEY - Combat Bot - Idle");
                    break;
                case battleEntityId.ufo1:
                    animator.Play("BEY - UFO 1 - Idle");
                    break;
                case battleEntityId.ufo2:
                    animator.Play("BEY - UFO 2 - Idle");
                    break;
                case battleEntityId.ufo3:
                    animator.Play("BEY - UFO 3 - Idle");
                    break;

                case battleEntityId.insect1:
                    animator.Play("BEY - Insect 1 - Idle");
                    break;

                case battleEntityId.insect2:
                    animator.Play("BEY - Insect 2 - Idle");
                    break;

                case battleEntityId.ghost1:
                    animator.Play("BEY - Space Ghost 1 - Idle");
                    break;

                case battleEntityId.ghost2:
                    animator.Play("BEY - Space Ghost 2 - Idle");
                    break;

                case battleEntityId.comet:
                    animator.Play("BEY - Comet - Idle");
                    break;

                case battleEntityId.sunRock1:
                    animator.Play("BEY - Sun Rock 1 - Idle");
                    break;

                case battleEntityId.sunRock2:
                    animator.Play("BEY - Sun Rock 2 - Idle");
                    break;

                case battleEntityId.moonRock1:
                    animator.Play("BEY - Moon Rock 1 - Idle");
                    break;

                case battleEntityId.moonRock2:
                    animator.Play("BEY - Moon Rock 2 - Idle");
                    break;

                case battleEntityId.fireBot1:
                    animator.Play("BEY - Fire Bot 1 - Idle");
                    break;

                case battleEntityId.fireBot2:
                    animator.Play("BEY - Fire Bot 2 - Idle");
                    break;

                case battleEntityId.waterBot1:
                    animator.Play("BEY - Water Bot 1 - Idle");
                    break;

                case battleEntityId.waterBot2:
                    animator.Play("BEY - Water Bot 2 - Idle");
                    break;

                case battleEntityId.earthBot1:
                    animator.Play("BEY - Earth Bot 1 - Idle");
                    break;

                case battleEntityId.earthBot2:
                    animator.Play("BEY - Earth Bot 2 - Idle");
                    break;

                case battleEntityId.airBot1:
                    animator.Play("BEY - Air Bot 1 - Idle");
                    break;

                case battleEntityId.airBot2:
                    animator.Play("BEY - Air Bot 2 - Idle");
                    break;

                case battleEntityId.sharp1:
                    animator.Play("BEY - Sharp 1 - Idle");
                    break;

                case battleEntityId.sharp2:
                    animator.Play("BEY - Sharp 2 - Idle");
                    break;

                case battleEntityId.cBugRed1:
                    animator.Play("BEY - Red Virus 1 - Idle");
                    break;

                case battleEntityId.cBugRed2:
                    animator.Play("BEY - Red Virus 2 - Idle");
                    break;

                case battleEntityId.cBugBlue1:
                    animator.Play("BEY - Blue Virus 1 - Idle");
                    break;

                case battleEntityId.cBugBlue2:
                    animator.Play("BEY - Blue Virus 2 - Idle");
                    break;

                case battleEntityId.cBugYellow1:
                    animator.Play("BEY - Yellow Virus 1 - Idle");
                    break;

                case battleEntityId.cBugYellow2:
                    animator.Play("BEY - Yellow Virus 2 - Idle");
                    break;

                case battleEntityId.blackHole:
                    animator.Play("BEY - Black Hole - Idle");
                    break;

                case battleEntityId.planet1:
                    animator.Play("BEY - Planet 1 - Idle");
                    break;

                case battleEntityId.planet2:
                    animator.Play("BEY - Planet 2 - Idle");
                    break;

                default:
                    animator.Play("No Idle");
                    success = false;
                    break;
            }

            return success;
        }


        // Update is called once per frame
        new void Update()
        {
            base.Update();
        }
    }
}