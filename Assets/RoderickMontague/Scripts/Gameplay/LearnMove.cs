using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RM_BBTS
{
    public class LearnMove : MonoBehaviour
    {
        // The player.
        public Player player;

        // The new move.
        public Move newMove;

        // The existing moves.
        public Move move0;
        public Move move1;
        public Move move2;
        public Move move3;

        // Start is called before the first frame update
        void Start()
        {

        }

        // This function is called when the object becomes enabled and visible.
        private void OnEnable()
        {
            move0 = player.Move0;
            move1 = player.Move1;
            move2 = player.Move2;
            move3 = player.Move3;
        }

        // Switches the new move with move 0.
        public void SwitchWithMove0()
        {
            player.Move0 = newMove;

            Move temp = move0;
            move0 = newMove;
            newMove = temp;
        }

        // Switches the new move with move 1.
        public void SwitchWithMove1()
        {
            player.Move1 = newMove;

            Move temp = move1;
            move1 = newMove;
            newMove = temp;
        }

        // Switches the new move with move 2.
        public void SwitchWithMove2()
        {
            player.Move2 = newMove;

            Move temp = move2;
            move2 = newMove;
            newMove = temp;
        }

        // Switches the new move with move 3.
        public void SwitchWithMove3()
        {
            player.Move3 = newMove;

            Move temp = move3;
            move3 = newMove;
            newMove = temp;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}