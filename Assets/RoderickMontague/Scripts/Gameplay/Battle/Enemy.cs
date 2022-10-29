using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_BBTS
{
    // A class for the enemeies.
    public class Enemy : BattleEntity
    {
        // becomes 'true', when the enemy is in the process of selecting a move.
        private bool selectingMove;

        // Start is called before the first frame update
        new void Start()
        {
            base.Start();
        }

        // Called to have the enemy select a move.
        public void SelectMove()
        {
            selectedMove = Move0;
            // TODO: implement AI
            // StartCoroutine(DecideNextMove());
        }

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

        // Update is called once per frame
        new void Update()
        {
            base.Update();
        }
    }
}