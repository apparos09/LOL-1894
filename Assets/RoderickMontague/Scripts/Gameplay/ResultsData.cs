using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_BBTS
{
    // Holds the results data for the game, which is read in during the results screen.
    public class ResultsData : MonoBehaviour
    {
        // Rooms cleared
        public int roomsCleared = 0;

        // Total rooms
        public int totalRooms = 0;

        // The total time in seconds.
        public float totalTime = 0.0F;

        // The total turns.
        public int totalTurns = 0;

        // The 4 moves.
        public string move0 = "-";
        public string move1 = "-";
        public string move2 = "-";
        public string move3 = "-";
    }
}